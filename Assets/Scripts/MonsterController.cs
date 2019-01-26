using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    [HideInInspector]
    public bool occupyplace;
    public bool actionchosen;
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public float timeLeft;

    [SerializeField]
    private Image emoteImg;
    [SerializeField]
    private Sprite star;


    private bool playingImgAnimation;

    public List<ObjectData> reactionObjects;

    private void Start()
    {
        LevelManager.instance.AssignPOI(this);
        occupyplace = false;
        playingImgAnimation = false;
        actionchosen = false;
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
        // Image facing billboard
        if (Camera.main != null)
        {
            emoteImg.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                Camera.main.transform.rotation * Vector3.up);
        }
        else
        {
            Debug.Log("The active camera should be tagged with MainCamera ");
        }

        // moving if target is not reached => move
        if (Vector3.Distance(this.transform.position, target.transform.position) > 2f)
        {
            MoveTo(target);
        }
        else // target reached
        {
            if (!actionchosen)
            {
                // room is occupied, monster is angry and change target
                if (target.GetComponent<RoomObject>().occupied == true)
                {
                    actionchosen = true;

                    Debug.Log("agnrrrrrrrryyyy");
                    // monster is angry 
                    LevelManager.instance.AssignPOI(this); // change target
                }
                // room is free, monster will occupy the room
                else
                {
                    target.GetComponent<RoomObject>().occupied = true;
                    actionchosen = true;
                    occupyplace = true;
                }
            }
            else if (occupyplace)
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft < 0)
                {
                    StartFinishedAction();
                    target.GetComponent<RoomObject>().occupied = false;

                    LevelManager.instance.AssignPOI(this); // Assign POI and set doingAction to false
                }
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
                if (_obj.name.Equals(objData.obj.name + "(Clone)"))
                {
                    switch (objData.reaction)
                    {
                        case ObjectData.REACTION.Love:
                            emoteImg.sprite = objData.emote;
                            StartEmoteAnim();
                            //StartCoroutine(FadeINandOutImage(0.7f, false, emoteImg, 0.5f));
                            // Score
                            PlayerManager.instance.UpdateScore(0.1f);
                            PlayerManager.instance.UpdateMoney(10);
                            break;
                        case ObjectData.REACTION.Hate:
                            emoteImg.sprite = objData.emote;

                            StartEmoteAnim();
                            //StartCoroutine(FadeINandOutImage(0.7f, false, emoteImg, 0.5f));
                            // Score
                            PlayerManager.instance.UpdateScore(-0.5f);
                            break;
                    }
                }
            }
        }
    }

    // NOT USED ANYMORE
    IEnumerator FadeINandOutImage(float alpha, bool fadeAway, Image img, float speed)
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

    public void StartEmoteAnim()
    {
        CancelInvoke("FinishedEmoteAnim"); // Cancel other animation
        CancelInvoke("FinishedAction"); // Cancel other animation

        playingImgAnimation = true;
        emoteImg.GetComponent<Animation>().Play("EmotionAnimation");

        Invoke("FinishedEmoteAnim", emoteImg.GetComponent<Animation>()["EmotionAnimation"].length);
    }


    public void FinishedEmoteAnim()
    {
        playingImgAnimation = false;
    }

    public void StartFinishedAction()
    {
        CancelInvoke("FinishedEmoteAnim"); // Cancel other animation
        CancelInvoke("FinishedAction"); // Cancel other animation

        emoteImg.enabled = true;
        emoteImg.sprite = star;

        playingImgAnimation = true;
        emoteImg.GetComponent<Animation>().Play("StarAnimation");

        Invoke("FinishedAction", emoteImg.GetComponent<Animation>()["StarAnimation"].clip.length);
    }

    public void FinishedAction()
    {
        playingImgAnimation = false;
        PlayerManager.instance.UpdateScore(0.2f);
        CancelInvoke("FinishedAction");
    }
}
