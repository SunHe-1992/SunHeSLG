using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageBattle;
using UnityEngine;
using UniFramework.Event;
using SunHeTBS;
public class UIPage_CombatPredict : FUIBase
{

    UI_CombatPredict ui;
    public static int leftPawnSid = 0;
    public static int rightPawnSid = 0;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_CombatPredict;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        this.ui.btn_close.onClick.Set(OnBtnClose);
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


    void RefreshContent()
    {
        var p = BLogic.Inst.selectedPawn;
        p.CalculateCombatAttr(p);
        var attr = p.GetCombatAttr();
    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
}
