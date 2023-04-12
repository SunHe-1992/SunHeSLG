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
public class UIPage_BattleMain : FUIBase
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
    }
    #region battle map input control
    void OnInputAxis(IEventMessage msg)
    {
        var inputInst = InputReceiver.Inst;
        //right=x+1 up=z+1
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

    void OnCursorMoved(IEventMessage msg)
    {
        if (BLogic.Inst.oldCursorPos != BLogic.Inst.cursorPos)
        {
            Pawn selectedPawn = BLogic.Inst.selectedPawn;
            if (selectedPawn != null) //selected a pawn ,show its info
            {
                //todo ui show pawn summary
                //todo map show planes
                //Debugger.Print($"selected a pawn {selectedPawn.ToString()}");
                TBSMapService.Inst.ShowPawnCoverPlanes(selectedPawn);
            }
            else
            {
                TBSMapService.Inst.UnspawnAllCoverPlanes();
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
        var selectedPawn = BLogic.Inst.selectedPawn;
        if (selectedPawn == null) //todo open menu
        {

        }
        else
        {
            var pawnCamp = selectedPawn.camp;
            if (pawnCamp == PawnCamp.Player)
            {

            }
            else if (pawnCamp == PawnCamp.Villain || pawnCamp == PawnCamp.Neutral)
            {
                //todo click enemy pawn ,toggle pawn's atk range planes
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
