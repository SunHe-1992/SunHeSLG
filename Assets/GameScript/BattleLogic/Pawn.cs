using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YooAsset;
using cfg.SLG;
using cfg;
namespace SunHeTBS
{

    public class PawnBase
    {
        public Vector3Int curPosition;
        public PawnCamp camp = PawnCamp.Default;
        public bool actionEnd = false;

        public int TilePosId()
        {
            return TBSMapService.Inst.GetTileId(curPosition);
        }
    }
    public class Pawn : PawnBase
    {
        public static int globalSequence = 0;
        public int sequenceId = 0;
        public PawnController controller;
        /// <summary>
        /// 3d model res name
        /// </summary>
        public string modelName;
        /// <summary>
        /// move 1 tile need time
        /// </summary>
        public float moveTileTime = 0.15f;
        public PawnState curState;
        /// <summary>
        /// if false:show action menu on move end,true: act to target pawn
        /// </summary>
        public bool InstantActionAfterMove = false;
        public Pawn targetPawn = null;
        public List<Pawn> targetPawnList = null;
        /// <summary>
        /// when player controlling
        /// </summary>
        public Vector3Int savePos;

        #region Character Info
        /// <summary>
        /// id in TbCharacter
        /// </summary>
        public int CharacterId;
        /// <summary>
        /// id in Class
        /// </summary>
        public int ClassId;
        /// <summary>
        /// character's config data
        /// </summary>
        public cfg.CharacterData charCfg;
        /// <summary>
        /// class's config data
        /// </summary>
        public cfg.ClassData classCfg;

        public bool deathMark = false;
        public bool deathAnimPlayed = false;
        #endregion

        public void Init()
        {
            globalSequence++;
            sequenceId = globalSequence;

            curState = PawnState.Idle;

            this.savePos = this.curPosition;
            deathMark = false;
            deathAnimPlayed = false;
            charCfg = ConfigManager.table.Character.Get(CharacterId);
            if (charCfg != null)
            {
                this.charAttr = new BasicAttribute(charCfg.CharAttr);
            }
            classCfg = ConfigManager.table.Class.Get(ClassId);
            if (classCfg != null)
            {
                if (classCfg.ClassType == ClassType.Flying)
                    this.moveType = PawnMoveType.Flier;
                else
                    this.moveType = PawnMoveType.Ground;
            }

            GetAttribute();
            GetCombatAttr();
            this.HP = this.attrCache.HPMax;
            InitSkills();

            this.LoadModel();
        }
        public override string ToString()
        {
            return $"Pawn sid={this.sequenceId} Camp:{camp} model={modelName}";
        }

        public void LoadModel()
        {
            if (this.controller == null)
            {
                string resPath = $"Pawns/{modelName}";
                AssetHandle handler = YooAssets.LoadAssetAsync<GameObject>(resPath);
                handler.Completed += ModelLoadDone;

            }
        }
        void ModelLoadDone(AssetHandle handle)
        {
            if (handle.AssetObject != null)
            {
                var obj = GameObject.Instantiate(handle.AssetObject as GameObject);
                obj.name = "model";
                GameObject pCtrlObj = new GameObject(GetObjName());
                obj.transform.SetParent(pCtrlObj.transform, false);
                this.controller = pCtrlObj.AddComponent<PawnController>();
                this.controller.Initialize(this);
            }
        }
        string GetObjName()
        {
            return this.modelName;
        }

        #region Attribute for test
        public PawnMoveType moveType = PawnMoveType.Ground;
        public int GetMovement()
        {
            if (this.attrCache != null)
            {
                int move = 0;
                move += attrCache.Mov;
                move += tileAttr.MoveChange;
                return move;
            }
            return 4;
        }

