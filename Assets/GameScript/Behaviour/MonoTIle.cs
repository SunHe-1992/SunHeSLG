using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MonoTile : MonoBehaviour
{
    public float cubeHeight = 1;
    public float cubeWidth = 1;

    Transform cube;
    public int Index = 0;
    private void Start()
    {
        cube = this.transform.Find("Cube");

    }
    public void SetCubeSize()
    {
        if (cube == null)
            cube = this.transform.Find("Cube");
        cube.localScale = new Vector3(cubeWidth, cube.localScale.y, cubeHeight);
    }
    /// <summary>
    /// set cube size
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void SetRect(float width, float height)
    {
        this.cubeWidth = width;
        this.cubeHeight = height;
        SetCubeSize();
    }
    public void SetRotation(float rot)
    {
        this.transform.localRotation = Quaternion.Euler(0, rot, 0);
    }
    public void SetCanvasText()
    {
        TextMeshProUGUI text1 = this.transform.Find("Canvas/Text1").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI text2 = this.transform.Find("Canvas/Text2").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI text3 = this.transform.Find("Canvas/Text3").GetComponent<TextMeshProUGUI>();

        text1.text = "" + Index;
        text2.text = "";
        text3.text = "";
    }
}
