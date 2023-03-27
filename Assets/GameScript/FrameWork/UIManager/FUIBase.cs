using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using FairyGUI;

public class FUIBase : Window
{
    public FUIBase()
    {
    }

    /// <summary>
    /// 已经初始化
    /// </summary>
    protected bool _initialize = false;
    /// <summary>
    /// 已经显示
    /// </summary>
    protected bool isActived = false;



    public bool isActive()
    {
        return isActived;
    }
    /// <summary>
    /// 响应安卓小退按钮
    /// </summary>
    public bool backBtnEnabled = true;
    /*
     战斗专用 层级1000 - 2000
         */
    public enum UIShowType
    {
        /// <summary>
        /// 窗口UI
        /// </summary>
        WINDOW = 0,
        /// <summary>
        /// 全屏UI 同时显示一个
        /// </summary>
        SCREEN = 1,
        /// <summary>
        /// 主UI的底层，特殊处理（一直显示的）
        /// </summary>
        MAINPAGE = 2,
        /// <summary>
        /// 提示UI 永远最上面
        /// </summary>
        TIPS = 3,
    }
    /// <summary>
    /// UI显示类型，默认为全屏模式，显示时会隐藏前一个全屏UI
    /// </summary>
    public UIShowType uiShowType = UIShowType.WINDOW;

    public int FUIWindowType;    // 用于保存具体的UI界面类型

    //添加新的类型（用于动画类型）
    public int animationType = 0;

    public int WindowLiveness;     //指示该界面已经被关闭了多久了，每有新界面打开时+1，自己切换到前台的时候重置为0

    /// <summary>
    /// Fairy里面的包名
    /// </summary>
    public string packageName;

    public virtual string GetPackageName()
    {
        return packageName;
    }

    /// <summary>
    /// Fairy里面的组件名
    /// </summary>
    public string compName;

    public virtual string GetCompName()
    {
        return compName;
    }


    protected override void OnInit()
    {
        base.OnInit();
        //这里需要提供package name  and comp name 才能加载

        if (this.contentPane == null)
        {
            string packageName = GetPackageName();
            string compName = GetCompName();

            GObject gobj = UIPackage.CreateObject(packageName, compName);
            if (gobj != null)
            {
                this.contentPane = gobj.asCom;
            }
            else
            {
                Debugger.LogError("Create Content Panel Failed by Package name: " + packageName + ", comp name: " + compName);
            }

        }
        this.Center();
        _initialize = true;

    }

    protected override void OnShown()
    {
        base.OnShown();
        isActived = true;


    }

    protected override void OnHide()
    {

        base.OnHide();
        isActived = false;
        //这里写UI关闭时的逻辑
    }

    //窗口彻底销毁时的操作
    public override void Dispose()
    {
        if (this.contentPane != null)
        {
            this.contentPane.Dispose();
            this.contentPane = null;
        }
        base.Dispose();
    }

    /// <summary>
    /// 这里刷新UI
    /// </summary>
    /// <param name="param"></param>
    public virtual void Refresh(System.Object param)
    {

    }

    public virtual void NotifyRefresh()
    {
    }


    #region 控制方法

    protected void ShowGameObject(bool show)
    {
        if (this.contentPane != null && this.contentPane.displayObject != null)
        {
            this.contentPane.visible = show;
        }
    }

    protected bool IsDestroyUIObject()
    {
        return this.contentPane == null;
    }

    #endregion

}
