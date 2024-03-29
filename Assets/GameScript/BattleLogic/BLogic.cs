using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Singleton;
using UniFramework.Event;
using System.Linq;

namespace SunHeTBS
{
    public enum GamePlayState : int
    {
        Default = 0,
        /// <summary>
        /// pawn swap space
        /// </summary>
        BeforeBattle = 1,
        MovieTime = 2,
        DialoguePlay = 3,
        ActionPlay = 4,
        PhaseStart = 5,
        PhaseEnding = 6,
        /// <summary>
        /// player controll pawns
        /// </summary>
        PlayerControl = 7,

        /// <summary>
        /// waiting
        /// </summary>
        PawnMoving = 8,
        /// <summary>
        /// combat HUD, and pawn attack anim
        /// </summary>
        CombatPlay = 9,


    }
    public enum PlayerControlState
    {
        Default,
        /// <summary>
        /// cursor moves freely
        /// </summary>
        TileSelect,
        /// <summary>
        /// click confirm ,go to cursor pos 
        /// </summary>
        PawnSelected,
        PawnMoving,
        /* UI stats : for UI and map switch*/
        UIControl,
    }
    public enum PerformMode
    {
        SkipPerform,
        AutoPerform,
        PlayerCommand,
    }
    /// <summary>
    /// Battle Logic
    /// </summary>
    public class BLogic : ISingleton
    {

        //todo read data , deploy battle field
        public static PerformMode performMode = PerformMode.PlayerCommand;
        public static bool skipNPCPhase = true;
        float FixedLogicTime = 0;
        public bool Running;
        /// <summary>
        /// key = tile id, value = pawn ref
        /// </summary>
        public Dictionary<int, Pawn> mapPawnDic = new Dictionary<int, Pawn>();

        #region battle machine state manage

        /// <summary>
        /// current battle machine state
        /// </summary>
        GamePlayState gameState = GamePlayState.Default;
        /// <summary>
        /// change to target state
        /// </summary>
        GamePlayState nextGameState = GamePlayState.Default;
        /// <summary>
        /// Player control state
        /// </summary>
        public PlayerControlState pCtrlState = PlayerControlState.Default;
        void SetPlayerCtrlState(PlayerControlState state)
        {
            pCtrlState = state;
            Debugger.Log($"SetPlayerCtrlState {state}");
        }

        public GamePlayState GetGamePlayState()
        {
            return gameState;
        }
        public void SetNextGamePlayState(GamePlayState gps)
        {
            nextGameState = gps;
        }
        bool SwitchBattleState()
        {
            //state changes
            if (nextGameState != GamePlayState.Default && gameState != nextGameState)
            {
                Debug.Log($"game play state {gameState}=>{nextGameState}");
                gameState = nextGameState;
                switch (nextGameState)
                {
                    case GamePlayState.BeforeBattle: OnEnterBeforeBattle(); break;
                    case GamePlayState.MovieTime: OnEnterMovieTime(); break;
                    case GamePlayState.ActionPlay: break;
                    case GamePlayState.DialoguePlay: break;
                    case GamePlayState.PhaseStart: OnEnterPhaseStart(); break;
                    case GamePlayState.PhaseEnding: EndCurrentPhase(); break;
                    case GamePlayState.PlayerControl: OnEnterPlayerControl(); break;
                    case GamePlayState.PawnMoving: break;
                    case GamePlayState.CombatPlay: break;
                }
                return true;
            }
            //no change
            return false;
        }
        public void OnLogicUpdate(float dt)
        {
            FixedLogicTime += dt;
            LogicStateUpdate(dt);
        }

        void LogicStateUpdate(float dt)
        {
            if (!this.Running)
                return;

            if (SwitchBattleState())
            {
                return;
            }
            switch (gameState)
            {
                case GamePlayState.PawnMoving: PawnMovingUpdate(dt); break;
            }
        }
        #endregion

        #region BattleState: before battle

