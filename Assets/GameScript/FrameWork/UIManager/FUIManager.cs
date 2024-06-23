using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using YooAsset;
using UniFramework.Singleton;
public class FUIManager : ISingleton
{
    public static FUIManager Inst { get; private set; }
    public static void Init()
    {
        Inst = UniSingleton.CreateSingleton<FUIManager>();
    }
    public void OnCreate(object createParam)
    {
        FUIAwake();
    }

    public void OnUpdate()
    {
        UIMgrUpdate();
    }

    public void OnDestroy()
    {
    }
    public void OnFixedUpdate()
    {
    }


    /// <summary>
    /// window 实例存放
    /// </summary>
    private Dictionary<FUIDef.FWindow, FUIBase> windowsDic = new Dictionary<FUIDef.FWindow, FUIBase>();  //所有界面的存储

    private Stack<FUIBase> windowStack = new Stack<FUIBase>();//界面的栈存储

    //已加载的UI资源包的引用计数
    private Dictionary<string, int> packageRefCountDict = new Dictionary<string, int>();
    private Dictionary<string, bool> loadedPackageDict = new Dictionary<string, bool>();


    /// <summary>
    /// 窗口命令表
    /// </summary>
    private Queue<ControlCommand> m_controlCmdQueue = new Queue<ControlCommand>();

    //正在加载或打开中的窗口，避免重复开启
    private HashSet<FUIDef.FWindow> m_openingWindows = new HashSet<FUIDef.FWindow>();


    //动画类型
    public enum OpenUIAnimationType
    {
        NoAnimation,   //没有动画
        Default,           //初始设置（window和TipWindow_Check使用默认动画）
        MainPageChange, //上一个界面渐影，下一个界面直接渐变过来
        WindowOpen,     //界面直接缩放变大
        ScreenOpen,    //界面直接移动过来的，渐隐
        BattleOpen,    //战斗界面，渐隐
    }



    void FUIAwake()
    {
        windowsDic = new Dictionary<FUIDef.FWindow, FUIBase>();
        windowStack = new Stack<FUIBase>();
        packageRefCountDict = new Dictionary<string, int>();
        loadedPackageDict = new Dictionary<string, bool>();

        //packageUseDic = new Dictionary<string, List<FUIBase>>();
        //packageUsedNum = new Dictionary<string, List<string>>();
        //packageCanRemove = new List<string>();

        m_controlCmdQueue = new Queue<ControlCommand>();
        m_openingWindows = new HashSet<FUIDef.FWindow>();
    }


    /// <summary>
    /// 外部接口 ShowUI
    /// </summary>
    /// <param name="win"></param>
    /// <param name="winType"></param>
    /// <param name="openDone"></param>
    /// <param name="isBlock"></param>
    FUIDef.FWindow lastWinType;  //上一个显示的界面
    public void ShowUI<T>(FUIDef.FWindow winType, CreateWindowDelegate openDone = null, System.Object refresh_param = null) where T : FUIBase, new()
    {
        if (IsWindowOpening(winType))
        {

            Debug.Log("此UI已经被打开了: " + winType.ToString());
            return;
        }

        //获取当前显示的内容
        lastWinType = winType;
        CheckLoadListBeforeShowUI(winType);

        if (isWindowInControlCmdQueue(winType))
        {
            Debug.Log("UI有重复的打开命令： " + winType);
            return;
        }

        FUIBase win = GetWindow(winType);
        if (win == null)
        {
            win = new T();
            win.FUIWindowType = (int)winType;
        }

        ControlCommand cmd = new ControlCommand(ControlCommand.CommandType.SHOW, winType, win, openDone, refresh_param);
        m_controlCmdQueue.Enqueue(cmd);
    }
    /// <summary>
    /// show 界面之前 查找消息队列里面这个界面的hide指令 并无效化 
    /// </summary>
    /// <param name="winType"></param>
    void CheckLoadListBeforeShowUI(FUIDef.FWindow winType)
    {
        foreach (var ctrl in m_controlCmdQueue)
        {
            if (ctrl.windowType == winType)
                if (ctrl.cmdType == ControlCommand.CommandType.HIDE ||
                        ctrl.cmdType == ControlCommand.CommandType.DESTROY)
                    ctrl.invalid = true;
        }
    }


