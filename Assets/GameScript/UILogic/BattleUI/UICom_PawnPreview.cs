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

        //show tile info
        ShowTileInfo();
    }
    void ShowPawnPreview(Pawn p)
    {
        ui.bottomBar.visible = true;
        ui.nameBar.visible = true;
        ui.nameBar.txt_name.text = p.charCfg.CharName;

        var btmBar = ui.bottomBar;
        var attr = p.GetAttribute();
        btmBar.txt_hp1.text = "" + p.HP;
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

    #region Tile info display

    void ShowTileInfo()
    {
        var cursorPos = BLogic.Inst.cursorPos;
        TileEntity curTile = TBSMapService.Inst.map.Tile(cursorPos);
        if (curTile != null)
        {
            DisplayTileInfo(curTile);
        }
        else
            HideTileInfo();
    }
    void DisplayTileInfo(TileEntity tile)
    {
        ui.tileInfoComp.visible = true;
        var tiComp = ui.tileInfoComp;
        tiComp.txt_tileName.text = tile.name;

        if (tile.passType == TilePassType.Passable)
        {
            tiComp.lbl_passible.visible = false;
        }
        else
        {
            tiComp.lbl_passible.visible = true;
            if (tile.passType == TilePassType.FliersOnly)
                tiComp.lbl_passible.title = "FliersOnly";
            else if (tile.passType == TilePassType.Impassable)
                tiComp.lbl_passible.title = "Impassable";
        }
        string effStr = UIService.TileEffToString((int)tile.effectType);
        if (string.IsNullOrEmpty(effStr))
        {
            tiComp.lbl_effect.visible = false;
        }
        else
        {
            tiComp.lbl_effect.visible = true;
            tiComp.lbl_effect.title = effStr;
        }
        bool haveSklIcons = false; //todo  skl icons display
        if (haveSklIcons)
        {
            tiComp.lbl_skl.visible = true;
            tiComp.ctrl_showEffects.selectedIndex = 0;
        }
        else
        {
            tiComp.lbl_skl.visible = false;
            tiComp.ctrl_showEffects.selectedIndex = 1;
        }
        List<GComponent> lblList = new List<GComponent>()
        {
            tiComp.lbl_passible,tiComp.lbl_effect,tiComp.lbl_skl,
        };

        float lblHeight = 30;
        float lblY = 65;
        int visibleCount = 0;
        for (int i = 0; i < lblList.Count; i++)
        {
            var comp = lblList[i];
            if (comp.visible)
            {
                visibleCount++;
                comp.y = lblY + lblHeight * (visibleCount - 1);
            }
        }
    }
    void HideTileInfo()
    {
        ui.tileInfoComp.visible = false;
    } 
    #endregion
}
