using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoTileArrange : MonoBehaviour
{
    /// <summary>
    /// total tiles
    /// </summary>
    const int count = 40;
    public float smallWidth = 0.65f;
    public float halfSmallWidth;
    public float fullWidth = 1;
    public float tileHeight = 1;
    public float tileHalfHeight;
    float halfFullWidth;
    float boardRadius;
    public GameObject obj;
    List<MonoTIleController> tileObjList;
    List<MonoTIleController> tileObjList_corner;

    float tile_interval = 0.03f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CopyTiles()
    {
        tileObjList = new List<MonoTIleController>();
        for (int i = 0; i < 36; i++)
        {
            var newObj = GameObject.Instantiate(obj);
            var mTile = newObj.AddComponent<MonoTIleController>();
            tileObjList.Add(mTile);
            newObj.transform.parent = this.transform;
            newObj.name = "tile" + i;
        }
        obj.gameObject.SetActive(false);

    }
    public void ArrangeTiles()
    {
        halfFullWidth = fullWidth * 0.5f;
        halfSmallWidth = smallWidth * 0.5f;
        tileHalfHeight = tileHeight * 0.5f;
        boardRadius = (smallWidth * 9 + tile_interval * 8) / 2f - smallWidth / 2f;
        float zPos = boardRadius + tileHalfHeight + halfSmallWidth + tile_interval;


        List<float> xposList = new List<float>();
        for (int i = 0; i < 9; i++)
        {
            float xPos = i * (smallWidth + tile_interval);
            xposList.Add(xPos);
        }
        List<Vector3> zposList = new List<Vector3>() {
            new Vector3( 0,0,-zPos),
            new Vector3( zPos,0,0),
            new Vector3( 0,0,zPos),
            new Vector3( -zPos,0,0),
            };
        //4 directions arrangement
        int idx = 0;
        float rot = 0;
        for (int i = 0; i < 4; i++)
        {
            //xz,zx,-x-z,-z-x
            switch (i)
            {
                case 0: rot = 0; break;
                case 1: rot = -90; break;
                case 2: rot = 180; break;
                case 3: rot = 90; break;
            }
            Vector3 posOffset = zposList[i];
            for (int j = 0; j < 9; j++)
            {
                MonoTIleController tileObj = tileObjList[idx];
                float xpos = xposList[j];
                Vector3 pos = Vector3.zero;
                switch (i)
                {
                    case 0: pos.x = xpos - boardRadius; break;
                    case 1: pos.z = xpos - boardRadius; break;
                    case 2: pos.x = -(xpos - boardRadius); break;
                    case 3: pos.z = -(xpos - boardRadius); break;
                }

                tileObj.SetRect(smallWidth, tileHeight);
                tileObj.SetRotation(rot);
                tileObj.transform.localPosition = pos + posOffset;
                idx++;
            }
        }
        //corner idx 0 9 17 26
        float cornerOffset = halfFullWidth + halfSmallWidth + tile_interval;
        List<Vector3> cornerPosList = new List<Vector3>();
        cornerPosList.Add(tileObjList[0].transform.localPosition + new Vector3(-cornerOffset, 0, 0));
        cornerPosList.Add(tileObjList[9].transform.localPosition + new Vector3(0, 0, -cornerOffset));
        cornerPosList.Add(tileObjList[17].transform.localPosition + new Vector3(0, 0, cornerOffset));
        cornerPosList.Add(tileObjList[26].transform.localPosition + new Vector3(-cornerOffset, 0, 0));

        tileObjList_corner = new List<MonoTIleController>();
        for (int i = 0; i < 4; i++)
        {
            var newObj = GameObject.Instantiate(obj);
            newObj.SetActive(true);
            var mTile = newObj.AddComponent<MonoTIleController>();
            tileObjList_corner.Add(mTile);
            newObj.transform.parent = this.transform;
            newObj.name = "cornerTile" + i;
            newObj.transform.localPosition = cornerPosList[i];
            mTile.SetRect(fullWidth, fullWidth);

        }
        tileObjList_corner[0].Index = 0;
        tileObjList_corner[1].Index = 10;
        tileObjList_corner[2].Index = 20;
        tileObjList_corner[3].Index = 30;

        //tileObjList total count 36
        for (int i = 0; i < 9; i++)//index: 1-10
        {
            tileObjList[i].Index = i + 1;
        }
        for (int i = 0; i < 9; i++)//index: 11-20
        {
            tileObjList[i + 9].Index = i + 11;
        }
        for (int i = 0; i < 9; i++)//index: 21-30
        {
            tileObjList[i + 18].Index = i + 21;
        }
        for (int i = 0; i < 9; i++)//index: 31-40
        {
            tileObjList[i + 27].Index = i + 31;
        }

        foreach (var tileObj in tileObjList)
        {
            tileObj.SetCanvasText();
        }
        foreach (var tileObj in tileObjList_corner)
        {
            tileObj.SetCanvasText();
        }
    }
}