    private void AddOpeningWindow(FUIDef.FWindow winType)
    {
        if (m_openingWindows.Contains(winType))
        {
            Debug.Log("Internal Error, Windows Already in Opening List: " + winType);
            return;
        }

        m_openingWindows.Add(winType);
    }

    private bool IsWindowOpening(FUIDef.FWindow winType)
    {
        return m_openingWindows.Contains(winType);
    }

    private bool RemoveOpeningWindow(FUIDef.FWindow winType)
    {
        if (!m_openingWindows.Contains(winType))
        {
            Debug.Log("Internal Error, Window is not in opening list, but trying to remove: " + winType);
            return false;
        }
        m_openingWindows.Remove(winType);
        return true;
    }

    /// <summary>
    /// 外部接口 关闭UI
    /// </summary>
    /// <param name="win"></param>
    public void HideUI(FUIBase win)
    {
        ControlCommand cmd = new ControlCommand(ControlCommand.CommandType.HIDE, (FUIDef.FWindow)win.FUIWindowType, win);
        m_controlCmdQueue.Enqueue(cmd);
    }

    //外部接口，删除所有的disposeUI
    public void DestoryUI(FUIBase win)
    {
        ControlCommand cmd = new ControlCommand(ControlCommand.CommandType.DESTROY, (FUIDef.FWindow)win.FUIWindowType, win);
        m_controlCmdQueue.Enqueue(cmd);
    }

    //除战斗外全部关闭
    public void HideUIWithOutFight()
    {
        foreach (FUIBase win in windowsDic.Values)
        {
            if (win == null) continue;
            if (win.compName == null) continue;

            if (win.uiShowType != FUIBase.UIShowType.SCREEN && win.isShowing)
            {
                if (win.uiShowType == FUIBase.UIShowType.TIPS)
                    continue;
                HideUI(win);
            }
        }
    }

    //除主界面之外的所有界面关闭
    public void HideUIWithOutMain()
    {
        foreach (FUIBase win in windowsDic.Values)
        {
            if (win == null) continue;
            if (win.compName == null) continue;

            if (win.uiShowType != FUIBase.UIShowType.MAINPAGE && win.isShowing)
            {
                if (win.uiShowType == FUIBase.UIShowType.TIPS)
                    continue;
                HideUI(win);
            }
        }
    }


    //战斗结束界面之外都关闭
    public void HideAllWindowPage()
    {
        foreach (var kv in windowsDic)
        {
            FUIBase win = kv.Value;
            if (win == null) continue;

            FUIDef.FWindow winType = kv.Key;

            if (win.uiShowType == FUIBase.UIShowType.WINDOW && win.isShowing)
            {
                HideUI(win);
            }
        }
    }

    private void InternalHideUI(FUIBase win)
    {
        FUIBase.UIShowType closedUIType = win.uiShowType;

        if (!win.isDisposed)
            GRoot.inst.HideWindow(win);
        //先将这个栈转为list，然后将list从尾部开始遍历，将内容移除
        RemoveFromStack(win);

        if (windowStack.Count > 0)
        {
            var nextWin = windowStack.Peek();
            //nextShowMainPage = null;

            if (nextWin != null)
            {
                var uiType = nextWin.uiShowType;

                if (nextWin.isShowing == false && uiType == FUIBase.UIShowType.SCREEN)
                {
                    GRoot.inst.ShowWindow(nextWin);
                }
                if (closedUIType != FUIBase.UIShowType.TIPS)
                {
                    nextWin.NotifyRefresh();
                }


                //往下找到不是tip类型的刷新一下
                if (uiType == FUIBase.UIShowType.TIPS)
                    FindNextToNotifyRefresh(nextWin, closedUIType);


                BringTipsWindowToFront();

            }
        }
    }


