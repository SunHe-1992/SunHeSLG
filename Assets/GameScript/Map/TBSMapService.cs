using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UniFramework.Pooling;

namespace SunHeTBS
{

    public class TBSMapService : LogicSingleton<TBSMapService>
    {
        public MapEntity map;

        public void ClearData()
        {
            map = new MapEntity();
            UnspawnAllCoverPlanes();
        }
        public void LoadJsonData(string jsonStr)
        {
            List<TileData4Json> tdjList = new List<TileData4Json>();
            JsonData jd = LitJson.JsonMapper.ToObject(jsonStr);
            int xMin = 0;
            int xMax = 0;
            int yMin = 0;
            int yMax = 0;
            foreach (object obj in jd)
            {
                TileData4Json tdj = new TileData4Json();
                var subJd = obj as JsonData;
                tdj.pType = int.Parse(subJd["pType"].ToString());
                tdj.cost = int.Parse(subJd["cost"].ToString());
                tdj.effect = int.Parse(subJd["effect"].ToString());
                tdj.x = int.Parse(subJd["x"].ToString());
                tdj.y = int.Parse(subJd["y"].ToString());
                tdj.name = subJd["name"].ToString();
                tdjList.Add(tdj);

                xMax = Mathf.Max(tdj.x, xMax);
                xMin = Mathf.Min(tdj.x, xMin);
                yMin = Mathf.Min(tdj.y, yMin);
                yMax = Mathf.Max(tdj.y, yMax);
            }
            map.InitData(xMax - xMin + 1, yMax - yMin + 1, tdjList);

        }
        public void TestPath()
        {
            Debugger.Print("start test path");
            UnspawnAllCoverPlanes();
            var tiles = map.WalkableTiles(new Vector3Int(2, 2, 0), 3);
            foreach (var tile in tiles)
            {
                Debugger.Print(tile.ToString());
                var pos = map.WorldPosition(tile);
                SpawnCoverPlaneBlue(pos);
            }
        }
        #region cover planes on map
        public static readonly string str_PlaneBlue = "Gizmos/CoverPlaneBlue";
        public static readonly string str_PlanePurple = "Gizmos/CoverPlanePurple";
        public static readonly string str_PlaneRed = "Gizmos/CoverPlaneRed";
        Transform CoverPlaneTrans = null;
        List<SpawnHandle> spHandleList = new List<SpawnHandle>();
        public void UnspawnAllCoverPlanes()
        {
            if (spHandleList != null)
            {
                for (int i = spHandleList.Count - 1; i >= 0; i--)
                {
                    spHandleList[i].Restore();
                    spHandleList.RemoveAt(i);
                }
            }
        }
        //public void DiscardAllCoverPlanes()
        //{

        //}
        public void SpawnCoverPlaneBlue(Vector3 pos)
        {
            SpawnCoverPlane(pos, str_PlaneBlue);
        }
        public void SpawnCoverPlanePurple(Vector3 pos)
        {
            SpawnCoverPlane(pos, str_PlanePurple);
        }
        public void SpawnCoverPlaneRed(Vector3 pos)
        {
            SpawnCoverPlane(pos, str_PlaneRed);
        }
        void SpawnCoverPlane(Vector3 pos, string prefabName)
        {
            if (CoverPlaneTrans == null)
            {
                CoverPlaneTrans = new GameObject("CoverPlaneObj").transform;
                CoverPlaneTrans.position = Vector3.zero;

            }
            var spawner = BattleDriver.Instance.spawner;
            spawner.SpawnAsync(prefabName, CoverPlaneTrans, pos, Quaternion.identity);
        }
        #endregion
    }

}
