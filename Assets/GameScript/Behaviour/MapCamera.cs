using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public Camera m_camera;
    /*cam : map tiles   20  * 13
cam rotation ( 86,0,0)
cam pos : x =tile , y =10 , z= tile_z -1.5
*/
    // Start is called before the first frame update
    void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var curPos = this.transform.position;
        this.transform.position = Vector3.Slerp(curPos, targetCamPos, -10f);
        //float lerpX = Mathf.Lerp(this.transform.rotation.x, camTargetRotateY, 0.1f);
        //this.transform.rotation = Quaternion.Euler(camTargetRotateX, 0, 0);
    }
    Vector3 targetTilePos;
    Vector3 targetCamPos;
    float camTargetRotateX = 86f;
    public void SetTargetTilePos(Vector3 pos)
    {
        targetTilePos = pos;
        targetCamPos = pos;// new Vector3(pos.x, 10f, pos.z - 1.5f);
        TrimCamPos();
    }
    Rect mapBorder;
    Rect camBorder;

    public void SetMapBorderPos(Rect border)
    {
        mapBorder = border;
        Debug.Log($"map borders ={border}");
        camBorder.xMin = 6;//static value
        camBorder.yMin = 1;//static value
        camBorder.xMax = border.height - 6;
        camBorder.yMax = border.width - 5;
    }
    public void TrimCamPos()
    {
        if (targetCamPos.x < camBorder.xMin)
            targetCamPos.x = camBorder.xMin;
        if (targetCamPos.z < camBorder.yMin)
            targetCamPos.z = camBorder.yMin;
        if (targetCamPos.x > camBorder.xMax)
            targetCamPos.x = camBorder.xMax;
        if (targetCamPos.z > camBorder.yMax)
            targetCamPos.z = camBorder.yMax;
    }
}