    private void removeWindow(FUIBase win)
    {
        if (!win.isDisposed)
        {
            win.Dispose();
            //win.restartGame();

            RemoveWindowFromDic(win);
        }
        //先将这个栈转为list，然后将list从尾部开始遍历，将内容移除
        RemoveFromStack(win);

    }

    // 彻底释放指定的UI的所有资源
    private void InternalCloseUI(FUIBase win)
    {
        FUIBase.UIShowType closedUIType = win.uiShowType;

        removeWindow(win);

        if (windowStack.Count > 0)
        {
            var nextWin = windowStack.Peek();
            //nextShowMainPage = null;

            if (nextWin != null)
            {
                var uiType = nextWin.uiShowType;

                if (nextWin.isShowing == false && uiType == FUIBase.UIShowType.SCREEN)
                {
                    GRoot.inst.ShowWindow(nextWin);
                }
                if (closedUIType != FUIBase.UIShowType.TIPS)
                {
                    nextWin.NotifyRefresh();
                }


                //往下找到不是tip类型的刷新一下
                if (uiType == FUIBase.UIShowType.TIPS)
                    FindNextToNotifyRefresh(nextWin, closedUIType);


                BringTipsWindowToFront();


            }
        }
    }

    //往下找到可以刷新的
    void FindNextToNotifyRefresh(FUIBase skilPage, FUIBase.UIShowType uIShowType)
    {

        FUIBase[] fuiArray = windowStack.ToArray();

        for (int i = fuiArray.Length - 1; i >= 0; i--)
        {
            FUIBase fui = fuiArray[i];
            if (fui != null && fui.compName != skilPage.compName && fui.uiShowType != FUIBase.UIShowType.TIPS)
            {
                if (fui.isShowing)
                {
                    fui.NotifyRefresh();
                }

                break;
            }
        }
    }

    public string PeekTopWindowName()
    {
        List<FUIBase> allWindows = new List<FUIBase>(windowStack);
        foreach (var e in allWindows)
        {
            if (e != null && e.isShowing && e.uiShowType != FUIBase.UIShowType.TIPS)
            {
                if (!string.IsNullOrEmpty(e.compName))
                    return e.compName;
            }
        }
        return null;
    }

    //先将这个栈转为list，然后将list从尾部开始遍历，将内容移除
    private void RemoveFromStack(FUIBase window)
    {
        //需要确保栈里当前没有这个指定的window，有的话，需要弹出
        FUIBase[] fuiArray = windowStack.ToArray();
        windowStack.Clear();
        for (int i = fuiArray.Length - 1; i >= 0; i--)
        {
            FUIBase fui = fuiArray[i];
            if (fui.compName != window.compName)
                windowStack.Push(fui);
        }
    }


    //判断最近的两个screen是否连在一起
    private bool isWindowInStack(int windowType)
    {
        if (windowStack == null)
            return false;
        FUIBase[] fuiArray = windowStack.ToArray();
        for (int i = fuiArray.Length - 1; i >= 0; i--)
        {
            FUIBase fui = fuiArray[i];
            if (fui.FUIWindowType == windowType)
                return true;
        }

        return false;
    }

    //判断界面是否放在队列中准备显示
    private bool isWindowInControlCmdQueue(FUIDef.FWindow winType)
    {
        foreach (var ctrl in m_controlCmdQueue)
        {
            if (ctrl.windowType == winType)
                if (ctrl.cmdType == ControlCommand.CommandType.SHOW)
                    return true;
        }
        return false;
    }

    /// <summary>
    /// 外部接口 关闭UI
    /// </summary>
    /// <param name="winName"></param>
    public void HideUI(FUIDef.FWindow winName)
    {
        FUIBase win = GetWindow(winName);
        if (win != null)
            HideUI(win);
    }


