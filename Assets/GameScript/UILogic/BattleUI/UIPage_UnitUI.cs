using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageBattle;
using UnityEngine;
using UniFramework.Event;
using SunHeTBS;
public class UIPage_UnitUI : FUIBase
{

    UI_UnitUI ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_UnitUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.pawn_detail.list_BasicStats.itemRenderer = BasicStatsListRender;
        ui.pawn_detail.list_combatStats.itemRenderer = CombatStatsListRenderer;
        ui.btn_close.onClick.Set(OnBtnBack);

        basicStatList = new List<BasicStats>()
        { BasicStats.Str, BasicStats.Mag,
         BasicStats.Dex,BasicStats.Spd,
            BasicStats.Def,BasicStats.Res,
            BasicStats.Luk};
        combatStatList = new List<CombatStats>()
        { CombatStats.PhAtk, CombatStats.Hit, CombatStats.Avo,
       CombatStats.Crit,CombatStats.Ddg };
    }
    protected override void OnShown()
    {
        base.OnShown();
        InputReceiver.SwitchInputToUI();
        UniEvent.AddListener(GameEventDefine.ClickCancel, OnClickCancel);

    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

        RefreshContent();
    }
    protected override void OnHide()
    {
        base.OnHide();
        UniEvent.RemoveListener(GameEventDefine.ClickCancel, OnClickCancel);
        this.basicAttr = null;
        this.combatAttr = null;

    }
    BasicAttribute basicAttr;
    CombatAttribute combatAttr;
    void RefreshContent()
    {
        var pdCom = ui.pawn_detail;
        var pawn = BLogic.Inst.pointedPawn;
        //base stats
        basicAttr = pawn.GetAttribute();
        combatAttr = pawn.GetCombatAttr();
        pdCom.list_BasicStats.numItems = 7;
        //combat stats
        pdCom.list_combatStats.numItems = 5;

        SetUIStatsCom(pdCom.stat_build, "Bld", basicAttr.Bld.ToString());
        //sp display
        SetUIStatsCom(pdCom.stat_SP, "SP", "???");
        SetUIAttrUnit(pdCom.moveCom, "Mov", basicAttr.Mov.ToString());
        SetUIAttrUnit(pdCom.levelCom, "Level", "???");

        pdCom.stat_HP.txt_HPMAX.text = $"/{basicAttr.HPMax}";
        pdCom.stat_HP.txt_HP.text = pawn.HP.ToString();

        pdCom.txt_class.text = pawn.classCfg.Name;
        pdCom.txt_pawnName.text = pawn.charCfg.CharName;
        string strClass = Translator.GetStr(pawn.classCfg.ClassType.ToString());
        pdCom.txt_classType.text = $"/{strClass}";
    }

    //BasicStats
    List<BasicStats> basicStatList;
    List<CombatStats> combatStatList;
    void BasicStatsListRender(int index, GObject obj)
    {
        BasicStats statType = basicStatList[index];


        int attrValue = basicAttr.GetAttr(statType);
        var mItem = obj as UI_StatsCom;

        mItem.ctrl_arrow.selectedIndex = 0;

        mItem.txt_attrName.text = statType.ToString();
        mItem.txt_attrValue.text = "" + attrValue;
    }
    void CombatStatsListRenderer(int index, GObject obj)
    {
        CombatStats cStats = combatStatList[index];
        int attrValue = combatAttr.GetAttr(cStats);
        string attrStr = attrValue + "";
        string attrName = cStats.ToString();
        var pawn = BLogic.Inst.pointedPawn;
        if (cStats == CombatStats.Hit && pawn.HoldingWeapon() == false)
        {
            attrStr = "--";
        }

        if (cStats == CombatStats.PhAtk)
        {
            if (pawn.HoldingWeapon() == false)
            {
                attrStr = "--";
            }
            else
            {
                attrValue = pawn.GetAttackValue();
                attrStr = attrValue + "";

                attrStr = attrValue + "";
                cfg.SLG.DamageType dmgType = pawn.GetDamageType();
                Debugger.Log($"test: atkvalue={attrStr} dmgType={dmgType}");
                attrName = Translator.GetStr(dmgType.ToString());
            }
        }
        var mItem = obj as UI_StatsCom;
        mItem.ctrl_arrow.selectedIndex = 0;
        mItem.txt_attrName.text = attrName;
        mItem.txt_attrValue.text = attrStr;
    }
    void SetUIStatsCom(UI_StatsCom com, string name, string valueStr)
    {
        com.txt_attrName.text = name;
        com.txt_attrValue.text = valueStr;
    }
    void SetUIAttrUnit(UI_AttributeUnit com, string name, string valueStr)
    {
        com.txt_attrName.text = name;
        com.txt_attrValue.text = valueStr;
    }

    void OnBtnBack()
    {
        FUIManager.Inst.HideUI(this);
        InputReceiver.SwitchInputToMap();
        FUIManager.Inst.ShowUI<UIPage_BattleMain>(FUIDef.FWindow.BattlePanel);

    }
    void OnClickCancel(IEventMessage msg)
    {
        OnBtnBack();

    }
}
