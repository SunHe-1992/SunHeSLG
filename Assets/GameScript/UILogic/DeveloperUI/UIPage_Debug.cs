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
        ui.btn_addGold100.onClick.Set(BtnAddGold);
        ui.btn_slot.onClick.Set(BtnSlotGame);
        ui.btn_fishing.onClick.Set(BtnFishingGame);


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
        var pawnCfg = ConfigManager.table.TbPawn.Get(1);
        if (pawnCfg != null)
        {
            Debug.Log(pawnCfg.ToString());
        }

    }


    void RefreshContent()
    {


    }

    void BtnGotoBattle()
    {
        //BattleManager.StartLocalBattle();
        string mapName = "World1";
        SceneHandle handle = YooAssets.LoadSceneAsync("Scene/" + mapName, LoadSceneMode.Single);
        handle.Completed += (scene) =>
        {
            FUIManager.Inst.ShowUI<UIPage_WorldUI>(FUIDef.FWindow.WorldPanel);
        };
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


    void BtnAddGold()
    {
        TBSPlayer.UpdateGoldAmount(10000);
        UniEvent.SendMessage(GameEventDefine.POINTS_CHANGED);
    }

    void BtnSlotGame()
    {
        OnBtnClose();
        MinigameService.Inst.SetUpSlotGameData();
        FUIManager.Inst.ShowUI<UIPage_SlotGame>(FUIDef.FWindow.SlotGame);
    }


    void BtnFishingGame()
    {
        OnBtnClose();
        FUIManager.Inst.ShowUI<UIPage_Fishing>(FUIDef.FWindow.Fishing);
    }
}
