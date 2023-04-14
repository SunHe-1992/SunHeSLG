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
        UniEvent.AddListener(GameEventDefine.CURSOR_MOVED, OnCursorMoved);
        UniEvent.AddListener(GameEventDefine.InputAxis, OnInputAxis);
        UniEvent.AddListener(GameEventDefine.ClickConfirm, OnClickConfirm);
        UniEvent.AddListener(GameEventDefine.PhaseSwitch, OnPhaseSwitch);
        UniEvent.AddListener(GameEventDefine.ShowSelectPawn, ShowSelectPawn);
        UniEvent.AddListener(GameEventDefine.ShowActionMenu, ShowActionMenu);
        UniEvent.AddListener(GameEventDefine.ClickCancel, OnClickCancel);

        //test  instant switch to map pawn control
        InputReceiver.Inst.inputComp.SwitchCurrentActionMap("Player");

    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

        RefreshContent();
    }
    protected override void OnHide()
    {
        base.OnHide();
        UniEvent.RemoveListener(GameEventDefine.CURSOR_MOVED, OnCursorMoved);
        UniEvent.RemoveListener(GameEventDefine.InputAxis, OnInputAxis);
        UniEvent.RemoveListener(GameEventDefine.ClickConfirm, OnClickConfirm);
        UniEvent.RemoveListener(GameEventDefine.PhaseSwitch, OnPhaseSwitch);
        UniEvent.RemoveListener(GameEventDefine.ShowSelectPawn, ShowSelectPawn);
        UniEvent.RemoveListener(GameEventDefine.ShowActionMenu, ShowActionMenu);
        UniEvent.RemoveListener(GameEventDefine.ClickCancel, OnClickCancel);

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
    bool AllowMapCursorMoving()
    {
        var gameState = BLogic.Inst.GetGamePlayState();
        if (gameState == GamePlayState.SelectingPawn ||
            gameState == GamePlayState.SelectingMoveDest ||
            gameState == GamePlayState.UICombatPredict)
            return true;
        return false;
    }
    void OnInputAxis(IEventMessage msg)
    {
        var inputInst = InputReceiver.Inst;
        //right=x+1 up=z+1
        if (AllowMapCursorMoving())
        {
            var cursorObj = BattleDriver.Inst.CursorObj;
            if (cursorObj != null)
            {
                int xAdd = 0;
                int zAdd = 0;
                if (inputInst.axisDown) zAdd = -1;
                else if (inputInst.axisUp) zAdd = 1;
                if (inputInst.axisLeft) xAdd = -1;
                else if (inputInst.axisRight) xAdd = 1;
                BLogic.Inst.CursorInputMove(xAdd, zAdd);
                BattleDriver.Inst.MoveCursorObj();
                UniEvent.SendMessage(GameEventDefine.CURSOR_MOVED);
            }
        }
    }

    void OnCursorMoved(IEventMessage msg)
    {
        var gameState = BLogic.Inst.GetGamePlayState();

        if (AllowMapCursorMoving())
        {
            if (BLogic.Inst.oldCursorPos != BLogic.Inst.cursorPos)
            {
                if (gameState == GamePlayState.SelectingPawn)
                {
                    Pawn selectedPawn = BLogic.Inst.pointedPawn;
                    if (selectedPawn != null) //cursor pointed at a pawn ,show its info
                    {
                        //todo ui show pawn summary
                        TBSMapService.Inst.ShowPawnCoverPlanes(selectedPawn);
                    }
                    else
                    {
                        TBSMapService.Inst.UnspawnAllCoverPlanes();
                    }
                }
                else if (gameState == GamePlayState.SelectingMoveDest)
                {

                }
            }
        }
    }


    /*player pawn control state:
     * 1 not selected any pawn,move to pawn show its summary 
       2 selected players pawn, move cursor selected destination
       3 confirm and perform pawn walking
    */
    void OnClickConfirm(IEventMessage msg)
    {
        var pointedPawn = BLogic.Inst.pointedPawn;
        var selectedPawn = BLogic.Inst.selectedPawn;
        var gameState = BLogic.Inst.GetGamePlayState();
        if (gameState == GamePlayState.SelectingPawn)
        {
            bool openMenu = false;
            if (pointedPawn == null)
            {
                openMenu = true;
            }
            if (selectedPawn == null && pointedPawn != null)//click confirm on a pawn
            {
                if (pointedPawn.camp == PawnCamp.Player)//player's pawn
                {
                    if (pointedPawn.actionEnd == false)//move player's pawn
                    {
                        BLogic.Inst.SetNextGamePlayState(GamePlayState.SelectingMoveDest);
                    }
                    else
                    {
                        openMenu = true;
                    }
                }
                else if (pointedPawn.camp == PawnCamp.Villain)//todo toggle enemy's attack range
                {

                }
            }

            if (openMenu)
            {
                //todo open menu
            }
        }

        else if (gameState == GamePlayState.SelectingMoveDest)
        {
            //situation 1: cursor is out of walkable area, do nothing,  play alert sound
            //situation 2: cursor is inside walkable area,pawn move
            //situation 3: cursor is under a enemy pawn ,show combat predict
            //situation 4: cursor is under a ally pawn and ready to heal this ally,show combat predict (heal)
            var cursorPos = BLogic.Inst.cursorPos;
            int cursorTileId = TBSMapService.Inst.GetTileId(cursorPos);
            if (pointedPawn == null)//confirm on empty tile
            {
                if (selectedPawn.moveTileIds.Contains(cursorTileId))//walk to this tile
                {
                    Debugger.Log($"walk to tile {cursorPos}");
                    BLogic.Inst.PawnStartMove(selectedPawn, cursorTileId);
                }
                else //alert sound
                {

                }
            }
            else //confirm on a pawn
            {
                bool canMoveToCursor = true;
                if (false == selectedPawn.moveTileIds.Contains(cursorTileId))
                {
                    canMoveToCursor = false;
                }
                if (pointedPawn.camp == PawnCamp.Player || pointedPawn.camp == PawnCamp.PlayerAlly)
                {
                    //todo can heal pointedPawn ?
                }
            }
        }
    }

    void OnClickCancel(IEventMessage msg)
    {
        var gameState = BLogic.Inst.GetGamePlayState();
        var selectedPawn = BLogic.Inst.selectedPawn;
        if (gameState == GamePlayState.SelectingMoveDest)
        {
            //click cancel on path selecting, cursor relocate to seletedPawn's pos
            if (selectedPawn != null)
            {
                selectedPawn.tempPos = selectedPawn.curPosition;
                BLogic.Inst.CursorInputMoveTo(selectedPawn.curPosition);
                BLogic.Inst.SetNextGamePlayState(GamePlayState.SelectingPawn);
                BattleDriver.Inst.MoveCursorObj();
                BLogic.Inst.selectedPawn = null;
                UniEvent.SendMessage(GameEventDefine.CURSOR_MOVED);
            }
        }
        else if (gameState == GamePlayState.UIActionMenu)
        {
            if (ui.actionCom.visible)
            {

                if (selectedPawn != null)
                {
                    //todo check sub UI first: combatPredictUI/weaponSelectUI/pawnItemUI/TradeUI/TradeSelectPawnUI/
                    //pawn model relocate to real pos, show move/atk planes
                    ui.actionCom.visible = false;
                    BLogic.Inst.oldCursorPos = selectedPawn.tempPos;
                    selectedPawn.tempPos = selectedPawn.curPosition;
                    selectedPawn.ResetPosition();
                    BLogic.Inst.SetNextGamePlayState(GamePlayState.SelectingMoveDest);
                    BattleDriver.Inst.MoveCursorObj();
                    UniEvent.SendMessage(GameEventDefine.CURSOR_MOVED);
                }
            }
        }
    }
    #endregion

    void OnPhaseSwitch(IEventMessage msg)
    {
        HideUIComp();
        int campNum = (int)BLogic.Inst.curCamp;
        ui.phaseCom.visible = true;
        ui.phaseCom.ctrl_phase.selectedIndex = campNum;
        ui.anim_phase.Play(PhaseSwitchEnd);
    }
    void PhaseSwitchEnd()
    {
        HideUIComp();
        BLogic.Inst.PhaseSwitchDone();
    }

    void ShowSelectPawn(IEventMessage msg)
    {
        HideUIComp();

    }
}
