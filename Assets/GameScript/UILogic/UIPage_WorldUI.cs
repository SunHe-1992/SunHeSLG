using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UniFramework.Event;
using PackageBattle;
using SunHeTBS;
using static LandMark;
public class UIPage_WorldUI : FUIBase
{

    UI_WorldPanel ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_WorldPanel;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.btn_test.onClick.Set(BtnTestClick);
        ui.btn_minigame.onClick.Set(BtnMiniGame);
        ui.btn_NPC.onClick.Set(BtnNPC);
        ui.btn_inventory.onClick.Set(BtnInventory);
        ui.btn_quest.onClick.Set(BtnQuests);
    }
    protected override void OnShown()
    {
        base.OnShown();
        InputReceiver.SwitchInputToMap();


    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

        RefreshContent();
    }
    protected override void OnHide()
    {
        base.OnHide();

    }
    void BtnTestClick()
    {
        FUIManager.Inst.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
        //FUIManager.Inst.HideUI(this);
    }

    void RefreshContent()
    {


    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();

        RefreshHUD();

        if (BLogic.recentLandMark == null)
            ui.showMiniGame.selectedIndex = 0;
        else
        {
            ui.showMiniGame.selectedIndex = 1;
            ui.btn_minigame.text = BLogic.recentLandMark.eventType.ToString();
        }
        if (BLogic.recentNPCMark == null)
            ui.showNPCBtn.selectedIndex = 0;
        else
        {
            ui.showNPCBtn.selectedIndex = 1;
            var npcMark = BLogic.recentNPCMark;

            switch (npcMark.PawnCfg.MapEvent)
            {
                case cfg.SLG.PawnMapEvent.Combat:
                    ui.btn_NPC.text = "Combat";
                    break;
                case cfg.SLG.PawnMapEvent.Store:
                    ui.btn_NPC.text = "Shop";
                    break;
                case cfg.SLG.PawnMapEvent.Talk:
                    ui.btn_NPC.text = "Dialogue";
                    break;
            }

        }
    }
    void RefreshHUD()
    {
        ui.txt_hud.text = "";
    }

    void BtnMiniGame()
    {
        OnBtnClose();
        BLogic.Inst.StartLandMarkMiniGame();
    }
    void BtnNPC()
    {
        var npcMark = BLogic.recentNPCMark;
        switch (npcMark.PawnCfg.MapEvent)
        {
            case cfg.SLG.PawnMapEvent.Combat:
                //go to combat
                BattleDriver.Inst.StartCombat();
                OnBtnClose();
                break;
            case cfg.SLG.PawnMapEvent.Store:
                //store npc show dialogue window
                OpenStore();
                break;
            case cfg.SLG.PawnMapEvent.Talk:
                //store npc show dialogue window
                OpenDialogue();
                break;
        }
    }

    void BtnInventory()
    {
        FUIManager.Inst.ShowUI<UIPage_Inventory>(FUIDef.FWindow.InventoryUI);
    }
    void OpenStore()
    {
        Debug.Log("go to store UI");
        FUIManager.Inst.ShowUI<UIPage_Store>(FUIDef.FWindow.StoreUI);

    }
    void OpenDialogue()
    {
        FUIManager.Inst.ShowUI<UIPage_Dialogue>(FUIDef.FWindow.DialoguePage, null, 1000);
    }
    void BtnQuests()
    {
        FUIManager.Inst.ShowUI<UIPage_QuestUI>(FUIDef.FWindow.QuestUI);
    }
}
