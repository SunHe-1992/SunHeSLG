using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageDebug;
using UnityEngine;
using UniFramework.Event;
using PackageBattle;
using YooAsset;
using UnityEngine.SceneManagement;
using SunHeTBS;

public class UIPage_MenuScreen : FUIBase
{

    UI_MenuScreenUI ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_MenuScreenUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        //ui.btn_ok.onClick.Set(BtnOKClick);
        //ui.btn_close.onClick.Set(OnBtnClose);
        ui.btn_exit.onClick.Set(this.OnClickBtnExit);
        ui.btn_loadGame.onClick.Set(this.OnClickBtnLoadGame);
        ui.btn_newGame.onClick.Set(this.OnClickBtnNewGame);
        ui.btn_options.onClick.Set(this.OnClickBtnOptions);
        ui.btn_RPGGame.onClick.Set(this.OnClickStartRPG);


    }
    protected override void OnShown()
    {
        base.OnShown();
        //0 rpg game, 1 platform jump
        ui.ctrl_mode.selectedIndex = Boot.playRPG ? 0 : 1;
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

    void RefreshContent()
    {


    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
    protected void OnClickBtnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    protected void OnClickBtnOptions()
    {
        //ui options
        FUIManager.Inst.ShowUI<UIPage_SettingUI>(FUIDef.FWindow.SettingUI);
    }
    protected void OnClickBtnNewGame()
    {
        //SceneManager.LoadScene("GameScene");
        //  UIFrame.Show<UIGameplayScreen>();
    }

    protected void OnClickBtnLoadGame()
    {
        //GameManager.Instance.loadSavedGame = true;
        //OnClickBtnNewGame();
    }
    void OnClickStartRPG()
    {
        //start rpg
        string mapName = "World1";
        SceneHandle handle = YooAssets.LoadSceneAsync("Scene/" + mapName, LoadSceneMode.Single);
        handle.Completed += (scene) =>
        {
            BattleDriver.Inst.LoadObjInScene();
            FUIManager.Inst.ShowUI<UIPage_WorldUI>(FUIDef.FWindow.WorldPanel);
        };
        OnBtnClose();
    }
    void Load3DGameScene()
    {
        //start 3d platform game
        string mapName = "World1";
        SceneHandle handle = YooAssets.LoadSceneAsync("Scene/" + mapName, LoadSceneMode.Single);
        handle.Completed += (scene) =>
        {
            BattleDriver.Inst.LoadObjInScene();
            FUIManager.Inst.ShowUI<UIPage_WorldUI>(FUIDef.FWindow.WorldPanel);
        };
        OnBtnClose();
    }
}
