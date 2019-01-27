using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    [HideInInspector]
    public bool occupyplace;
    [HideInInspector]
    public bool actionchosen;
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public float timeLeft; // timeleft to finish action


    private Transform vision;

    [SerializeField]
    private Image emoteImg;
    [SerializeField]
    private Sprite annoyed;

    private Animator animator;
    private bool playingImgAnimation;

    [SerializeField]
    private GameObject panel_Character;

    [Header("[Caractéristique]")]
    [SerializeField]
    private string namemonster;
    [SerializeField]
    private Sprite portrait;
    [SerializeField]
    private string descriptionLove;
    [SerializeField]
    private string descriptionHate;

    [Header("[Animation Length for Happy/Angry]")]
    [SerializeField]
    private float angryAnimationLenght;
    [SerializeField]
    private float happyAnimationLenght;
    [Header("[Waiting score penalty]")]
    public float waitingPenalty; // penalty when monster cannot do action

    [Header("[Delay]")]
    public float canReactDelay; // delay before next reaction to object
    private bool canReact;

    [Header("[Objects that got reaction with monster]")]
    public List<ObjectData> reactionObjects;

    private void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
        LevelManager.instance.AssignPOI(this);
        occupyplace = false;
        playingImgAnimation = false;
        actionchosen = false;
        canReact = true;
        vision = this.transform.Find("Vision");
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

    // Move to a gameobject
    public void Stop()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
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
        if (target != null && Vector2.Distance(new Vector2(vision.position.x,vision.position.z), new Vector2(target.transform.position.x, target.transform.position.z)) > 1.5f)
        {
            if(!animator.GetBool("angry") && !animator.GetBool("happy"))
            {
                GetComponent<NavMeshAgent>().isStopped = false;
                animator.SetBool("walking", true);
                MoveTo(target); // need to be called once to be cleaner
            }
            else
            {
                Stop();
            }
        }
        else // target reached
        {
            if (!actionchosen)
            {
                animator.SetBool("walking", false);
                // room is occupied, monster is angry and change target
                if (target.GetComponent<RoomObject>().occupied == true)
                {
                    actionchosen = true;

                    PlayerManager.instance.UpdateScore(waitingPenalty);
                    animator.SetBool("angry", true);
                    emoteImg.sprite = annoyed;
                    StartEmotionAnim();
                    StartCoroutine(ChangeAnimationAndAssignTarget("angry", false, angryAnimationLenght));
                    this.GetComponent<SFXSound>().PlayTheSound(1, 1f); //annoyed
                    // monster is angry 
                }
                // room is free, monster will occupy the room
                else
                {
                    emoteImg.sprite = target.GetComponent<RoomObject>().spr;
                    StartRoomEmote();

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
                    FinishedAction();
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

    IEnumerator ChangeAnimationAndAssignTarget(string animationName, bool value, float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("walking", true);
        GetComponent<NavMeshAgent>().isStopped = false;
        animator.SetBool(animationName, value);
        LevelManager.instance.AssignPOI(this);
    }

    IEnumerator ChangeAnimation(string animationName, bool value, float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(animationName, value);
    }

    // Move to a gameobject
    public void ReactToObject(GameObject _obj)
    {
        if (!playingImgAnimation && !occupyplace && !actionchosen && canReact ) // if the monster is not occupied by something else
        {
            canReact = false;
            Invoke("BlockReaction", canReactDelay);
            foreach (ObjectData objData in reactionObjects)
            {
                if (_obj.name.Equals(objData.obj.name + "(Clone)"))
                {
                    switch (objData.reaction)
                    {
                        case ObjectData.REACTION.Love:
                            emoteImg.sprite = objData.emote;
                            StartEmotionAnim();
                            //StartCoroutine(FadeINandOutImage(0.7f, false, emoteImg, 0.5f));
                            // Score
                            animator.SetBool("happy", true);
                            StartCoroutine(ChangeAnimationAndAssignTarget("happy", false, angryAnimationLenght));
                            this.GetComponent<SFXSound>().PlayTheSound(0, 1f); //angry

                            PlayerManager.instance.UpdateScore(objData.satisfactionScore);
                            PlayerManager.instance.EarnMoney(1, this.transform.position);
                            break;
                        case ObjectData.REACTION.Hate:
                            emoteImg.sprite = objData.emote;
                            this.GetComponent<SFXSound>().PlayTheSound(2,1f); //angry

                            animator.SetBool("angry", true);
                            StartCoroutine(ChangeAnimationAndAssignTarget("angry", false, angryAnimationLenght));

                            StartEmotionAnim();
                            //StartCoroutine(FadeINandOutImage(0.7f, false, emoteImg, 0.5f));
                            // Score
                            PlayerManager.instance.UpdateScore(objData.satisfactionScore);
                            break;
                    }
                }
            }
        }
        else
        {
            //Debug.Log("cannot react");
        }
    }

    private void BlockReaction()
    {
        canReact = true;
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
    
    // Happy or angry animation
    public void StartEmotionAnim()
    {
        animator.SetBool("walking", false);
        CancelInvoke("FinishedEmoteAnim"); // Cancel other animation
        CancelInvoke("FinishedAction"); // Cancel other animation

        playingImgAnimation = true;
        emoteImg.GetComponent<Animation>().Play("EmotionAnimation");

        Invoke("FinishedEmoteAnim", emoteImg.GetComponent<Animation>()["EmotionAnimation"].length);
    }
    public void FinishedEmoteAnim()
    {
        playingImgAnimation = false;
        animator.SetBool("walking", true);
    }

    // Not used anymore
    public void StartRoomEmote()
    {
        CancelInvoke("FinishedEmoteAnim"); // Cancel other animation
        CancelInvoke("FinishedAction"); // Cancel other animation

        emoteImg.enabled = true;

        playingImgAnimation = true;

        Animation anim = emoteImg.GetComponent<Animation>();
        anim.Play("ActionAnimation");
        anim["ActionAnimation"].time = 0f;
        anim["ActionAnimation"].speed = 1f;

        anim.Play("ActionAnimation");

        //Invoke("FinishedAction", emoteImg.GetComponent<Animation>()["StarAnimation"].clip.length);
    }

    // Not used anymore
    public void FinishedAction()
    {
        playingImgAnimation = false;
        CancelInvoke("FinishedAction");

        Animation anim = emoteImg.GetComponent<Animation>();
        anim["ActionAnimation"].time = anim["ActionAnimation"].length;
        anim["ActionAnimation"].speed = -1f;

        anim.Play("ActionAnimation");
    }

    
    public void OpenCharacterCharac()
    {
        panel_Character.transform.Find("Name").GetComponent<Text>().text = namemonster;
        panel_Character.transform.Find("Portrait").GetComponent<Image>().sprite = portrait;
        panel_Character.transform.Find("Love").GetComponent<Text>().text = descriptionLove;
        panel_Character.transform.Find("Hate").GetComponent<Text>().text = descriptionHate;
        if (target!=null)
            panel_Character.transform.Find("Target").GetComponent<Image>().sprite = target.GetComponent<RoomObject>().spr;
        panel_Character.SetActive(true);
    }


}