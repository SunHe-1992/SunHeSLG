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
    int previewPawnSid = 0;
    void OnPointToPawn(IEventMessage msg)
    {
        var pointedPawn = BLogic.Inst.pointedPawn;
        var selectedPawn = BLogic.Inst.selectedPawn;
        if (pointedPawn == null)
        {
            if (selectedPawn == null)//no pawn to preview hide ui
            {
                ui.bottomBar.visible = false;
                ui.nameBar.visible = false;
                previewPawnSid = 0;
            }
            else //preview selected pawn info
            {
                previewPawnSid = selectedPawn.sequenceId;
                ShowPawnPreview(selectedPawn);
            }
        }
        else
        {
            if (previewPawnSid != pointedPawn.sequenceId)
            {
                previewPawnSid = pointedPawn.sequenceId;
                ShowPawnPreview(pointedPawn);
            }
        }

        //show tile info
        ShowTileInfo();
    }
    void ShowPawnPreview(Pawn p)
    {
        ui.bottomBar.visible = true;
        ui.nameBar.visible = true;
        ui.nameBar.txt_name.text = p.charCfg.CharName;
        int colorIdx = 0;
        switch (p.camp)
        {
            case PawnCamp.Player: colorIdx = 0; break;
            case PawnCamp.Villain:
            case PawnCamp.Neutral: colorIdx = 1; break;
            case PawnCamp.PlayerAlly: colorIdx = 2; break;
        }
        ui.bottomBar.ctrl_color.selectedIndex = colorIdx;
        var btmBar = ui.bottomBar;
        var attr = p.GetAttribute();
        btmBar.txt_hp1.text = "" + p.HP;
        btmBar.txt_hp2.text = attr.HPMax + "";
        btmBar.txt_class.text = p.classCfg.Name;

        var combatAttr = p.GetCombatAttr();
        SetUIAttrUnit(btmBar.AU_avo, "Avo", combatAttr.Avoid + "");
        SetUIAttrUnit(btmBar.AU_def, "Def", combatAttr.Defence + "");
        SetUIAttrUnit(btmBar.AU_res, "Res", combatAttr.Resistance + "");
        SetUIAttrUnit(btmBar.AU_spd, "Spd", combatAttr.AttackSpeed + "");
        SetUIAttrUnit(btmBar.AU_mov, "Mov", attr.Mov + "");

        if (p.HoldingWeapon())
        {
            var damageType = p.GetDamageType();
            switch (damageType)
            {
                case cfg.SLG.DamageType.PH:
                    SetUIAttrUnit(btmBar.AU_atk, "PhAtk", combatAttr.PhAtk + "");
                    break;
                case cfg.SLG.DamageType.MAG:
                    SetUIAttrUnit(btmBar.AU_atk, "MagAtk", combatAttr.MagAtk + "");
                    break;
                case cfg.SLG.DamageType.PN:
                    SetUIAttrUnit(btmBar.AU_atk, "PnAtk", combatAttr.PnAtk + "");
                    break;
            }
        }
        else //no weapon
        {
            SetUIAttrUnit(btmBar.AU_hit, "Hit", "--");
            SetUIAttrUnit(btmBar.AU_atk, "Atk", "--");
        }

    }
    void SetUIAttrUnit(UI_AttributeUnit com, string attrName, string attrValue)
    {
        com.txt_attrName.text = attrName;
        com.txt_attrValue.text = attrValue;
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
