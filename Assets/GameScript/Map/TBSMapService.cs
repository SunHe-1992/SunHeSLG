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
                tdjList.Add(tdj);

                xMax = Mathf.Max(tdj.x, xMax);
                xMin = Mathf.Min(tdj.x, xMin);
                yMin = Mathf.Min(tdj.y, yMin);
                yMax = Mathf.Max(tdj.y, yMax);
            }
            map.InitData(xMax - xMin + 1, yMax - yMin + 1, tdjList);

        }
        public void ShowPawnCoverPlanes(Pawn p)
        {
            UnspawnAllCoverPlanes();
            HashSet<TileEntity> walkableTiles = map.WalkableTiles(p.curPosition, p.move_points, p.IsExtraMoveCost(), p.IsPassFoe());
            foreach (var tile in walkableTiles)
            {
                moveTileIds.Add(tile.tileId);
                var pos = map.WorldPosition(tile);
                SpawnCoverPlaneBlue(pos);
            }
            //int atkRange = p.GetAtkRange();
            //if (atkRange > 0)
            //{

            //}
        }
        #region cover planes on map
        public static readonly string str_PlaneBlue = "Gizmos/CoverPlaneBlue";
        public static readonly string str_PlanePurple = "Gizmos/CoverPlanePurple";
        public static readonly string str_PlaneRed = "Gizmos/CoverPlaneRed";

        public HashSet<int> moveTileIds;
        public HashSet<int> atkTileIds;
        public HashSet<int> healTileIds;
        public HashSet<int> staffTileIds;
        Transform CoverPlaneTrans = null;
        List<SpawnHandle> spHandleList = new List<SpawnHandle>();
        public void UnspawnAllCoverPlanes()
        {
            moveTileIds = new HashSet<int>();
            atkTileIds = new HashSet<int>();
            healTileIds = new HashSet<int>();
            staffTileIds = new HashSet<int>();
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
            var spawner = BattleDriver.UniSpawner;
            var spHandle = spawner.SpawnAsync(prefabName, CoverPlaneTrans, pos, Quaternion.identity);
            spHandleList.Add(spHandle);
        }
        #endregion

        public int GetTileId(Vector3Int pos)
        {
            if (map != null)
                return map.XY2TileId(pos);
            return 0;
        }
    }

}
