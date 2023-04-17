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
        PhaseSwitch = 5,
        /// <summary>
        /// player controll pawns
        /// </summary>
        PlayerControl = 6,

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

    /// <summary>
    /// Battle Logic
    /// </summary>
    public class BLogic : ISingleton
    {

        //todo read data , deploy battle field

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
        public int BattleTurn = 0;
        public PawnCamp curCamp { get; private set; }
        List<PawnCamp> campList;
        public PawnCamp GetNextCamp()
        {

            campList = new List<PawnCamp>();
            HashSet<PawnCamp> hash = new HashSet<PawnCamp>();
            foreach (var pawn in pawnList)
            {
                hash.Add(pawn.camp);
            }
            campList.AddRange(hash);
            campList.Sort();
            if (campList.Count == 0)
            {
                Debugger.LogError("no valid camp !");
            }
            PawnCamp camp = campList[0];
            if ((int)camp + 1 > campList.Count)
                camp = PawnCamp.Player;
            else
                camp = curCamp + 1;
            return camp;
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
                    case GamePlayState.PhaseSwitch: OnEnterPhaseSwitch(); break;
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
                Debugger.LogError($"map tile {p.curPosition.ToString()} already contains pawn!");
            }
        }
        public void AddTestPawn()
        {
            Pawn p = new Pawn();
            p.camp = PawnCamp.Player;
            p.curPosition = new Vector3Int(0, 0, 0);
            p.modelName = "M_AA_001";
            p.Init();
            AddPawn(p);
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
            AddTestPawn();

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

        #region cursor select pawn

        public Vector3Int cursorPos;
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
        public void OnFixedUpdate()
        {

        }
        #endregion

        public void OnEnterMovieTime()
        {
            //no movie in demo ,show battle main page
            FUIManager.Inst.ShowUI<UIPage_BattleMain>(FUIDef.FWindow.BattlePanel,
                (win) => { SetNextGamePlayState(GamePlayState.PhaseSwitch); });
        }
        public void OnEnterPhaseSwitch()
        {

            UniEvent.SendMessage(GameEventDefine.PhaseSwitch);
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
                SetNextGamePlayState(GamePlayState.PlayerControl);
                SetPlayerCtrlState(PlayerControlState.TileSelect);
                InputReceiver.SwitchInputToMap();
            }
            else //todo AI pawn actions
            {

            }
        }

        public void OnEnterPlayerControl()
        {

        }


        #region Pawn Move
        Pawn movingPawn = null;
        private void PawnTempMove()
        {
            int cursorTileId = TBSMapService.Inst.GetTileId(cursorPos);
            if (pointedPawn == null)
            {
                if (selectedPawn.moveTileIds.Contains(cursorTileId))
                {
                    PawnStartMove(selectedPawn, cursorTileId);
                }
            }
        }
        public void PawnStartMove(Pawn p, int tileId)
        {
            movingPawn = p;
            var mapInst = GetMapInst();
            var toTile = mapInst.GetTileFromDic(tileId);
            var startTile = mapInst.Tile(movingPawn.curPosition);
            var nodeList = NodePathFinder.Path(mapInst, startTile, toTile, movingPawn.IsPassFoe());
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
                    //show action menu
                    SetPlayerCtrlState(PlayerControlState.UIControl);
                    UniEvent.SendMessage(GameEventDefine.ShowActionMenu);
                    SetNextGamePlayState(GamePlayState.PlayerControl);
                }
                movingPawn = null;
            }
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

            var nodes = TBSMapService.Inst.map.NeighborsMovable(tile).Where(neigh => neigh != null);
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
                    if (pointedPawn != null)
                        TBSMapService.Inst.ShowPawnCoverPlanes(pointedPawn);
                    else
                        TBSMapService.Inst.UnspawnPawnCoverPlanes(null);
                    break;
                case PlayerControlState.PawnSelected:
                    CursorInputMove(xAdd, yAdd);
                    //arrows
                    break;
            }

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
            ChangeCursorPos(newPos);
        }
        public void CursorInputMoveTo(Vector3Int pos)
        {
            var newPos = TBSMapService.Inst.map.TrimPos_Border(pos);
            ChangeCursorPos(newPos);
        }
        /// <summary>
        /// handle cursor pos changed
        /// </summary>
        /// <param name="newPos"></param>
        void ChangeCursorPos(Vector3Int newPos)
        {
            if (newPos == cursorPos)
                return;
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

            var cursorObj = BattleDriver.Inst.CursorObj;
            if (cursorObj != null)
            {
                BattleDriver.Inst.MoveCursorObj();
            }

        }
        public void OnMouseClick(Vector3 pos)
        {
            if (gameState == GamePlayState.PlayerControl)
            {
                if (pCtrlState == PlayerControlState.TileSelect)
                {
                    var clickTile = TBSMapService.Inst.map.Tile(pos);
                    if (clickTile != null)
                    {
                        CursorInputMoveTo(clickTile.Position);//set pointedPawn
                    }
                }
            }
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
                        if (selectedPawn.camp == PawnCamp.Player)//player's pawn
                        {
                            //goto pos
                            PawnTempMove();
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

        public void CheckPhaseSwitch()
        {
            int activePawnCount = 0;
            foreach (var pawn in pawnList)
            {
                if (pawn.camp == curCamp && pawn.actionEnd == false)
                {
                    activePawnCount++;
                }
            }
            if (activePawnCount == 0)
            {
                //go to next phase
                EndCurrentPhase();
            }
        }
        void EndCurrentPhase()
        {
            curCamp = GetNextCamp();
            if (curCamp == PawnCamp.Player)
            {
                BattleTurn++;
                SetNextGamePlayState(GamePlayState.PhaseSwitch);
            }
        }
        public void RefreshPawnMovement()
        {
            mapPawnDic.Clear();
            foreach (var pawn in pawnList)
            {
                int tileId = pawn.TilePosId();
                mapPawnDic[tileId] = pawn;
            }
        }
        public void OnPawnEndAction(Pawn p)
        {
            if (p.camp == PawnCamp.Player)
            {
                SetPlayerCtrlState(PlayerControlState.TileSelect);
            }
        }
    }
}
