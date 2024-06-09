using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Singleton;

namespace SunHeTBS
{

    public class InputManager : ISingleton
    {


        public static InputManager Inst { get; private set; }
        public static void Init()
        {
            Inst = UniSingleton.CreateSingleton<InputManager>();
        }
        public void OnCreate(object createParam)
        {
        }

        public void OnUpdate()
        {
            InputUpdate();
        }

        public void OnDestroy()
        {
        }


        /// <summary>
        /// last axis input time stamp (milli seconds)
        /// </summary>
        long milliTS_cursor_moved = 0;
        /// <summary>
        /// if second input triggered within this time interval ,ignore it .(milli seconds)
        /// </summary>
        long interval_cursor_move = 230;
        /// <summary>
        /// how deep the input is considered taking effect
        /// </summary>
        float axis_move_sensitivity = 0.2f;
        // Update is called once per frame
        public bool axisLeft { get; private set; }
        public bool axisRight { get; private set; }
        public bool axisUp { get; private set; }
        public bool axisDown { get; private set; }

        void InputUpdate()
        {
            #region Read input Axis
            //axisLeft = false;
            //axisRight = false;
            //axisUp = false;
            //axisDown = false;
            //long milliTS = TimeUtil.GetMilliseconds();
            //if (milliTS - milliTS_cursor_moved < interval_cursor_move)
            //{ }
            //else
            //{
            //    float axis_horizontal = Input.GetAxis("Horizontal");
            //    float axis_vertical = Input.GetAxis("Vertical");
            //    if (Mathf.Abs(axis_horizontal) > axis_move_sensitivity)
            //    {
            //        if (axis_horizontal > 0)
            //            axisRight = true;
            //        else
            //            axisLeft = true;
            //        milliTS_cursor_moved = milliTS;
            //    }
            //    if (Mathf.Abs(axis_vertical) > axis_move_sensitivity)
            //    {
            //        if (axis_vertical > 0)
            //            axisUp = true;
            //        else
            //            axisDown = true;
            //        milliTS_cursor_moved = milliTS;
            //    }
            //}
            #endregion
            

        }

        public static Camera mapCamera;
    
    }
}
