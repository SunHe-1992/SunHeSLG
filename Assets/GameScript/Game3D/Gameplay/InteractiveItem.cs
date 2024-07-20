using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractiveItem : MonoBehaviour
{
    [SerializeField]
    public ItemType itemType = ItemType.Default;
    [SerializeField]
    public bool hideAfterTrigger = true;
    public float regenerateTime = 5;

    public void MakeInvisible()
    {
        if (hideAfterTrigger)
        {
            this.gameObject.SetActive(false);
            DelayInvoker.Inst.DelayInvoke(DelayShow, regenerateTime);
        }
    }
    void DelayShow()
    {
        if (this != null && this.gameObject != null)
            this.gameObject.SetActive(true);
    }
}
public enum ItemType
{
    Default,
    /// <summary>
    /// add coins
    /// </summary>
    Coin,
    /// <summary>
    /// add HP
    /// </summary>
    HealthPotion,
    /// <summary>
    /// add game time
    /// </summary>
    TimePotion,
    /// <summary>
    /// reduce HP
    /// </summary>
    ToxicPotion,
    /// <summary>
    /// add score
    /// </summary>
    Score,
}