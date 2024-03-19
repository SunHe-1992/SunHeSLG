using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBuildingController : MonoBehaviour
{
    public GameObject objLv1;
    public GameObject objLv2;
    public GameObject objLv3;
    public GameObject objLv4;
    public GameObject objLv5;
    public GameObject objLv6;

    public int level = 4;
    List<GameObject> objList;
    public void SetGameObjectByLevel(int _level)
    {
        level = _level;
        objList = new List<GameObject>() { objLv1, objLv2, objLv3, objLv4, objLv5, objLv6 };

        for (int i = 0; i < objList.Count; i++)
        {
            bool isshow = false;
            var obj = objList[i];
            if (i < level)
                isshow = true;
            if (obj != null)
            {
                obj.SetActive(isshow);
            }
        }
    }
}
