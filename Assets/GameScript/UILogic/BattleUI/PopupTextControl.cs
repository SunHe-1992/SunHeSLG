using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using FairyGUI;
using System.Collections;
using UnityEngine.Events;


//飘字创建
public static class PopupTextControl
{
    public static void CreateTextFairyLeft(string content, FontType ft)
    {
        CreateTextFairy(content, ft, 0.30f, 0.33f);
    }
    public static void CreateTextFairyRight(string content, FontType ft)
    {
        CreateTextFairy(content, ft, 0.70f, 0.33f);
    }


    /// <summary>
    /// 新版伤害文本
    /// </summary>
    public static void CreateTextFairy(string content, FontType ft, float xPct, float yPct)
    {
        //空提示，跳过
        if (string.IsNullOrEmpty(content))
            return;

        if (UIPage_CombatPanel.Instance == null)
            return;
        var uiBattle = UIPage_CombatPanel.Instance;
        var obj = uiBattle.GetIndicatorObj();
        var mItem = obj as PackageBattle.UI_DamageIndicator;
        mItem.t0.Play(uiBattle.UpdateRecycleIndicator);

        obj.visible = true;
        obj.SetXY(xPct * GRoot.inst.width, yPct * GRoot.inst.height);
        //伤害文本对象
        GTextField numText = null;
        //文本前缀
        string preStr = "";

        mItem.txt_green.visible = false;
        mItem.txt_yellow.visible = false;

        switch (ft)
        {
            case FontType.Heal: numText = mItem.txt_green; break;
            case FontType.PAtk: numText = mItem.txt_yellow; break;
            case FontType.EAtk: numText = mItem.txt_yellow; break;
        }
        string full_content = preStr + content;
        if (numText != null)
        {
            numText.text = full_content;
            numText.visible = true;
        }
    }

}
//字体类型
public enum FontType
{
    PAtk,
    EAtk,
    Heal,   //恢复
    Crit,   //暴击
    Other,  //其他
    FightText,  //buff飘字
    MeasureText, //护盾飘字
}
