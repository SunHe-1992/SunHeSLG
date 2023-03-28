using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UnityEngine.Events;

public class UIPage_Sample : FUIBase
{

    UI_SamplePage ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_SamplePage;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = FUIManager.allUIAnimation[FUIManager.OpenUIAnimationType.NoAnimation];
        ui.btn_ok.onClick.Set(BtnOKClick);
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
    void BtnOKClick()
    {
        FUIManager.Instance.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
        FUIManager.Instance.HideUI(this);
    }

    void RefreshContent()
    {


    }
}
