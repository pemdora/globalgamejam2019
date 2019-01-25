using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {

    public List<GameObject> pointOfInterest;
    private List<GameObject> freePointOfInterest;

    public static MovementManager instance;
    
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void AssignPOI(MonsterController monsterC)
    {
        int index = Random.Range(0, pointOfInterest.Count-1);
        Debug.Log("index " + index);

        monsterC.target =  pointOfInterest[index].transform.position;
        monsterC.changedTarget = false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
