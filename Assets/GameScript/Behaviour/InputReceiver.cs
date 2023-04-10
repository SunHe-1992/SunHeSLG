using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputReceiver : MonoBehaviour
{
    public static InputReceiver Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
    }
    private void OnDestroy()
    {
        Inst = null;
    }
    #region TBS gameplay

    public void GameplayConfirmClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            //Debug.Log("on confirm click");
            //Debug.Log("input " + m_Move);

        }
    }
    public void GameplayCancelClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            //Debug.Log("on cancel click");

        }
    }
    private Vector2 m_Move;
    public void GameplayMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
        //Debug.Log("input " + m_Move);
    }

    #endregion

    #region axis receive
    private void Update()
    {
        #region Read input Axis
        axisLeft = false;
        axisRight = false;
        axisUp = false;
        axisDown = false;
        long milliTS = TimeUtil.GetMilliseconds();
        if (milliTS - milliTS_cursor_moved < interval_cursor_move)
        { }
        else
        {
            float axis_horizontal = m_Move.x;
            float axis_vertical = m_Move.y;
            if (Mathf.Abs(axis_horizontal) > axis_move_sensitivity)
            {
                if (axis_horizontal > 0)
                    axisRight = true;
                else
                    axisLeft = true;
                milliTS_cursor_moved = milliTS;
            }
            if (Mathf.Abs(axis_vertical) > axis_move_sensitivity)
            {
                if (axis_vertical > 0)
                    axisUp = true;
                else
                    axisDown = true;
                milliTS_cursor_moved = milliTS;
            }
        }
        #endregion
    }
    /// <summary>
    /// last axis input time stamp (milli seconds)
    /// </summary>
    long milliTS_cursor_moved = 0;
    /// <summary>
    /// if second input triggered within this time interval ,ignore it .(milli seconds)
    /// </summary>
    long interval_cursor_move = 100;
    /// <summary>
    /// how deep the input is considered taking effect
    /// </summary>
    float axis_move_sensitivity = 0.20f;
    // Update is called once per frame
    public bool axisLeft { get; private set; }
    public bool axisRight { get; private set; }
    public bool axisUp { get; private set; }
    public bool axisDown { get; private set; }
    #endregion
}
