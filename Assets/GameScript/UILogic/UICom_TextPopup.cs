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
    #region 伤害指示器
    /// <summary>
    /// 伤害指示器的列表
    /// </summary>
    GList indicatorPoolList;
    /// <summary>
    /// 战斗内相机
    /// </summary>
    Camera battleCam;
    /// <summary>
    /// 相机移动向量
    /// </summary>
    public Vector3 camScreenOffset;
    void ResetIndicator(GObject obj)
    {
        if (obj != null)
        {
            var mItem = obj as UI_DamageIndicator;
            mItem.txt_green.text = null;
            mItem.txt_yellow.text = null;
            mItem.data = null;
        }
    }
    /// <summary>
    /// 资源池拿出一个组件
    /// </summary>
    /// <returns></returns>
    public GObject GetIndicatorObj()
    {
        GObject obj = indicatorPoolList.GetFromPool(null);
        ResetIndicator(obj);
        ui.ComIndicator.AddChild(obj);
        return obj;
    }
    /// <summary>
    /// 返还资源池
    /// </summary>
    /// <param name="obj"></param>
    public void IndicatorBackToPool(GObject obj)
    {
        if (obj != null && indicatorPoolList != null)
        {
            ui.ComIndicator.RemoveChild(obj);
            indicatorPoolList.RemoveChildToPool(obj);
        }
    }
    /// <summary>
    /// 清空界面的伤害组件
    /// </summary>
    void ClearIndicators()
    {
        var comIndi = ui.ComIndicator;
        int count = comIndi.numChildren;
        for (int i = count - 1; i > 0; i--)
        {
            comIndi.RemoveChildAt(i, true);
        }
        indicatorPoolList.RemoveChildrenToPool();
    }
    bool IndicatorUpdatedFrame = false;
    /// <summary>
    /// 清理播放完成的伤害组件
    /// </summary>
    public void UpdateRecycleIndicator()
    {
        if (IndicatorUpdatedFrame)
            return;
        if (ui.ComIndicator == null)
            return;
        IndicatorUpdatedFrame = true;
        var comIndi = ui.ComIndicator;
        int count = comIndi.numChildren;
        for (int i = count - 1; i > 0; i--)
        {
            var obj = comIndi.GetChildAt(i) as UI_DamageIndicator;
            if (obj != null && !obj.t0.playing)
            {
                comIndi.RemoveChildAt(i);
                indicatorPoolList.RemoveChildToPool(obj);
            }
        }
    }
    void UpdateIndicatorPos()
    {
        if (ui.ComIndicator == null)
            return;

        var comIndi = ui.ComIndicator;
        int count = comIndi.numChildren;
        for (int i = count - 1; i > 0; i--)
        {
            var mItem = comIndi.GetChildAt(i) as UI_DamageIndicator;
            if (mItem != null && mItem.data != null)
            {
                Vector3 headPos = (Vector3)mItem.data;
                var screenPoint = battleCam.WorldToScreenPoint(headPos);
                var grootInst = GRoot.inst;
                Vector2 pt = grootInst.GlobalToLocal(screenPoint);
                pt.y = grootInst.height - pt.y;

                int borderWidth = 80;

                if (pt.y > grootInst.height)
                {
                    pt.y = grootInst.height;
                }

                if (pt.x < borderWidth)
                {
                    pt.x = borderWidth;
                }

                if (pt.x > grootInst.width - borderWidth)
                {
                    pt.x = grootInst.width - borderWidth;
                }

                mItem.xy = pt;
            }
        }

    }

    void TestFunction()
    {
        Debugger.Log("actionPawnList");
        var list = BLogic.Inst.actionPawnList;
        foreach (var p in list)
        {
            Debugger.Log(p.ToString());
        }

        Debugger.Log("villianPawnList");
        var list2 = BLogic.Inst.villianPawnList;
        foreach (var p in list2)
        {
            Debugger.Log(p.ToString());
        }
    }
    #endregion
}
