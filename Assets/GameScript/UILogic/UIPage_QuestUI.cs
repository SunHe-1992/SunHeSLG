using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UniFramework.Event;
using PackageBattle;

public class UIPage_QuestUI : FUIBase
{

    UI_QuestUI ui;
    List<QuestEntry> boardQuests;
    List<QuestEntry> acceptedQuests;

    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_QuestUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.btn_close.onClick.Set(OnBtnClose);
        ui.list_board.itemRenderer = this.BoardListRenderer;
        ui.list_myQuests.itemRenderer = this.MyListRenderer;

        TBSPlayer.GenerateBoardQuests();
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
    void BtnOKClick()
    {
        FUIManager.Inst.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
        FUIManager.Inst.HideUI(this);
    }
    int QuestComparer(QuestEntry a, QuestEntry b)
    {
        if (a.status != b.status)
        {
            return a.status.CompareTo(b.status);
        }
        return a.questId.CompareTo(b.questId);
    }
    void RefreshContent()
    {
        this.boardQuests = new List<QuestEntry>();
        boardQuests.AddRange(TBSPlayer.UserDetail.boardQuests.Values);
        this.acceptedQuests = new List<QuestEntry>();
        acceptedQuests.AddRange(TBSPlayer.UserDetail.myQuests.Values);
        acceptedQuests.Sort(QuestComparer);

        ui.list_board.numItems = boardQuests.Count;
        ui.list_myQuests.numItems = acceptedQuests.Count;

    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
    void RefreshQuestItem(UI_QuestEntry mItem, QuestEntry data)
    {
        var cfg = ConfigManager.table.TbQuest.Get(data.questId);
        mItem.txt_quest.text = cfg.Content;
        mItem.ctrl_status.selectedIndex = (int)data.status;
        mItem.btn_accept.data = data.questId;
        mItem.btn_accept.onClick.Set(BtnAcceptQuest);
        //gold coin display
        UIService.Inst.ShowItemComp(mItem.itemHead, 102, cfg.RewardGold);
    }

    void BoardListRenderer(int index, GObject obj)
    {
        var data = this.boardQuests[index];
        var mItem = obj as UI_QuestEntry;
        RefreshQuestItem(mItem, data);

    }
    void BtnAcceptQuest(EventContext ec)
    {
        int questId = (int)(ec.sender as GComponent).data;
        TBSPlayer.AcceptQuest(questId);
        RefreshContent();
    }
    void MyListRenderer(int index, GObject obj)
    {
        var data = this.acceptedQuests[index];
        var mItem = obj as UI_QuestEntry;
        var cfg = ConfigManager.table.TbQuest.Get(data.questId);
        RefreshQuestItem(mItem, data);
        string progress = $"\n{data.currentProgress}/{data.maxProgress}";
        mItem.txt_quest.text = cfg.Content + progress;
    }
}
