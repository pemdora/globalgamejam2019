using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public List<GameObject> pointOfInterest;
    private List<GameObject> freePointOfInterest;
    [SerializeField]
    private List<GameObject> shopableObjects;
    [SerializeField]
    private Transform movableObjectsParent;

    public static LevelManager instance;
    
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
            freePointOfInterest = new List<GameObject>(pointOfInterest);
        }
        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void AssignPOI(MonsterController monsterC )
    {
        int index = Random.Range(0, freePointOfInterest.Count-1);
        GameObject newPOI = freePointOfInterest[index];

        
        // while random target is equal to current target
        while (freePointOfInterest[index]==null||freePointOfInterest[index]==monsterC.target)
        {
            index = Random.Range(0, freePointOfInterest.Count - 1);
            newPOI = freePointOfInterest[index];
        }
        
        monsterC.target = newPOI;
        monsterC.timeLeft = newPOI.GetComponent<RoomObject>().time;

        monsterC.doingAction = true;
    }
    
    public void SpawnObject(string objectName)
    {
        foreach(GameObject obj in shopableObjects)
        {
            if (obj.name.Equals(objectName))
            {
                Instantiate(obj, movableObjectsParent);
            }
        }
    }
}
