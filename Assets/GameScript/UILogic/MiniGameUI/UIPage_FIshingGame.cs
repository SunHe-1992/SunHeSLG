using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UniFramework.Event;
using PackageMinigame;

public class UIPage_Fishing : FUIBase
{

    UI_Fishing ui;
    UI_FishingCircle circleComp;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_Fishing;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        //ui.btn_ok.onClick.Set(BtnOKClick);
        //ui.btn_close.onClick.Set(OnBtnClose);
        circleComp = ui.compCircle;
        ui.clickLoader.onClick.Set(OnRodClick);
    }
    protected override void OnShown()
    {
        base.OnShown();

        circleComp.anim_bad.Stop();
        circleComp.anim_good.Stop();
        circleComp.anim_excellent.Stop();
        circleComp.anim_waiting.Stop();

        SetStateWait();
        circleComp.anim_waiting.Play(WaitingPlayDone);
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
    void BtnOKClick()
    {
        FUIManager.Inst.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
        FUIManager.Inst.HideUI(this);
    }

    void RefreshContent()
    {


    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }

    void WaitingPlayDone()
    {
        SetStateFishing();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (state == FishingState.Fishing)
        {
            OnUpdateFishing();
        }
        UpdateProgressText();
    }
    FishingState state = FishingState.Wait;
    enum FishingState
    {
        Wait = 0,
        Fishing = 1,
        Finished = 2,
    }
    void SetStateWait()
    {
        progress = 0;
        curRodCount = rodCountMax;
        state = FishingState.Wait;
        circleComp.ctrl_status.selectedIndex = 1;
    }
    void SetStateFishing()
    {
        SetMotionCircleWidth(biggestWidth);
        circleComp.ctrl_status.selectedIndex = 2;
        state = FishingState.Fishing;
    }
    void UpdateProgressText()
    {
        string pgsStr = string.Format("{0:N2}", this.progress * 100f);
        ui.txt_rodCount.text = $"Fish Rod Count: {this.curRodCount}\nProgress {pgsStr}%";
    }
    #region motion circle control
    int rodCountMax = 4;
    int curRodCount = 0;
    readonly float biggestWidth = 455f;
    readonly float smallestWidth = 55f;
    readonly float targetWidth = 250f;
    float curWidth = 0;
    float shrinkSpeed = 133f / 30f;

    float progress = 0;
    void OnUpdateFishing()
    {
        if (curWidth > smallestWidth)
        {
            curWidth -= shrinkSpeed;
        }
        else
        {
            OnRodClick();
        }
        SetMotionCircleWidth(curWidth);
    }
    void SetMotionCircleWidth(float width)
    {
        curWidth = width;
        circleComp.motionCircle.width = curWidth;
        circleComp.motionCircle.height = curWidth;
    }
    void CostRodCount()
    {
        curRodCount--;

        if (curRodCount <= 0)
        {
            //todo game failed;
            FishingGameFinish();
        }
        else
        {
            SetMotionCircleWidth(biggestWidth);
        }
    }

    void OnRodClick()
    {
        if (this.state == FishingState.Fishing)
        {
            float distance = Mathf.Abs(curWidth - targetWidth);
            float addProgress = RodDistanceToProgress(distance);
            AddProgress(addProgress);
            Debugger.Log($"distance = {distance} add pgs = {addProgress}  ,curwidth={curWidth} , targetWidth={targetWidth}");

            CostRodCount();
            SetMotionCircleWidth(biggestWidth);
            DisplayProgressAnim(addProgress);
            this.state = FishingState.Wait;
            DelayInvoker.Inst.DelayInvoke(ResumeFishing, 0.5f);
            UIService.Inst.PlaySound("ui://AudioPackage/SkillUp");//play sound SkillUp
        }

    }
    void ResumeFishing()
    {
        this.state = FishingState.Fishing;
    }

    float perfectProgress = 0.40f;
    float RodDistanceToProgress(float distance)
    {
        //distance < 20 perfect 0.4f, <50 good 0.,else bad
        float pgs = perfectProgress - (distance / 450f);
        if (pgs > perfectProgress)
            pgs = perfectProgress;
        if (pgs < 0) pgs = 0;
        return pgs;
    }
    void AddProgress(float pgs)
    {
        this.progress += pgs;
    }
    void DisplayProgressAnim(float addProgress)
    {
        if (addProgress >= perfectProgress * 0.9f) //perfect
        {
            circleComp.anim_excellent.Play();
        }
        else if (addProgress >= perfectProgress * 0.7f)//good
        {
            circleComp.anim_good.Play();
        }
        else //bad
        {
            circleComp.anim_bad.Play();
        }
    }
    #endregion

    void FishingGameFinish()
    {
        this.state = FishingState.Finished;

        long amount = 9999;
        UIService.Inst.ShowMoneyAnim(amount);
        TBSPlayer.UpdateGoldAmount(amount);
        GoBackToMainPage();
    }
    void GoBackToMainPage()
    {
        FUIManager.Inst.HideUI(this);
        FUIManager.Inst.ShowUI<UIPage_WorldUI>(FUIDef.FWindow.WorldPanel);

    }
}
