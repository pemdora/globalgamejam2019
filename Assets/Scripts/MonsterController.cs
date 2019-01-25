using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public bool changedTarget;
    public GameObject target;

    private void Start()
    {
        MovementManager.instance.AssignPOI(this);
        changedTarget = false;
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
        if (!changedTarget && Vector3.Distance(this.transform.position, target.transform.position) > 1.5f)
        {
            MoveTo(target);
        }
        else
        {
            if (!changedTarget)
            {
                changedTarget = true;
                MovementManager.instance.AssignPOI(this);
            }
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
