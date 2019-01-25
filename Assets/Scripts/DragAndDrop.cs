using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

    private Vector3 screenSpace;
    private  Vector3 offset;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    //Script to drag an object in world space using the mouse
    private void OnMouseDown()
    {        
        
        //translate the cubes position from the world to Screen Point
        screenSpace = Camera.main.WorldToScreenPoint(transform.position);

        //calculate any difference between the cubes world position and the mouses Screen position converted to a world point  
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));


    }

    /*
    OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse.
    OnMouseDrag is called every frame while the mouse is down.
    */
    private void OnMouseDrag()
    {
        //keep track of the mouse position
        Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

        //convert the screen mouse position to world point and adjust with offset
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

        if (curPosition.y < 0)
        {
            curPosition.y = 1f;
        }
        //update the position of the object in the world
        transform.position = curPosition;
    }

}
