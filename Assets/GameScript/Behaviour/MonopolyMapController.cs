using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonopolyMapController : MonoBehaviour
{
    public MonoPawnController pawnCtrl;
    public MonoTileArrange tileArrange;
    public static MonopolyMapController inst;

    List<MonoTileController> tilesList;
    private void Awake()
    {
        inst = this;
        pawnCtrl = GameObject.Find("MonoPawn").GetComponent<MonoPawnController>();
        tileArrange = GameObject.Find("MonoTileGroup").GetComponent<MonoTileArrange>();

    }
    private void OnDestroy()
    {
        inst = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        tileArrange.GenerateTiles();
        tileArrange.ArrangeTiles();

        tilesList = tileArrange.GetAllTiles();

    }

    // Update is called once per frame
    void Update()
    {

    }
    int countJump = 0;
    public void TestJump()
    {
        if (countJump + 1 >= tilesList.Count)
        {
            countJump = 0;
        }
        if (countJump + 1 < tilesList.Count)
        {
            Vector3 from = tilesList[countJump].transform.position;
            Vector3 to = tilesList[countJump + 1].transform.position;
            pawnCtrl.PerformJump(from, to, 0.25f);
            countJump++;
        }
    }
}
