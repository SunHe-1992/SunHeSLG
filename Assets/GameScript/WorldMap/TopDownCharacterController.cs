using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunHeTBS
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        private Animator animator;
        bool faceRight = true;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }


        private void Update()
        {

            Vector2 dir = Vector2.zero;
            float moveH = Input.GetAxis("Horizontal");
            float moveV = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.A))
            {
                dir.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
            }

            if (Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
            }

            if (Input.GetKey(KeyCode.J))
            {
                animator.SetTrigger("attack");
            }
            dir.Normalize();
            float speed = 0;
            if (dir.magnitude > 0)
                speed = 1;
            animator.SetFloat("speed", speed);
            //animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().velocity = speed * dir;

            faceRight = dir.x > 0;

            if (dir.x != 0)
                SRFlip();
        }
        void SRFlip()
        {
            var sr = this.GetComponent<SpriteRenderer>();
            sr.flipX = faceRight == false;
        }
    }
}
