using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UniFramework.Event;
using SunHeTBS;
public class InputReceiver : MonoBehaviour
{
    public static InputReceiver Inst { get; private set; }
    public PlayerInput inputComp;
    private void Awake()
    {
        Inst = this;
        inputComp = this.GetComponent<PlayerInput>();
    }
    private void OnDestroy()
    {
        Inst = null;
    }


    #region axis receive
    private Vector2 m_Move;
    public void GameplayMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        ReadInputAxis();
        if (axisDown || axisUp || axisLeft || axisRight)
            UniEvent.SendMessage(GameEventDefine.InputAxis);
    }
    private void ReadInputAxis()
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
    float axis_move_sensitivity = 0.10f;
    // Update is called once per frame
    public bool axisLeft { get; private set; }
    public bool axisRight { get; private set; }
    public bool axisUp { get; private set; }
    public bool axisDown { get; private set; }
    #endregion


    #region UI control events
    public void UIMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
    }


    #endregion
    public static bool InputInUI = true;
    public static void SwitchInputToMap()
    {
        if (Inst?.inputComp != null)
            Inst.inputComp.SwitchCurrentActionMap("Player");
        InputInUI = false;
        Debugger.Log($"SwitchInputTo Map");
    }
    public static void SwitchInputToUI()
    {
        if (Inst?.inputComp != null)
            Inst.inputComp.SwitchCurrentActionMap("UI");
        InputInUI = true;
        Debugger.Log($"SwitchInputTo UI");
    }

}
