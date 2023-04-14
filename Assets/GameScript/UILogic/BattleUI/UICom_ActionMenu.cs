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
    private void ShowActionMenu(IEventMessage msg)
    {
        var actionCom = ui.actionCom;
        actionCom.visible = true;

        actionCom.list_action.itemRenderer = ListActionRenderer;
        actionCom.list_action.numItems = 5;//test
        actionCom.list_action.ResizeToFit();
    }
    void ListActionRenderer(int index, GObject obj)
    {
        var mItem = obj as UI_MenuItem;
        mItem.title = "123";
        mItem.txt_des.text = "des";
    }
}
