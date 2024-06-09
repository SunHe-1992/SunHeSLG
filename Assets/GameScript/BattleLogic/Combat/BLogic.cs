using System.Collections;
using System.Collections.Generic;
using UniFramework.Singleton;
using UniFramework.Event;
using SunHeTBS;

namespace SunHeTBS
{
    public class BLogic : ISingleton
    {
        public static BLogic Inst;
        public static void Init()
        {
            Inst = UniSingleton.CreateSingleton<BLogic>();


        }
        public static LandMark recentLandMark;
        public void OnCreate(object createParam)
        {
            UniEvent.AddListener(GameEventDefine.LandMarkTriggered, OnLandMarkTriggered);
            UniEvent.AddListener(GameEventDefine.StartFishing, AskFishing);
            UniEvent.AddListener(GameEventDefine.StartSlotGame, AskSLotGame);
        }

        public void OnDestroy()
        {

        }

        public void OnUpdate()
        {
        }

        void OnLandMarkTriggered(IEventMessage msg)
        {


        }
        void AskFishing(IEventMessage msg)
        {
            
        }
        void AskSLotGame(IEventMessage msg)
        {

        }
    }
}
