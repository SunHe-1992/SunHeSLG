using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UnityEngine.Events;

public class UIPage_Debug : FUIBase
{

    UI_TestUI ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_TestUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = FUIManager.allUIAnimation[FUIManager.OpenUIAnimationType.NoAnimation];
        ui.btn_test.onClick.Set(BtnNoClick);

    }
    protected override void OnShown()
    {
        base.OnShown();

    }

    public void ResetInfo(string notice, UnityAction success, UnityAction failAction, int showBtn)
    {

        RefreshContent();
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
    void BtnNoClick()
    {
        //FUIManager.Instance.HideUI(this);
        Debugger.Log("click btn ");
        FUIManager.Instance.ShowUI<UIPage_Sample>(FUIDef.FWindow.SamplePage);
        FUIManager.Instance.HideUI(this);
    }

    void RefreshContent()
    {


    }
}
