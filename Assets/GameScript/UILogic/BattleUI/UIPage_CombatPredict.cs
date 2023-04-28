using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageBattle;
using UnityEngine;
using UniFramework.Event;
using SunHeTBS;
public class UIPage_CombatPredict : FUIBase
{
    //select target pawn, self weapon 
    UI_CombatPredict ui;
    public static int leftPawnSid = 0;
    public static int rightPawnSid = 0;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_CombatPredict;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        this.ui.btn_close.onClick.Set(OnBtnClose);
    }
    protected override void OnShown()
    {
        base.OnShown();
        InputReceiver.SwitchInputToUI();

        UniEvent.AddListener(GameEventDefine.InputAxis, OnInputAxis);


        OnWeaponChanged();
        SelectNextPawn();
    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

    }
    protected override void OnHide()
    {
        base.OnHide();
        UniEvent.RemoveListener(GameEventDefine.InputAxis, OnInputAxis);

    }


    /// <summary>
    /// refresh on target pawn/weapon changed
    /// </summary>
    void RefreshContent()
    {
        //show combat predict content
        var p = BLogic.Inst.selectedPawn;
        p.CalculateCombatAttr(p);
        //var attr = p.GetCombatAttr();

    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }

    List<Pawn> targetList;
    int curTargetIdx;
    int targetPawnSid = 0;
    void OnWeaponChanged()
    {
        var pawn = BLogic.Inst.selectedPawn;
        targetList = BLogic.Inst.SelectFoes(pawn.GetAtkRangeMin(), pawn.GetAtkRangeMax());

    }

    void OnTargetPawnChanged(Pawn targetPawn)
    {
        if (targetPawn == null) return;
        var logicInst = BLogic.Inst;
        logicInst.CursorMoveToTargetPawn(targetPawn);

        if (targetPawnSid != targetPawn.sequenceId)
        {
            targetPawnSid = targetPawn.sequenceId;
            // refresh on target pawn changed
            RefreshContent();
        }
    }
    void SelectNextPawn()
    {
        curTargetIdx++;
        if (curTargetIdx > targetList.Count - 1)
            curTargetIdx = 0;
        if (curTargetIdx < 0)
            curTargetIdx = targetList.Count - 1;
        SelectPawn();

    }
    void SelectPrevPawn()
    {
        curTargetIdx--;
        if (curTargetIdx > targetList.Count - 1)
            curTargetIdx = 0;
        if (curTargetIdx < 0)
            curTargetIdx = targetList.Count - 1;
        SelectPawn();
    }
    void AutoSelectPawn()
    {
        curTargetIdx = 0;
        SelectPawn();
    }
    void SelectPawn()
    {
        if (curTargetIdx < 0 || curTargetIdx >= targetList.Count)
        {
            ;
        }
        if (targetList != null)
        {
            var pawn = targetList[curTargetIdx];
            OnTargetPawnChanged(pawn);
        }
    }

    void OnInputAxis(IEventMessage msg)
    {
        var inputInst = InputReceiver.Inst;
        //right=x+1 up=z+1
        bool pressNext = false;
        if (inputInst.axisDown || inputInst.axisRight)
            pressNext = true;
        else if (inputInst.axisLeft || inputInst.axisUp)
            pressNext = false;

        if (pressNext)
            SelectNextPawn();
        else
            SelectPrevPawn();
    }
}