        /// <summary>
        /// pawn will cost extra move points in certain tiles
        /// </summary>
        /// <returns></returns>
        public bool IsExtraMoveCost()
        {
            if (this.moveType == PawnMoveType.Flier)
                return false;
            //any else skill or buff
            return true;
        }
        /// <summary>
        /// foes do not block this unit's movement 
        /// </summary>
        /// <returns></returns>
        public bool IsPassFoe()
        {
            //todo check skill can pass foe tiles
            return false;
        }
        public int GetAtkRangeMax()
        {
            //check this pawn has attack ability 
            if (HoldingWeaponOrStaff())
            {
                return equippedWeapon.rangeMax;
            }
            else return 0;
        }
        public int GetAtkRangeMin()
        {
            if (HoldingWeaponOrStaff())
            {
                return equippedWeapon.rangeMin;
            }
            else return 0;
        }
        /// <summary>
        /// get min and max atk range ,in possible weapons in my inventory
        /// </summary>
        /// <returns></returns>
        public int[] GetPossibleAtkRange()
        {
            int[] rangeArray = new int[2] { 0, 0 };
            int nearRange = int.MaxValue;
            int longRange = 0;
            foreach (var wp in this.itemList)
            {
                if (wp is Weapon)
                {
                    var wep = wp as Weapon;
                    if (CanEquipWeapon(wep))
                    {
                        if (wep.rangeMin < nearRange) nearRange = wep.rangeMin;
                        if (wep.rangeMax > longRange) longRange = wep.rangeMax;
                    }
                }
            }
            if (nearRange == int.MaxValue && longRange == 0)
            {
                return null;
            }
            rangeArray[0] = nearRange;
            rangeArray[1] = longRange;
            return rangeArray;
        }
        public bool IsFlier()
        {
            return this.moveType == PawnMoveType.Flier;
        }
        /// <summary>
        /// equipped with a weapon/staff
        /// </summary>
        /// <returns></returns>
        public bool HoldingWeaponOrStaff()
        {
            return this.equippedWeapon != null;
        }
        /// <summary>
        /// equipped with a weapon
        /// </summary>
        /// <returns></returns>
        public bool HoldingWeapon()
        {
            if (this.equippedWeapon != null)
            {
                return BattleTools.IsWeaponType(equippedWeapon.itemCfg.ItemType);
            }
            return false;
        }
        public bool HoldingStaff()
        {
            if (this.equippedWeapon != null)
            {
                return BattleTools.IsStaffType(equippedWeapon.itemCfg.ItemType);
            }
            return false;
        }
        public ItemType GetHoldingWeaponType()
        {
            if (HoldingWeaponOrStaff())
            {
                return this.equippedWeapon.itemCfg.ItemType;
            }
            return ItemType.Item;
        }
        #endregion

        #region cache move and attack tile data

        /// <summary>
        /// save movable tiles, based on current pos and move points
        /// </summary>
        public HashSet<int> moveTileIds;
        /// <summary>
        /// save tiles no pawns standing on
        /// </summary>
        public List<int> destTileIds;
        /// <summary>
        /// save all moveable tile ids into moveTileIds
        /// </summary>
        public void CalculateMoveArea()
        {
            var map = TBSMapService.Inst.map;
            moveTileIds = new HashSet<int>();
            destTileIds = new List<int>();
            HashSet<TileEntity> walkableTiles = map.WalkableTiles(this.curPosition, this.GetMovement(), this.IsExtraMoveCost(), this.IsPassFoe(), this.IsFlier());
            //show blue planes in walkable tiles
            foreach (var tile in walkableTiles)
            {
                moveTileIds.Add(tile.tileId);
                if (tile.camp == PawnCamp.Default)
                {
                    destTileIds.Add(tile.tileId);
                }
            }
        }
        /// <summary>
        /// key=range,value=tile ids
        /// </summary>
        public Dictionary<int, HashSet<int>> rangeTileDic;
        /// <summary>
        /// save tile ids for every attack range
        /// </summary>
        public void CalculateRangeArea()
        {
            rangeTileDic = new Dictionary<int, HashSet<int>>();
            int[] rngArray = GetPossibleAtkRange();
            if (rngArray == null)
            {
                return;
            }
            CalculateRangeArea(rngArray[0], rngArray[1]);
        }
        public void CalculateRangeArea(int rangeMin, int rangeMax)
        {
            var map = TBSMapService.Inst.map;
            foreach (int tileId in moveTileIds)//for every movable tile
            {
                var centerTile = map.GetTileFromDic(tileId);

                for (int m = -rangeMax; m <= rangeMax; m++)
                {
                    int rangeN = rangeMax - Mathf.Abs(m);
                    for (int n = -rangeN; n <= rangeN; n++) //m,n loop the diamond shape around centerPos
                    {
                        int tileRange = Mathf.Abs(m) + Mathf.Abs(n);
                        if (rangeMin > 0 && tileRange < rangeMin)//consider min range
                        {
                            continue;
                        }
                        if (!rangeTileDic.ContainsKey(tileRange))
                            rangeTileDic.Add(tileRange, new HashSet<int>());
                        var rangeHashSet = rangeTileDic[tileRange];
                        // m,n is the pos
                        int targetTileId = map.XY2TileId(centerTile.Position.x + m, centerTile.Position.y + n);
                        if (!rangeHashSet.Contains(targetTileId))
                        {
                            if (map.GetTileFromDic(targetTileId) != null)
                            {
                                rangeTileDic[tileRange].Add(targetTileId);
                            }
                        }
                    }
                }
            }
        }

