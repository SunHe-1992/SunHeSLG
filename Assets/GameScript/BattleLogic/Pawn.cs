using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YooAsset;
using cfg.SLG;
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
        public BasicAttribute charAttr;
        public cfg.SLG.CharacterData charCfg;
        public cfg.SLG.ClassData classCfg;
        #endregion

        public void Init()
        {
            globalSequence++;
            sequenceId = globalSequence;

            curState = PawnState.Idle;
            this.LoadModel();
            this.savePos = this.curPosition;

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
        }
        public override string ToString()
        {
            return $"Pawn camp={camp} pos={curPosition} posId={TilePosId()} model={modelName}";
        }

        public void LoadModel()
        {
            if (this.controller == null)
            {
                string resPath = $"Pawns/{modelName}";
                AssetOperationHandle handler = YooAssets.LoadAssetAsync<GameObject>(resPath);
                handler.Completed += ModelLoadDone;

            }
        }
        void ModelLoadDone(AssetOperationHandle handle)
        {
            if (handle.AssetObject != null)
            {
                var obj = GameObject.Instantiate(handle.AssetObject as GameObject);
                obj.name = GetObjName();
                this.controller = obj.AddComponent<PawnController>();
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
                return attrCache.Mov;
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
            //todo check skill
            return false;
        }
        public int GetAtkRangeMax()
        {
            //check this pawn has attack ability 
            if (IsArmed())
            {
                return equippedWeapon.rangeMax;
            }
            else return 0;
        }
        public int GetAtkRangeMin()
        {
            if (IsArmed())
            {
                return equippedWeapon.rangeMin;
            }
            else return 0;
        }
        public bool IsFlier()
        {
            return this.moveType == PawnMoveType.Flier;
        }
        /// <summary>
        /// equipped with a weapon/staff
        /// </summary>
        /// <returns></returns>
        public bool IsArmed()
        {
            return this.equippedWeapon != null;
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
        Dictionary<int, HashSet<int>> rangeTileDic;
        public void CalculateRangeArea()
        {
            var map = TBSMapService.Inst.map;

            rangeTileDic = new Dictionary<int, HashSet<int>>();
            int rangeMax = this.GetAtkRangeMax();
            int rangeMin = this.GetAtkRangeMin();
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
            List<TileEntity> atkTiles = GetInRangePosOneTile(this.GetAtkRangeMin(), this.GetAtkRangeMax(), curPosition);
            HashSet<int> tileHash = new HashSet<int>();
            foreach (var tile in atkTiles)
            {
                tileHash.Add(tile.tileId);
            }
            var pawnList = BLogic.Inst.GetPawnsOnTiles(tileHash);
            List<Pawn> foeList = pawnList.FindAll(p => { return PawnCampTool.CampsHostile(this.camp, p.camp); });
            atkPawns = foeList.Count;

            var neighbours = BLogic.Inst.GetAdjacentPawns(this);
            foreach (var pawn in neighbours)
            {
                if (pawn.camp == this.camp)
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
            }
        }
        #endregion

        #region Attribute
        /// <summary>
        /// self attr included: character basic attr/cap fix,class basic attr/capfix,
        /// </summary>
        BasicAttribute attrCache;
        BasicAttribute attrCapTotal;
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
        public CombatAttribute GetCombatAttr()
        {
            if (combatAttr == null)
            {
                combatAttr = new CombatAttribute();
                CalculateCombatAttr();
            }
            return combatAttr;
        }
        public void CalculateCombatAttr()
        {
            GetCombatAttr();
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
            if (IsArmed())
            {
                if (IsMagicAtk())
                {
                    combatAttr.DisplayedDamage = combatAttr.MagAtk - foeAttr.Resistance;
                }
                else
                {
                    combatAttr.DisplayedDamage = combatAttr.PhAtk - foeAttr.Defence;
                }
            }
            combatAttr.ReviseDisplayValues();
        }
        void CalculateAtk()
        {
            if (this.IsArmed() == false)
            {
                combatAttr.MagAtk = 0;
                combatAttr.PhAtk = 0;
                combatAttr.DisplayedDamage = 0;
                return;
            }
            else
            {
                var weaponCfg = equippedWeapon.itemCfg;
                int weaponMt = weaponCfg.Might;
                int atk = weaponMt;
                if (weaponCfg.Magical)
                {
                    atk += attrCache.Mag;
                    combatAttr.MagAtk = atk;
                    combatAttr.PhAtk = 0;
                }
                else
                {
                    atk += attrCache.Str;
                    combatAttr.PhAtk = atk;
                    combatAttr.MagAtk = 0;
                }
            }
        }

        void CalculateAtkSpd()
        {
            int spdBurden = 0;
            if (IsArmed())
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
            if (IsArmed())
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
            if (IsArmed())
            {
                avo_weapon = equippedWeapon.itemCfg.Avoid;
            }
            avo += avo_weapon + combatAttr.AttackSpeed * 2 + attrCache.Luk / 2;
            //todo  terrian effect
            combatAttr.Avoid = avo;
        }
        void CalculateCrit()
        {
            if (IsArmed())
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
            if (IsArmed())
            {
                weaponDdg = equippedWeapon.itemCfg.Dodge;
            }
            dodge += weaponDdg;
            dodge += attrCache.Luk;
            combatAttr.Dodge = dodge;
        }
        void CalculateStaffHit()
        {
            if (IsArmed() && equippedWeapon.itemCfg.ItemType == ItemType.Staff)
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
            combatAttr.Defence = def;
        }
        void CalculateResistance()
        {
            int res = attrCache.Res;
            combatAttr.Resistance = res;
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
                    EquipAWeapon(wep);
            }
        }
        public void RemoveItem(int sid)
        {
            if (equippedWeapon?.sid == sid)
            {
                equippedWeapon = null;
            }
            itemList.RemoveAll(item => item.sid == sid);
        }
        public bool CanEquipWeapon(Weapon weapon)
        {
            //todo
            return true;
        }
        public void EquipAWeapon(Weapon weapon)
        {
            this.equippedWeapon = weapon;
        }
        public bool IsMagicAtk()
        {
            if (!IsArmed())
                return false;
            if (equippedWeapon.itemCfg.ItemType == ItemType.Tome)
                return true;
            return equippedWeapon.itemCfg.Magical;
        }
        #endregion
    }
}