        void OnEnterBeforeBattle()
        {
            FUIManager.Inst.ShowUI<UIPage_BattlePrepare>(FUIDef.FWindow.BattlePrepare);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void StartPlayerPhase()
        {
            //nextGameState =  GamePlayState.;

        }

        #region Pawn manage

        public List<Pawn> pawnList = new List<Pawn>();

        public void AddPawn(Pawn p)
        {
            if (pawnList == null)
                pawnList = new List<Pawn>();

            pawnList.Add(p);
            int tileId = p.TilePosId();

            if (!mapPawnDic.ContainsKey(tileId))
            {
                mapPawnDic[tileId] = p;
                var tileEntity = TBSMapService.Inst.map.Tile(p.curPosition);
                tileEntity.camp = p.camp;
            }
            else
            {
                Debugger.LogError($"map tile {p.ToString()} already contains pawn!");
            }
        }
        public void AddTestPawn(PawnCamp camp, Vector3Int pos, PawnMoveType moveType, List<int> weaponList)
        {
            Pawn p = new Pawn();
            p.camp = camp;
            p.CharacterId = 1000;
            p.ClassId = 1001;
            p.curPosition = pos;
            p.modelName = "M_AA_001";
            p.moveType = moveType;
            p.Init();
            AddPawn(p);
            foreach (int wpId in weaponList)
            {
                p.InsertItem(new Weapon(wpId));
            }
            p.CalculateCombatAttr();
        }
        public Pawn GetPawnBySid(int sid)
        {
            foreach (var pawn in pawnList)
            {
                if (pawn.sequenceId == sid) return pawn;
            }
            return null;
        }
        #endregion

        #region Select pawns
        public TargetSelector targetSelector;
        public List<Pawn> SelectFoes(int rangeMin, int rangeMax)
        {
            var list = targetSelector.SelectTargetFoes(this.pawnList, selectedPawn.camp, selectedPawn.curPosition, rangeMin, rangeMax);
            return list;
        }
        #endregion

        public void ResetBattleState()
        {
            pawnList.Clear();
            mapPawnDic.Clear();
        }

        #region create map pawns 
        //
        public void SetMapThing()
        {
            //
            // SetMonsterInfo();
        }
        public void PostInitProcess()
        {
            //test
            AddTestPawn(PawnCamp.Player, new Vector3Int(2, 2), PawnMoveType.Ground, new List<int>() { 1002, 1007 });
            //AddTestPawn(PawnCamp.Player, new Vector3Int(4, 5), PawnMoveType.Ground);
            AddTestPawn(PawnCamp.Villain, new Vector3Int(4, 2), PawnMoveType.Ground, new List<int>() { 1002 });
            AddTestPawn(PawnCamp.Villain, new Vector3Int(4, 1), PawnMoveType.Ground, new List<int>() { 1002 });


            InitPlayerPosition();

            SetBattleCameraBoundary();
        }
        void InitPlayerPosition()
        {
            //todo

        }
        void SetBattleCameraBoundary()
        {
            //todo
        }
        #endregion
        #region Cursor Path Management

        /// <summary>
        /// current cursor pos,controlled by player
        /// </summary>
        public Vector3Int cursorPos;
        /// <summary>
        /// cursor path stack,current cursor pos is not included
        /// </summary>
        public Stack<Vector3Int> cursorStack = new Stack<Vector3Int>();
        /// <summary>
        /// modify cursor obj color,check cursor stack
        /// </summary>
        /// <param name="newPos"></param>
        void OnCursorPosChange(Vector3Int oldPos, Vector3Int newPos)
        {
            #region set cursor obj color and arrow visible
            bool pawnSelected = pCtrlState == PlayerControlState.PawnSelected;

            //check if a pawn standing on this pos, if yes:return this function
            var cursorTile = TBSMapService.Inst.map.Tile(newPos);
            Pawn pointPawn = GetPawnOnTile(cursorTile);
            bool noPointAtPawn = pointPawn == null;

            bool cursorColorRed = false;
            bool showArrow = false;
            bool selectedPawnArmed = selectedPawn != null && selectedPawn.HoldingWeapon();
            bool cursorInWalkableTile = false;
            if (pawnSelected == false)//no pawn is selected
            {
                if (noPointAtPawn)
                {
                    showArrow = false;
                    cursorColorRed = false;
                }
                else
                {
                    showArrow = true;
                    cursorColorRed = false;
                }
            }
            else
            {
                if (noPointAtPawn)
                {
                    showArrow = false;
                    //1. check if new pos is inside walkable area
                    int posId = TBSMapService.Inst.GetTileId(newPos);
                    if (selectedPawn != null && selectedPawn.moveTileIds.Contains(posId))
                    {
                        cursorColorRed = false;
                        cursorInWalkableTile = true;
                    }
                    else
                        cursorColorRed = true;
                }
                else
                {
                    if (pointPawn.sequenceId == selectedPawn.sequenceId)//cursor at self
                    {
                        showArrow = true;
                        cursorColorRed = false;
                    }
                    else
                    {
                        bool targetInMyRange = selectedPawn.CheckTileInRange(newPos);
                        if (PawnCampTool.IsFoe(pointPawn.camp))
                        {
                            if (selectedPawnArmed && targetInMyRange)
                            {
                                showArrow = true;
                                cursorColorRed = false;
                            }
                            else
                            {
                                showArrow = true;
                                cursorColorRed = true;
                            }
                        }
                        else //point at ally
                        {
                            showArrow = true;
                            cursorColorRed = true;
                        }
                    }
                }
            }
            BattleDriver.Inst.CursorShowArrow(showArrow);
            if (cursorColorRed)
                BattleDriver.Inst.SetCursorRed();
            else
                BattleDriver.Inst.SetCursorWhite();
            #endregion

            #region logic check path cost and path finding
            //2. check if new pos appended to cursorStack, move point is enough
            //if yes , only push pos to stack
            //if no , do pathfinding again and save result to stack
            if (selectedPawn != null)
            {
                bool backToOldPos = false;
                int count = 0;
                foreach (var pos in cursorStack)
                {
                    count++;
                    if (newPos.Equals(pos))
                    {
                        backToOldPos = true;
                        break;
                    }
                }
                if (backToOldPos)
                    for (int i = 0; i < count; i++)
                    {
                        cursorStack.Pop();
                    }
                if (false == backToOldPos)//cursor moves back
                {
                    if (cursorInWalkableTile)//if cursor is outside walkable tiles,ignore this
                    {
                        cursorStack.Push(newPos);
                        int pathCost = CheckCursorStackMovable();
                        Debugger.Log($"cost={pathCost} ");
                        if (pathCost <= selectedPawn.GetMovement())//accept this path
                        {
                        }
                        else//redo path finding
                        {
                            cursorStack.Clear();
                            TileEntity startTile = TBSMapService.Inst.map.GetTileFromDic(selectedPawn.TilePosId());
                            TileEntity toTile = TBSMapService.Inst.map.Tile(cursorPos);
                            List<INode> nodeList = NodePathFinder.Path(TBSMapService.Inst.map, startTile, toTile, selectedPawn.IsExtraMoveCost(), selectedPawn.IsPassFoe(), selectedPawn.IsFlier());
                            if (nodeList.Count > 0)
                                for (int i = 0; i < nodeList.Count; i++)//save nodes to stack
                                {
                                    INode node = nodeList[i];
                                    cursorStack.Push(node.Position);
                                }
                        }
                    }
                }
            }

            Debugger.Log("cursor stack count  = " + cursorStack.Count);
            #endregion
        }
        int CheckCursorStackMovable()
        {
            List<Vector3Int> posList = new List<Vector3Int>(cursorStack);
            List<INode> nodes = new List<INode>();
            foreach (Vector3Int pos in posList)
            {
                var tile = TBSMapService.Inst.map.Tile(pos);
                nodes.Add(tile);
            }
            bool extraMoveCost = selectedPawn.IsExtraMoveCost();
            return NodePathFinder.EstimatePathCost(nodes, TBSMapService.Inst.map, extraMoveCost);
        }
        bool PosInWalkableTile(Vector3Int pos)
        {
            if (selectedPawn == null || selectedPawn.moveTileIds == null)
                return false;
            int posId = TBSMapService.Inst.GetTileId(pos);
            return selectedPawn.moveTileIds.Contains(posId);
        }
        /// <summary>
        /// when cancel pawn destination select
        /// </summary>
        void ClearCursorStack()
        {
            cursorStack.Clear();
            TBSMapService.Inst.UnspawnAllPathHint();
        }
        #endregion
        #region cursor select pawn

        /// <summary>
        /// pawn under cursor
        /// </summary>
        public Pawn pointedPawn;
        /// <summary>
        /// players pawn selected ,waiting for order
        /// </summary>
        public Pawn selectedPawn;

        public BLogic()
        {
        }




        public static BLogic Inst { get; private set; }
        public static void Init()
        {
            Inst = UniSingleton.CreateSingleton<BLogic>();
        }
        public void OnCreate(object createParam)
        {
            targetSelector = new TargetSelector();
        }

        public void OnUpdate()
        {
            OnLogicUpdate(Time.deltaTime);

        }

        public void OnDestroy()
        {
            Running = false;
            pointedPawn = null;
            ResetBattleState();
        }
        #endregion

        public void OnEnterMovieTime()
        {
            //no movie in demo ,show battle main page
            FUIManager.Inst.ShowUI<UIPage_BattleMain>(FUIDef.FWindow.BattlePanel);

            SetNextGamePlayState(GamePlayState.PhaseStart);
        }


        public void OnEnterPlayerControl()
        {
        }


        #region Pawn Move
        Pawn movingPawn = null;

        /// <summary>
        /// when select a pawn's destination, confirmed and check what to do on this tile
        /// </summary>
        private void PawnConfirmMove()
        {
            int cursorTileId = TBSMapService.Inst.GetTileId(cursorPos);
            if (pointedPawn == null)//select a tile that no one standing on it,pawn move to this tile
            {
                if (selectedPawn.moveTileIds.Contains(cursorTileId))
                {
                    PawnStartMove(selectedPawn, cursorTileId);
                }
            }
            else //select a pawn,if is a foe check attack ,if is a ally check heal
            {
                Debugger.Log($"PawnTempMove selected {pointedPawn}");
            }
            TBSMapService.Inst.UnspawnAllPathHint();
        }
        public void PawnStartMove(Pawn p, int tileId)
        {
            movingPawn = p;
            var mapInst = GetMapInst();
            var toTile = mapInst.GetTileFromDic(tileId);
            var startTile = mapInst.Tile(movingPawn.curPosition);
            var nodeList = NodePathFinder.Path(mapInst, startTile, toTile, movingPawn.IsExtraMoveCost(), movingPawn.IsPassFoe(), movingPawn.IsFlier());
            movingPawn.SetMovePath(nodeList);
            pawnMoveTime = movingPawn.moveTileTime * nodeList.Count;
            SetNextGamePlayState(GamePlayState.PawnMoving);
            movingPawn.StartMove();
        }
        float pawnMoveTime = 0;
        private void PawnMovingUpdate(float dt)
        {
            pawnMoveTime -= dt;
            if (pawnMoveTime < 0)
            {
                pawnMoveTime = 0;
                PawnMoveToTile();
            }
        }



        void PawnMoveToTile()
        {
            //movingPawn
            if (movingPawn != null)
            {
                Debugger.Log($" move end {movingPawn}");
                movingPawn.StopMove();
                if (movingPawn.InstantActionAfterMove)//todo act to target pawn : attack/heal/dance
                {
                    SetNextGamePlayState(GamePlayState.CombatPlay);
                }
                else
                {
                    //show pawn's atk planes on this tile
                    TBSMapService.Inst.ShowPawnCoverPlanesOneTile(movingPawn, movingPawn.curPosition);
                    ShowActionMenu();
                }
                movingPawn = null;
            }
        }
        void ShowActionMenu()
        {
            //show action menu
            SetPlayerCtrlState(PlayerControlState.UIControl);
            UniEvent.SendMessage(GameEventDefine.ShowActionMenu);
            SetNextGamePlayState(GamePlayState.PlayerControl);

        }
        /// <summary>
        /// click cancel on Action Menu
        /// </summary>
        public void PawnSetPostionBack()
        {
            if (selectedPawn == null) return;
            if (gameState != GamePlayState.PlayerControl) return;
            selectedPawn.curPosition = selectedPawn.savePos;
            selectedPawn.ResetPosition();
            SetPlayerCtrlState(PlayerControlState.TileSelect);
            CursorInputMoveTo(selectedPawn.curPosition);
            BattleDriver.Inst.MoveCursorObj();
            TBSMapService.Inst.ShowPawnCoverPlanes(selectedPawn);
            InputReceiver.SwitchInputToMap();
            selectedPawn = null;
            ClearCursorStack();
        }
        #endregion

        MapEntity GetMapInst()
        {
            return TBSMapService.Inst.map;
        }

        public List<Pawn> GetPawnsOnTiles(HashSet<int> tileHash)
        {
            List<Pawn> pawns = new List<Pawn>();
            foreach (var pawn in this.pawnList)
            {
                if (tileHash.Contains(pawn.TilePosId()))
                {
                    pawns.Add(pawn);
                }
            }
            return pawns;
        }
        public Pawn GetPawnOnTile(TileEntity tile)
        {
            if (mapPawnDic.ContainsKey(tile.tileId))
                return mapPawnDic[tile.tileId];
            return null;
        }
        public List<Pawn> GetAdjacentPawns(Pawn p)
        {
            List<Pawn> pList = new List<Pawn>();
            int tileId = p.TilePosId();
            var tile = TBSMapService.Inst.map.GetTileFromDic(tileId);

            var nodes = TBSMapService.Inst.map.Neighbors(tile).Where(neigh => neigh != null);
            foreach (var node in nodes)
            {
                var pawn = GetPawnOnTile(node as TileEntity);
                if (pawn != null)
                    pList.Add(pawn);
            }
            return pList;
        }

        #region Cursor Move and Selecting Pawn
        public void OnInputAxis(int xAdd, int yAdd)
        {
            switch (pCtrlState)
            {
                case PlayerControlState.TileSelect:
                    CursorInputMove(xAdd, yAdd);
                    RefreshCoverPlanes();
                    break;
                case PlayerControlState.PawnSelected:
                    CursorInputMove(xAdd, yAdd);
                    //arrows
                    break;
            }

        }
        void RefreshCoverPlanes()
        {
            if (pointedPawn != null)
                TBSMapService.Inst.ShowPawnCoverPlanes(pointedPawn);
            else
                TBSMapService.Inst.UnspawnPawnCoverPlanes(null);
        }
        /// <summary>
        /// input order try move cursor,
        /// </summary>
        /// <param name="xAdd"></param>
        /// <param name="yAdd"></param>
        public void CursorInputMove(int xAdd, int yAdd)
        {
            Vector3Int newPos = new Vector3Int(xAdd, yAdd) + cursorPos;
            newPos = TBSMapService.Inst.map.TrimPos_Border(newPos);
            if (newPos == cursorPos) return;
            ChangeCursorPos(newPos);
        }
        public void CursorInputMoveTo(Vector3Int pos)
        {
            var newPos = TBSMapService.Inst.map.TrimPos_Border(pos);
            if (newPos == cursorPos) return;
            ChangeCursorPos(newPos);
        }
        /// <summary>
        /// handle cursor pos changed
        /// </summary>
        /// <param name="newPos"></param>
        void ChangeCursorPos(Vector3Int newPos)
        {
            Vector3Int oldCursorPos = cursorPos;
            cursorPos = newPos;

            int tileId = TBSMapService.Inst.GetTileId(cursorPos);
            if (mapPawnDic.ContainsKey(tileId))//todo cursor points a pawn,show info and move area
            {
                pointedPawn = mapPawnDic[tileId];
            }
            else
            {
                pointedPawn = null;
            }
            if (gameState == GamePlayState.PlayerControl)
            {
                if (pCtrlState == PlayerControlState.TileSelect || pCtrlState == PlayerControlState.PawnSelected)
                {
                    OnCursorPosChange(oldCursorPos, newPos);
                    //refresh cursor hint objs
                    if (cursorStack.Count > 0)
                    {
                        List<Vector3Int> posList = new List<Vector3Int>(cursorStack);
                        if (PosInWalkableTile(cursorPos))
                            posList.Add(cursorPos);
                        TBSMapService.Inst.ReGeneratePathHintObj(posList);
                    }
                }
            }
            UniEvent.SendMessage(GameEventDefine.CursorPointToPawn);
            var cursorObj = BattleDriver.Inst.cursorCtrl;
            if (cursorObj != null)
            {
                BattleDriver.Inst.MoveCursorObj();
            }

        }
        public void CursorMoveToTargetPawn(Pawn targetPawn)
        {
            this.cursorPos = targetPawn.curPosition;
            BattleDriver.Inst.MoveCursorObj();
        }
        public void OnMouseClick(Vector3 pos)
        {
            //if (gameState == GamePlayState.PlayerControl)
            //{
            //    if (pCtrlState == PlayerControlState.TileSelect)
            //    {
            //        var clickTile = TBSMapService.Inst.map.Tile(pos);
            //        if (clickTile != null)
            //        {
            //            CursorInputMoveTo(clickTile.Position);//set pointedPawn
            //        }
            //    }
            //}
        }


        #endregion
        public void OnClickConfirm()
        {
            if (gameState != GamePlayState.PlayerControl)
            {
                return;
            }
            switch (pCtrlState)
            {
                case PlayerControlState.TileSelect:
                    {
                        if (pointedPawn != null)
                        {
                            if (pointedPawn.camp == PawnCamp.Player)//player's pawn
                            {
                                if (pointedPawn.actionEnd == false)//move player's pawn
                                {
                                    selectedPawn = pointedPawn;
                                    cursorStack.Clear();
                                    cursorStack.Push(cursorPos);
                                    SetPlayerCtrlState(PlayerControlState.PawnSelected);
                                }
                            }
                            else if (PawnCampTool.CampsHostile(PawnCamp.Player, pointedPawn.camp))
                            {
                                //todo toggle enemy's attack range
                            }
                        }
                    }
                    break;

                case PlayerControlState.PawnSelected:
                    {
                        if (selectedPawn != null)
                        {
                            if (cursorPos == selectedPawn.curPosition)
                            {
                                ShowActionMenu();
                            }
                            else
                            {
                                if (selectedPawn.camp == PawnCamp.Player)//player's pawn
                                {
                                    //goto pos
                                    PawnConfirmMove();
                                }
                            }
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// On click cancel button
        /// </summary>
        public void OnClickCancel()
        {
            if (gameState != GamePlayState.PlayerControl)
            {
                return;
            }
            switch (gameState)
            {
                case GamePlayState.PlayerControl:
                    PlayerControlCancel();
                    break;
            }
        }
        /// <summary>
        /// Cancel on Player control state
        /// </summary>
        void PlayerControlCancel()
        {
            switch (pCtrlState)
            {
                case PlayerControlState.PawnSelected: /* click cancel when selecting dest tile*/
                    selectedPawn.curPosition = selectedPawn.savePos;
                    CursorInputMoveTo(selectedPawn.curPosition);
                    SetPlayerCtrlState(PlayerControlState.TileSelect);
                    BattleDriver.Inst.MoveCursorObj();
                    TBSMapService.Inst.ShowPawnCoverPlanes(selectedPawn);
                    selectedPawn = null;
                    ClearCursorStack();
                    InputReceiver.SwitchInputToMap();
                    break;
            }
        }
        /// <summary>
        /// ui control back to map control
        /// </summary>
        public void UIControlToMap()
        {
            selectedPawn.curPosition = selectedPawn.savePos;
            selectedPawn.ResetPosition();
            BattleDriver.Inst.MoveCursorObj();
            TBSMapService.Inst.ShowPawnCoverPlanes(selectedPawn);
            InputReceiver.SwitchInputToMap();
        }
        public void BattleUIToMap()
        {
            InputReceiver.SwitchInputToMap();
            this.pCtrlState = PlayerControlState.TileSelect;
            this.gameState = GamePlayState.PlayerControl;
            this.CheckPhaseSwitch();
            //todo check 2nd move
            selectedPawn.ActionWait();
        }

        #region Phase and Turn switch
        public int BattleTurn = 0;
        public PawnCamp curCamp { get; private set; }

        public PawnCamp GetNextCamp()
        {
            Dictionary<PawnCamp, int> countDic = new Dictionary<PawnCamp, int>();
            countDic[PawnCamp.Default] = 0;
            countDic[PawnCamp.Player] = 0;
            countDic[PawnCamp.Villain] = 0;
            countDic[PawnCamp.PlayerAlly] = 0;
            countDic[PawnCamp.Neutral] = 0;

            foreach (var pawn in pawnList)
            {
                countDic[pawn.camp]++;
            }
            PawnCamp nextCamp = PawnCamp.Default;
            for (PawnCamp i = PawnCamp.Player; i <= PawnCamp.Neutral; i++)
            {
                if (i > curCamp && countDic[i] > 0)
                {
                    nextCamp = i;
                }
            }
            if (nextCamp == PawnCamp.Default)
                nextCamp = PawnCamp.Player;
            return nextCamp;
        }
        public void OnEnterPhaseStart()
        {
            Debugger.Log($"Start Phase");
            UniEvent.SendMessage(GameEventDefine.PhaseSwitch);
            OnEnterNewPhase();
            PhaseSwitchDone();
        }
        public void PhaseSwitchDone()
        {
            foreach (var pawn in pawnList)
            {
                if (pawn.camp == curCamp)
                    pawn.actionEnd = false;
            }
            if (curCamp == PawnCamp.Default)
                curCamp = PawnCamp.Player;

            if (curCamp == PawnCamp.Player)
            {
                performMode = PerformMode.PlayerCommand;
                SetPlayerCtrlState(PlayerControlState.TileSelect);
                InputReceiver.SwitchInputToMap();
                SetNextGamePlayState(GamePlayState.PlayerControl);
                OnBattleTurnAdd();
                ClearCursorStack();
                ChangeCursorPos(cursorPos);
                RefreshCoverPlanes();
            }
            else //todo AI pawn actions
            {
                if (skipNPCPhase)
                {
                    performMode = PerformMode.SkipPerform;
                    StartNPCCtrl();
                }
                else
                    performMode = PerformMode.AutoPerform;

            }
        }
        void OnBattleTurnAdd()
        {
            BattleTurn++;
            foreach (var pawn in this.pawnList)
            {
                pawn.OnBattleTurnAdd();
            }
        }
        public void CheckPhaseSwitch()
        {
            //when a pawn ends action,  if there is no actable pawns,
            int actableCount = GetActablePawnCount(curCamp);
            if (actableCount == 0)
            {
                //go to next phase
                SetNextGamePlayState(GamePlayState.PhaseEnding);
                RepositionAllPawns();
            }
            else if (curCamp == PawnCamp.Player)//set up control to map
            {
                SetPlayerCtrlState(PlayerControlState.TileSelect);
                InputReceiver.SwitchInputToMap();
                SetNextGamePlayState(GamePlayState.PlayerControl);
            }
        }
        void EndCurrentPhase()
        {
            var oldCamp = curCamp;
            curCamp = GetNextCamp();
            Debugger.Log($"End Phase :camp {oldCamp}=>{curCamp}");
            SetNextGamePlayState(GamePlayState.PhaseStart);
        }
        void OnEnterNewPhase()
        {
            UniEvent.SendMessage(GameEventDefine.PhaseSwitch);

        }
        #endregion

        /// <summary>
        /// reset data when a pawn changed position
        /// </summary>
        /// <param name="p"></param>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        public void RefreshDataOnPawnMoved(Pawn p, Vector3Int fromPos, Vector3Int toPos)
        {
            if (fromPos == toPos)
                return;
            int fromTileId = TBSMapService.Inst.GetTileId(fromPos);
            int toTileId = TBSMapService.Inst.GetTileId(toPos);
            //remake dic <tileId,Pawn>
            mapPawnDic[fromTileId] = null;
            mapPawnDic[toTileId] = p;
            //remake tileEntity.camp
            var tileDic = TBSMapService.Inst.map.GetTileDic();
            tileDic[fromTileId].camp = PawnCamp.Default;
            tileDic[toTileId].camp = p.camp;
        }

        public void OnPawnEndAction(Pawn p)
        {


        }
        int GetActablePawnCount(PawnCamp camp)
        {
            int count = 0;
            foreach (var pawn in pawnList)
            {
                if (pawn.camp == camp && pawn.actionEnd == false)
                    count++;
            }
            return count;
        }
        #region NPC pawn controls

        void StartNPCCtrl()
        {
            SetNextGamePlayState(GamePlayState.Default);
            SetPlayerCtrlState(PlayerControlState.Default);
            var idlePawn = GetIdlePawn(PawnCamp.Villain);
            if (idlePawn != null)
            {
                idlePawn.DoAutoAction();
                StartNPCCtrl();
            }
            else
            {
                CheckPhaseSwitch();
            }
        }
        Pawn GetIdlePawn(PawnCamp camp)
        {
            foreach (var pawn in pawnList)
            {
                if (pawn.actionEnd == false && pawn.camp == camp)
                {
                    return pawn;
                }
            }
            return null;
        }
        #endregion

        public void RepositionAllPawns()
        {
            foreach (var pawn in pawnList)
            {
                pawn.ResetPosition();
            }
        }

        #region Combat Rules
        static readonly int FollowUpSpeedDiff = 4;
        public List<StrikeInfo> strikeList;
        public List<StrikeInfo> ArrangeStrikeList(Pawn attacker, Pawn defender)
        {
            bool vantageEff = defender.IsTriggerVantage();
            bool alacrityEffect = attacker.IsTriggerAlacrity();
            int attackerSpd = attacker.GetCombatAttr().AttackSpeed;
            int defenderSpd = defender.GetCombatAttr().AttackSpeed;
            bool attackerFollowUp = (attackerSpd - defenderSpd) > FollowUpSpeedDiff;
            bool defenderFollowUp = (defenderSpd - attackerSpd) > FollowUpSpeedDiff;
            strikeList = new List<StrikeInfo>();
            //int attackerSid = attacker.sequenceId;
            //int defenderSid = defender.sequenceId;

            //todo chain attack from attacker

            if (vantageEff)//defender's vantage eff: BABA
            {
                strikeList.Add(new StrikeInfo(1));
                strikeList.Add(new StrikeInfo(0));
                if (defenderFollowUp)
                    strikeList.Add(new StrikeInfo(1));
                if (attackerFollowUp)
                    strikeList.Add(new StrikeInfo(0));
            }
            else if (alacrityEffect)//attacker's alacrity eff:AABB
            {
                strikeList.Add(new StrikeInfo(0));
                if (attackerFollowUp)
                    strikeList.Add(new StrikeInfo(0));
                strikeList.Add(new StrikeInfo(1));
                if (defenderFollowUp)
                    strikeList.Add(new StrikeInfo(1));
            }
            else //normal attack:ABAB
            {
                strikeList.Add(new StrikeInfo(0));
                strikeList.Add(new StrikeInfo(1));
                if (attackerFollowUp)
                    strikeList.Add(new StrikeInfo(0));
                if (defenderFollowUp)
                    strikeList.Add(new StrikeInfo(1));
            }
            return strikeList;
        }
        #endregion
    }

    public class StrikeInfo
    {
        public StrikeResult result = StrikeResult.Miss;
        public StrikeType sType;
        public int attackerSid;
        public int defenderSid;
        public int attackerHP;
        public int attackerHPMax;
        public int defenderHP;
        public int defenderHPMax;
        /// <summary>
        /// 0=attacker 1=defender
        /// </summary>
        public int id = 0;
        public StrikeInfo(int _id)
        {
            id = _id;
        }
        public int attackerHPChange;
        public int defenderHPChange;
    }
}

