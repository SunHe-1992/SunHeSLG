using System.Collections;
using UniFramework.Event;
using UniFramework.Module;
using UnityEngine;
using YooAsset;

public class Boot : MonoBehaviour
{
    /// <summary>
    /// 资源系统运行模式
    /// </summary>
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
    public static EPlayMode GamePlayMode;

    void Awake()
    {
        Debug.Log($"资源系统运行模式：{PlayMode}");
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
    }
    IEnumerator Start()
    {
        GamePlayMode = PlayMode;

        // 游戏管理器
        GameManager.Instance.Behaviour = this;

        // 初始化事件系统
        UniEvent.Initalize();


        // 初始化管理系统
        UniModule.Initialize();

        // 初始化资源系统
        YooAssets.Initialize();
        YooAssets.SetOperationSystemMaxTimeSlice(30);

        // 开始补丁更新流程
        PatchOperation operation = new PatchOperation("DefaultPackage", EDefaultBuildPipeline.BuiltinBuildPipeline.ToString(), PlayMode);
        YooAssets.StartOperation(operation);
        yield return operation;

       

        // 切换到主页面场景
        SceneEventDefine.ChangeToHomeScene.SendEventMessage();
    }
}
