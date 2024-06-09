using System.Collections;
using System.Collections.Generic;
using SunHeTBS;
using UniFramework.Event;
using UnityEngine;

public class LandMark : MonoBehaviour
{
    public enum LandMarkEventType
    {
        Default,
        Fishing,
        Slot,
        Harvest,
    }
    PlayerCharacter hero;
    public LandMarkEventType eventType;

    public float triggerDistance = 0.7f;
    // Start is called before the first frame update
    void Start()
    {

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
            if (triggerDistance > Vector3.Distance(this.transform.position, hero.transform.position))
            {
                if (BLogic.recentLandMark != this)
                    TriggerEvent();
            }
            else
            {
                if (this == BLogic.recentLandMark)
                {
                    BLogic.recentLandMark = null;
                }
            }
        }
    }
    void TriggerEvent()
    {
        BLogic.recentLandMark = this;
        //trigger something
        UniEvent.SendMessage(GameEventDefine.LandMarkTriggered);
        switch (eventType)
        {
            case LandMarkEventType.Fishing:
                UniEvent.SendMessage(GameEventDefine.StartFishing); break;
            case LandMarkEventType.Slot:
                UniEvent.SendMessage(GameEventDefine.StartSlotGame); break;
        }
    }
}
