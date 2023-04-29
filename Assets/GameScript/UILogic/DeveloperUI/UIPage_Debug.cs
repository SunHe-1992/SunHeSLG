using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;
using SunHeTBS;
using UniFramework.Pooling;
using System.Collections;
using UniFramework.Event;
public class UIPage_Debug : FUIBase
{

    UI_TestUI ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_TestUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.btn_test.onClick.Set(BtnTestClick);
        ui.btn_slg.onClick.Set(BtnGotoBattle);
        ui.btn_close.onClick.Set(OnBtnClose);
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


    }


    void RefreshContent()
    {


    }

    void BtnGotoBattle()
    {
        BattleManager.StartLocalBattle();
        OnBtnClose();
    }


    protected override void OnUpdate()
    {
        base.OnUpdate();


    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }

}
