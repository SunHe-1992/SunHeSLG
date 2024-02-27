using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using PackageMonopoly;
using UnityEngine;
using UniFramework.Event;

public class UIPage_MonopolyMain : FUIBase
{

    UI_MonopolyMain ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_MonopolyMain;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;

        ui.btn_Test.onClick.Set(BtnTest);
        ui.btn_Jump.onClick.Set(BtnJump);
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
    void BtnTest()
    {

    }
    void BtnJump()
    {
        MonopolyMapController.inst.TestJump();
    }
}
