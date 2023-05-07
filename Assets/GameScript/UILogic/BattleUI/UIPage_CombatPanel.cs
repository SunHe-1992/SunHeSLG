using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageBattle;
using UnityEngine;
using UniFramework.Event;
using UniFramework.Tween;
using SunHeTBS;
public class UIPage_CombatPanel : FUIBase
{

    UI_CombatPanel ui;
    readonly float hitAnimTime = 0.2f;
    readonly float critAnimTime = 0.3f;
    readonly float endWaitTime = 1.0f;

    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_CombatPanel;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;

        //ui.btn_close.onClick.Set(OnBtnClose);
        UI_CombatHpBar leftBar = ui.hpbar.barLeft;
        leftBar.reverse = true;
        leftBar.barFade.pivotX = 1;
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
        sQueue.Clear();
        leftSid = 0;
        rightSid = 0;
    }

    bool IsLeftCamp(PawnCamp pc)
    {
        return pc == PawnCamp.Player || pc == PawnCamp.PlayerAlly;
    }
    Queue<StrikeInfo> sQueue = null;
    int leftSid = 0;
    int rightSid = 0;
    void RefreshContent()
    {
        var blInst = BLogic.Inst;
        sQueue = new Queue<StrikeInfo>();
        foreach (StrikeInfo sinfo in blInst.strikeList)
        {
            sQueue.Enqueue(sinfo);
        }
        var strikeInfo = blInst.strikeList[0];
        var attacker = blInst.GetPawnBySid(strikeInfo.attackerSid);
        var defender = blInst.GetPawnBySid(strikeInfo.defenderSid);
        bool atkerIsLeft = IsLeftCamp(attacker.camp);
        Pawn leftPawn, rightPawn;
        if (atkerIsLeft)
        {
            leftSid = strikeInfo.attackerSid;
            rightSid = strikeInfo.defenderSid;
            leftPawn = attacker;
            rightPawn = defender;
        }
        else
        {
            rightSid = strikeInfo.attackerSid;
            leftSid = strikeInfo.defenderSid;
            leftPawn = defender;
            rightPawn = attacker;
        }
        //show dmg, hit, crit
        SetUpCombatBar(ui.infoLeft, leftPawn, rightPawn);
        SetUpCombatBar(ui.infoRight, rightPawn, leftPawn);

        DelayInvoker.Inst.DelayInvoke(CheckNextAnim, hitAnimTime);
        //foreach (StrikeInfo info in blInst.strikeList)
        //{

        //    SetUpHpValue(info, atkerIsLeft);

        //    Debugger.Log($"step {step} attacker {attacker} hp {info.attackerHP}/{info.attackerHPMax} hpChange={info.attackerHPChange}");
        //    Debugger.Log($"step {step} defender {defender} hp {info.defenderHP}/{info.defenderHPMax} hpChange={info.defenderHPChange}");
        //    step++;
        //}
    }
    void CheckNextAnim()
    {
        if (!this.ui.visible) return;
        if (sQueue.Count == 0)//
        {
            ui.anim_hide.Play();
            DelayInvoker.Inst.DelayInvoke(OnBtnClose, endWaitTime);
        }
        else
        {
            var info = sQueue.Dequeue();
            bool atkIsLeft = info.attackerSid == leftSid;
            SetUpHpValue(atkIsLeft, info.attackerHP, info.attackerHPMax, info.attackerHPChange);
            SetUpHpValue(!atkIsLeft, info.defenderHP, info.defenderHPMax, info.defenderHPChange);

            DelayInvoker.Inst.DelayInvoke(CheckNextAnim, hitAnimTime);
        }
    }

    void SetUpCombatBar(UI_CombatBar com, Pawn pawn1, Pawn pawn2)
    {
        int dmg = AttrCalculator.PredictDamage(pawn1, pawn2);
        com.txt_value1.text = "" + dmg;

        int hit = AttrCalculator.PredictHit(pawn1, pawn2);
        com.txt_value2.text = "" + hit;

        int crit = AttrCalculator.PredictCrit(pawn1, pawn2);
        com.txt_value3.text = "" + crit;
    }
    void SetUpHpValue(bool isLeft, int hpValue, int hpMax, int hpChange)
    {
        var com = ui.hpbar;
        string hpStr = "" + (hpValue + hpChange);
        if (isLeft)
        {
            com.HPLeft.text = hpStr;
        }
        else
        {
            com.HPRight.text = hpStr;
        }
        float pctStart = (float)hpValue / hpMax;
        float pctEnd = (float)(hpValue + hpChange) / hpMax;
        UI_CombatHpBar sliderBar = null;
        if (isLeft)
            sliderBar = com.barLeft;
        else
            sliderBar = com.barRight;

        sliderBar.max = 1;
        //value is for tweening
        sliderBar.value = pctStart;
        if (hpChange != 0)
            sliderBar.TweenValue(pctEnd, hitAnimTime);
        sliderBar.barFade.width = (sliderBar.width - 2) * pctEnd;
    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
}
