using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovableObject : MonoBehaviour {

    public float delay;
    private float timeLeft;
    private bool blockDrag;

    private Vector3 screenSpace;
    private Vector3 offset;
    private bool isDragged;
    private GameObject canvas;

    private RaycastHit hit;
    private Ray ray;
    
	// Update is called once per frame
	void Update () {

        if (isDragged && !blockDrag)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);
            float distance;

            if (plane.Raycast(ray, out distance))
            {
                Vector3 temp = ray.GetPoint(distance);
                transform.position = temp;
            }
        }
        else
        {
            timeLeft -= Time.deltaTime;
            canvas.GetComponent<TextMeshPro>().text = timeLeft.ToString();
            if (timeLeft < 0)
            {
                blockDrag = false;
            }
            else
            {
                blockDrag = true;
            }
        }
    }
    
    //Script to drag an object in world space using the mouse
    private void OnMouseDown()
    {
        isDragged = true;
    }

    private void OnMouseUp()
    {
        isDragged = false;
    }

    public void SetCanvas(GameObject _canvas)
    {
        canvas = _canvas;
        timeLeft = delay;
    }

}
