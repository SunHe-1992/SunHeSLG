using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using YooAsset;
/// <summary>
/// 流程更新完毕
/// </summary>
internal class FsmUpdaterDone : IStateNode
{
    void IStateNode.OnCreate(StateMachine machine)
    {
    }
    void IStateNode.OnEnter()
    {
        PatchEventDefine.PatchStatesChange.SendEventMessage("start game！");

        var obj = new GameObject("GameLoader");
        var gl = obj.AddComponent<GameLoader>();
        UnityEngine.Object.DontDestroyOnLoad(obj);
        // 设置默认的资源包
        var gamePackage = YooAssets.GetPackage("DefaultPackage");
        YooAssets.SetDefaultPackage(gamePackage);
        gl.InitEnv();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }
}
