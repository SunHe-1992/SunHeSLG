using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageBattle;
using UnityEngine;
using UniFramework.Event;
using PackageMinigame;
using SunHeTBS;
public class UIPage_CombatEnd : FUIBase
{

    UI_CombatEndUI ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_CombatEndUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.clickLoader.onClick.Set(BtnOKClick);
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
    void BtnOKClick()
    {
        //FUIManager.Inst.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
        FUIManager.Inst.HideUI(this);
        //back to world UI
        FUIManager.Inst.ShowUI<UIPage_WorldUI>(FUIDef.FWindow.WorldPanel);
    }

    void RefreshContent()
    {
        ui.txt_result.text = BLogic.Inst.combatResult + "";

    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
}