        List<int> GetDestTileList()
        {
            if (moveTileIds == null)
                CalculateMoveArea();
            return destTileIds;
        }
        /// <summary>
        /// get tile ids in range
        /// </summary>
        /// <param name="minRange"></param>
        /// <param name="maxRange"></param>
        /// <returns></returns>
        public List<int> GetTileIdsInRange(int minRange, int maxRange)
        {
            List<int> tileIdList = new List<int>();
            for (int i = minRange; i <= maxRange; i++)
            {
                if (rangeTileDic.ContainsKey(i))
                {
                    tileIdList.AddRange(rangeTileDic[i]);
                }
            }
            return tileIdList;
        }
        public List<TileEntity> GetInRangePosOneTile(int rangeMin, int rangeMax, Vector3Int startPos)
        {
            List<TileEntity> tileList = new List<TileEntity>();
            var map = TBSMapService.Inst.map;
            var startTile = map.Tile(startPos);
            for (int m = -rangeMax; m <= rangeMax; m++)
            {
                int rangeN = rangeMax - Mathf.Abs(m);
                for (int n = -rangeN; n <= rangeN; n++) //m,n loop the diamond shape around centerPos
                {
                    int tileRange = Mathf.Abs(m) + Mathf.Abs(n);
                    if (rangeMin > 0 && tileRange < rangeMin)//consider min range
                    {
                        continue;
                    }
                    Vector3Int pos = new Vector3Int(startTile.Position.x + m, startTile.Position.y + n, 0);
                    var findTile = map.Tile(pos);
                    if (findTile != null)
                    {
                        tileList.Add(findTile);
                    }
                }
            }
            return tileList;
        }
        public EffectType GetTileEffect()
        {
            var map = TBSMapService.Inst.map;
            var tile = map.Tile(this.curPosition);
            if (tile != null)
                return tile.effectType;
            return EffectType.None;
        }
        /// <summary>
        /// check if this pos is in my attack range
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool CheckTileInRange(Vector3Int pos)
        {
            int posId = TBSMapService.Inst.GetTileId(pos);
            foreach (var posHash in rangeTileDic.Values)
            {
                if (posHash.Contains(posId))
                    return true;
            }
            return false;
        }
        #endregion

        #region Pawn move functions

        List<INode> moveTileList;
        public TileEntity moveDestTile;

        public void SetMovePath(List<INode> nodeList)
        {
            this.moveTileList = nodeList;
            if (this.controller != null)
            {
                controller.SetPaths(nodeList);
            }
        }
        public void StartMove()
        {
            this.curState = PawnState.Moving;
            controller.StartMove();
            //hide this pawn's move/atk  planes
            TBSMapService.Inst.UnspawnAllCoverPlanes();
        }
        public void StopMove()
        {
            this.curState = PawnState.Idle;
            controller.StopMove();
            moveDestTile = moveTileList[moveTileList.Count - 1] as TileEntity;
            this.curPosition = moveDestTile.Position;
            CalculateCombatAttr();
        }
        public void ResetPosition()
        {
            this.controller.SetPosition();
        }
        #endregion

