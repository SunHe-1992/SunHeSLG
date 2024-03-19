using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBuildingMapController : MonoBehaviour
{
    public static MonoBuildingMapController Inst;
    Dictionary<int, MonoBuildingController> buildingDic;
    private void Awake()
    {
        Inst = this;
        Init();
    }
    private void OnDestroy()
    {
        Inst = null;
    }
    void Init()
    {
        buildingDic = new Dictionary<int, MonoBuildingController>();

        //building obj 1,2,3,4,5
        for (int i = 1; i <= 5; i++)
        {
            string findName = "House" + i.ToString();
            var bd = GameObject.Find(findName).GetComponent<MonoBuildingController>();
            buildingDic.Add(i, bd);
        }

    }
    public void RefreshBuildingsVisible()
    {
        foreach (var pair in MonoPlayer.buildingDic)
        {
            BuildingDetail info = pair.Value;
            int bdId = info.buildingId;
            if (buildingDic.ContainsKey(bdId))
            {
                buildingDic[bdId].SetGameObjectByLevel(info.buildingLevel);
            }
        }
    }
}
