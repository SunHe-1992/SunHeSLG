using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UniFramework.Event;
using PackageMonopoly;
public class UIPage_BankHeist : FUIBase
{

    UI_BankHeist ui;

    List<UI_VaultBar> uI_VaultBars;
    //key=slot index, value=tokenId
    Dictionary<int, int> openedSlotId;

    //store opened slot index
    HashSet<int> openedSlotHash;
    int GetOpenedCount()
    {
        return openedSlotHash.Count;
    }
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_BankHeist;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        uI_VaultBars = new List<UI_VaultBar>()
        { ui.bar1,ui.bar2,ui.bar3};

        ui.list_vault.itemRenderer = ItemListRenderer;
    }
    protected override void OnShown()
    {
        base.OnShown();
        openedSlotId = new Dictionary<int, int>();
        openedSlotHash = new HashSet<int>();
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


    void RefreshContent()
    {
        RefreshRewards();
        RefreshList();

    }
    void GoBackToMainPage()
    {
        FUIManager.Inst.HideUI(this);
        FUIManager.Inst.ShowUI<UIPage_MonopolyMain>(FUIDef.FWindow.MonopolyMain);
    }
    void RefreshRewards()
    {
        var gameData = MonopolyService.Inst.gameBankHeist;
        for (int i = 0; i < 3; i++)
        {
            long moneyBase = gameData.rewardDic[i];
            RefreshOneRewards(i, moneyBase);
        }
    }
    void RefreshOneRewards(int index, long moneyBase)
    {
        //index = 1,2,3
        UI_VaultBar barItem = uI_VaultBars[index];
        //reward money = moneyBase * diceFactor * chapterPriceBase
        int diceFactor = MonoPlayer.diceFactor;
        long chapterPriceBase = MLogic.Inst.chapterCfg.PriceBase;
        long money = moneyBase * diceFactor * chapterPriceBase;
        barItem.txt_des.text = "$" + money;


        var gameData = MonopolyService.Inst.gameBankHeist;
        int thisRewardOpenedSum = 0;
        for (int i = 0; i < GetOpenedCount(); i++)
        {
            if (index == gameData.tokenList[i])
            {
                thisRewardOpenedSum++;
            }
        }
        List<UI_VaultIcon> iconList = new List<UI_VaultIcon>() { barItem.item1, barItem.item2, barItem.item3 };
        for (int i = 0; i < iconList.Count; i++)
        {
            var icon = iconList[i];
            icon.ctrl_icon.selectedIndex = index;
            if (thisRewardOpenedSum > i)
                icon.ctrl_active.selectedIndex = 1;
            else
                icon.ctrl_active.selectedIndex = 0;
        }
    }
    void RefreshList()
    {
        this.ui.list_vault.numItems = 12;

    }
    void ItemListRenderer(int index, GObject go)
    {
        UI_VaultItem obj = go as UI_VaultItem;

        bool isOpened = openedSlotHash.Contains(index);
        if (isOpened)
        {
            obj.ctrl_opened.selectedIndex = 1;

            int tokenId = openedSlotId[index];
            obj.ctrl_icon.selectedIndex = tokenId;
            obj.onClick.Clear();
        }
        else
        {
            obj.ctrl_opened.selectedIndex = 0;
            obj.data = index;
            obj.onClick.Set(VaultItemOnClick);
        }
    }
    void VaultItemOnClick(EventContext ec)
    {
        int index = (int)(ec.sender as GComponent).data;
        var gameData = MonopolyService.Inst.gameBankHeist;
        int tokenId = gameData.tokenList[GetOpenedCount()];
        openedSlotHash.Add(index);
        openedSlotId[index] = tokenId;
        RefreshList();

        //todo calculate if this mini game is finished
    }
}
