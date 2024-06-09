using System.Collections;
using System.Collections.Generic;
using UniFramework.Singleton;


public class BLogic : ISingleton
{
    public static BLogic Inst;
    public static void Init()
    {
        Inst = UniSingleton.CreateSingleton<BLogic>();
    }

    public void OnCreate(object createParam)
    {
    }

    public void OnDestroy()
    {
    }

    public void OnUpdate()
    {
    }
}
