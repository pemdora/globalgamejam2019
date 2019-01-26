using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float panSpeed = 5.0f;
    public float boundery = 30.0f;

	void Update () {

        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 1, 6);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize + 1, 14);
        }
        if (Input.GetMouseButton(2))
        {
            float horizontal = Input.GetAxis("Mouse X");
            float vertical = Input.GetAxis("Mouse Y");

            Vector3 newPos = new Vector3(Camera.main.transform.position.x - horizontal * panSpeed, Camera.main.transform.position.y - vertical * panSpeed);
            
            if(newPos.x < boundery && newPos.x > -boundery && newPos.y < boundery && newPos.y > -boundery)
            {
                Camera.main.transform.position = newPos;
            }
        }
    }
}
