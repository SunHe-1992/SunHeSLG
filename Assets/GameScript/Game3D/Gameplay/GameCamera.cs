using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Transform campos;
    public Transform lookAt;

    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.position - campos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePlayManager.Instance.running == false)
            return;
        if (campos != null)
        {
            Vector3 targetPos = campos.transform.position;
            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * 25);
            if (lookAt != null)
                transform.LookAt(lookAt.position);
        }

    }
}
