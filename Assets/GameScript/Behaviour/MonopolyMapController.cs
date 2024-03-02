using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonopolyMapController : MonoBehaviour
{
    public MonopolyCamera monoCam;
    public static MonopolyMapController inst;
    public MonoPawnController pawnCtrl;
    public MonoTileArrange tileArrange;
    public MonoDiceController DiceCtrl;

    //key=Index, value=tile controller
    static int tileIndexMax = MLogic.tileIndexMax;
    static int tileCount = MLogic.tileCount;
    Dictionary<int, MonoTileController> tilesList;

    //convert any int value to index range: 0-39
    public int GetThisId(int id)
    {
        id = (id + 1) % tileCount - 1;
        if (id < 0)
        {
            return tileCount + id;
        }
        else if (id < tileIndexMax)
            return id;
        else
            return (id - tileIndexMax);
    }

    public MonoTileController GetTileById(int id)
    {
        id = GetThisId(id);
        if (tilesList.ContainsKey(id))
            return tilesList[id];
        else return null;
    }
    private void Awake()
    {
        inst = this;

    }
    private void OnDestroy()
    {
        inst = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        pawnCtrl = GameObject.Find("MonoPawn").GetComponent<MonoPawnController>();
        tileArrange = GameObject.Find("MonoTileGroup").GetComponent<MonoTileArrange>();
        DiceCtrl = GameObject.Find("MonoDice").GetComponent<MonoDiceController>();
        monoCam = GameObject.Find("MonopolyCamera").GetComponent<MonopolyCamera>();

        tileArrange.GenerateTiles();
        tileArrange.ArrangeTiles();

        var tempList = tileArrange.GetAllTiles();
        tilesList = new Dictionary<int, MonoTileController>();
        foreach (var tile in tempList)
        {
            tilesList[tile.Index] = tile;
        }
        ResetPawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (pawnCtrl != null && monoCam != null && monoCam.target == null)
        {
            monoCam.target = pawnCtrl.transform;
        }
    }

    readonly float diceAnimDuration = 0.26f;
    readonly float jumpAnimDuration = 0.26f;
    [HideInInspector]
    public bool playingAnim = false;
    int lastIndex = 0;
    int currIndex = 0;
    int step = 0;
    public void PlayDiceAnim(int value)
    {
        lastIndex = MLogic.Inst.lastTileIndex;
        currIndex = MLogic.Inst.currentTileIndex;
        step = CalculateStep(lastIndex, currIndex);
        Debugger.Log($"PlayDiceAnim from {lastIndex}=>{currIndex}");
        playingAnim = true;
        DiceCtrl.PlayDiceAnim(value);
        StartCoroutine(PerformJump());
    }
    IEnumerator PerformJump()
    {
        //wait for dice
        yield return new WaitForSeconds(diceAnimDuration);
        //jump step by step
        for (int i = 0; i < step; i++)
        {
            int fromId = GetThisId(lastIndex + i);
            int toId = GetThisId(lastIndex + i + 1);
            var tile1 = GetTileById(GetThisId(lastIndex + i));
            var tile2 = GetTileById(GetThisId(lastIndex + i + 1));
            Vector3 from = tile1.transform.position;
            Vector3 to = tile2.transform.position;
            //Debugger.LogError($"start jump from {fromId} => {toId}");
            pawnCtrl.PerformJump(from, to, jumpAnimDuration);
            yield return new WaitForSeconds(jumpAnimDuration);
            tile2.PlayShakeAnim();
        }
        yield return null;
        playingAnim = false;
    }
    public void ResetPawnPosition()
    {
        Vector3 to = tilesList[MLogic.Inst.currentTileIndex].transform.position;
        pawnCtrl.transform.position = to;
    }
    int CalculateStep(int fromIdx, int toIdx)
    {
        if (toIdx >= fromIdx)
            return toIdx - fromIdx;
        else
        {
            return (tileCount - fromIdx) + (toIdx);
        }
    }
}
