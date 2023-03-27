using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GameValues
{
    public static int InternalVersion;    //登录之后获取到的内部版本号
}

public class GameUtilFunc
{
    // Help function to construct json uicontrolData
    public static void AddJsonStart(StringBuilder sb)
    {
        sb.Append("{");
    }

    public static void AddJsonField(StringBuilder sb, string name, string value)
    {
        sb.Append("\"");
        sb.Append(name);
        sb.Append("\":\"");
        sb.Append(value);
        sb.Append("\"");
    }

    public static void AddJsonSeparator(StringBuilder sb)
    {
        sb.Append(",");
    }

    public static void AddJsonEnd(StringBuilder sb)
    {
        sb.Append("}");
    }

    public static void UnhandledExpHander(object sender, UnhandledExceptionEventArgs expArgs)
    {
        string debugInfo = "Unhandled Exception:";

        if (expArgs != null && expArgs.ExceptionObject != null)
        {
            debugInfo += expArgs.ExceptionObject.ToString();
        }

        Debugger.LogError(debugInfo);

        // TODO: Report To Server
    }


    public static string BuildAssetAddress(string assetName)
    {
        string address = assetName.Replace("/", "_");
        address = address.Replace("@", "_");
        address = address.ToLower();

        return address;
    }

    // 从指定的字符串中过滤掉trim子串
    public static string TrimString(string fullStr, string trimStr)
    {
        string resultStr = fullStr.Replace(trimStr, "");
        return resultStr;
    }


    public static string GetBundleExt(string bundleLabel, string bundleName)
    {
        int labelExtIdx = bundleLabel.LastIndexOf('_');
        string labelExt = null;
        if (labelExtIdx >= 0)
        {
            labelExt = bundleLabel.Substring(labelExtIdx + 1);
        }
        else
        {
            labelExt = bundleLabel;
        }

        int lastIdx;
        if (labelExtIdx >= 0)
        {
            lastIdx = bundleName.IndexOf(labelExt);
            int labelLen = labelExt.Length;
            lastIdx = bundleName.IndexOf('_', lastIdx + labelLen);
        }
        else
        {
            lastIdx = bundleName.LastIndexOf('_');
        }

        if (lastIdx < 0)
        {
            return "";
        }
        else
        {
            return bundleName.Substring(lastIdx);
        }
    }



    public static void RoundOneMagic(ref byte[] arr)
    {
        string decStr = "sLbIdwlddtywstSf";
        byte[] rand = Encoding.ASCII.GetBytes(decStr);

        for (int i = 0; i < arr.Length && i < rand.Length; i++)
        {
            arr[i] = (byte)((int)arr[i] ^ (int)rand[i]);
        }
    }

    // URL中使用的加密秘钥
    public static byte[] DaYuCoolGame()
    {
        string key = "ILl6dhgf1h";

        return UTF8Encoding.UTF8.GetBytes(key);
    }

    public static int tempUserId, tempServerId;
    public static bool reloginIn = false;
    //public static void CloseAndReLogin(int userId, int serverId)
    //{
    //    tempUserId = userId;
    //    tempServerId = serverId;
    //    reloginIn = true;
    //    NetworkManager.Instance().CloseConnection(0);

    //    //NetworkManager.Instance().OnConnect += LoginService.Instance().ReQuireLogin;
    //    reloginIn = false;

    //    NetworkManager.Instance().Init(GlobalConfig.GAME_SERVER_HOST, GlobalConfig.GAME_SERVER_PORT);
    //    NetworkManager.Instance().Connect();
    //}


    //退出游戏
    //public static void QuitGame(string reason, float delay = 0)
    //{
    //    DataReport.ReportExit();
    //    if (delay > 0)
    //    {
    //        DelayInvoker.Instance.DelayInvoke(() =>
    //        {
    //            DeviceHelper.QuitGame();
    //        }, delay);
    //    }
    //    else
    //    {
    //        DeviceHelper.QuitGame();
    //    }
    //    FusingService.GameCoverPointEvent("logout"); //SDK 退出游戏
    //}

    //public static void RestartGame(string reason, float delay = 0)
    //{
    //    DataReport.ReportExit();
    //    if (delay > 0)
    //    {
    //        DelayInvoker.Instance.DelayInvoke(() =>
    //        {
    //            DeviceHelper.RestartApp(reason);
    //        }, delay);
    //    }
    //    else
    //    {
    //        DeviceHelper.RestartApp(reason);
    //    }
    //}

    public static void ReportErrorToServer(string msg)
    {

    }

