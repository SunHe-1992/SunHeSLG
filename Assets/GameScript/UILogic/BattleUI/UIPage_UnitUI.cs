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

        ui.btn_close.onClick.Set(OnBtnBack);
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

    }
    BasicAttribute basicAttr;
    void RefreshContent()
    {
        var pdCom = ui.pawn_detail;
        var pawn = BLogic.Inst.pointedPawn;
        //base stats
        basicAttr = pawn.GetAttribute();
        pdCom.list_BasicStats.numItems = 7;

        //todo combat stats

        SetUIStatsCom(pdCom.stat_build, "Bld", basicAttr.Bld.ToString());
        //sp display
        SetUIStatsCom(pdCom.stat_SP, "SP", "???");
        SetUIAttrUnit(pdCom.moveCom, "Mov", basicAttr.Mov.ToString());
        SetUIAttrUnit(pdCom.levelCom, "Level", "???");

        pdCom.stat_HP.txt_HPMAX.text = $"/{basicAttr.HPMax}";
        pdCom.stat_HP.txt_HP.text = "??";

        pdCom.txt_class.text = pawn.classCfg.Name;
        pdCom.txt_pawnName.text = pawn.charCfg.CharName;
        string strClass = Translator.GetStr(pawn.classCfg.ClassType.ToString());
        pdCom.txt_classType.text = $"/{strClass}";
    }

    //BasicStats

    void BasicStatsListRender(int index, GObject obj)
    {
        BasicStats statType = (BasicStats)(index + 1);
        int attrValue = basicAttr.GetAttr(statType);
        var mItem = obj as UI_StatsCom;

        mItem.ctrl_arrow.selectedIndex = 0;

        mItem.txt_attrName.text = statType.ToString();
        mItem.txt_attrValue.text = "" + attrValue;
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
