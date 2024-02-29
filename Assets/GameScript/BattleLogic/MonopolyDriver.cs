using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using System;
using UnityEngine.SceneManagement;
using UniFramework.Pooling;
using UniFramework.Singleton;

namespace SunHeTBS
{
    public enum MonoDriveState
    {
        STATE_IDLE = 1,
        STATE_PREPARE_BATTLE = 2,
        STATE_LOAD_SCENE = 3,
        STATE_PRELOAD_RES = 4,
        STATE_LOAD_MAP_DATA = 5,
        STATE_LOAD_MAP_PREFAB = 6,
        STATE_IN_BATTLE = 7,   //game play
    }

    public class MonopolyDriver : ISingleton
    {


        public static MonopolyDriver Inst { get; private set; }
        public static void Init()
        {
            Inst = UniSingleton.CreateSingleton<MonopolyDriver>();
        }
        public void OnCreate(object createParam)
        {
            isWindowEditor = Application.platform == RuntimePlatform.WindowsEditor;
            currDriveState = MonoDriveState.STATE_IDLE;
            nextDriveState = MonoDriveState.STATE_IDLE;
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

        #endregion

        bool isWindowEditor = false;


        #region state machine manage

        public MonoDriveState currDriveState = MonoDriveState.STATE_IDLE;
        private MonoDriveState nextDriveState = MonoDriveState.STATE_IDLE;   //下一帧将要切换到的驱动状态


        public void SwitchDriveState(MonoDriveState driveState)
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
                case MonoDriveState.STATE_IDLE:
                    OnEnterIdleState();
                    break;
                case MonoDriveState.STATE_PREPARE_BATTLE:
                    OnEnterPrepareBattleState();
                    break;
                case MonoDriveState.STATE_LOAD_SCENE:
                    OnEnterLoadSceneState();
                    break;
                case MonoDriveState.STATE_LOAD_MAP_DATA:
                    OnEnterLoadMapDataState();
                    break;

                case MonoDriveState.STATE_LOAD_MAP_PREFAB:
                    OnEnterLoadMapPrefabState();
                    break;
                case MonoDriveState.STATE_PRELOAD_RES:
                    OnEnterPreloadResState();
                    break;
                case MonoDriveState.STATE_IN_BATTLE:
                    OnEnterInBattleState();
                    break;
                default:
                    Debugger.LogError("Invalid Battle Driver State To Change: " + nextDriveState);
                    break;
            }


        }

        private void OnEnterIdleState() { }


        private void OnEnterPrepareBattleState()
        {
            //todo prepare load config blabla
            SwitchDriveState(MonoDriveState.STATE_LOAD_SCENE);
        }

        string mapName = "MonopolyBoard";
        private void OnEnterLoadSceneState()
        {
            SceneHandle handle = YooAssets.LoadSceneAsync("Scene/" + mapName, LoadSceneMode.Single);
            handle.Completed += (scene) =>
            {
                SwitchDriveState(MonoDriveState.STATE_PRELOAD_RES);
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

            SwitchDriveState(MonoDriveState.STATE_LOAD_MAP_PREFAB);
        }

        private void OnEnterPreWarmConfigState()
        {

        }

        private void OnEnterLoadMapPrefabState()
        {

            SwitchDriveState(MonoDriveState.STATE_IN_BATTLE);

        }

        private void OnEnterPreloadResState()
        {
            SwitchDriveState(MonoDriveState.STATE_IN_BATTLE);


        }



        private void OnEnterInBattleState()
        {
            FUIManager.Inst.ShowUI<UIPage_MonopolyMain>(FUIDef.FWindow.MonopolyMain);
            MLogic.Inst.MapSceneLoaded();
        }


        #endregion
        public void StartTest()
        {
            SwitchDriveState(MonoDriveState.STATE_PREPARE_BATTLE);

        }

    }
}