        #region trun and phase
        public void OnNewTurnStart(int turnNum)
        {

        }
        public void OnNewPhaseStart(PawnCamp phaseCamp)
        {

        }
        public void EndAction()
        {
            this.actionEnd = true;
        }
        #endregion


        public void ExecuteWait()
        {
            this.EndAction();
        }

        /// <summary>
        /// on temp position
        /// </summary>
        public void CountNearPawns(out int atkPawns, out int tradablePawns, out int conveyPawns)
        {
            tradablePawns = 0;
            conveyPawns = 0;
            atkPawns = 0;
            //find foes again
            int[] range = GetPossibleAtkRange();
            List<TileEntity> atkTiles = null;
            HashSet<int> tileHash = new HashSet<int>();
            if (range != null)//find tiles in possible attack range
            {
                atkTiles = GetInRangePosOneTile(range[0], range[1], curPosition);
                foreach (var tile in atkTiles)
                {
                    tileHash.Add(tile.tileId);
                }
            }
            else //find neighbor tiles 
            {
                var curTile = TBSMapService.Inst.map.Tile(this.curPosition);
                atkTiles = TBSMapService.Inst.map.GetNeighbors(curTile);
                foreach (var tile in atkTiles)
                    tileHash.Add(tile.tileId);
            }
            var pawnList = BLogic.Inst.GetPawnsOnTiles(tileHash);
            List<Pawn> foeList = pawnList.FindAll(p => { return PawnCampTool.CampsHostile(this.camp, p.camp); });
            atkPawns = foeList.Count;

            var neighbours = BLogic.Inst.GetAdjacentPawns(this);
            foreach (var pawn in neighbours)
            {
                if (pawn.camp == this.camp && pawn.sequenceId != this.sequenceId)
                    tradablePawns++;
            }
        }

        public void ActionWait()
        {
            BLogic.Inst.RefreshDataOnPawnMoved(this, this.savePos, this.curPosition);
            this.savePos = this.curPosition;
            this.EndAction();
            BLogic.Inst.OnPawnEndAction(this);
            TBSMapService.Inst.UnspawnAllCoverPlanes();
            //recalculate movable tile datas
            //CalculateMoveArea();
            //CalculateRangeArea();
            moveTileIds = null;
            rangeTileDic = null;
            destTileIds = null;
            BLogic.Inst.CheckPhaseSwitch();
        }


        #region AI Logic


        public void DoAutoAction()
        {
            //test move randomly
            int tileId = GetRandomDestTile();
            InstantMove(tileId);
            this.EndAction();
        }

        int GetRandomDestTile()
        {
            var destList = GetDestTileList();
            return RandUtil.PickRandValue(destList);
        }
        void InstantMove(int tileId)
        {
            var map = TBSMapService.Inst.map;
            var tile = map.GetTileFromDic(tileId);
            if (tile != null)
            {
                this.curPosition = tile.Position;
                BLogic.Inst.RefreshDataOnPawnMoved(this, this.savePos, this.curPosition);
                this.savePos = this.curPosition;
                BLogic.Inst.OnPawnEndAction(this);
                moveTileIds = null;
                rangeTileDic = null;
                destTileIds = null;
                this.EndAction();
                CalculateCombatAttr();
            }
        }
        #endregion

        #region Attribute
        /// <summary>
        /// character's attr (grow with level up)
        /// </summary>
        public BasicAttribute charAttr;
        /// <summary>
        /// pawn's summed attr,capped by attrCapTotal
        /// </summary>
        BasicAttribute attrCache;
        /// <summary>
        /// attr cap,based on character and class
        /// </summary>
        BasicAttribute attrCapTotal;
        /// <summary>
        /// attr should not be lower than floor attr
        /// </summary>
        BasicAttribute attrFloor;