    /// <summary>
    /// 安卓小退按钮
    /// </summary>
    public void BtnBack()
    {
        //FUIBase win = windowStack.Peek();

        //if (win == null || win.uiShowType == FUIBase.UIShowType.MAINPAGE || win.uiShowType == FUIBase.UIShowType.MAINPAGEOther)
        //{
        //    //弹出提示，是否退出游戏
        //    FUI_UIService.Instance.ShowCommonTipWindow("TXT-8326", "TXT-12", "TXT-15");
        //    FUI_UIService.Instance.ShowCommonTipWindow(null, () =>
        //    {
        //        GameUtilFunc.QuitGame("BtnBack");
        //    });
        //}
        //else
        //{
        //    if (win.backBtnEnabled)//&& GuideSystem.Instance != null && GuideSystem.Instance.curGuideID == 0)
        //    {
        //        HideUI(win);
        //    }
        //}
    }

    /// <summary>
    /// 找某个UI的控制脚本实例
    /// </summary>
    /// <param name="win"></param>
    /// <param name="script"></param>
    /// <returns></returns>
    public FUIBase GetWindow(FUIDef.FWindow win)
    {
        FUIBase winUI = null;
        if (windowsDic.TryGetValue(win, out winUI)
              && winUI != null)
        { }
        else
            winUI = null;

        return winUI;
    }

    private bool isPackageLoaded(string packageName)
    {
        return loadedPackageDict.ContainsKey(packageName);
    }

    //累加包引用计数
    public int IncPackageReference(string packageName)
    {
        int refCount = 0;
        if (packageRefCountDict.ContainsKey(packageName))
        {
            refCount = packageRefCountDict[packageName];
        }
        refCount++;

        packageRefCountDict[packageName] = refCount;

        return refCount;
    }

    // 减少包引用计数
    private int decPackageReference(string packageName)
    {
        int refCount = 0;
        if (packageRefCountDict.ContainsKey(packageName))
        {
            refCount = packageRefCountDict[packageName];
        }
        else
        {
            Debug.Log("No Reference Count Recorded For Package: " + packageName);
        }

        refCount--;

        if (refCount < 0)
        {
            Debug.Log("Internal Error: Package Reference Count Invalid: " + refCount + ", Package Name: " + packageName);
        }

        return refCount;
    }

    /// <summary>
    /// 加载一个UI并显示
    /// </summary>
    private bool LoadUIAndShow(ControlCommand cmd)
    {
        FUIDef.FWindow winType = cmd.windowType;

        AddOpeningWindow(winType);

        FUIDef.FPackage package = FUIDef.windowUIpair[cmd.windowType];
        FUIBase win = cmd.window;
        win.packageName = package.ToString();
        win.compName = cmd.windowType.ToString();
        IncPackageReference(win.packageName);
        if (isPackageLoaded(win.packageName))
        {
            RemoveOpeningWindow(winType);
            AfterLoad(cmd);
        }
        else
        {
            ////#if UNITY_EDITOR
            //UIPackage.AddPackage(win.packageName, LoadFunc,(paramPack)=> {
            //    loadedPackageDict.Add(win.packageName, true);
            //    RemoveOpeningWindow(winType);
            //    AfterLoad(cmd);
            //});
            LoadDonePakage(win.packageName, () =>
            {
                UIPackage.AddPackage(win.packageName, LoadFunc);
                loadedPackageDict.Add(win.packageName, true);
                RemoveOpeningWindow(winType);
                AfterLoad(cmd);

            });
        }
        return true;
    }
    #region yooasset 加载资源
    // 资源句柄列表
    //private List<AssetOperationHandle> _handles = new List<AssetOperationHandle>();


    public void PreAddPackage(string package_name, UnityAction unityaction)
    {
        //UIPackage.AddPackage(package_name, LoadFunc,(packageParam)=> {
        //    loadedPackageDict.Add(package_name, true);

        //    if (unityaction != null)
        //        unityaction();
        //});
        LoadDonePakage(package_name, () =>
        {
            UIPackage.AddPackage(package_name, LoadFunc);
            loadedPackageDict.Add(package_name, true);

            if (unityaction != null)
                unityaction();
        });
    }
    #endregion

