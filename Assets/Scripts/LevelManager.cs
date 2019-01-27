using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField]
    private GameObject roomObjects;
    private List<GameObject> freePointOfInterest;
    [SerializeField]
    private Transform movableObjectsParent;
    private List<Transform> spawnPoints;
    [SerializeField]
    private GameObject canvasCountDownObject;

    [Header("[List of shopable Objects]")]
    [SerializeField]
    private List<GameObject> shopableObjects;

    public static LevelManager instance;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;

            // Set point of interest
            freePointOfInterest = new List<GameObject>();
            foreach (Transform poi in roomObjects.transform)
            {
                freePointOfInterest.Add(poi.gameObject);
            }

            // Set spawn points for shoppable items
            Transform spawnPointsTransform = movableObjectsParent.transform.Find("SpawnPoints");
            spawnPoints = new List<Transform>();
            foreach (Transform tr in spawnPointsTransform)
            {
                spawnPoints.Add(tr);
            }
        }
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        
    }

    public void AssignPOI(MonsterController monsterC)
    {
        // First random try
        int index = Random.Range(0, freePointOfInterest.Count);
        GameObject newPOI = freePointOfInterest[index];
        
        // while random target is equal to current target
        while (freePointOfInterest[index] == null || freePointOfInterest[index] == monsterC.target)
        {
            index = Random.Range(0, freePointOfInterest.Count);
            newPOI = freePointOfInterest[index];
        }

        monsterC.target = newPOI;
        monsterC.timeLeft = newPOI.GetComponent<RoomObject>().time;
        monsterC.occupyplace = false;
        monsterC.actionchosen = false;
    }

    public void SpawnObject(string objectName)
    {
        foreach (GameObject obj in shopableObjects)
        {
            if (obj.name.Equals(objectName))
            {
                // Instanciate object
                GameObject newobj = Instantiate(obj, movableObjectsParent);
                // Instanciate UI
                GameObject canvas = Instantiate(canvasCountDownObject, newobj.transform);
                canvas.transform.localScale = Vector3.one;
                canvas.transform.position = new Vector3(0, 2.5f, 0);

                newobj.GetComponent<MovableObject>().SetCanvas(canvas);

                int index = Random.Range(0, spawnPoints.Count);
                Transform pos = spawnPoints[index];
                newobj.transform.position = pos.position;
            }
        }
    }
}
