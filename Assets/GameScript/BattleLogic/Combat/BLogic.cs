using System.Collections;
using System.Collections.Generic;
using UniFramework.Singleton;
using UniFramework.Event;
using SunHeTBS;
using static LandMark;

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

        }

        public void OnDestroy()
        {
            UniEvent.RemoveListener(GameEventDefine.LandMarkTriggered, OnLandMarkTriggered);

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
        public void StartLandMarkMiniGame()
        {
            var type = recentLandMark.eventType;
            switch (type)
            {
                case LandMarkEventType.Fishing:
                    FUIManager.Inst.ShowUI<UIPage_Fishing>(FUIDef.FWindow.Fishing);
                    break;
                case LandMarkEventType.Slot:
                    MinigameService.Inst.SetUpSlotGameData();
                    FUIManager.Inst.ShowUI<UIPage_SlotGame>(FUIDef.FWindow.SlotGame);
                    break;
                case LandMarkEventType.Harvest:
                    MinigameService.Inst.SetUpBankHeistData();
                    FUIManager.Inst.ShowUI<UIPage_BankHeist>(FUIDef.FWindow.BankHeist);
                    break;
            }
        }
    }
}