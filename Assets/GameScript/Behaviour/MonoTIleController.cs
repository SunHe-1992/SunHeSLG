using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using cfg;
public class MonoTileController : MonoBehaviour
{
    public float cubeHeight = 1;
    public float cubeWidth = 1;
    public bool isCornerTile = false;
    Transform cube;
    public int Index = 0;
    TextMeshProUGUI text1;
    TextMeshProUGUI text2;
    TextMeshProUGUI text3;

    cfg.MapConfig cfgData;
    public long saveData;
    private void Awake()
    {
        FindChildren();
    }
    void FindChildren()
    {
        if (cube == null)
        {
            text1 = this.transform.Find("Cube/Canvas/Text1").GetComponent<TextMeshProUGUI>();
            text2 = this.transform.Find("Cube/Canvas/Text2").GetComponent<TextMeshProUGUI>();
            text3 = this.transform.Find("Cube/Canvas/Text3").GetComponent<TextMeshProUGUI>();
            cube = this.transform.Find("Cube");
        }
    }
    private void Start()
    {

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
        FindChildren();
        text1.text = "" + cfgData.TileTitle;
        text2.text = "";
        text3.text = "";
    }
    public void PlayShakeAnim()
    {
        Animation anim = this.transform.GetComponent<Animation>();
        string animName = isCornerTile ? "TileShakeAnim_Corner" : "TileShakeAnim";
        AnimationClip clip = anim.GetClip(animName);
        if (clip != null)
            anim.Play(animName);
    }
    public void SetConfigData()
    {
        cfgData = ConfigManager.table.TbMapConfig.Get(Index);
        SetCanvasText();
    }
    public void PlayTileEventEffect()
    {
        if (MLogic.Inst.triggerTileIdList.Contains(this.Index))
        {
            UIService.Inst.ShowMoneyAnim(saveData);
        }
    }
}
