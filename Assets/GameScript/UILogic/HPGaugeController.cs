//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using SunHeTBS;
//using UnityEngine.UI;
//using cfg.SLG;
//public class HPGaugeController : MonoBehaviour
//{

//    Slider hpSlider;
//    Image weaponImage1;
//    Image weaponImage2;
//    //Camera mapCam;
//    // Start is called before the first frame update
//    void Start()
//    {
//        FindTrans();
//    }
//    void FindTrans()
//    {
//        if (hpSlider == null)
//            hpSlider = transform.Find("Slider").GetComponent<Slider>();
//        if (weaponImage1 == null)
//            weaponImage1 = transform.Find("weaponImage1").GetComponent<Image>();
//        if (weaponImage2 == null)
//            weaponImage2 = transform.Find("weaponImage2").GetComponent<Image>();

//    }

//    // Update is called once per frame
//    //void Update()
//    //{
//    //    if (mapCam == null)
//    //        mapCam = TBSMapService.Inst.mapCamera.m_camera;
//    //    this.transform.rotation = mapCam.transform.rotation;
//    //}
//    public void InitGauge(PawnCamp camp)
//    {
//        FindTrans();
//        SetUpHpBarColor(camp);
//        hpSlider.gameObject.SetActive(true);
//        //SetHpValue(hp, hpMax);
//        this.GetComponent<RectTransform>().localPosition = new Vector3(0, 0.58f, -0.58f);
//    }
//    void SetUpHpBarColor(PawnCamp camp)
//    {
//        Image hpBarImage = transform.Find("Slider/Fill").GetComponent<Image>();
//        Color barColor = Color.blue;
//        switch (camp)
//        {
//            case PawnCamp.Player: barColor = new Color(14f / 255f, 118f / 255f, 201f / 255f); break;
//            case PawnCamp.PlayerAlly: barColor = new Color(123 / 255f, 172 / 255f, 32 / 255f); break;
//            case PawnCamp.Villain: barColor = new Color(210 / 255f, 66 / 255f, 70 / 255f); break;
//            case PawnCamp.Neutral: barColor = new Color(225 / 255f, 207 / 255f, 93 / 255f); break;
//        }
//        hpBarImage.color = barColor;
//    }
//    public void SetHpValue(int hp, int hpMax)
//    {
//        hpSlider.value = hp / hpMax;

//    }
//    public void SetWeaponIcons(ItemType iType)
//    {
//        if (iType != ItemType.Item)
//        {
//            string picName = "UISprite/icon" + iType.ToString();
//            UIService.Inst.LoadUnitySprite(picName, weaponImage1);
//            weaponImage1.gameObject.SetActive(true);
//        }
//        else
//        {
//            weaponImage1.gameObject.SetActive(false);
//        }
//        weaponImage2.gameObject.SetActive(false);
//    }
//}
