using System.Collections;
using System.Collections.Generic;
using cfg;
using SunHeTBS;
using TMPro;
using UniFramework.Event;
using UnityEngine;

public class NPCMark : MonoBehaviour
{
    public PawnData PawnCfg;
    public TextMeshProUGUI txt_name;
    public int NPCId = 1;

    PlayerCharacter hero;

    public float triggerDistance = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        txt_name = transform.Find("Canvas/txt_name").GetComponent<TextMeshProUGUI>();
        LoadPawnCfg();
        txt_name.text = PawnCfg.Name;
    }

    void LoadPawnCfg()
    {
        PawnCfg = ConfigManager.table.TbPawn.Get(this.NPCId);
        this.triggerDistance = PawnCfg.TriggerDistance;
    }
    // Update is called once per frame
    void Update()
    {
        if (hero == null)
        {
            hero = PlayerCharacter.Inst;
        }

        if (hero != null)
        {
            if (triggerDistance > 0)
            {
                if (triggerDistance > Vector3.Distance(this.transform.position, hero.transform.position))
                {
                    if (BLogic.recentNPCMark != this)
                        TriggerEvent();
                }
                else
                {
                    if (this == BLogic.recentNPCMark)
                    {
                        BLogic.recentNPCMark = null;
                    }
                }
            }
        }
    }
    void TriggerEvent()
    {
        BLogic.recentNPCMark = this;
        Debugger.Log(" TriggerEvent NPC mark");
    }

    public void RefreshTxtName(string name)
    {
        this.txt_name.text = name;
    }
}
