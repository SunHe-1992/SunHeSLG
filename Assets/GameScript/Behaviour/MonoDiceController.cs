using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoDiceController : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ResetTransform();
    }

    public void PlayDiceAnim(int value)
    {
        if (value >= 1 && value <= 6)
        {
            ResetTransform();
            string animName = "DiceRoll" + value;
            animator.Play(animName);
        }
    }
    void ResetTransform()
    {
        this.transform.localRotation = Quaternion.identity;
    }
}
