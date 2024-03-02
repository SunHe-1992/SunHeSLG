using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using PackageMonopoly;
using UnityEngine;
using UniFramework.Event;
using SunHeTBS;
using CommonPackage;
using cfg.MONO;
public class UIPage_Construction : FUIBase
{

    UI_Construction ui;
    Chapter cptCfg;

    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_Construction;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;

        ui.btn_close.onClick.Set(OnBtnClose);
        ui.list_building.itemRenderer = this.BuildingListRenderer;
        int chapterId = MonoPlayer.UserDetail.currentChapter;
        cptCfg = ConfigManager.table.TbChapter.Get(chapterId);
    }
    protected override void OnShown()
    {
        base.OnShown();
        UniEvent.AddListener(GameEventDefine.Building_Changed, RefreshOnChanged);
        UniEvent.AddListener(GameEventDefine.POINTS_CHANGED, RefreshTopBar);
    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

        RefreshContent();
    }
    protected override void OnHide()
    {
        base.OnHide();
        UniEvent.RemoveListener(GameEventDefine.Building_Changed, RefreshOnChanged);
        UniEvent.RemoveListener(GameEventDefine.POINTS_CHANGED, RefreshTopBar);

    }

    void RefreshOnChanged(IEventMessage msg)
    {
        RefreshContent();
    }
    int currentLevelSum = 0;
    int totalLevelSum = 0;
    void RefreshContent()
    {
        currentLevelSum = 0;
        totalLevelSum = 0;
        //read user data and configs
        foreach (var data in ConfigManager.table.TbBuilding.DataList)
        {
            if (data.Level > 0)
                totalLevelSum++;
        }

        foreach (var bd in cptCfg.BuildingList)
        {
            var bdd = MonoPlayer.buildingDic[bd];
            currentLevelSum += bdd.buildingLevel;
        }
        ui.txt_count.text = $"INDUSTRIES:{currentLevelSum}/{totalLevelSum}";
        ui.list_building.numItems = cptCfg.BuildingList.Count;
        ui.txt_story.text = cptCfg.Description;
        RefreshTopBar(null);

        if (currentLevelSum >= totalLevelSum)
        {
            //todo: if all buildings in this chapter are max level, change to next chapter
        }
    }
    void OnBtnClose()
    {
        FUIManager.Inst.ShowUI<UIPage_MonopolyMain>(FUIDef.FWindow.MonopolyMain);
        FUIManager.Inst.HideUI(this);
    }

    void BuildingListRenderer(int index, GObject go)
    {
        int buildingId = cptCfg.BuildingList[index];
        if (MonoPlayer.buildingDic.ContainsKey(buildingId) == false)
        {
            Debugger.LogError($"building id {buildingId} is not found in UserDetail");
            return;
        }
        var buildingData = MonoPlayer.buildingDic[buildingId];//read user data
        int buildingLevel = buildingData.buildingLevel;
        UI_BuildingItem obj = go as UI_BuildingItem;
        var lvCfg = ConfigManager.table.TbBuilding.Get(buildingId, buildingLevel);
        var lv1Cfg = ConfigManager.table.TbBuilding.Get(buildingId, 1);
        //image
        obj.loader_pic.url = lv1Cfg.Image;
        //text: price 
        if (lvCfg.IsMax)
            obj.txt_price.text = "";
        else
        {
            obj.txt_price.text = "" + lvCfg.Price;
        }
        //dot item: set level
        obj.dotItem.ctrl_tier.selectedIndex = buildingLevel;

        obj.data = buildingId;
        obj.onClick.Set(UpgradeBuilding);
    }
    void UpgradeBuilding(EventContext ec)
    {
        int buildingId = (int)(ec.sender as GObject).data;
        //if level is max do nothing
        BuildingDetail buildingData = MonoPlayer.buildingDic[buildingId];//read user data
        var bdCfg = ConfigManager.table.TbBuilding.Get(buildingId, buildingData.buildingLevel);
        if (bdCfg.IsMax)
        {
            return;
        }
        bool isAfford = MonoPlayer.IsAffordGold(bdCfg.Price);
        if (isAfford)
        {
            //if money is enough,request upgrade
            LocalServer.Inst.RequestBuildingUpgrade(buildingId);
        }
        else
        {
            //todo if money is not enough, popup charging window
            Debugger.LogError("can't afford the price");
        }
    }

    #region top bar
    void RefreshTopBar(IEventMessage msg)
    {
        UIService.Inst.RefreshTopBar(this.ui.topBar);
    }
    #endregion
}