        public BasicAttribute GetAttribute()
        {
            //todo calculate attrs: buff,skill
            //calculate attr cap
            if (attrCapTotal == null)
            {
                attrCapTotal = new BasicAttribute();
                attrCapTotal.AddConfigAttr(this.classCfg.Cap);
                attrCapTotal.AddConfigAttr(this.charCfg.CapFix);
            }

            if (attrFloor == null)
            {
                attrFloor = new BasicAttribute();
                attrFloor.AddConfigAttr(this.classCfg.BaseAttr);
                attrFloor.Mov = classCfg.Movement;
                attrCapTotal.Mov = classCfg.Movement;
            }
            //char attr
            attrCache = new BasicAttribute(this.charAttr);
            attrCache.ApplyAttrFloor(attrFloor);
            attrCache.ApplyAttrCap(attrCapTotal);

            return this.attrCache;
        }
        CombatAttribute combatAttr;
        /// <summary>
        /// current position map tile attr
        /// </summary>
        CombatAttribute tileAttr;
        public CombatAttribute GetCombatAttr()
        {
            if (combatAttr == null)
            {
                CalculateCombatAttr();
            }
            return combatAttr;
        }
        void CalculateTileEffectAttr()
        {
            bool isFoe = PawnCampTool.IsFoe(this.camp);
            tileAttr = TBSMapService.Inst.GetTileEffectAttr(GetTileEffect(), isFoe);
        }
        public void CalculateCombatAttr()
        {
            CalculateTileEffectAttr();
            combatAttr = new CombatAttribute();
            CalculateAtk();
            CalculateAtkSpd();
            CalculateHit();
            CalculateAvo();
            CalculateCrit();
            CalculateDodge();
            CalculateStaffHit();
            CalculateStaffAvo();
            CalculateDefence();
            CalculateResistance();
        }
        public void CalculateCombatAttr(Pawn foe)
        {
            CalculateCombatAttr();
            var foeAttr = foe.GetCombatAttr();
            combatAttr.DisplayedCrit -= foeAttr.Dodge;
            combatAttr.DisplayedHit -= foeAttr.Avoid;
            combatAttr.DisplayedStaffHit -= foeAttr.StaffAvo;
            if (HoldingWeapon())
            {
                if (IsPhDmg())
                {
                    combatAttr.DisplayedDamage = combatAttr.MagAtk - foeAttr.Resistance;
                }
                else if (IsMagDmg())
                {
                    combatAttr.DisplayedDamage = combatAttr.PhAtk - foeAttr.Defence;
                }
                else if (IsPnDmg())
                {
                    combatAttr.DisplayedDamage = combatAttr.PnAtk;
                }
            }
            combatAttr.ReviseDisplayValues();
        }
        void CalculateAtk()
        {
            if (this.HoldingWeapon() == false)
            {
                combatAttr.MagAtk = 0;
                combatAttr.PhAtk = 0;
                combatAttr.DisplayedDamage = 0;
                return;
            }
            else
            {
                int weaponMt = equippedWeapon.itemCfg.Might;
                int atk = weaponMt;
                var dmgType = equippedWeapon.itemCfg.DmgType;
                combatAttr.PnAtk = 0;
                combatAttr.PhAtk = 0;
                combatAttr.MagAtk = 0;

                if (dmgType == DamageType.MAG)
                {
                    atk += attrCache.Mag;
                    combatAttr.MagAtk = atk;
                }
                else if (dmgType == DamageType.PH)
                {
                    atk += attrCache.Str;
                    combatAttr.PhAtk = atk;
                }
                else if (dmgType == DamageType.PN)
                {
                    combatAttr.PnAtk = atk;
                }
            }
        }

        void CalculateAtkSpd()
        {
            int spdBurden = 0;
            if (HoldingWeapon())
            {
                if (equippedWeapon.itemCfg.Weight > attrCache.Bld)
                {
                    spdBurden = equippedWeapon.itemCfg.Weight - attrCache.Bld;
                }
            }
            int atkspd = attrCache.Spd - spdBurden;
            combatAttr.AttackSpeed = atkspd;
        }

