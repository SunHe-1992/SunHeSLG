using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using UniFramework.Module;

/// <summary>
/// 流程更新完毕
/// </summary>
internal class FsmPatchDone : IStateNode
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
        gl.InitEnv();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }
}
