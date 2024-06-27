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

    UI_CombatPanel ui;
    List<UI_RPGStatsCom> statComList;
    List<UI_VillianStatsCom> vStatComList;
    public static UIPage_CombatPanel Instance;
    protected override void OnInit()
    {
        Instance = this;
        base.OnInit();
        ui = this.contentPane as UI_CombatPanel;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;

        statComList = new List<UI_RPGStatsCom>()
        {
            ui.stat0,ui.stat1,ui.stat2,ui.stat3
        };

        ui.actionList.itemRenderer = this.ActionListRenderer;

        vStatComList = new List<UI_VillianStatsCom>(){
            ui.villian0,ui.villian1,ui.villian2,ui.villian3
        };

        indicatorPoolList = ui.ComIndicator.GetChildAt(0).asList;

        ui.btn_test.onClick.Set(TestFunction);
    }
    protected override void OnShown()
    {
        base.OnShown();
        LoadBG();
        UniEvent.AddListener(GameEventDefine.TurnSwitch, RefreshContent);
        UniEvent.AddListener(GameEventDefine.NextActionPawn, RefreshContent);
        UniEvent.AddListener(GameEventDefine.HPChanged, UpdateStats);
        UniEvent.AddListener(GameEventDefine.NextActionPawn, OnNextActionPawn);

    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

        RefreshContent(null);
    }
    protected override void OnHide()
    {
        base.OnHide();
        UniEvent.RemoveListener(GameEventDefine.TurnSwitch, RefreshContent);
        UniEvent.RemoveListener(GameEventDefine.NextActionPawn, RefreshContent);
        UniEvent.RemoveListener(GameEventDefine.HPChanged, UpdateStats);
        UniEvent.RemoveListener(GameEventDefine.NextActionPawn, OnNextActionPawn);

    }
    void LoadBG()
    {
        string bgPath = "UISprite/background/battleback1";
        UIService.Inst.LoadUnitySprite(bgPath, ui.bg);
    }

    void RefreshContent(IEventMessage msg)
    {

        UpdateStats(null);
        RefreshActionComp();
        int curTurn = BLogic.Inst.currentTurn;
        ui.txt_turn.text = $"Turn: {curTurn}";

        ShowActionMenu();
    }
    void UpdateStats(IEventMessage msg)
    {
        RefreshTeamStats();
        RefreshVillianStats();
    }
    public void HideUI()
    {
        FUIManager.Inst.HideUI(this);
    }
    #region Top team member stats
    void RefreshTeamStats()
    {
        var pList = BLogic.Inst.teamPawnList;

        for (int i = 0; i < statComList.Count; i++)
        {
            var com = statComList[i];
            if (i < pList.Count)
            {
                com.visible = true;
                RefreshStatsCom(com, pList[i]);
            }
            else
            {
                com.visible = false;
            }
        }
    }
    void RefreshStatsCom(UI_RPGStatsCom com, Pawn p)
    {
        var attr = p.GetAttr();

        com.HPCom.txt_title.text = "HP";
        com.HPCom.txt_value.text = p.HP + "";
        com.HPCom.txt_valueMax.text = "/" + p.GetHPMax();
        com.HPCom.comBar.color.selectedIndex = 2; //green bar
        com.HPCom.comBar.value = (float)p.HP / p.GetHPMax() * 100f;


        com.SPCom.txt_title.text = "SP";
        com.SPCom.txt_value.text = p.SP + "";
        com.SPCom.txt_valueMax.text = "/" + attr.SPMax;
        com.SPCom.comBar.color.selectedIndex = 0; //blue bar
        com.SPCom.comBar.value = (float)p.SP / attr.SPMax * 100f;

        var actonP = BLogic.Inst.GetCurrentActionPawn();
        bool highLighted = actonP.seqId == p.seqId;
        com.ctrl_highlight.selectedIndex = highLighted ? 1 : 0;
    }
    #endregion

    #region all pawns action list
    void OnNextActionPawn(IEventMessage msg)
    {
        UpdateStats(null);
        RefreshActionComp();
    }
    void RefreshActionComp()
    {
        var aList = BLogic.Inst.actionPawnList;
        int count = aList.Count - BLogic.Inst.actionPawnIndex;
        ui.actionList.numItems = count;
    }
    void ActionListRenderer(int index, GObject obj)
    {
        var aList = BLogic.Inst.actionPawnList;
        var p = aList[index + BLogic.Inst.actionPawnIndex];
        var mItem = obj as UI_RPGPawnBlock;
        mItem.txt_name.text = $"{p.side} {p.PawnCfg.Name}";
    }
    #endregion

    #region Villians HP com
    void RefreshVillianStats()
    {
        var vList = BLogic.Inst.villianPawnList;

        for (int i = 0; i < 4; i++)
        {
            var mItem = vStatComList[i];

            if (i < vList.Count)
            {
                mItem.visible = true;
                RefreshOneVillianStats(mItem, vList[i]);
            }
            else
            {
                mItem.visible = false;
            }
        }

    }
    void RefreshOneVillianStats(UI_VillianStatsCom mItem, Pawn p)
    {
        var comBar = mItem.stats.comBar;
        comBar.color.selectedIndex = 1;

        mItem.stats.txt_value.text = "" + p.HP;
        mItem.stats.txt_valueMax.text = "" + p.GetHPMax();
        comBar.value = (float)p.HP / p.GetHPMax() * 100f;
        mItem.ctrl_dead.selectedIndex = p.dead ? 1 : 0;

        bool isSelected = false;
        if (BLogic.Inst.GetCurrentSide() == RPGSide.Player
            && BLogic.Inst.selectedPawn == p)
            isSelected = true;
        mItem.ctrl_selected.selectedIndex = isSelected ? 1 : 0;
    }
    #endregion
}
