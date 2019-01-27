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
    [HideInInspector]
    public bool isDragged;
    private GameObject canvas;

    private RaycastHit hit;
    private Ray ray;
    private TextMeshProUGUI textMeshPro;
    private BoxCollider boxCollider;

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

            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(new Vector3(0, -1.5f, 0));
            }
            if (Input.GetKey(KeyCode.R))
            {
                transform.Rotate(new Vector3(0, 1.5f, 0));
            }
        }
        else if(blockDrag)
        {
            timeLeft -= Time.deltaTime;
            textMeshPro.text = Mathf.Round(timeLeft).ToString();
            if (timeLeft < 0)
            {
                blockDrag = false;
                timeLeft = delay;
                textMeshPro.text = "";
            }
        }
    }

    //Script to drag an object in world space using the mouse
    private void OnMouseDown()
    {
        isDragged = true;
        this.gameObject.tag = "Untagged";// don't want to be triggered when dragged
    }

    private void OnMouseUp()
    {
        isDragged = false;
        blockDrag = true;
        this.gameObject.tag = "Movable"; 
    }

    public void SetCanvas(GameObject _canvas)
    {
        canvas = _canvas;
        timeLeft = delay;
        textMeshPro = canvas.GetComponentInChildren<TextMeshProUGUI>();
        boxCollider = this.GetComponent<BoxCollider>();
        //boxCollider.isTrigger = true;
    }

}
