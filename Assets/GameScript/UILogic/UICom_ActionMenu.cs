using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UniFramework.Event;
using PackageBattle;
using SunHeTBS;
public partial class UIPage_CombatPanel : FUIBase
{
    #region current pawn action menu
    string strAttack = "Attack";
    string strItems = "Items";
    string strSkill = "Skill";
    string strDefend = "Defend";
    string strFlee = "Flee";

    string curAction = "";
    void ShowActionMenu()
    {
        var aList = BLogic.Inst.actionPawnList;
        var currentP = aList[BLogic.Inst.actionPawnIndex];
        if (currentP.side == RPGSide.Player)
        {
            ui.actionCom.visible = true;
            RefreshActionCom();
        }
        else
        {
            ui.actionCom.visible = false;
        }
    }
    List<string> orderList = new List<string>()
    {

    };

    void RefreshActionCom()
    {
        orderList = new List<string>() { strAttack, strSkill, strItems, strDefend, strFlee };
        ui.actionCom.list_action.itemRenderer = ActionMenuRenderer;
        //Attack, Skills, Item, Defend, Flee
        ui.actionCom.list_action.numItems = orderList.Count;
    }
    void ActionMenuRenderer(int index, GObject obj)
    {
        var mItem = obj as UI_MenuItem;
        mItem.title = orderList[index];
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
        curAction = orderStr;
        if (orderStr == strAttack)
        {
            EnterTargetSelect();
        }
        else if (orderStr == strSkill)
        {

        }
        else if (orderStr == strItems)
        {

        }
        else if (orderStr == strDefend)
        {

        }
        else if (orderStr == strFlee)
        {

        }


    }
    #endregion

    #region targetSelect
    int selectedVillianIndex = 0;
    void EnterTargetSelect()
    {
        //先固定写死选择第一个敌人
        selectedVillianIndex = 0;
        ConfirmTargetSelect();
    }
    void ConfirmTargetSelect()
    {
        if (curAction == strAttack)
        {
            //do attack: 伤害计算, 界面表现展示, 推进流程
            //伤害计算: 需要 skill对象,普攻skill,配置表
        }
    }
    #endregion
}
