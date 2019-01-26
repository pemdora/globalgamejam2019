using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Movable")
        {
            this.GetComponentInParent<MonsterController>().ReactToObject(other.gameObject);
        }
    }
}
