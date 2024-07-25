using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using YooAsset;
using System;
using YooAsset;
using UnityEngine.SceneManagement;
using UniFramework.Pooling;
using UniFramework.Singleton;
using UnityEngine.Rendering.Universal;
using UnityEditor.Rendering;

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
            //prepare load config blabla
            SwitchDriveState(BattleDriveState.STATE_LOAD_SCENE);
        }

        string mapName = "World1";//"SLGMapTest" "Map2dTest";
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

                //TBSMapService.Inst.InitMapCamera();
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
            //load pawns and art resources
            //
            if (UniSpawner == null)
                UniSpawner = UniPooling.CreateSpawner("DefaultPackage");

            //spawner.CreateGameObjectPoolAsync()
            UniSingleton.StartCoroutine(CreateSpawners());
            this.director = GameObject.Find("PlayableDirector").GetComponent<PlayableDirector>();

            this.director.Play();
        }
        IEnumerator CreateSpawners()
        {



            SwitchDriveState(BattleDriveState.STATE_LOAD_MAP_DATA);
            yield return null;
        }

        private void OnEnterWaitBattleState()
        {
            throw new NotImplementedException();
        }

        private void OnEnterInBattleState()
        {

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


        Transform effTrans;
        public void SpawnEffect(string effectPath, Transform givenTrans, float timer = -1f)
        {
            if (effTrans == null)
            {
                effTrans = new GameObject("effTrans").transform;
                effTrans.localPosition = Vector3.zero;
            }
            Transform effFather = effTrans;
            if (givenTrans != null)
                effFather = givenTrans;

            SpawnHandle handler = UniSpawner.SpawnSync(effectPath, effFather,
                      effFather.position, Quaternion.identity);
            if (timer > 0)
            {
                handler.SetSelfDestroy(timer);
            }
        }

        public void LoadObjInScene()
        {
        }


        #endregion

        public void StartCombat()
        {
            BLogic.Inst.StartCombat();
            FUIManager.Inst.ShowUI<UIPage_CombatPanel>(FUIDef.FWindow.CombatPanel);
        }

        #region timeline 
        public PlayableDirector director;

        #endregion
    }
}
