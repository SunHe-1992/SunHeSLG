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
public partial class UIPage_BattleMain : FUIBase
{
    #region Show Inventory 
    /// <summary>
    /// action menu: Items
    /// </summary>
    private void OpenInventoryCom()
    {
        ui.inventoryCom.visible = true;
        ShowInventoryContent();

        this.focusedList = ui.inventoryCom.list_inventory;
        this.UIConfirmAction = OnInventoryConfirm;
        this.UICancelAction = HideInventoryCom;
    }


    private void HideInventoryCom()
    {
        ui.inventoryCom.visible = false;
        ShowActionMenu(null);
    }

    List<Item> inventoryItemList = null;
    void ShowInventoryContent()
    {
        var sPawn = BLogic.Inst.selectedPawn;
        inventoryItemList = sPawn.itemList;
        var ivtCom = ui.inventoryCom;
        ivtCom.list_inventory.itemRenderer = InventoryListRenderer;

        ivtCom.list_inventory.numItems = inventoryItemList.Count;
        ivtCom.list_inventory.selectedIndex = 0;
    }
    void InventoryListRenderer(int index, GObject obj)
    {
        var mItem = obj as UI_BagItemComp;
        Item itemData = inventoryItemList[index];
        var itemCfg = ConfigManager.table.Item.GetOrDefault(itemData.itemId);
        string itemName = Translator.GetStr(itemCfg.Name);
        if (itemData is Weapon)
        {
            var weaponData = itemData as Weapon;
            if (weaponData.forgeLevel > 0)
            {
                itemName += $"+{weaponData.forgeLevel}";
            }
            mItem.txt_name.text = itemName;
            mItem.txt_left.text = null;
        }
        else
        {
            mItem.txt_name.text = itemName;

            int leftTimes = itemCfg.Uses - itemData.usedTimes;
            mItem.txt_left.text = "" + leftTimes;
        }
    }
    void OnInventoryConfirm()
    {
        //todo show menu [Equip,Discard,Trade,Use]
        Debugger.Log("OnInventoryConfirm");
    }

    #endregion
    #region Show Weapon select
    /// <summary>
    /// action menu: attack,weapon select
    /// </summary>
    private void OpenWeaponSelect()
    {
        ui.inventoryCom.visible = true;
        ShowWeaponSelectContent();

        this.focusedList = ui.inventoryCom.list_inventory;
        this.UIConfirmAction = OnWeaponSelectConfirm;
        this.UICancelAction = HideInventoryCom;
    }
    void ShowWeaponSelectContent()
    {
        var sPawn = BLogic.Inst.selectedPawn;
        inventoryItemList = sPawn.itemList;
        var ivtCom = ui.inventoryCom;
        ivtCom.list_inventory.itemRenderer = WeaponListRenderer;

        ivtCom.list_inventory.numItems = inventoryItemList.Count;
        ivtCom.list_inventory.selectedIndex = 0;
    }
    void WeaponListRenderer(int index, GObject obj)
    {
        var mItem = obj as UI_BagItemComp;
        Item itemData = inventoryItemList[index];
        var itemCfg = ConfigManager.table.Item.GetOrDefault(itemData.itemId);
        string itemName = Translator.GetStr(itemCfg.Name);
        if (itemData is Weapon)
        {
            mItem.ctrl_grayed.selectedIndex = 0;
            var weaponData = itemData as Weapon;
            if (weaponData.forgeLevel > 0)
            {
                itemName += $"+{weaponData.forgeLevel}";
            }
            mItem.txt_name.text = itemName;
            mItem.txt_left.text = null;
        }
        else
        {
            mItem.ctrl_grayed.selectedIndex = 1;
            mItem.txt_name.text = itemName;

            int leftTimes = itemCfg.Uses - itemData.usedTimes;
            mItem.txt_left.text = "" + leftTimes;
        }
        mItem.clickLoader.data = index;
        mItem.clickLoader.onClick.Set(OnWeaponSelectClick);
    }

    void OnWeaponSelectConfirm()
    {
        var ivtCom = ui.inventoryCom;
        int idx = ivtCom.list_inventory.selectedIndex;
        var wep = inventoryItemList[idx] as Weapon;
        WeaponMenuConfirm(wep.sid);
    }
    void OnWeaponSelectClick(EventContext ec)
    {
        int idx = (int)((GObject)ec.sender).data;
        var wep = inventoryItemList[idx] as Weapon;
        WeaponMenuConfirm(wep.sid);
    }
    void WeaponMenuConfirm(int wepSid)
    {
        //[Map target foe select]
        Debugger.Log("OnWeaponSelectConfirm sid= " + wepSid);
        var sPawn = BLogic.Inst.selectedPawn;
        //equip this weapon
        sPawn.EquipWeapon(wepSid);
        FUIManager.Inst.ShowUI<UIPage_CombatPredict>(FUIDef.FWindow.CombatPredict);
        HideInventoryCom();
    }
    #endregion
}
