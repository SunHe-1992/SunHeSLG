using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UniFramework.Event;
using PackageMonopoly;
public class UIPage_BankHeist : FUIBase
{

    UI_BankHeist ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_BankHeist;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
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


    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
}
