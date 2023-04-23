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
    int curPawnSid = 0;
    void OnPointToPawn(IEventMessage msg)
    {
        var pointedPawn = BLogic.Inst.pointedPawn;
        if (pointedPawn == null)
        {
            ui.bottomBar.visible = false;
            ui.nameBar.visible = false;
            curPawnSid = 0;
        }
        else
        {
            curPawnSid = pointedPawn.sequenceId;
            ShowPawnPreview(pointedPawn);
        }
    }
    void ShowPawnPreview(Pawn p)
    {
        ui.bottomBar.visible = true;
        ui.nameBar.visible = true;
        ui.nameBar.txt_name.text = p.cfgData.CharName;

        var btmBar = ui.bottomBar;
        var attr = p.GetAttribute();
        btmBar.txt_hp2.text = attr.HPMax + "";
        btmBar.txt_class.text = "?class";

        btmBar.AU_atk.txt_attrName.text = "PhAtk";
        btmBar.AU_avo.txt_attrName.text = "Avo";
        btmBar.AU_def.txt_attrName.text = "Def";
        btmBar.AU_hit.txt_attrName.text = "Hit";
        //btmBar.AU_level.txt_attrName.text = "Level";
        btmBar.AU_mov.txt_attrName.text = "Mov";
        btmBar.AU_res.txt_attrName.text = "Res";
        btmBar.AU_spd.txt_attrName.text = "Spd";
    }
}
