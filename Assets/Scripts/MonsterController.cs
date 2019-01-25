using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [HideInInspector]
    public bool doingAction;
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public float timeLeft;

    private void Start()
    {
        MovementManager.instance.AssignPOI(this);
        doingAction = true;
    }

    // Move to a position
    public void MoveTo(Vector3 target)
    {
        GetComponent<NavMeshAgent>().SetDestination(target);
    }

    // Move to a gameobject
    public void MoveTo(GameObject target)
    {
        GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
    }

    void Update()
    {
        // moving if target is not reached
        if (Vector3.Distance(this.transform.position, target.transform.position) > 1.5f)
        {
            MoveTo(target);
        }
        else if(doingAction)
        {
            Debug.Log(timeLeft);
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                MovementManager.instance.AssignPOI(this);
            }
        }
        else
        {
            Debug.Log("lolol");
        }

        /*
        // moving the monster by click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                MoveTo(hit.point);
            }
        }*/
    }
}
