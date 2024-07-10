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
    GList focusedList = null;

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
            BLogic.GCState = GameControlState.ActionMenu;
            ui.actionCom.visible = true;
            RefreshActionCom();
            this.focusedList = ui.actionCom.list_action;
        }
        else
        {
            HideActionMenu();
        }
    }
    void HideActionMenu()
    {
        ui.actionCom.visible = false;
        this.focusedList = null;
        BLogic.GCState = GameControlState.Default;
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
            BLogic.Inst.OnPawnActionEnd();
        }
        else if (orderStr == strItems)
        {
            BLogic.Inst.OnPawnActionEnd();
        }
        else if (orderStr == strDefend)
        {
            BLogic.Inst.OnPawnActionEnd();
        }
        else if (orderStr == strFlee)
        {
            bool success = BLogic.Inst.AttemptToFlee();
            Debug.Log($"AttemptToFlee result is success={success}");
            if (success)
            {
                BLogic.Inst.DoCombatFlee();
            }
            else
            {
                BLogic.Inst.OnPawnActionEnd();
            }
        }


    }
    #endregion

    #region targetSelect
    void EnterTargetSelect()
    {
        BLogic.GCState = GameControlState.TargetPawnSelecting;
        //先固定写死选择第一个敌人
        //BLogic.Inst.AutoSelectVillian();
    }
    void ConfirmTargetSelect()
    {
        if (curAction == strAttack)
        {
            BLogic.GCState = GameControlState.CastingSkill;
            var caster = BLogic.Inst.GetCurrentActionPawn();
            var vList = BLogic.Inst.villianPawnList;
            var target = BLogic.Inst.selectedPawn;
            //do attack: 伤害计算, 界面表现展示, 推进流程
            //伤害计算: 需要 skill对象,普攻skill,配置表
            caster.NormalAttack(target);
            HideActionMenu();
        }
    }
    #endregion


    #region input control
    void OnInputAxis(IEventMessage msg)
    {
        var inputInst = InputReceiver.Inst;
        //right=x+1 up=z+1
        int xAdd = 0;
        int zAdd = 0;
        if (inputInst.axisDown) zAdd = -1;
        else if (inputInst.axisUp) zAdd = 1;
        if (inputInst.axisLeft) xAdd = -1;
        else if (inputInst.axisRight) xAdd = 1;

        if (InputReceiver.InputInUI)
        {
            if (this.isTop)
            {
                UIListNavigation();
            }
        }
        else
        {
            //BLogic.Inst.OnInputAxis(xAdd, zAdd);
        }

    }
    void UIListNavigation()
    {
        var inputInst = InputReceiver.Inst;
        if (focusedList != null)
        {
            //list navigation
            if (inputInst.axisDown)
                focusedList.OnInputNext();
            else if (inputInst.axisUp)
                focusedList.OnInputPrevious();
        }
    }

    void OnClickConfirm(IEventMessage msg)
    {

    }
    void OnClickCancel(IEventMessage msg)
    {

    }
    #endregion

}
