using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using System;
using YooAsset;
using UnityEngine.SceneManagement;
using UniFramework.Pooling;
using UniFramework.Singleton;

namespace SunHeTBS
{
    public enum BattleDriveState
    {
        STATE_IDLE = 1,  // 空闲状态，不做战斗更新

        STATE_PREPARE_BATTLE = 2,  //进入准备战斗状态
        STATE_LOAD_SCENE = 3,      //切换场景
        STATE_PRELOAD_RES = 4,     //预加载战斗内要用到的角色和怪物资源

        STATE_PREWARM_CONFIG = 5,  //预热配置文件
        STATE_LOAD_MAP_DATA = 6,
        STATE_LOAD_MAP_PREFAB = 7,

        STATE_WAIT_BATTLE = 8, //等待所有玩家都准备好战斗开始。 组队战斗中用，保证所有玩家都同时切换到战斗状态

        STATE_IN_BATTLE = 9,   //正式进入战斗状态，执行战斗逻辑更新

        STATE_GO_NEXT_STAGE = 10,  //进入下一小关战斗

        STATE_CLEAN_UP = 11,  //战斗结束，清理资源 
    }

    public class BattleDriver : ISingleton
    {


        public static BattleDriver Inst { get; private set; }
        public static void Init()
        {
            Inst = UniSingleton.CreateSingleton<BattleDriver>();
        }
        public void OnCreate(object createParam)
        {
            isWindowEditor = Application.platform == RuntimePlatform.WindowsEditor;
            currDriveState = BattleDriveState.STATE_IDLE;
            nextDriveState = BattleDriveState.STATE_IDLE;
            PawnControllers = new List<PawnController>();
        }

        public void OnUpdate()
        {
            DoUpdate();
        }

        public void OnDestroy()
        {
        }

        float deltaTime = 0;
        // Update is called once per frame
        void DoUpdate()
        {
            if (isWindowEditor)
            {
                CheckGMKey();
            }
            deltaTime = Time.deltaTime;
            OnBattleFrameUpdate(deltaTime);
            ChangeDriveState();
        }

        private void OnBattleFrameUpdate(float deltaTime)
        {

        }

        private void CheckGMKey()
        {
            //GM key open developer UI
            if (Input.GetKeyUp(KeyCode.F1))
            {
                FUIManager.Inst.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
            }
        }

        #region controller list
        private List<PawnController> PawnControllers = new List<PawnController>();
        #endregion

        bool isWindowEditor = false;


        #region state machine manage

        public BattleDriveState currDriveState = BattleDriveState.STATE_IDLE;
        private BattleDriveState nextDriveState = BattleDriveState.STATE_IDLE;   //下一帧将要切换到的驱动状态


        public void SwitchDriveState(BattleDriveState driveState)
        {
            nextDriveState = driveState;
        }

        public void ChangeDriveState()
        {
            if (nextDriveState == currDriveState)
            {
                //Debugger.LogError("Already In Drive State: " + nextDriveState);
                return;
            }

            currDriveState = nextDriveState;

            Debugger.Log(" ChangeDriveState " + currDriveState);


            switch (currDriveState)
            {
                case BattleDriveState.STATE_IDLE:
                    OnEnterIdleState();
                    break;
                case BattleDriveState.STATE_PREPARE_BATTLE:
                    OnEnterPrepareBattleState();
                    break;
                case BattleDriveState.STATE_LOAD_SCENE:
                    OnEnterLoadSceneState();
                    break;
                case BattleDriveState.STATE_LOAD_MAP_DATA:
                    OnEnterLoadMapDataState();
                    break;
                case BattleDriveState.STATE_PREWARM_CONFIG:
                    OnEnterPreWarmConfigState();
                    break;
                case BattleDriveState.STATE_LOAD_MAP_PREFAB:
                    OnEnterLoadMapPrefabState();
                    break;
                case BattleDriveState.STATE_PRELOAD_RES:
                    OnEnterPreloadResState();
                    break;
                case BattleDriveState.STATE_WAIT_BATTLE:
                    OnEnterWaitBattleState();
                    break;
                case BattleDriveState.STATE_IN_BATTLE:
                    OnEnterInBattleState();
                    break;
                case BattleDriveState.STATE_GO_NEXT_STAGE:
                    OnEnterGoNextStageState();
                    break;
                case BattleDriveState.STATE_CLEAN_UP:
                    OnEnterCleanUpState();
                    break;
                default:
                    Debugger.LogError("Invalid Battle Driver State To Change: " + nextDriveState);
                    break;
            }


        }

        private void OnEnterIdleState() { }

        private BLogic logicInst;
        private void OnEnterPrepareBattleState()
        {
            logicInst = BLogic.Inst;
            //todo prepare load config blabla
            SwitchDriveState(BattleDriveState.STATE_LOAD_SCENE);
        }