    public static string CalculateFileMD5(string filename)
    {
        if (string.IsNullOrEmpty(filename))
            return "";

        FileStream file = new FileStream(filename, FileMode.Open);

        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] retVal = md5.ComputeHash(file);
        file.Close();

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < retVal.Length; i++)
        {
            sb.Append(retVal[i].ToString("x2"));
        }
        return sb.ToString();
    }

    // 用当前工程的根目录作为保存已经打包的信息的存储Key
    public static string GetDllHashStoreKey()
    {
        string currDir = Directory.GetCurrentDirectory();

        Debugger.LogError("Store Key: " + currDir);

        return currDir;
    }

    private static List<string> patten = new List<string>() { @"\p{Cs}", @"\p{Co}", @"\p{Cn}", @"[\u2702-\u27B0]" };

    public static string FilterEmoji(string str)
    {
        for (int i = 0; i < patten.Count; i++)
        {
            str = Regex.Replace(str, patten[i], "");//屏蔽emoji   
        }
        return str;
    }

    // 判断输入的是否是颜文字，注意输入如要是当前的输入文字，不是整个字符
    public static bool IsEmojiText(string text)
    {
        for (int i = 0; i < patten.Count; i++)
        {
            if (Regex.IsMatch(text, patten[i]))
            {
                return true;
            }
        }

        return false;
    }

    // 修正编辑器上的Shader
    public static void FixShaderInEditor(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        if (Boot.GamePlayMode == YooAsset.EPlayMode.EditorSimulateMode)
        {
            return;
        }

        // 在网络模式下，因为加载的是其他平台的资源，shader会出错，需要针对编辑器修正
        Debugger.Log("Fix Editor Shader");

        SkinnedMeshRenderer[] skinRenders = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (skinRenders != null && skinRenders.Length > 0)
        {
            for (int i = 0; i < skinRenders.Length; i++)
            {
                SkinnedMeshRenderer render = skinRenders[i];

                if (render.materials == null)
                {
                    if (render.material != null)
                    {
                        render.material.shader = Shader.Find(render.material.shader.name);
                    }
                }
                else
                {
                    int materialCount = render.materials.Length;
                    for (int mi = 0; mi < materialCount; mi++)
                    {
                        Material mat = render.materials[mi];
                        if (mat == null)
                        {
                            continue;
                        }

                        mat.shader = Shader.Find(mat.shader.name);
                    }
                }
            }
        }

        MeshRenderer[] meshRenders = obj.GetComponentsInChildren<MeshRenderer>();
        if (meshRenders != null && meshRenders.Length > 0)
        {
            for (int i = 0; i < meshRenders.Length; i++)
            {
                MeshRenderer render = meshRenders[i];

                if (render.materials == null)
                {
                    if (render.material != null)
                    {
                        render.material.shader = Shader.Find(render.material.shader.name);
                    }
                }
                else
                {
                    int materialCount = render.materials.Length;
                    for (int mi = 0; mi < materialCount; mi++)
                    {
                        Material mat = render.materials[mi];
                        if (mat == null)
                        {
                            continue;
                        }

                        mat.shader = Shader.Find(mat.shader.name);
                    }
                }
            }
        }


        //Renderer[] renders = obj.GetComponentsInChildren<Renderer>();
        //if (renders != null && renders.Length > 0)
        //{
        //    for (int i = 0; i < renders.Length; i++)
        //    {
        //        Renderer render = renders[i];
        //        if (render.materials == null)
        //        {
        //            if(render.material != null)
        //            {
        //                render.material.shader = Shader.Find(render.material.shader.name);
        //            }
        //        }
        //        else
        //        {
        //            int materialCount = render.materials.Length;
        //            for (int mi = 0; mi < materialCount; mi++)
        //            {
        //                Material mat = render.materials[mi];
        //                if (mat == null)
        //                {
        //                    continue;
        //                }

        //                mat.shader = Shader.Find(mat.shader.name);
        //            }
        //        }
        //    }
        //}

        ParticleSystemRenderer[] psRenders = obj.GetComponentsInChildren<ParticleSystemRenderer>();
        if (psRenders != null && psRenders.Length > 0)
        {
            for (int i = 0; i < psRenders.Length; i++)
            {
                ParticleSystemRenderer render = psRenders[i];

                if (render.materials == null)
                {
                    if (render.material != null)
                    {
                        render.material.shader = Shader.Find(render.material.shader.name);
                    }
                }
                else
                {
                    int materialCount = render.materials.Length;
                    for (int mi = 0; mi < materialCount; mi++)
                    {
                        Material mat = render.materials[mi];
                        if (mat == null)
                        {
                            continue;
                        }

                        mat.shader = Shader.Find(mat.shader.name);
                    }
                }
            }
        }
    }

    public static void FixMaterialInEditor(Material mat)
    {
        if (mat == null)
        {
            return;
        }

        if (mat.shader != null)
        {
            mat.shader = Shader.Find(mat.shader.name);
        }
    }



}