    /// <summary>
    /// 加载后打开UI时的操作
    /// </summary>
    /// <param name="cmd"></param>
    void AfterLoad(ControlCommand cmd)
    {

        windowsDic[cmd.windowType] = cmd.window;
        PushToStack(cmd.window, cmd.windowType);

        cmd.window.Show();

        var windowType = cmd.window.uiShowType;

        if (windowType == FUIBase.UIShowType.MAINPAGE)
            cmd.window.WindowLiveness = -1;
        else
            cmd.window.WindowLiveness = 0;

        var animationType = cmd.window.animationType;
        if (animationType == 0 && (windowType == FUIBase.UIShowType.WINDOW))
            animationType = (int)OpenUIAnimationType.WindowOpen;
        UIAnimationService.Inst.StartAnimationTypeBafore(animationType, cmd.window);


        //将这个内容变动
        UIAnimationService.Inst.StartAnimationByType(animationType, cmd.window);

        if (cmd.jobDoneDo != null)
        {
            cmd.jobDoneDo(cmd.window);
            cmd.jobDoneDo = null;
        }

        cmd.window.Refresh(cmd.param);
        //这里层级管理
        switch (cmd.window.uiShowType)
        {
            case FUIBase.UIShowType.MAINPAGE:
                break;
            case FUIBase.UIShowType.SCREEN:
                HideAllWindowPage();
                HideSomeLayer(FUIBase.UIShowType.SCREEN, cmd.window);
                break;
            case FUIBase.UIShowType.WINDOW:
                break;
            case FUIBase.UIShowType.TIPS:
                break;
        }

        BringTipsWindowToFront();


        UpdateWindowsLiveness();


    }

    // 更新所有窗口的生命周期
    private void UpdateWindowsLiveness()
    {
        if (windowsDic == null)
        {
            return;
        }

        int winCount = windowsDic.Count;
        if (winCount <= 0)
        {
            return;
        }

        FUIBase oldestWin = null;
        int oldestWinLiveness = 0;
        FUIDef.FWindow fWindow = FUIDef.FWindow.TestUI;

        foreach (var win in windowsDic)
        {
            var winFuibase = win.Value;
            if (winFuibase.WindowLiveness != -1)
            {
                winFuibase.WindowLiveness++;

                if (winFuibase.WindowLiveness > oldestWinLiveness)
                {
                    fWindow = win.Key;
                    oldestWin = winFuibase;
                    oldestWinLiveness = winFuibase.WindowLiveness;
                }
            }
        }

        // 最大开启的窗口超过6个了，尝试把最老的不在栈上的窗口释放掉
        if (winCount > 6 && oldestWinLiveness > 10 && oldestWin != null && !isWindowInStack(oldestWin.FUIWindowType) && !isWindowInControlCmdQueue(fWindow))
        {
            removeWindow(oldestWin);
            int refCount = decPackageReference(oldestWin.packageName);

            Debug.Log("Oldest Window Closed: " + (FUIDef.FWindow)oldestWin.FUIWindowType);

            // 如果窗口释放后资源包也没有引用了，是释放资源包
            if (refCount == 0)
            {
                string bundleName = GetUIAbName(oldestWin.packageName);

                Debug.Log("++++++++++++ Remove FGUIPackage: " + oldestWin.packageName);

                //BundleLoader.RemoveFGUIPackage(bundleName, () =>
                //{
                //    loadedPackageDict.Remove(oldestWin.packageName);
                //});
            }
        }
    }


    //删除一个窗口从字典中
    private void RemoveWindowFromDic(FUIBase window)
    {
        FUIDef.FWindow winName = (FUIDef.FWindow)window.FUIWindowType; // Enum.Parse(typeof(FUIDef.FWindow), window.compName);
        if (windowsDic.ContainsKey(winName))
        {
            windowsDic.Remove(winName);
        }
    }


