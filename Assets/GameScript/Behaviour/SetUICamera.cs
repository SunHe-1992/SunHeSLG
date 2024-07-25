using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SetUICamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        var thisCam = this.GetComponent<Camera>();
        if (thisCam != null)
        {
            var cameraData = thisCam.GetUniversalAdditionalCameraData();
            if (cameraData != null)
            {
                cameraData.cameraStack.Add(FairyGUI.StageCamera.main);
            }
        }
    }


}
