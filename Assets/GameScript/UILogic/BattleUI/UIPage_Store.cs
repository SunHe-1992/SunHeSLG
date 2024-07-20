using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UniFramework.Event;
using PackageBattle;
using SunHeTBS;
public class UIPage_Store : FUIBase
{

    UI_StoreUI ui;
    UserItem buyingItem;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_StoreUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        //ui.btn_ok.onClick.Set(BtnOKClick);
        ui.btn_close.onClick.Set(OnBtnClose);
        ui.inventory_list.itemRenderer = this.ListItemRenderer;
        ui.itemDetailCom.btn_close.onClick.Set(HideDetailCom);
        ui.itemDetailCom.btn_use.title = "Buy";
        ui.itemDetailCom.btn_use.onClick.Set(ClickBuyItem);

    }
    protected override void OnShown()
    {
        base.OnShown();
        buyingItem = null;
        GenerateStoreData();
    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

        RefreshContent();
    }
    protected override void OnHide()
    {
        base.OnHide();
        buyingItem = null;
    }
    void BtnOKClick()
    {
        FUIManager.Inst.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
        FUIManager.Inst.HideUI(this);
    }

    void RefreshContent()
    {
        HideDetailCom();
        int count = this.itemList.Count;
        this.ui.inventory_list.numItems = count;
    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }

    void ListItemRenderer(int index, GObject obj)
    {
        var mItem = obj as UI_InventoryItem;
        var info = itemList[index];
        var cfg = ConfigManager.table.Item.Get(info.itemId);

        UIService.Inst.ShowItemComp(mItem, info.itemId, info.itemCount);

        mItem.data = index;
        mItem.onClick.Set(OnItemClick);

        mItem.txt_count.text = "$" + cfg.Price;
    }
    void OnItemClick(EventContext ec)
    {
        int index = (int)(ec.sender as GObject).data;
        var info = TBSPlayer.UserDetail.items[index];
        buyingItem = info;
        Debugger.Log("click item id  = " + info.itemId);
        var cfg = ConfigManager.table.Item.Get(info.itemId);
        if (cfg.UseOutCombat)
        {

        }
        UIService.Inst.ShowItemDetail(this.ui.itemDetailCom, info.itemId, info.itemCount);
        ui.itemDetailCom.itemCom.txt_count.text = "$" + cfg.Price;
        ui.itemDetailCom.btn_use.onClick.Set(ClickBuyItem);
    }
    void HideDetailCom()
    {
        ui.itemDetailCom.visible = false;
    }

    #region store data

    List<UserItem> itemList;
    void GenerateStoreData()
    {
        itemList = new List<UserItem>();
        itemList.Add(new UserItem(100, 333));
        itemList.Add(new UserItem(101, 555));
    }
    #endregion

    void ClickBuyItem()
    {
        if (buyingItem != null)
        {
            var cfg = ConfigManager.table.Item.Get(buyingItem.itemId);
            if (TBSPlayer.IsAffordGold(cfg.Price))
            {
                TBSPlayer.SpendGold(cfg.Price);
                TBSPlayer.InsertItem(buyingItem.itemId, 1);
                HideDetailCom();
                RefreshContent();
                UniEvent.SendMessage(GameEventDefine.BuyInShop);
            }
            else
            {
                Debugger.Log("can not afford");
            }
        }
    }
}
