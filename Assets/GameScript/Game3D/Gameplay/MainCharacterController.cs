using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    CharacterController ctrl;
    public static MainCharacterController Inst;
    private void Awake()
    {
        Inst = this;
    }
    private void OnDestroy()
    {
        Inst = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        GamePlayManager.Instance.heroCtrl = this;
        ctrl = this.GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        ResetHP();
        ResetPosition();

        GamePlayManager.Instance.DoLoadSavedGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePlayManager.Instance.running == false)
        {
            return;
        }
        //ProcessRotate();
        //ProcessMove();
        CheckFallen();

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    //DoJump();
        //}
        //this.isGrounded = CheckGrounded();

        // ProcessJump();
    }

    public float addForceSpeed = 400f;
    public float moveSpeed = 1f;
    #region move by axis
    void ProcessMove()
    {
        float axisH = Input.GetAxis("Horizontal");
        float axisV = Input.GetAxis("Vertical");
        animator.SetFloat("moveH", axisH);
        animator.SetFloat("moveV", axisV);
        if (isAxisSmall(axisV) && isAxisSmall(axisH))
        {
            animator.SetBool("noMove", true);
            rb.velocity = Vector3.zero;
        }
        else
        {
            animator.SetBool("noMove", false);


            Vector3 forwardVect = this.transform.forward * axisV;
            Vector3 rightVect = this.transform.right * axisH;
            Vector3 moveVect = (forwardVect + rightVect) * Time.deltaTime * moveSpeed;
            //this.transform.position += moveVect;
            ctrl.Move(moveVect);
        }
    }
    bool isAxisSmall(float value)
    {
        return Mathf.Abs(value) < 0.05f;
    }
    #endregion

    #region rotate by mouse
    bool holdingMouseR = false;

    private Vector3 rotationSpeed;
    public float sensivity = 1;
    void ProcessRotate()
    {
        //if (Input.GetMouseButtonUp(1))
        //{
        //    //transform.Rotate(Vector3.zero * Time.deltaTime);

        //}
        //else if (Input.GetMouseButton(1))
        //{
        //    float y = Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        //    rotationSpeed.y = y;
        //    transform.Rotate(rotationSpeed * Time.deltaTime);
        //}
    }
    #endregion

    void CheckFallen()
    {
        bool fallen = this.transform.localPosition.y < -3;
        if (fallen)
            GamePlayManager.Instance.GameOver(GameOverReason.Fallen);
    }


    public void ResetPosition()
    {

        this.transform.position = new Vector3(-15, 0, -29);
    }
    #region Health system
    public float HP = 0;
    public float HPMax = 100;
    public void ResetHP()
    {
        this.HP = HPMax;
    }
    void AddHP(float value)
    {
        this.HP += value;
        ChechHP();
    }
    void ChechHP()
    {
        if (this.HP <= 0)
        {
            //trigger game over
            GamePlayManager.Instance.GameOver(GameOverReason.Died);
        }
        if (this.HP > HPMax)
        {
            HP = HPMax;
        }
    }
    #endregion

    #region collision events

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision Entered with " + collision.gameObject.name);
        var item = collision.gameObject.GetComponent<InteractiveItem>();
        if (item != null)
        {
            item.MakeInvisible();
            switch (item.itemType)
            {
                case ItemType.TimePotion:
                    GamePlayManager.Instance.AddTime(10);
                    break;
                case ItemType.Coin:
                    GamePlayManager.Instance.AddCoin(10);
                    break;
                case ItemType.Score:
                    GamePlayManager.Instance.AddScore(5);
                    break;
                case ItemType.HealthPotion: AddHP(10); break;
                case ItemType.ToxicPotion: AddHP(-10); break;
            }
        }



    }
    #endregion
}
