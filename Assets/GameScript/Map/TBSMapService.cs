using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UniFramework.Pooling;
using UniFramework.Singleton;

namespace SunHeTBS
{

    public class TBSMapService : ISingleton
    {
        public static TBSMapService Inst { get; private set; }
        public static void Init()
        {
            Inst = UniSingleton.CreateSingleton<TBSMapService>();
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
        public void OnFixedUpdate()
        {
        }

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
                tdj.h = float.Parse(subJd["h"].ToString());
                tdjList.Add(tdj);

                xMax = Mathf.Max(tdj.x, xMax);
                xMin = Mathf.Min(tdj.x, xMin);
                yMin = Mathf.Min(tdj.y, yMin);
                yMax = Mathf.Max(tdj.y, yMax);
            }
            int row = xMax - xMin + 1;
            int col = yMax - yMin + 1;
            map.InitData(row, col, tdjList);

            //map.TileSize
            var rect = new Rect();
            rect.xMin = 0; rect.yMin = 0;
            rect.xMax = col * map.TileSize;
            rect.yMax = row * map.TileSize;
            mapCamera.SetMapBorderPos(rect);
        }
        public void ShowPawnCoverPlanes(Pawn p)
        {
            UnspawnAllCoverPlanes();
            if (p.moveTileIds == null)
            {
                p.CalculateMoveArea();
                p.CalculateRangeArea();
            }
            foreach (int tileId in p.moveTileIds)
            {
                var tile = map.GetTileFromDic(tileId);
                var pos = map.WorldPosition(tile);
                SpawnCoverPlaneBlue(pos, tile.topHeight);
            }

            int atkRangeMax = p.GetAtkRangeMax();
            int atkRangeMin = p.GetAtkRangeMin();
            var atkTileIdList = p.GetTileIdsInRange(atkRangeMin, atkRangeMax);

            //show red planes in attackable tiles, walkable tiles excluded
            foreach (int tileId in atkTileIdList)
            {
                if (!p.moveTileIds.Contains(tileId))
                {
                    var tile = map.GetTileFromDic(tileId);
                    var pos = map.WorldPosition(tile);
                    SpawnCoverPlaneRed(pos, tile.topHeight);

                }
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
        public void SpawnCoverPlaneBlue(Vector3 pos, float height)
        {
            SpawnCoverPlane(pos, str_PlaneBlue, height);
        }
        public void SpawnCoverPlanePurple(Vector3 pos, float height)
        {
            SpawnCoverPlane(pos, str_PlanePurple, height);
        }
        public void SpawnCoverPlaneRed(Vector3 pos, float height)
        {
            SpawnCoverPlane(pos, str_PlaneRed, height);
        }
        void SpawnCoverPlane(Vector3 pos, string prefabName, float height)
        {
            if (CoverPlaneTrans == null)
            {
                CoverPlaneTrans = new GameObject("CoverPlaneObj").transform;
                CoverPlaneTrans.position = Vector3.zero;
            }
            var spawner = BattleDriver.UniSpawner;
            var spHandle = spawner.SpawnAsync(prefabName, CoverPlaneTrans,
               new Vector3(0, height, 0) + pos, Quaternion.identity);
            spHandleList.Add(spHandle);
        }
        #endregion

        public int GetTileId(Vector3Int pos)
        {
            if (map != null)
                return map.XY2TileId(pos);
            return 0;
        }

        #region map camera manage
        public MapCamera mapCamera;
        public void InitMapCamera()
        {
            var camObj = GameObject.Find("Main Camera");
            if (camObj != null)
            {
                mapCamera = camObj.GetComponent<MapCamera>();
            }
        }
        #endregion
    }

}
