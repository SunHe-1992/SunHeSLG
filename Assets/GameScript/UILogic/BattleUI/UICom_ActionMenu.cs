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
    readonly string strAttack = "Attack";
    readonly string strItems = "Items";
    readonly string strTrade = "Trade";
    readonly string strWait = "Wait";
    readonly string strConvey = "Convey";

    List<string> orderList = new List<string>();
    private void ShowActionMenu(IEventMessage msg)
    {
        orderList = new List<string>();
        var actionCom = ui.actionCom;
        actionCom.visible = true;

        actionCom.list_action.itemRenderer = ListActionRenderer;

        //menu orders: items, wait, attack,trade,convoy
        var sPawn = BLogic.Inst.selectedPawn;
        sPawn.CountNearPawns(out int foeCount, out int tradeCount, out int conveyCount);
        if (foeCount > 0)
            orderList.Add(strAttack);
        orderList.Add(strItems);
        if (tradeCount > 0)
            orderList.Add(strTrade);
        if (conveyCount > 0)
            orderList.Add(strConvey);
        orderList.Add(strWait);
        actionCom.list_action.numItems = orderList.Count;
        actionCom.list_action.ResizeToFit();
        actionCom.list_action.selectedIndex = 0;

        this.focusedList = actionCom.list_action;
        this.UIConfirmAction = ActionMenu_confirm;
        this.UICancelAction = ActionMenu_cancel;
        InputReceiver.SwitchInputToUI();
    }
    void ListActionRenderer(int index, GObject obj)
    {
        var mItem = obj as UI_MenuItem;
        string order = orderList[index];
        mItem.title = "T-" + order;
        mItem.txt_des.text = "Des-" + order;
        mItem.clickLoader.data = index;
        mItem.clickLoader.onClick.Set(MenuItemClick);
    }
    void MenuItemClick(EventContext ec)
    {
        int idx = (int)((ec.sender as GObject).data);
        ui.actionCom.list_action.selectedIndex = idx;
        ActionMenu_confirm();
    }
    void ActionMenu_confirm()
    {
        int actionIdx = ui.actionCom.list_action.selectedIndex;
        string orderStr = orderList[actionIdx];
        Debug.Log($"menu order clicked {orderStr}");
        if (orderStr == strWait)
        {
            BLogic.Inst.selectedPawn.ActionWait();
            HideActionMenu();
        }
        else if (orderStr == strAttack)
        {
            //show combat predict
            HideActionMenu();
            //if there are multiple weapons to select, show [weapon select UI]
            var sPawn = BLogic.Inst.selectedPawn;
            var weaponList = sPawn.GetPossibleWeaponCount();
            int weaponCount = weaponList.Count;
            if (weaponCount == 1) //go to foe selecting
            {
                CloseUI();
                sPawn.EquipWeapon(weaponList[0]);
                FUIManager.Inst.ShowUI<UIPage_CombatPredict>(FUIDef.FWindow.CombatPredict, null, "AttackMenu");
            }
            else if (weaponCount > 1) //show weapon select UI
            {
                HideActionMenu();
                OpenWeaponSelect(null);
            }
            //after weapon selected, show [Map target foe select]
            //after Foe selected,show [combat predic ui]
        }
        else if (orderStr == strItems)
        {
            HideActionMenu();
            OpenInventoryCom();
        }

    }
    void ActionMenu_cancel()
    {
        HideActionMenu();
        BLogic.Inst.PawnSetPostionBack();
    }
    void HideActionMenu()
    {
        ui.actionCom.visible = false;
        this.UIConfirmAction = null;
        this.UICancelAction = null;
    }
}
