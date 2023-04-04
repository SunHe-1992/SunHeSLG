using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using YooAsset;
using SunHeTBS;
public class UIPage_Debug : FUIBase
{

    UI_TestUI ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_TestUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = FUIManager.allUIAnimation[FUIManager.OpenUIAnimationType.NoAnimation];
        ui.btn_test.onClick.Set(BtnTestClick);
        ui.btn_slg.onClick.Set(BtnGotoBattle);

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
    void BtnTestClick()
    {
        //FUIManager.Instance.HideUI(this);
        //Debugger.Log("click btn ");
        //FUIManager.Instance.ShowUI<UIPage_Sample>(FUIDef.FWindow.SamplePage);
        //FUIManager.Instance.HideUI(this);
        TBSMapService.Instance.TestPath();
    }

    void RefreshContent()
    {


    }

    void BtnGotoBattle()
    {
        BattleManager.StartLocalBattle();

    }


    protected override void OnUpdate()
    {
        base.OnUpdate();


    }


}
