using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour
{
    Transform trans_arrow;
    Transform trans_plane;
    private void Awake()
    {
        trans_arrow = this.transform.Find("arrow");
        trans_plane = this.transform.Find("plane");
    }
    void ChangePlaneColor(Color color)
    {
        //var mr = trans_plane.GetComponent<MeshRenderer>();
        //mr.material.color = color;
        var sr = this.GetComponent<SpriteRenderer>();
        sr.color = color;
    }
    public void ChangeRed()
    {
        ChangePlaneColor(Color.red);
    }
    public void ChangeWhite()
    {
        ChangePlaneColor(Color.white);
    }

    public void ShowHideArrow(bool isshow)
    {
        if (trans_arrow != null)
            trans_arrow.gameObject.SetActive(isshow);
    }
}
