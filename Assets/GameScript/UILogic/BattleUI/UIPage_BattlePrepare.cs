using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using PackageBattle;
using UnityEngine;
using UniFramework.Event;
using SunHeTBS;
public class UIPage_BattlePrepare : FUIBase
{

    UI_BattlePrepare ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_BattlePrepare;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.btn_fight.onClick.Set(BtnFightClick);
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
    void BtnFightClick()
    {
        BLogic.Inst.SetNextGamePlayState(GamePlayState.MovieTime);
        OnBtnClose();
    }

    void RefreshContent()
    {


    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);

    }
}