        string mapName = "Map2dTest";//"SLGMapTest";
        private void OnEnterLoadSceneState()
        {
            SceneHandle handle = YooAssets.LoadSceneAsync("Scene/" + mapName, LoadSceneMode.Single);
            handle.Completed += (scene) =>
            {
                //hide EditorGizmos
                var gizmos = GameObject.Find("EditorGizmos");
                if (gizmos != null)
                {
                    gizmos.SetActive(false);
                }

                TBSMapService.Inst.InitMapCamera();
                SwitchDriveState(BattleDriveState.STATE_PRELOAD_RES);
            };


        }

        private void OnEnterLoadMapDataState()
        {
            //if (!MapView)
            //{
            //    MapView = GameObject.FindObjectOfType<MapView>();
            //}
            //var assetInfo = YooAssets.GetAssetInfo($"Scene/{mapName}_MapSquare");
            //YooAssets.LoadAssetSync(assetInfo).Completed += (handle) =>
            //{
            //    mapSetting = handle.AssetObject as MapSettings;
            //    var tileData = mapSetting.Tiles[0];

            //    MapEntity = new MapEntity(mapSetting, MapView);
            //};

            TBSMapService.Inst.ClearData();
            var jsonAssetInfo = YooAssets.GetAssetInfo($"MapData/{mapName}");
            YooAssets.LoadAssetSync(jsonAssetInfo).Completed += (handle) =>
            {
                var jStr = handle.AssetObject.ToString();
                TBSMapService.Inst.LoadJsonData(jStr);
            };

            SwitchDriveState(BattleDriveState.STATE_LOAD_MAP_PREFAB);
        }

        private void OnEnterPreWarmConfigState()
        {

        }

        private void OnEnterLoadMapPrefabState()
        {
            //load arrow cursor object
            var arrowHandle = YooAssets.LoadAssetSync("Effect/Signs/CursorObj2D", typeof(GameObject));
            var obj = GameObject.Instantiate(arrowHandle.AssetObject as GameObject);
            obj.name = "CursorObj";
            cursorCtrl = obj.AddComponent<CursorControl>();

            SwitchDriveState(BattleDriveState.STATE_IN_BATTLE);

        }

        private void OnEnterPreloadResState()
        {
            //todo load pawns and art resources
            //
            if (UniSpawner == null)
                UniSpawner = UniPooling.CreateSpawner("DefaultPackage");

            //spawner.CreateGameObjectPoolAsync()
            UniSingleton.StartCoroutine(CreateSpawners());

        }
        IEnumerator CreateSpawners()
        {

            var op1 = UniSpawner.CreateGameObjectPoolAsync(TBSMapService.str_PlaneBlue);
            yield return op1;
            var op2 = UniSpawner.CreateGameObjectPoolAsync(TBSMapService.str_PlanePurple);
            yield return op2;
            var op3 = UniSpawner.CreateGameObjectPoolAsync(TBSMapService.str_PlaneRed);
            yield return op3;

            SwitchDriveState(BattleDriveState.STATE_LOAD_MAP_DATA);
        }

        private void OnEnterWaitBattleState()
        {
            throw new NotImplementedException();
        }

        private void OnEnterInBattleState()
        {
            logicInst.PostInitProcess();
            logicInst.Running = true;
            logicInst.SetNextGamePlayState(GamePlayState.BeforeBattle);
        }

        private void OnEnterGoNextStageState()
        {
            throw new NotImplementedException();
        }

        private void OnEnterCleanUpState()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region manage Game objects and pooling
        public CursorControl cursorCtrl;
        /// <summary>
        /// UniPooling's spawner
        /// </summary>
        public static Spawner UniSpawner;

        /// <summary>
        /// cursor obj relocate to BLogic cursorPos
        /// </summary>
        public void MoveCursorObj()
        {
            if (cursorCtrl)
            {
                Vector3 pos = TBSMapService.Inst.map.WorldPosition(logicInst.cursorPos);
                var tile = TBSMapService.Inst.map.Tile(logicInst.cursorPos);
                float topHeight = 0f;
                if (tile != null)
                {
                    topHeight = tile.topHeight;
                }
                cursorCtrl.transform.position = pos + new Vector3(0, topHeight, 2);
                TBSMapService.Inst.mapCamera.SetTargetTilePos(pos);
            }
        }
        public void SetCursorRed()
        {
            cursorCtrl.ChangeRed();
        }
        public void SetCursorWhite()
        {
            cursorCtrl.ChangeWhite();
        }
        public void CursorShowArrow(bool isshow)
        {
            cursorCtrl.ShowHideArrow(isshow);
        }
        #endregion
    }
}
