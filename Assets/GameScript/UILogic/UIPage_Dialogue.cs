using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using CommonPackage;
using UnityEngine;
using UniFramework.Event;
using cfg;
public class UIPage_Dialogue : FUIBase
{
    UI_DialoguePage ui;
    int cfgId;
    cfg.DialogueData data;
    List<string> opStrList;
    List<int> opActionList;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_DialoguePage;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.list_options.itemRenderer = this.OptionRenderer;
        ui.bgLoader.onClick.Set(OnBgLoaderClick);
    }
    protected override void OnShown()
    {
        base.OnShown();

    }


    public override void Refresh(object param)
    {
        base.Refresh(param);
        this.cfgId = (int)param;

        RefreshContent();
    }
    protected override void OnHide()
    {
        base.OnHide();

    }
    void BtnOKClick()
    {
        FUIManager.Inst.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
        FUIManager.Inst.HideUI(this);
    }

    void RefreshContent()
    {
        this.data = ConfigManager.table.TbDialogue.Get(cfgId);

        ui.txt_content.text = this.data.Content;

        opStrList = new List<string>();
        opActionList = new List<int>();
        ReadData(data.Option1, data.OPAction1);
        ReadData(data.Option2, data.OPAction2);
        ReadData(data.Option3, data.OPAction3);
        ReadData(data.Option4, data.OPAction4);

        ui.list_options.numItems = opStrList.Count;
    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
        FUIManager.Inst.ShowUI<UIPage_WorldUI>(FUIDef.FWindow.WorldPanel);
    }

    void OptionRenderer(int index, GObject obj)
    {
        var mItem = obj as GLabel;
        mItem.title = opStrList[index];
        mItem.data = opActionList[index];
        mItem.onClick.Set(OnOptionClick);
    }
    void OnOptionClick(EventContext ec)
    {
        int actionId = (int)(ec.sender as GLabel).data;
        ProcessActionId(actionId);
    }
    void ProcessActionId(int actionId)
    {
        Debug.Log($"action id = {actionId}");
        if (ConfigManager.table.TbDialogue.DataMap.ContainsKey(actionId))
        {
            this.cfgId = actionId;
            RefreshContent();
        }
    }
    void ReadData(string str, int action)
    {
        if (string.IsNullOrEmpty(str) == false && action != 0)
        {
            opStrList.Add(str);
            opActionList.Add(action);
        }
    }
    //click for the only action
    void OnBgLoaderClick()
    {

        if (opActionList != null && opActionList.Count == 1)
        {
            this.cfgId = opActionList[0];
            RefreshContent();
        }
        else if (opActionList == null || opActionList.Count == 0 || opActionList[0] == 0)
        {
            OnBtnClose();
        }
    }
}
