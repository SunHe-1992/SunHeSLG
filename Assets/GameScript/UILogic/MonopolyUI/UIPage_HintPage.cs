using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UniFramework.Event;
using PackageShared;
using SunHeTBS;
public class UIPage_HintPage : FUIBase
{
    UI_HintPage ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_HintPage;
        this.uiShowType = UIShowType.TIPS;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;

    }
    protected override void OnShown()
    {
        base.OnShown();

    }
    Queue<long> dataQueue = new Queue<long>();
    public override void Refresh(object param)
    {
        base.Refresh(param);
        dataQueue.Enqueue((long)param);
        RefreshContent();
    }
    protected override void OnHide()
    {
        base.OnHide();

    }

    void RefreshContent()
    {
        if (ui.numberComp.anim1.playing)
            return;
        if (dataQueue.Count > 0)
        {
            long moneyValue = dataQueue.Dequeue();
            string moneyStr = moneyValue.ToString();
            if (moneyValue < 0)
                moneyStr = "" + moneyStr;
            else
                moneyStr = "+" + moneyStr;
            this.ui.numberComp.title = moneyStr;
            ui.numberComp.anim1.Play(MoneyPlayCallback);
        }
        else
            MoneyAnimPlayEnd();
    }
    void MoneyPlayCallback()
    {
        RefreshContent();
    }
    void MoneyAnimPlayEnd()
    {
        UniEvent.SendMessage(GameEventDefine.POINTS_CHANGED);
        OnBtnClose();
    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
}