        void CalculateHit()
        {
            if (HoldingWeapon())
            {
                int weaponHit = equippedWeapon.itemCfg.Hit;
                int hit = weaponHit + attrCache.Dex * 2 + attrCache.Luk / 2;
                combatAttr.DisplayedHit = hit;
                combatAttr.Hit = hit;
            }
            else
            {
                combatAttr.Hit = 0;
                combatAttr.DisplayedHit = 0;
            }
        }
        void CalculateAvo()
        {
            int avo = 0;
            int avo_weapon = 0;
            if (HoldingWeapon())
            {
                avo_weapon = equippedWeapon.itemCfg.Avoid;
            }
            avo += avo_weapon + combatAttr.AttackSpeed * 2 + attrCache.Luk / 2;
            //tile effect
            avo += tileAttr.Avoid;
            combatAttr.Avoid = avo;
        }
        void CalculateCrit()
        {
            if (HoldingWeapon())
            {
                int weaponCrit = equippedWeapon.itemCfg.Critical;
                int crit = weaponCrit + attrCache.Dex / 2;
                combatAttr.CriticalRate = crit;
                combatAttr.DisplayedCrit = crit;
            }
            else
            {
                combatAttr.CriticalRate = 0;
                combatAttr.DisplayedCrit = 0;
            }
        }
        void CalculateDodge()
        {
            int dodge = 0;
            int weaponDdg = 0;
            if (HoldingWeapon())
            {
                weaponDdg = equippedWeapon.itemCfg.Dodge;
            }
            dodge += weaponDdg;
            dodge += attrCache.Luk;
            combatAttr.Dodge = dodge;
        }
        void CalculateStaffHit()
        {
            if (HoldingStaff())
            {
                int staffHit = equippedWeapon.itemCfg.Hit + attrCache.Mag + attrCache.Dex;
                combatAttr.StaffHit = staffHit;
            }
            else
            {
                combatAttr.StaffHit = 0;
            }
        }
        void CalculateStaffAvo()
        {
            int staffAvo = (attrCache.Res * 3 + attrCache.Luk) / 2;
            combatAttr.StaffAvo = staffAvo;
        }
        void CalculateDefence()
        {
            int def = attrCache.Def;
            def += tileAttr.Defence;
            combatAttr.Defence = def;
        }
        void CalculateResistance()
        {
            int res = attrCache.Res;
            res += tileAttr.Resistance;
            combatAttr.Resistance = res;
        }
        /// <summary>
        /// attack value based on weapon damage type
        /// </summary>
        /// <returns></returns>
        public int GetAttackValue()
        {
            int atkValue = 0;
            if (this.HoldingWeapon())
            {
                switch (GetDamageType())
                {
                    case DamageType.PH: atkValue = combatAttr.PhAtk; break;
                    case DamageType.MAG: atkValue = combatAttr.MagAtk; break;
                    case DamageType.PN: atkValue = combatAttr.PnAtk; break;
                }
            }
            return atkValue;
        }
        #endregion

        #region Item and Weapon Inventory
        public List<Item> itemList = new List<Item>();
        /// <summary>
        /// weapon/staff
        /// </summary>
        public Weapon equippedWeapon = null;
        public void InsertItem(Item item)
        {
            itemList.Add(item);
            if (item is Weapon && equippedWeapon == null)
            {
                var wep = item as Weapon;
                if (CanEquipWeapon(wep))
                    EquipWeapon(wep);
            }
        }
        public void RemoveItem(int sid)
        {
            if (equippedWeapon?.sid == sid)
            {
                equippedWeapon = null;
                if (this.controller != null)
                    this.controller.UpdateWeaponGauge();
            }
            itemList.RemoveAll(item => item.sid == sid);
        }
        public bool CanEquipWeapon(Weapon weapon)
        {
            //todo
            return true;
        }
        public void EquipWeapon(Weapon weapon)
        {
            Debugger.Log("EquipWeapon " + weapon.ToString());

            this.equippedWeapon = weapon;
            if (this.controller != null)
                this.controller.UpdateWeaponGauge();
        }
        public void EquipWeapon(int sid)
        {
            var wep = GetWeapon(sid);
            if (wep != null)
            {
                EquipWeapon(wep);
            }
        }
        public Weapon GetWeapon(int sid)
        {
            foreach (var item in itemList)
            {
                if (item.sid == sid)
                {
                    return item as Weapon;
                }
            }
            return null;
        }
        public DamageType GetDamageType()
        {
            if (!HoldingWeapon())
                return DamageType.NONE;
            return equippedWeapon.itemCfg.DmgType;
        }
        public bool IsPhDmg()
        {
            return GetDamageType() == DamageType.PH;
        }
        public bool IsMagDmg()
        {
            return GetDamageType() == DamageType.MAG;
        }
        public bool IsPnDmg()
        {
            return GetDamageType() == DamageType.PN;
        }

