using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float panSpeed = 5.0f;
    public float XMin = 30.0f;
    public float XMax = 30.0f;
    public float YMin = 30.0f;
    public float YMax = 30.0f;

    void Update () {

        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 1, 6);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize + 1, 14);
        }
        if (Camera.main.orthographicSize < 13 && Input.GetMouseButton(1))
        {
            float horizontal = Input.GetAxis("Mouse X") * panSpeed;
            float vertical = Input.GetAxis("Mouse Y") * panSpeed;

            Vector3 newPos = new Vector3(Camera.main.transform.position.x + horizontal, Camera.main.transform.position.y + vertical, Camera.main.transform.position.z - horizontal);

            if (newPos.x < XMax && newPos.x > -XMin && newPos.y < YMax && newPos.y > -YMin)
            {
                Camera.main.transform.position = newPos;
            }
        }
    }
}
