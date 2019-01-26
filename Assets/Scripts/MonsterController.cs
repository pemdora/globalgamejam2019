using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    [HideInInspector]
    public bool doingAction;
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public float timeLeft;

    [SerializeField]
    private Image emoteImg;

    private bool playingImgAnimation;

    public List<ObjectData> reactionObjects;

    private void Start()
    {
        MovementManager.instance.AssignPOI(this);
        doingAction = true;
        playingImgAnimation = false;
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
        else if (doingAction)
        {
            timeLeft -= Time.deltaTime;
            target.GetComponent<RoomObject>().occupied = true;
            if (timeLeft < 0)
            {
                target.GetComponent<RoomObject>().occupied = false;
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

    // Move to a gameobject
    public void ReactToObject(GameObject _obj)
    {
        if (!playingImgAnimation)
        {
            foreach (ObjectData objData in reactionObjects)
            {
                if (_obj.name.Equals(objData.obj.name))
                {
                    switch (objData.reaction)
                    {
                        case ObjectData.REACTION.Love:
                            Debug.Log("Love");
                            emoteImg.sprite = objData.emote;
                            StartCoroutine(FadeINandOutImage(0.7f, false, emoteImg, 0.5f));
                            break;
                        case ObjectData.REACTION.Hate:
                            Debug.Log("Hate");
                            emoteImg.sprite = objData.emote;
                            StartCoroutine(FadeINandOutImage(0.7f, false, emoteImg, 0.5f));
                            break;
                    }
                }
            }
        }
    }

    IEnumerator FadeINandOutImage(float alpha,bool fadeAway, Image img, float speed)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = alpha; i >= 0; i -= Time.deltaTime * speed)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
            img.enabled = false;
            playingImgAnimation = false;
        }
        // fade from transparent to opaque
        else
        {
            playingImgAnimation = true;
            img.enabled = true;
            // loop over 0.5 second
            for (float i = 0; i <= alpha; i += Time.deltaTime * speed)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
            StartCoroutine(FadeINandOutImage(alpha, true, img, speed));
        }
    }

}
