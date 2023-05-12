using System.Collections;
using UnityEngine;
using UniFramework.Machine;
using UniFramework.Module;
using YooAsset;

/// <summary>
/// 下载更新文件
/// </summary>
public class FsmDownloadFiles : IStateNode
{
	private StateMachine _machine;

	void IStateNode.OnCreate(StateMachine machine)
	{
		_machine = machine;
	}
	void IStateNode.OnEnter()
	{
		PatchEventDefine.PatchStatesChange.SendEventMessage("start downloading patch files！");
		UniModule.StartCoroutine(BeginDownload());
	}
	void IStateNode.OnUpdate()
	{
	}
	void IStateNode.OnExit()
	{
	}

	private IEnumerator BeginDownload()
	{
		var downloader = PatchManager.Instance.Downloader;

		// 注册下载回调
		downloader.OnDownloadErrorCallback = PatchEventDefine.WebFileDownloadFailed.SendEventMessage;
		downloader.OnDownloadProgressCallback = PatchEventDefine.DownloadProgressUpdate.SendEventMessage;
		downloader.BeginDownload();
		yield return downloader;

		// 检测下载结果
		if (downloader.Status != EOperationStatus.Succeed)
			yield break;

		_machine.ChangeState<FsmPatchDone>();
	}
}
