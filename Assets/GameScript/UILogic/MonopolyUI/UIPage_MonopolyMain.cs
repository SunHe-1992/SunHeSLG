using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using PackageMonopoly;
using UnityEngine;
using UniFramework.Event;
using SunHeTBS;
using CommonPackage;
using YooAsset;
using UnityEngine.SceneManagement;
public class UIPage_MonopolyMain : FUIBase
{

    UI_MonopolyMain ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_MonopolyMain;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;

        ui.btn_Construction.onClick.Set(BtnConstruction);
        ui.btn_RollDice.onClick.Set(BtnRollDice);
        ui.compFactor.onClick.Set(OnBtnDiceFactor);
        ui.btn_test.onClick.Set(OnBtnTest);
    }
    protected override void OnShown()
    {
        base.OnShown();
        UniEvent.AddListener(GameEventDefine.DICE_COUNT_CHANGED, OnDiceCountChanged);
        UniEvent.AddListener(GameEventDefine.POINTS_CHANGED, RefreshTopBar);
        OnDiceCountChanged(null);
    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

        RefreshContent();
    }
    protected override void OnHide()
    {
        base.OnHide();
        UniEvent.RemoveListener(GameEventDefine.DICE_COUNT_CHANGED, OnDiceCountChanged);
        UniEvent.RemoveListener(GameEventDefine.POINTS_CHANGED, RefreshTopBar);

    }


    void RefreshContent()
    {
        if (MonoPlayer.diceFactor == 0)//first time enter this ui, make the dice factor at least 1
        {
            currentFactorIndex = 0;
            OnBtnDiceFactor();
        }
        CheckDiceFactor();
        RefreshDiceComp();

        RefreshTopBar(null);
    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
    void BtnConstruction()
    {
        SceneHandle handle = YooAssets.LoadSceneAsync("Scene/MonoBuildingMap", LoadSceneMode.Additive);
        handle.Completed += (scene) =>
        {
            FUIManager.Inst.ShowUI<UIPage_Construction>(FUIDef.FWindow.Construction);
            FUIManager.Inst.HideUI(this);
        };
    }


    protected override void OnUpdate()
    {
        base.OnUpdate();
#if UNITY_EDITOR
        RefreshHudInfo();
#endif
        RefreshRollDiceBtn();
    }
    void RefreshHudInfo()
    {
        var logic = MLogic.Inst;
        string hudMsg = $"curIndex={logic.currentTileIndex}\nlastIndex={logic.lastTileIndex} \nDiceValue={logic.diceValue}";

        ui.txt_hud.text = hudMsg;
    }
    #region roll dice button comps
    void BtnRollDice()
    {
        if (MonoPlayer.UserDetail.diceCount <= 0)
        {
            //no dice to roll
            Debugger.Log("no dice to roll");
            return;
        }
        if (MonopolyMapController.inst.playingAnim == false)
            MLogic.Inst.RollDice(MonoPlayer.diceFactor);
    }

    /// <summary>
    /// show info : roll dice btn,dice factor,dice count
    /// </summary>
    void RefreshDiceComp()
    {
        ui.diceBar.max = 40;
        int diceCount = MonoPlayer.UserDetail.diceCount;
        ui.diceBar.value = diceCount;

        ui.compFactor.title = "×" + MonoPlayer.diceFactor;
        ui.compFactor.ctrl_color.selectedIndex = diceFactorColor;

    }

    List<int> diceFactorList = new List<int>() { 1, 2, 5, 10, 20, 50, 100 };
    int diceFactorColor = 0;

    void RefreshRollDiceBtn()
    {
        ui.btn_RollDice.enabled = MonopolyMapController.inst.playingAnim == false;
    }

    void OnDiceCountChanged(IEventMessage msg)
    {
        RefreshDiceFactorAfterJump();
        RefreshDiceComp();
    }


    int maxFactorIndex = 0;
    int currentFactorIndex = 0;
    void CheckDiceFactor()
    {
        int diceCount = MonoPlayer.UserDetail.diceCount;
        for (int i = diceFactorList.Count - 1; i >= 0; i--)
        {
            if (diceCount >= diceFactorList[i])
            {
                maxFactorIndex = i;
                break;
            }
        }
    }
    /// <summary>
    /// btn: toggle all available dice factors
    /// </summary>
    void OnBtnDiceFactor()
    {
        CheckDiceFactor();
        int diceCount = MonoPlayer.UserDetail.diceCount;
        bool findBiggerFactor = false;
        for (int i = currentFactorIndex + 1; i <= maxFactorIndex; i++)
        {
            if (diceCount >= diceFactorList[i])
            {
                currentFactorIndex = i;
                MonoPlayer.diceFactor = diceFactorList[i];
                findBiggerFactor = true;
                break;
            }
        }
        if (findBiggerFactor == false)
        {
            currentFactorIndex = 0;
            MonoPlayer.diceFactor = diceFactorList[0];
        }

        RefreshDiceComp();
    }
    /// <summary>
    /// try to find a lower dice factor than current
    /// </summary>
    void RefreshDiceFactorAfterJump()
    {
        int diceCount = MonoPlayer.UserDetail.diceCount;
        bool findFactor = false;
        for (int i = currentFactorIndex; i >= 0; i--)
        {
            if (diceCount >= diceFactorList[i])
            {
                currentFactorIndex = i;
                MonoPlayer.diceFactor = diceFactorList[i];
                findFactor = true;
                break;
            }
        }
        if (findFactor == false)
        {
            currentFactorIndex = 0;
            MonoPlayer.diceFactor = diceFactorList[0];
        }

        RefreshDiceComp();
    }
    #endregion

    #region top bar
    void RefreshTopBar(IEventMessage msg)
    {
        UIService.Inst.RefreshTopBar(this.ui.topBar);
    }
    #endregion

    void OnBtnTest()
    {
        MLogic.Inst.HandleBankHeist(0, 0, 0, 0);
    }
}
