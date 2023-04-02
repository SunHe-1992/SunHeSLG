using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;
namespace SunHeTBS
{
    public static class MapDataTool
    {
        public static void ExportData()
        {
            var tilesObj = GameObject.Find("MapView/Tiles");
            if (tilesObj)
            {
                LitJson.JsonData jd = new JsonData();
                List<TileData4Json> tdList = new List<TileData4Json>();
                int childCount = tilesObj.transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    var tileTrans = tilesObj.transform.GetChild(i);
                    TilePresetData tpd = tileTrans.GetComponent<TilePresetData>();
                    var pos = tileTrans.transform.position;
                    TileData4Json data = new TileData4Json();
                    data.x = (int)pos.x;
                    data.y = (int)pos.z;
                    data.pType = (int)tpd.passType;
                    data.cost = tpd.moveCost;
                    data.effect = (int)tpd.effectType;
                    data.name = tpd.tileName;
                    tdList.Add(data);

                }


                string jsonStr = LitJson.JsonMapper.ToJson(tdList);
                string sceneName = SceneManager.GetActiveScene().name;
                string savePath = $"{Application.dataPath}/GameRes/Config/{sceneName}.json";
                //Debugger.Print(savePath);
                //Debugger.Print(jsonStr);
                if (!File.Exists(savePath))
                {
                    File.Create(savePath);
                }
                System.IO.File.WriteAllText(savePath, jsonStr);
            }
        }
    }

}
