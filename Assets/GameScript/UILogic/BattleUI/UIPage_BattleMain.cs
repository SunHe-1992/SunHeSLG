using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using PackageBattle;
using UnityEngine;
using UnityEngine.Events;
using SunHeTBS;
public class UIPage_BattleMain : FUIBase
{

    UI_BattlePanel ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_BattlePanel;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;

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


    void RefreshContent()
    {


    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        var inputInst = InputManager.Inst;
        //right=x+1 up=z+1
        var cursorObj = BattleDriver.Inst.CursorObj;
        if (cursorObj != null)
            if (inputInst.axisDown || inputInst.axisUp || inputInst.axisLeft || inputInst.axisRight)
            {
                int xAdd = 0;
                int zAdd = 0;
                if (inputInst.axisDown) zAdd = -1;
                else if (inputInst.axisUp) zAdd = 1;
                if (inputInst.axisLeft) xAdd = -1;
                else if (inputInst.axisRight) xAdd = 1;
                BLogic.Inst.CursorInputMove(xAdd, zAdd);
                BattleDriver.Inst.MoveCursorObj();
            }
    }
}
