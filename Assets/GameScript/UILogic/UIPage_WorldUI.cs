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
    }
    protected override void OnShown()
    {
        base.OnShown();

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
        FUIManager.Inst.HideUI(this);
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
    }
    void RefreshHUD()
    {
        ui.txt_hud.text = "";
    }

    void BtnMiniGame()
    {
        var type = BLogic.recentLandMark.eventType;
        switch (type)
        {
            case LandMarkEventType.Fishing:
                OnBtnClose();
                FUIManager.Inst.ShowUI<UIPage_Fishing>(FUIDef.FWindow.Fishing);
                break;
            case LandMarkEventType.Slot:
                MinigameService.Inst.SetUpSlotGameData();
                OnBtnClose();
                FUIManager.Inst.ShowUI<UIPage_SlotGame>(FUIDef.FWindow.SlotGame);
                break;
        }
    }
}
