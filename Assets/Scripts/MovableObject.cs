using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour {

    private Vector3 screenSpace;
    private Vector3 offset;
    private bool isDragged;

    private RaycastHit hit;
    private Ray ray;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isDragged)
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
}
