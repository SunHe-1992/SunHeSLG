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
        ui.btn_monopoly.onClick.Set(BtnMonopoly);
        ui.btn_addDice50.onClick.Set(BtnAddDice);
        ui.btn_addGold100.onClick.Set(BtnAddGold);
        ui.btn_bank.onClick.Set(BtnBank);
        ui.btn_slot.onClick.Set(BtnSlotGame);

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
    void BtnMonopoly()
    {
        MonopolyDriver.Inst.StartTest();
        OnBtnClose();
    }
    void BtnAddDice()
    {
        MonoPlayer.UserDetail.diceCount += 50;
        UniEvent.SendMessage(GameEventDefine.DICE_COUNT_CHANGED);
    }
    void BtnAddGold()
    {
        MonoPlayer.UpdateGoldAmount(10000);
        UniEvent.SendMessage(GameEventDefine.POINTS_CHANGED);
    }
    void BtnBank()
    {
        OnBtnClose();
        MLogic.Inst.HandleBankHeist(0, 0, 0, 0);
    }
    void BtnSlotGame()
    {
        OnBtnClose();
        MLogic.Inst.HandleSlogGame(0, 0, 0, 0);
    }
}