        /// <summary>
        /// get weapon count that has attack target, standing on cur tile
        /// </summary>
        /// <returns></returns>
        public List<Weapon> GetPossibleWeaponCount()
        {
            List<Weapon> wepList = new List<Weapon>();
            //find all foes inside this max range
            foreach (var item in itemList)
            {
                if (item is Weapon)
                {
                    var wep = item as Weapon;
                    var foeList = GetFoesInWeaponRange(wep);
                    if (foeList.Count > 0)
                    {
                        wepList.Add(wep);
                    }
                }
            }
            return wepList;
        }
        public HashSet<int> GetPossibleAttackTile()
        {
            HashSet<int> idHash = new HashSet<int>();
            foreach (var item in itemList)
            {
                if (item is Weapon)
                {
                    var wep = item as Weapon;
                    var hash = GetTileIdInWeaponRange(wep);
                    foreach (int id in hash)
                        idHash.Add(id);
                }
            }
            return idHash;
        }
        /// <summary>
        /// tile ids in this weapon's attack range,in current tile
        /// </summary>
        /// <param name="wep"></param>
        /// <returns></returns>
        public HashSet<int> GetTileIdInWeaponRange(Weapon wep)
        {
            HashSet<int> tileIdHash = new HashSet<int>();

            List<int> range = wep.itemCfg.Range;
            int rangeMin = 0;
            int rangeMax = 0;
            if (range.Count == 1)
            {
                rangeMin = range[0];
                rangeMax = range[0];
            }
            else if (range.Count == 2)
            {
                rangeMin = range[0];
                rangeMax = range[1];
            }
            var map = TBSMapService.Inst.map;

            var centerTile = map.GetTileFromDic(TilePosId());

            for (int m = -rangeMax; m <= rangeMax; m++)
            {
                int rangeN = rangeMax - Mathf.Abs(m);
                for (int n = -rangeN; n <= rangeN; n++) //m,n loop the diamond shape around centerPos
                {
                    int tileRange = Mathf.Abs(m) + Mathf.Abs(n);
                    if (rangeMin > 0 && tileRange < rangeMin)//consider min range
                        continue;

                    // m,n is the pos
                    int targetTileId = map.XY2TileId(centerTile.Position.x + m, centerTile.Position.y + n);
                    if (!tileIdHash.Contains(targetTileId))
                    {
                        if (map.GetTileFromDic(targetTileId) != null)
                            tileIdHash.Add(targetTileId);
                    }
                }
            }

            return tileIdHash;
        }
        /// <summary>
        /// get attackable foes with this weapon
        /// </summary>
        /// <param name="wep"></param>
        /// <returns></returns>
        public List<Pawn> GetFoesInWeaponRange(Weapon wep)
        {
            List<Pawn> foeList = new List<Pawn>();
            if (CanEquipWeapon(wep))
            {
                HashSet<int> tileHash = GetTileIdInWeaponRange(wep);
                var pawnList = BLogic.Inst.GetPawnsOnTiles(tileHash);
                foreach (var pawn in pawnList)
                {
                    if (IsMyFoe(pawn))
                    {
                        foeList.Add(pawn);
                    }
                }
            }
            return foeList;
        }
        #endregion

