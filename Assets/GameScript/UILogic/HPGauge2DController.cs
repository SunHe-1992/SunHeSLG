using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SunHeTBS;
using UnityEngine.UI;
using cfg.SLG;

public class HPGauge2DController : MonoBehaviour
{
    SpriteRenderer whiteBar;
    SpriteRenderer HPBar;
    SpriteRenderer weaponImage1;
    SpriteRenderer weaponImage2;
    SpriteRenderer forbidIcon;

    public void InitGauge(PawnCamp camp)
    {
        FindTrans();
        SetForbidIcon(false);
    }
    void FindTrans()
    {
        whiteBar = transform.Find("whiteBar").GetComponent<SpriteRenderer>();
        HPBar = transform.Find("HPBar").GetComponent<SpriteRenderer>();
        if (weaponImage1 == null)
            weaponImage1 = transform.Find("weaponImage1").GetComponent<SpriteRenderer>();
        if (weaponImage2 == null)
            weaponImage2 = transform.Find("weaponImage2").GetComponent<SpriteRenderer>();
        forbidIcon = transform.Find("forbidIcon").GetComponent<SpriteRenderer>();
    }
    Dictionary<PawnCamp, string> campSpNameDic = new Dictionary<PawnCamp, string>()
    {
        {PawnCamp.Player, "bar_blue" },
        {PawnCamp.PlayerAlly, "bar_green1" },
        {PawnCamp.Villain, "bar_red2" },
        {PawnCamp.Neutral, "bar_yellow" },

    };
    void SetUpHpBarColor(PawnCamp camp)
    {
        string spName = campSpNameDic[camp];
        HPBar.sprite = UIService.Inst.LoadUnitySprite(spName);
    }
    public void SetHpValue(int hp, int hpMax)
    {
        if (hpMax <= 0) SetHPPercent(0);
        SetHPPercent((float)hp / (float)hpMax);
    }
    void SetHPPercent(float pct)
    {
        if (pct > 1) pct = 1;
        if (pct < 0) pct = 0;
        var oldScale = HPBar.transform.localScale;
        HPBar.transform.localScale = new Vector3(pct, oldScale.y, oldScale.z);
        var oldPos = HPBar.transform.localPosition;
        HPBar.transform.localPosition = new Vector3((pct - 1) / 2f, oldPos.y, oldPos.z);
    }

    public void SetWeaponIcons(ItemType iType)
    {
        if (iType != ItemType.Item)
        {
            string picName = "UISprite/icon_" + iType.ToString();
            weaponImage1.sprite = UIService.Inst.LoadUnitySprite(picName);
            weaponImage1.gameObject.SetActive(true);
        }
        else
        {
            weaponImage1.gameObject.SetActive(false);
        }
        weaponImage2.gameObject.SetActive(false);
    }
    public void SetForbidIcon(bool isshow)
    {
        this.forbidIcon.gameObject.SetActive(isshow);
    }
}
