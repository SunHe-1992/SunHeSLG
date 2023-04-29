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
        UniEvent.AddListener(GameEventDefine.ClickCancel, OnClickCancel);

        OnWeaponChanged();
        AutoSelectPawn();
    }

    string backToPhase;
    public override void Refresh(object param)
    {
        base.Refresh(param);
        if (param != null)
            backToPhase = param.ToString();
    }
    protected override void OnHide()
    {
        base.OnHide();
        UniEvent.RemoveListener(GameEventDefine.InputAxis, OnInputAxis);
        UniEvent.RemoveListener(GameEventDefine.ClickCancel, OnClickCancel);

    }
    void OnClickCancel(IEventMessage msg)
    {
        if (this.isTop)
            OnBtnClose();
    }

    /// <summary>
    /// refresh on target pawn/weapon changed
    /// </summary>
    void RefreshContent()
    {
        //show combat predict content
        var playerPawn = BLogic.Inst.selectedPawn;
        RefreshPawnInfo(playerPawn, targetPawn, ui.comLeft);
        RefreshPawnInfo(targetPawn, playerPawn, ui.comRight);
    }
    void RefreshPawnInfo(Pawn self, Pawn targetPawn, UI_CombatPredictCom uiCom)
    {
        string pawnName = Translator.GetStr(self.charCfg.CharName);
        uiCom.txt_name.text = pawnName;
        RefreshWeaponCom(uiCom.weaponCom, self);
        uiCom.txt_hp.text = "" + self.HP;

        var selfAttr = self.GetCombatAttr();
        var targetAttr = targetPawn.GetCombatAttr();

        int showHit = selfAttr.Hit - targetAttr.Avoid;
        showHit = FixValue(showHit);
        int showCrit = selfAttr.CriticalRate - targetAttr.Dodge;
        showCrit = FixValue(showCrit);
        //pawn atk type  ph/mag/pn
        var dmgType = self.GetDamageType();
        string dmgName = Translator.GetStr(dmgType.ToString());
        int showDmg = PredictAttackDamage(self, targetPawn);
        FillLabel(uiCom.lbl_hit, "Hit", $"{showHit} %");
        FillLabel(uiCom.lbl_crit, "Crit", $"{showCrit} %");
        FillLabel(uiCom.lbl_dmg, dmgName, showDmg.ToString());

    }
    /// <summary>
    /// once attack damage
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <returns></returns>
    int PredictAttackDamage(Pawn attacker, Pawn defender)
    {
        int dmg = 0;
        var dmgType = attacker.GetDamageType();
        var atkerAttr = attacker.GetCombatAttr();
        var deferAttr = defender.GetCombatAttr();
        switch (dmgType)
        {
            case cfg.SLG.DamageType.PH:
                dmg = atkerAttr.PhAtk - deferAttr.Defence;
                break;
            case cfg.SLG.DamageType.MAG:
                dmg = atkerAttr.MagAtk - deferAttr.Resistance;
                break;
            case cfg.SLG.DamageType.PN:
                dmg = atkerAttr.PnAtk;
                break;
        }
        if (dmg < 0) dmg = 0;
        return dmg;
    }
    int FixValue(int value)
    {
        if (value < 0)
            value = 0;
        if (value > 100)
            value = 100;
        return value;
    }
    void RefreshWeaponCom(UI_ItemComp com, Pawn pawn)
    {

    }
    void FillLabel(UI_Label2 lbl, string name, string valueStr)
    {
        lbl.txt_value.text = valueStr;
        lbl.title = name;
    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
        FUIManager.Inst.ShowUI<UIPage_BattleMain>(FUIDef.FWindow.BattlePanel, OnBattlePanelOpen);
    }
    void OnBattlePanelOpen(FUIBase fui)
    {
        Debugger.Log("combat predict on btn close = " + backToPhase);
        if (backToPhase == "AttackMenu")
        {
            UniEvent.SendMessage(GameEventDefine.ShowActionMenu);
        }
        else if (backToPhase == "WeaponMenu")
        {
            UniEvent.SendMessage(GameEventDefine.ShowWeaponSelectUI);
        }
        else if (backToPhase == "TileSelect")
        {

        }
        backToPhase = null;
    }

    List<Pawn> targetList;
    int curTargetIdx;
    int targetPawnSid = 0;
    Pawn targetPawn;
    void OnWeaponChanged()
    {
        var pawn = BLogic.Inst.selectedPawn;
        targetList = BLogic.Inst.SelectFoes(pawn.GetAtkRangeMin(), pawn.GetAtkRangeMax());

    }

    void OnTargetPawnChanged(Pawn _pawn)
    {
        if (_pawn == null) return;
        var logicInst = BLogic.Inst;
        logicInst.CursorMoveToTargetPawn(_pawn);

        if (targetPawnSid != _pawn.sequenceId)
        {
            targetPawnSid = _pawn.sequenceId;
            // refresh on target pawn changed
            targetPawn = _pawn;
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