    //将tip的窗口放置所有窗口最前面，将guide窗口放置tip后面其他界面前面
    void BringTipsWindowToFront()
    {
        bool hasTip = false;
        foreach (var win in windowsDic.Values)
        {
            if (win != null && win.isShowing && win.uiShowType == FUIBase.UIShowType.TIPS)
            {
                GRoot.inst.BringToFront(win);
                hasTip = true;
                break;
            }
        }

        //foreach (var win in windowsDic.Values)
        //{
        //    if (win != null && win.isShowing)
        //    {
        //        if (hasTip)
        //        {
        //            GRoot.inst.GetChildIndex(win);
        //            GRoot.inst.SetChildIndex(win, GRoot.inst.numChildren - 1);
        //        }
        //        else
        //        {
        //            GRoot.inst.BringToFront(win);
        //        }
        //        break;
        //    }
        //}
    }

    private void PushToStack(FUIBase window, FUIDef.FWindow winType)
    {
        //需要确保栈里当前没有这个指定的window，有的话，需要弹出
        FUIBase[] fuiArray = windowStack.ToArray();
        windowStack.Clear();

        for (int i = fuiArray.Length - 1; i >= 0; i--)
        {
            FUIBase fui = fuiArray[i];
            if (fui.compName == winType.ToString()) continue;

            windowStack.Push(fui);
        }

        windowStack.Push(window);
    }

    //关闭某些界面类型
    void HideSomeLayer(FUIBase.UIShowType layer, FUIBase dontHideMe)
    {
        FUIDef.FWindow dontHideWin = (FUIDef.FWindow)dontHideMe.FUIWindowType; // Enum.Parse(typeof(FUIDef.FWindow), dontHideMe.compName);
        foreach (FUIBase win in windowsDic.Values)
        {
            if (win == null) continue;
            FUIDef.FWindow winName = (FUIDef.FWindow)win.FUIWindowType; // Enum.Parse(typeof(FUIDef.FWindow), win.compName);
            if (winName != dontHideWin && win.uiShowType == layer && win.isShowing)
            {
                if (layer == FUIBase.UIShowType.SCREEN)
                {
                    win.Hide();
                }

                else
                    HideUI(win);
            }
        }
    }

    /// <summary>
    /// 根据packageName获取asset bundle name
    /// </summary>
    /// <param name = "type" ></ param >
    /// < returns ></ returns >
    public string GetUIAbName(string packageName)
    {
        //return "FGUIRes/" + packageName;
        return packageName;
    }

    public delegate void CreateWindowDelegate(FUIBase baseUI);
    #region UI指令循环
    /// <summary>
    /// 用于记录指令
    /// </summary>
    private class ControlCommand
    {
        public enum CommandType
        {
            NONE = 0,
            SHOW = 1,
            HIDE = 2,
            DESTROY = 3,
            CREATE = 4,
            REFRESH = 5,
        }
        public ControlCommand(CommandType CMDTYPE,
            FUIDef.FWindow win,
            FUIBase _win,
            CreateWindowDelegate createWindowDelegate = null,
            System.Object _param = null)
        {
            cmdType = CMDTYPE;
            windowType = win;
            window = _win;
            jobDoneDo = createWindowDelegate;
            param = _param;
        }

        /// <summary>
        /// UI窗口名
        /// </summary>
        public FUIDef.FWindow windowType;

        /// <summary>
        /// 命令类型
        /// </summary>
        public CommandType cmdType = CommandType.NONE;
        /// <summary>
        /// window的实例
        /// </summary>
        public FUIBase window;
        /// <summary>
        /// 命令执行完毕的回调
        /// </summary>
        public CreateWindowDelegate jobDoneDo;
        /// <summary>
        /// UI打开后的refresh的参数
        /// </summary>
        public System.Object param;
        /// <summary>
        /// 如果是true 则不执行这个操作
        /// </summary>
        public bool invalid = false;
    }