        public bool IsMyFoe(Pawn other)
        {
            return PawnCampTool.CampsHostile(this.camp, other.camp);
        }

        #region HP manage
        private int _hp;
        public int HP
        {
            get { return _hp; }
            set { _hp = value; }
        }
        /// <summary>
        /// when hp decreased to 0,cost a life gem and fill HP
        /// </summary>
        public int LifeGem = 0;
        #endregion

        #region combat strike effect
        public bool IsTriggerVantage()
        {
            return false;
        }
        public bool IsTriggerSmash()
        {
            return false;
        }
        public bool IsTriggerAlacrity()
        {
            return false;
        }
        public bool IsBraveWeapon()
        {
            return false;
        }
        #endregion


        #region skills
        List<ActiveSkill> skillList;
        ActiveSkill normalAtkSkill;
        public void InitSkills()
        {
            skillList = new List<ActiveSkill>();
            normalAtkSkill = new ActiveSkill(101, this);//normal attack : default added
            skillList.Add(normalAtkSkill);
        }
        public int CalculateDamage(Pawn target, ActiveSkill skill)
        {
            float damage = 0;
            int weaponMt = equippedWeapon.itemCfg.Might;
            float atkMul = skill.skillCfg.AttackMultiplier;
            int atk = weaponMt;
            int def = 0;
            var dmgType = GetDamageType();
            var targetAttr = target.GetCombatAttr();
            if (dmgType == DamageType.PH)
            {
                atk += combatAttr.PhAtk;
                def = targetAttr.Defence;
            }
            else if (dmgType == DamageType.MAG)
            {
                atk += combatAttr.MagAtk;
                def = targetAttr.Resistance;
            }
            else if (dmgType == DamageType.PN)
            {

            }
            damage = atk * atkMul - def;
            //weaponMt * atkMul;
            return (int)damage;
        }
        #endregion

        #region Normal Attack
        public void StartNormalAttack(Pawn target)
        {
            var strikeList = BLogic.Inst.ArrangeStrikeList(this, target);
            foreach (StrikeInfo strike in strikeList)
            {
                if (strike.id == 0)
                    ProcessNormalAttack(target, strike);
                else if (strike.id == 1)
                    target.ProcessNormalAttack(this, strike);
            }
            this.OnCombatEnd();
            target.OnCombatEnd();
        }
        public void ProcessNormalAttack(Pawn target, StrikeInfo sInfo)
        {
            if (normalAtkSkill != null)
            {
                normalAtkSkill.StartCast(target, sInfo);
            }
        }
        public void OnCombatEnd()
        {
            combat_interrupt = false;
        }

        /// <summary>
        /// interrupt combat if true
        /// </summary>
        public bool combat_interrupt = false;
        public void TakeDamage(int dmgValue)
        {
            this.HP -= dmgValue;
            CheckHPValue();
        }
        void CheckHPValue()
        {
            if (this.HP >= this.attrCache.HPMax)
            {
                this.HP = this.attrCache.HPMax;
            }
            if (this.HP <= 0)
            {
                if (LifeGem > 0)
                {
                    LifeGem--;
                    this.HP = this.attrCache.HPMax;
                }
                else //mark to death
                {
                    this.deathMark = true;
                }
                combat_interrupt = true;
            }
        }
        /// <summary>
        /// when new turn starts
        /// </summary>
        public void OnBattleTurnAdd()
        {
            //release Break debuff
        }
        public void UpdateHP()
        {
            if (this.controller != null)
                this.controller.UpdateHPGauge();
        }
        public void ProcessDeath()
        {
            this.controller.PlayDeathAnim();
            //on death do something;

        }
        public void ProcessDeath2()
        {
            this.Restore();
        }
        void Restore()
        {
            BLogic.Inst.RemovePawn(this);
            this.controller.SelfDestroy();
            this.charCfg = null;
            this.classCfg = null;
            this.targetPawn = null;
            this.targetPawnList = null;
            this.curState = PawnState.Default;
            this.sequenceId = 0;
        }
    }

    #endregion
}

