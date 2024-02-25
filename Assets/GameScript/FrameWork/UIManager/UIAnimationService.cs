using FairyGUI;
using UnityEngine;
using UniFramework.Singleton;
using static FUIManager;

/// <summary>
/// ui动画的播放内容
/// </summary>
public class UIAnimationService : ISingleton
{

    public static UIAnimationService Inst { get; private set; }
    public static void Init()
    {
        Inst = UniSingleton.CreateSingleton<UIAnimationService>();
    }
    public void OnCreate(object createParam)
    {
    }

    public void OnUpdate()
    {
    }

    public void OnDestroy()
    {
    }

    int windowType = (int)OpenUIAnimationType.NoAnimation;

    Vector2 Vector2 = new Vector2(1, 1);
    //开启界面前的设置
    public void StartAnimationTypeBafore(int animationType, FUIBase fuibasePage)
    {
        if (fuibasePage == null)
            return;

        if (animationType == windowType)
        {
            var panel = fuibasePage.contentPane;
            if (panel == null || panel.numChildren < 1)
            {
                return;
            }

            var firstCom = panel.GetChildAt(0);
            //var type = firstCom.GetType().ToString();

            //if (type == "FairyGUI.GGraph" && firstCom.width >= 720 && firstCom.height >= 1280)
            {

                firstCom.SetPivot(0.5f, 0.5f, true);
                firstCom.SetScale(100, 100);
            }


            panel.SetPivot(0.5f, 0.5f, false);
            panel.SetScale(0.2f, 0.2f);

        }
    }

    //播放动画通过类型(暂时只处理window的类型)
    public void StartAnimationByType(int animationType, FUIBase fuibasePage)
    {
        if (fuibasePage == null)
            return;

        if (animationType == windowType)
        {
            var panel = fuibasePage.contentPane; ;
            //界面从小变到大
            if (panel == null || panel.numChildren < 1)
            {
                return;
            }
            panel.TweenScale(Vector2, 0.25f).SetEase(EaseType.QuartInOut);
            panel.TweenFade(1, 0.25f).SetEase(EaseType.QuartInOut).OnComplete(() =>
            {
                panel.alpha = 1f;
                var firstCom = panel.GetChildAt(0);
                //var type = firstCom.GetType().ToString();
                //if (type == "FairyGUI.GGraph" && firstCom.width >= 720 && firstCom.height >= 1280)
                {
                    firstCom.SetScale(1, 1);
                    firstCom.position = new Vector3(GRoot.inst.width / 2, GRoot.inst.height / 2, 0);
                }

            });
        }
    }


}