    void UIMgrUpdate()
    {
        //每帧最多处理一个UI事件
        ExecutiveCommand();
        if (Application.platform == RuntimePlatform.WindowsEditor)
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                KeyBoardESCClick();
            }
    }

    void KeyBoardESCClick()
    {
        if (windowStack.Count > 0)
        {
            var topWin = windowStack.Peek();
            if (topWin != null)
            {
                var winType = (FUIDef.FWindow)topWin.FUIWindowType;
                if (winType != FUIDef.FWindow.BattlePanel
                && winType != FUIDef.FWindow.BattlePrepare
                && winType != FUIDef.FWindow.WorldPanel
                )
                {
                    HideUI(topWin);
                }
            }
        }
    }


    //每帧检查内容
    private bool ExecutiveCommand()
    {
        if (m_controlCmdQueue.Count <= 0)
        {
            return false;
        }

        ControlCommand command = null;
        command = m_controlCmdQueue.Dequeue();
        while (command.invalid && m_controlCmdQueue.Count > 0)
        {
            command = m_controlCmdQueue.Dequeue();
        }
        if (command.invalid)
        {
            Debug.Log("got invalid Fuimanager Command");
            return false;
        }
        if (command == null)
        {
            Debug.Log("Internal Error: Invalid ControlCommand Encountered");
            return false;
        }

        //Debug.Log("command.cmdTypecommand.cmdType" + command.cmdType);
        switch (command.cmdType)
        {
            case ControlCommand.CommandType.DESTROY:
                {
                    InternalCloseUI(command.window);
                    break;
                }
            case ControlCommand.CommandType.HIDE:
                {
                    InternalHideUI(command.window);
                    break;
                }
            case ControlCommand.CommandType.SHOW:
                {
                    FUIBase ui = GetWindow(command.windowType);
                    if (ui != null)
                    {
                        AfterLoad(command);
                    }
                    else
                    {
                        if (IsWindowOpening(command.windowType))
                        {
                            Debug.Log("同时开了两个界面，但是有一个正在加载中");
                            break;
                        }
                        else
                            LoadUIAndShow(command);
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Internal Error: Unhandled ControlCommand Type: " + command.cmdType);
                    break;
                }
        }

        return true;

    }


    //重启界面时，先将所有的内容都关闭
    public void CloseAllPage()
    {
        if (windowsDic == null)
        {
            return;
        }

        int winCount = windowsDic.Count;
        if (winCount <= 0)
        {
            return;
        }

        foreach (var window in windowsDic.Values)
        {
            if (window != null)
            {
                window.Hide();
            }
        }

        List<FUIBase> windowsList = new List<FUIBase>(windowsDic.Values);

        foreach (var window in windowsList)
        {
            if (window != null)
            {
                removeWindow(window);
                decPackageReference(window.packageName);
            }
        }

        //checkReleasePackage();

        m_controlCmdQueue.Clear();
        windowsDic.Clear();
        m_openingWindows.Clear();
    }
    #endregion

    #region 将包全部分类下
    static Dictionary<string, List<string>> allSavePackageUI;
    public static void ReSetBundle()
    {
        if (allSavePackageUI == null)
            allSavePackageUI = new Dictionary<string, List<string>>();
        else
            allSavePackageUI.Clear();

        var allInfos = YooAssets.GetAssetInfos("fgui");
        foreach (var info in allInfos)
        {
            var path = info.AssetPath;
            var GetRealName = path.Split("/");
            path = GetRealName[GetRealName.Length - 1];

            var saveName = path.Split("_");
            if (!allSavePackageUI.ContainsKey(saveName[0]))
                allSavePackageUI.Add(saveName[0], new List<string>());


            var findName = info.AssetPath;
            if (findName.StartsWith("Assets/GameRes/"))
                findName = findName.Replace("Assets/GameRes/", "");
            //findName = System.IO.Path.GetFileName(findName);
            allSavePackageUI[saveName[0]].Add(findName);

        }

    }

    Dictionary<string, UnityEngine.Object> allLoadInfos = new Dictionary<string, UnityEngine.Object>(); //下载到的所有数据

    //加载完这个包
    Dictionary<string, int> loadingNum = null;
    public void LoadDonePakage(string packageName, UnityAction unityAction)
    {
        if (allSavePackageUI.ContainsKey(packageName))
        {
            if (loadingNum == null)
                loadingNum = new Dictionary<string, int>();

            var packageNames = allSavePackageUI[packageName];
            int allNum = packageNames.Count;
            int numSet = 0;
            loadingNum[packageName] = allNum;
            foreach (var name in packageNames)
            {
                numSet = numSet + 1;
                if (name.EndsWith(".bytes"))
                    ResObjPoolTake<TextAsset>(name, new CacheBack()
                    {
                        customParam = packageName,
                        customParam2 = name,
                        customAction = unityAction,
                        customActionBackCache = CallBack
                    });
                else if (name.EndsWith(".png"))
                    ResObjPoolTake<Texture>(name, new CacheBack()
                    {
                        customParam = packageName,
                        customParam2 = name,
                        customAction = unityAction,
                        customActionBackCache = CallBack
                    });
                else
                    ResObjPoolTake<AudioClip>(name, new CacheBack()
                    {
                        customParam = packageName,
                        customParam2 = name,
                        customAction = unityAction,
                        customActionBackCache = CallBack
                    });
            }
        }
        else
        {
            if (unityAction != null)
                unityAction();
            Debugger.LogError("没有这个包的ui啊" + packageName);
        }

    }

    void CallBack(CacheBack callBack)
    {
        if (callBack == null)
            return;

        if (allLoadInfos == null)
            allLoadInfos = new Dictionary<string, UnityEngine.Object>();
        allLoadInfos[callBack.customParam2] = callBack.CacheBackObj;
        Debugger.Log($"allLoadInfos insert asset {callBack.customParam2}");
        var packageNameShow = (string)callBack.customParam;

        loadingNum[packageNameShow] = loadingNum[packageNameShow] - 1;

        if (callBack.customAction != null && loadingNum[packageNameShow] <= 0)
            callBack.customAction();

    }

    #region Res Load
    private static Dictionary<string, UnityEngine.Object> cacheObjInfos = new Dictionary<string, UnityEngine.Object>(); //缓存的数据

    /// 从池中拿取一个GameObject
    static public void ResObjPoolTake<TObject>(string name, CacheBack cacheBack) where TObject : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(name))
        {
            Debugger.LogError("Invalid Name Give To Take, Path: " + name);
            return;
        }

        string poolName = name;
        UnityEngine.Object info;

        var bExist = cacheObjInfos.TryGetValue(poolName, out info);//寻找对应GameObject的池

        if (cacheBack == null)
        {
            Debugger.LogError("回调有问题啊" + name);
            return;
        }
        if (bExist)
        {
            info = cacheObjInfos[poolName];

            cacheBack.CacheBackObj = info;
            if (cacheBack.customActionBackCache != null)
                cacheBack.customActionBackCache(cacheBack);
        }
        else
        {

            AssetHandle assetOperationHandle;
            Debugger.Log($"1102 load {name}");
            assetOperationHandle = YooAssets.LoadAssetSync<TObject>(name);


            assetOperationHandle.cacheBack = new CacheBack();
            assetOperationHandle.cacheBack.customParam = name;  //名称
            assetOperationHandle.cacheBack.customParamObj = cacheBack; //cache的数据
            assetOperationHandle.Completed += (aassetLoad) =>
            {
                if (aassetLoad.AssetObject == null)
                {
                    Debugger.LogError("获取的组件怎么会是空呢" + aassetLoad.cacheBack.customParam);
                    return;
                }

                var getInfo = aassetLoad.AssetObject;

                cacheObjInfos[aassetLoad.cacheBack.customParam] = getInfo;

                var CacheBack = (CacheBack)assetOperationHandle.cacheBack.customParamObj;
                CacheBack.CacheBackObj = getInfo;
                if (CacheBack.customActionBackCache != null)
                    CacheBack.customActionBackCache(CacheBack);
            };

        }
    }

    #endregion

    private object LoadFunc(string name, string extension, System.Type type, out DestroyMethod method)
    {
        method = DestroyMethod.None; //注意：这里一定要设置为None
        string location = $"FGUIRes/{name}{extension}";
        if (allLoadInfos.ContainsKey(location))
            return allLoadInfos[location];
        else
        {
            Debugger.LogError("这个ui没找到啊" + name + "," + extension);
            return null;
        }
    }

    #endregion
}
