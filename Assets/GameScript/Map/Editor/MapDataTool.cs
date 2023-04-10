using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;
using UnityEditor;
namespace SunHeTBS
{
    public static class MapDataTool
    {
        public static int MapRows;
        public static int MapCols;
        static int XY2TileId(int x, int y)
        {
            return MapCols * x + y;
        }
        static Vector3Int TileId2XY(int tileId)
        {
            return new Vector3Int(tileId / MapCols, tileId % MapCols);
        }
        public static void ExportData()
        {
            /*check tile datas,must be rect map ,xz starts with 0,0*/
            var tilesObj = GameObject.Find("MapView/Tiles");
            if (tilesObj)
            {
                LitJson.JsonData jd = new JsonData();
                List<TileData4Json> tdList = new List<TileData4Json>();
                TilePresetData[] tpdDataList = tilesObj.GetComponentsInChildren<TilePresetData>();
                int sizeX = 0;
                int sizeY = 0;
                bool success = true;
                HashSet<Vector2Int> vectHash = new HashSet<Vector2Int>();
                for (int i = 0; i < tpdDataList.Length; i++)
                {
                    var tileTrans = tilesObj.transform.GetChild(i);
                    TilePresetData tpd = tileTrans.GetComponent<TilePresetData>();
                    var pos = tileTrans.transform.position;
                    TileData4Json data = new TileData4Json();
                    //position
                    data.x = (int)pos.x;
                    data.y = (int)pos.z;


                    if (data.x >= 0 && data.y >= 0)
                    {
                        var peakTrans = tileTrans.Find("peakObj");
                        float height = 0;
                        if (peakTrans != null)
                        {
                            height = peakTrans.localPosition.y;
                        }
                        var vect = new Vector2Int(data.x, data.y);
                        if (vectHash.Contains(vect))
                        {
                            success = false;
                            ShowEditorWarning($"repeated tile pos {vect}");
                            break;
                        }
                        else
                        {
                            vectHash.Add(vect);
                        }
                        sizeX = sizeX > data.x ? sizeX : data.x;
                        sizeY = sizeY > data.y ? sizeY : data.y;
                        data.pType = (int)tpd.passType;//pass type
                        data.cost = tpd.extraMovePrice;//move price
                        data.effect = (int)tpd.effectType;//effect
                        data.name = tpd.tileName;
                        data.h = height;
                        tdList.Add(data);
                    }
                    else
                    {
                        ShowEditorWarning("tile pos need to be positive!");
                        success = false;
                        break;
                    }
                }
                int tilesCount = (1 + sizeY) * (1 + sizeX);
                if (tdList.Count != tilesCount)
                {
                    success = false;
                    ShowEditorWarning($"tile amount is incorrect ,expected {tilesCount} {sizeX + 1}*{sizeY + 1} ,tdList count ={tdList.Count}");
                }
                if (success)
                {

                    string jsonStr = LitJson.JsonMapper.ToJson(tdList);
                    string sceneName = SceneManager.GetActiveScene().name;
                    string savePath = $"{Application.dataPath}/GameRes/Config/{sceneName}.json";
                    if (!File.Exists(savePath))
                    {
                        File.Create(savePath);
                    }
                    System.IO.File.WriteAllText(savePath, jsonStr);
                    Debug.Log("exported success");
                }
            }
            void ShowEditorWarning(string message)
            {
                Debug.LogError(message);
            }
        }
    }

}
