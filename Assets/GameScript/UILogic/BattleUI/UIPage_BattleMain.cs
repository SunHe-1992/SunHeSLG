using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using PackageBattle;
using UnityEngine;
using UnityEngine.Events;
using SunHeTBS;
using UniFramework.Event;
public partial class UIPage_BattleMain : FUIBase
{

    UI_BattlePanel ui;
    GList focusedList = null;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_BattlePanel;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        this.ui.bottomBar.btn_detail.onClick.Set(OpenUnitDetail);
    }
    protected override void OnShown()
    {
        base.OnShown();

        UniEvent.AddListener(GameEventDefine.InputAxis, OnInputAxis);
        UniEvent.AddListener(GameEventDefine.ClickConfirm, OnClickConfirm);
        UniEvent.AddListener(GameEventDefine.PhaseSwitch, OnPhaseSwitch);
        UniEvent.AddListener(GameEventDefine.ShowSelectPawn, ShowSelectPawn);
        UniEvent.AddListener(GameEventDefine.ShowActionMenu, ShowActionMenu);
        UniEvent.AddListener(GameEventDefine.ClickCancel, OnClickCancel);
        UniEvent.AddListener(GameEventDefine.CursorPointToPawn, OnPointToPawn);
        UniEvent.AddListener(GameEventDefine.ShowWeaponSelectUI, OpenWeaponSelect);

        //test  instant switch to map pawn control
        InputReceiver.Inst.inputComp.SwitchCurrentActionMap("Player");
        focusedList = null;
        HideUIComp();
        OnPointToPawn(null);
    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

        RefreshContent();
    }
    protected override void OnHide()
    {
        base.OnHide();

        UniEvent.RemoveListener(GameEventDefine.InputAxis, OnInputAxis);
        UniEvent.RemoveListener(GameEventDefine.ClickConfirm, OnClickConfirm);
        UniEvent.RemoveListener(GameEventDefine.PhaseSwitch, OnPhaseSwitch);
        UniEvent.RemoveListener(GameEventDefine.ShowSelectPawn, ShowSelectPawn);
        UniEvent.RemoveListener(GameEventDefine.ShowActionMenu, ShowActionMenu);
        UniEvent.RemoveListener(GameEventDefine.ClickCancel, OnClickCancel);
        UniEvent.RemoveListener(GameEventDefine.CursorPointToPawn, OnPointToPawn);
        UniEvent.RemoveListener(GameEventDefine.ShowWeaponSelectUI, OpenWeaponSelect);

        UICancelAction = null;
        UIConfirmAction = null;
        this.focusedList = null;
    }


    void RefreshContent()
    {

    }
    void HideUIComp()
    {
        //ui.bottomBar.visible = false;
        //ui.nameBar.visible = false;
        ui.tileInfoComp.visible = false;
        ui.phaseCom.visible = false;
        ui.actionCom.visible = false;
        ui.inventoryCom.visible = false;
    }
    #region battle map input control

    void OnInputAxis(IEventMessage msg)
    {
        var inputInst = InputReceiver.Inst;
        //right=x+1 up=z+1
        int xAdd = 0;
        int zAdd = 0;
        if (inputInst.axisDown) zAdd = -1;
        else if (inputInst.axisUp) zAdd = 1;
        if (inputInst.axisLeft) xAdd = -1;
        else if (inputInst.axisRight) xAdd = 1;

        if (InputReceiver.InputInUI)
        {
            if (this.isTop)
            {
                UIListNavigation();
            }
        }
        else
        {
            BLogic.Inst.OnInputAxis(xAdd, zAdd);
        }

    }
    void UIListNavigation()
    {
        var inputInst = InputReceiver.Inst;
        if (focusedList != null)
        {
            //list navigation
            if (inputInst.axisDown)
                focusedList.OnInputNext();
            else if (inputInst.axisUp)
                focusedList.OnInputPrevious();
        }
    }



    /*player pawn control state:
     * 1 not selected any pawn,move to pawn show its summary 
       2 selected players pawn, move cursor selected destination
       3 confirm and perform pawn walking
    */
    void OnClickConfirm(IEventMessage msg)
    {
        if (InputReceiver.InputInUI)
        {
            if (this.isTop)
                UIConfirmAction?.Invoke();
        }
        if (InputReceiver.InputInUI == false)
            BLogic.Inst.OnClickConfirm();

    }

    void OnClickCancel(IEventMessage msg)
    {
        if (InputReceiver.InputInUI)
            if (this.isTop)
            {
                UICancelAction?.Invoke();
            }
        if (InputReceiver.InputInUI == false)
            BLogic.Inst.OnClickCancel();

    }
    #endregion

    #region UI Click Button
    void OnUIConfirm(IEventMessage msg)
    {

    }
    void OnUICancel(IEventMessage msg)
    {

    }
    #endregion

    void OnPhaseSwitch(IEventMessage msg)
    {
        int campNum = (int)BLogic.Inst.curCamp;
        ui.phaseCom.visible = true;
        ui.phaseCom.ctrl_phase.selectedIndex = campNum;
        ui.anim_phase.Play(PhaseSwitchEnd);
    }
    void PhaseSwitchEnd()
    {
        ui.phaseCom.visible = false;
    }

    void ShowSelectPawn(IEventMessage msg)
    {

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
#if UNITY_EDITOR
        var state = BLogic.Inst.GetGamePlayState();
        string debugStr = "";
        string turnStr = $"Turn={BLogic.Inst.BattleTurn} {BLogic.Inst.curCamp} Phase\n";
        debugStr += turnStr;
        debugStr += UIService.ChangeGreen($"GameState = {state}\n");
        debugStr += UIService.ChangeGreen($"player control state = {BLogic.Inst.pCtrlState}\n");
        string input = InputReceiver.InputInUI ? "UI" : "MAP";
        debugStr += UIService.ChangeGreen($"input mode = {input}\n");

        var selectedPawn = BLogic.Inst.selectedPawn;
        if (selectedPawn != null)
            debugStr += $"selectedPawn={selectedPawn}\n";
        else
            debugStr += $"selectedPawn=null\n";
        var pointedPawn = BLogic.Inst.pointedPawn;
        if (pointedPawn != null)
            debugStr += $"pointedPawn={pointedPawn}\n";
        else
            debugStr += $"pointedPawn=null\n";
        ui.txt_logicState.text = debugStr;
#endif
    }

    private event Action UIConfirmAction;
    private event Action UICancelAction;

    private void OpenUnitDetail()
    {
        var pawn = BLogic.Inst.pointedPawn;
        if (pawn != null)
        {
            FUIManager.Inst.HideUI(this);
            FUIManager.Inst.ShowUI<UIPage_UnitUI>(FUIDef.FWindow.UnitUI, null);
        }
    }
    private void CloseUI()
    {
        FUIManager.Inst.HideUI(this);
    }
}
