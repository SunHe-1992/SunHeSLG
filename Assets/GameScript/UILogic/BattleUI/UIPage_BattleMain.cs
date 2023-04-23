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

        //test  instant switch to map pawn control
        InputReceiver.Inst.inputComp.SwitchCurrentActionMap("Player");
        focusedList = null;
        HideUIComp();
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

    }


    void RefreshContent()
    {

    }
    void HideUIComp()
    {
        ui.bottomBar.visible = false;
        ui.nameBar.visible = false;
        ui.tileInfoComp.visible = false;
        ui.phaseCom.visible = false;
        ui.actionCom.visible = false;
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
            UIConfirmAction?.Invoke();
        else
            BLogic.Inst.OnClickConfirm();
        var pointedPawn = BLogic.Inst.pointedPawn;
        var selectedPawn = BLogic.Inst.selectedPawn;
        var gameState = BLogic.Inst.GetGamePlayState();
        //if (gameState == GamePlayState.PlayerControl)
        //{
        //    bool openMenu = false;
        //    if (pointedPawn == null)
        //    {
        //        openMenu = true;
        //    }
        //    if (selectedPawn == null && pointedPawn != null)//click confirm on a pawn
        //    {
        //        if (pointedPawn.camp == PawnCamp.Player)//player's pawn
        //        {
        //            if (pointedPawn.actionEnd == false)//move player's pawn
        //            {

        //            }
        //            else
        //            {
        //                openMenu = true;
        //            }
        //        }
        //        else if (pointedPawn.camp == PawnCamp.Villain)//todo toggle enemy's attack range
        //        {

        //        }
        //    }

        //    if (openMenu)
        //    {
        //        //open menu
        //        ShowActionMenu(null);
        //    }
        //}

        //else
        //if (gameState == GamePlayState.SelectingMoveDest)
        //{
        //    //situation 1: cursor is out of walkable area, do nothing,  play alert sound
        //    //situation 2: cursor is inside walkable area,pawn move
        //    //situation 3: cursor is under a enemy pawn ,show combat predict
        //    //situation 4: cursor is under a ally pawn and ready to heal this ally,show combat predict (heal)
        //    var cursorPos = BLogic.Inst.cursorPos;
        //    int cursorTileId = TBSMapService.Inst.GetTileId(cursorPos);
        //    if (pointedPawn == null)//confirm on empty tile
        //    {
        //        if (selectedPawn.moveTileIds.Contains(cursorTileId))//walk to this tile
        //        {
        //            Debugger.Log($"walk to tile {cursorPos}");
        //            BLogic.Inst.PawnStartMove(selectedPawn, cursorTileId);
        //        }
        //        else //alert sound
        //        {

        //        }
        //    }
        //    else //confirm on a pawn
        //    {
        //        bool canMoveToCursor = true;
        //        if (false == selectedPawn.moveTileIds.Contains(cursorTileId))
        //        {
        //            canMoveToCursor = false;
        //        }
        //        if (pointedPawn.camp == PawnCamp.Player || pointedPawn.camp == PawnCamp.PlayerAlly)
        //        {
        //            //todo can heal pointedPawn ?
        //        }
        //    }
        //}

        //else if (gameState == GamePlayState.UIActionMenu)
        //{
        //    ClickConfirmOnActionMenu();
        //}
    }

    void OnClickCancel(IEventMessage msg)
    {
        if (InputReceiver.InputInUI)
            UICancelAction?.Invoke();
        else
            BLogic.Inst.OnClickCancel();
        //return; 
        //var gameState = BLogic.Inst.GetGamePlayState();
        //var selectedPawn = BLogic.Inst.selectedPawn;
        //if (gameState == GamePlayState.SelectingMoveDest)
        //{
        //    //click cancel on path selecting, cursor relocate to seletedPawn's pos
        //    if (selectedPawn != null)
        //    {
        //        selectedPawn.tempPos = selectedPawn.curPosition;
        //        BLogic.Inst.CursorInputMoveTo(selectedPawn.curPosition);
        //        BLogic.Inst.SetNextGamePlayState(GamePlayState.PlayerControl);
        //        BattleDriver.Inst.MoveCursorObj();
        //        BLogic.Inst.selectedPawn = null;
        //        TBSMapService.Inst.ShowPawnCoverPlanes(selectedPawn);
        //    }
        //}
        //else if (gameState == GamePlayState.UIActionMenu)
        //{
        //    if (ui.actionCom.visible)
        //    {

        //        if (selectedPawn != null)
        //        {
        //            //todo check sub UI first: combatPredictUI/weaponSelectUI/pawnItemUI/TradeUI/TradeSelectPawnUI/
        //            //pawn model relocate to real pos, show move/atk planes

        //            focusedList = null;
        //            ui.actionCom.visible = false;
        //            selectedPawn.tempPos = selectedPawn.curPosition;
        //            selectedPawn.ResetPosition();
        //            BLogic.Inst.SetNextGamePlayState(GamePlayState.SelectingMoveDest);
        //            BattleDriver.Inst.MoveCursorObj();
        //            TBSMapService.Inst.ShowPawnCoverPlanes(selectedPawn);
        //        }
        //    }
        //}
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
        HideUIComp();

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
}
