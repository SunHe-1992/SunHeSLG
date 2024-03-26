using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UniFramework.Event;
using PackageMonopoly;

public class UIPage_SlotGame : FUIBase
{

    UI_SlotGame ui;
    List<GList> wheelList;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_SlotGame;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        //ui.btn_ok.onClick.Set(BtnOKClick);
        ui.btn_slot.onClick.Set(OnBtnSlot);

        wheelList = new List<GList>() { ui.wheel1.list_icons, ui.wheel2.list_icons, ui.wheel3.list_icons };

        wheelList[0].itemRenderer = WheelItemRenderer1;
        wheelList[1].itemRenderer = WheelItemRenderer2;
        wheelList[2].itemRenderer = WheelItemRenderer3;
        foreach (var wheel in wheelList)
        {
            wheel.SetVirtualAndLoop();
        }
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

    }
    void BtnOKClick()
    {
        FUIManager.Inst.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
        FUIManager.Inst.HideUI(this);
    }

    void RefreshContent()
    {
        var outList = MonopolyService.Inst.slotGameData.slotIdList;

        for (int i = 0; i < wheelList.Count; i++)
        {
            var innerList = outList[i];
            GList wheel = wheelList[i];
            wheel.SetVirtualAndLoop();
            wheel.numItems = innerList.Count;
            wheel.scrollPane.SetPosY(0, false);
        }

    }
    void GoBackToMainPage()
    {
        FUIManager.Inst.HideUI(this);
        FUIManager.Inst.ShowUI<UIPage_MonopolyMain>(FUIDef.FWindow.MonopolyMain);
    }
    void OnBtnSlot()
    {
        for (int i = 0; i < wheelList.Count; i++)
        {
            var wList = wheelList[i];
            DelayInvoker.Inst.StartCoroutine(PlayScrollAnim(wList, i * 0.5f));
        }
        DelayInvoker.Inst.DelayInvoke(PlayReward, 5f);
    }

    IEnumerator PlayScrollAnim(GList glist, float startDelay)
    {
        yield return new WaitForSeconds(startDelay);
        float posY = 0f;
        float timePlayed = 0;
        float animSpan = 2.5f;
        float initStep = 60f;
        float curStep = initStep;
        float keepStepTime = 1.0f;
        while (animSpan > 0)
        {
            animSpan -= Time.deltaTime;
            timePlayed += Time.deltaTime;
            //glist.scrollPane.ScrollDown(true);
            if (timePlayed > keepStepTime)
            {
                curStep = Mathf.Lerp(initStep, 0, (animSpan - timePlayed));
            }
            posY += curStep;
            glist.scrollPane.SetPosY(posY, false);
            yield return null;
        }
        glist.scrollPane.SetPosY(64f * 5, true);
    }

    void WheelItemRenderer1(int index, GObject go)
    {
        var outList = MonopolyService.Inst.slotGameData.slotIdList;
        int icon = outList[0][index];
        WheelItemRenderer(index, go, icon);
    }
    void WheelItemRenderer2(int index, GObject go)
    {
        var outList = MonopolyService.Inst.slotGameData.slotIdList;
        int icon = outList[1][index];
        WheelItemRenderer(index, go, icon);
    }
    void WheelItemRenderer3(int index, GObject go)
    {
        var outList = MonopolyService.Inst.slotGameData.slotIdList;
        int icon = outList[2][index];
        WheelItemRenderer(index, go, icon);
    }
    void WheelItemRenderer(int index, GObject go, int icon)
    {
        UI_SlotIconCom obj = go as UI_SlotIconCom;
        obj.ctrl_icon.selectedIndex = icon;
        //obj.txt_number.text = "" + index;
    }

    void PlayReward()
    {
        long amount = MonopolyService.Inst.slotGameData.rewardMoney * MonoPlayer.diceFactor;
        MonoPlayer.UpdateGoldAmount(amount);
        UIService.Inst.ShowMoneyAnim(amount);
        GoBackToMainPage();
    }
}
