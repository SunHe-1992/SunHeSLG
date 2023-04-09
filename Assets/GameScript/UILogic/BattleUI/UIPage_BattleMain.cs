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
        UniEvent.AddListener(GameEventDefine.CURSOR_MOVED, OnCursorMoved);
    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

        RefreshContent();
    }
    protected override void OnHide()
    {
        base.OnHide();
        UniEvent.RemoveListener(GameEventDefine.CURSOR_MOVED, OnCursorMoved);

    }


    void RefreshContent()
    {


    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        var inputInst = InputReceiver.Inst;
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
                UniEvent.SendMessage(GameEventDefine.CURSOR_MOVED);
            }
    }

    void OnCursorMoved(IEventMessage msg)
    {
        Pawn selectedPawn = BLogic.Inst.selectedPawn;
        if (selectedPawn != null) //selected a pawn ,show its info
        {
            //todo ui show pawn summary
            //todo map show planes
            Debugger.Print($"selected a pawn {selectedPawn.ToString()}");
            TBSMapService.Inst.ShowPawnCoverPlanes(selectedPawn);
        }
        else
        {
            TBSMapService.Inst.UnspawnAllCoverPlanes();
        }
    }
}
