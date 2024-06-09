using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UniFramework.Event;
using PackageMinigame;
public class UIPage_BankHeist : FUIBase
{

    UI_BankHeist ui;

    List<UI_VaultBar> uI_VaultBars;
    //key=slot index, value=tokenId
    Dictionary<int, int> openedSlotId;

    //store opened slot index
    HashSet<int> openedSlotHash;
    Dictionary<int, long> rewardDic;
    bool gameFinished = false;
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
        rewardDic = new Dictionary<int, long>();
    }
    protected override void OnShown()
    {
        base.OnShown();
        ui.list_vault.itemRenderer = ItemListRenderer;
        openedSlotId = new Dictionary<int, int>();
        openedSlotHash = new HashSet<int>();
        gameFinished = false;
        uI_VaultBars = new List<UI_VaultBar>();
        uI_VaultBars.Add(ui.bar1);
        uI_VaultBars.Add(ui.bar2);
        uI_VaultBars.Add(ui.bar3);
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
        FUIManager.Inst.ShowUI<UIPage_WorldUI>(FUIDef.FWindow.WorldPanel);
    }
    void RefreshRewards()
    {
        var gameData = MinigameService.Inst.gameBankHeist;
        for (int i = 0; i < 3; i++)
        {
            long moneyBase = gameData.rewardDic[i];
            RefreshOneRewards(i, moneyBase);
        }
    }
    void RefreshOneRewards(int index, long moneyBase)
    {
        //index = 0,1,2
        UI_VaultBar barItem = uI_VaultBars[index];
        //reward money = moneyBase * diceFactor * chapterPriceBase
        long chapterPriceBase = 1000;
        long money = moneyBase * chapterPriceBase;
        barItem.txt_des.text = "$" + money;
        rewardDic[index] = money;
        var gameData = MinigameService.Inst.gameBankHeist;
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
        if (gameFinished)
            return;
        int index = (int)(ec.sender as GComponent).data;
        var gameData = MinigameService.Inst.gameBankHeist;
        int tokenId = gameData.tokenList[GetOpenedCount()];
        openedSlotHash.Add(index);
        openedSlotId[index] = tokenId;
        RefreshList();
        CheckGameFinished();
    }
    void CheckGameFinished()
    {
        //calculate if this mini game is finished

        //key=tokenId, value=count
        Dictionary<int, int> tokenCountDic = new Dictionary<int, int>();
        foreach (int tokenId in openedSlotId.Values)
        {
            if (tokenCountDic.ContainsKey(tokenId) == false)
                tokenCountDic.Add(tokenId, 0);
            tokenCountDic[tokenId]++;
        }
        bool finished = false;
        int finish_tokenId = 0;
        foreach (var pair in tokenCountDic)
        {
            if (pair.Value >= 3)
            {
                finished = true;
                finish_tokenId = pair.Key;
                break;
            }
        }
        if (finished)
        {
            gameFinished = true;
            //Debug.Log($"bank heist finished token ID = {finish_tokenId}");
            var rewardBar = uI_VaultBars[finish_tokenId];
            //play anim
            rewardBar.anim_shake.Play(CloseUI);
            long moneyReward = rewardDic[finish_tokenId];
            TBSPlayer.UpdateGoldAmount(moneyReward);
            UIService.Inst.ShowMoneyAnim(moneyReward);
        }
    }
    void CloseUI()
    {
        GoBackToMainPage();
    }
}
