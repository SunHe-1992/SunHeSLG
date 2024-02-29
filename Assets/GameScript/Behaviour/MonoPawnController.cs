using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoPawnController : MonoBehaviour
{

    Animator animator;
    Transform ModelFather;
    public void PerformJump(Vector3 from, Vector3 to, float time)
    {
        moveDest = to;
        moveSpeed = 1 / time;
        moveTime = time;
        moveVect = to - from;
        //Debug.Break();
        this.transform.position = from;
        ModelFather = transform.Find("ModelFather");
        animator = ModelFather.GetComponent<Animator>();
        animator.Play("PawnJump", -1, 0);
        isJumping = true;
    }

    private void Update()
    {
        if (isJumping)
        {
            UpdateJumping();

        }
    }

    #region model move
    bool isJumping = false;
    Vector3 moveVect;
    float moveTime;
    float moveSpeed = 1;
    Vector3 moveDest;
    void UpdateJumping()
    {
        moveTime -= Time.deltaTime;
        if (moveTime < 0)
        {
            moveTime = 0;
            isJumping = false;
            //this.transform.position = moveDest;
        }
        else
        {
            // Determine which direction to rotate towards
            Vector3 targetDirection = moveDest - transform.position;
            // The step size is equal to speed times frame time.
            //float singleStep = moveSpeed * Time.deltaTime;
            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, moveSpeed, 0.0f);
            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
            this.transform.position += moveVect * Time.deltaTime * moveSpeed;
        }
    }
    #endregion
}
