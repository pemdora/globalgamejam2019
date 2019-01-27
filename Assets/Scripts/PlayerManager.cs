using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Canvas mainCanvas;

    public Image scoreUIPlaceHolder;
    public Image scoreUI;

    public Text moneyUI;
    public Image coinIcon;

    private float score;
    private float scoreMax;

    private int money;

    [SerializeField]
    private Slider _slider;
    private float timeLeft;
    private bool finishedDay;
    [Header("Day param")]
    public float dayLength;

    private Animation theAnimEndOfTheDay;
    [SerializeField]
    private GameObject panel_End_Of_TheDAy;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
            theAnimEndOfTheDay = panel_End_Of_TheDAy.GetComponent<Animation>();
        }
        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);


        score = 2.5f;
        scoreMax = 5.0f;
        UpdateScore(0.0f);

        money = 100;
        UpdateMoney(0);
    }

    void Start()
    {
        ResetSlider();
        finishedDay = false;
    }

    public void ResetSlider()
    {
        _slider.minValue = 0f;
        _slider.maxValue = dayLength;
        timeLeft = dayLength;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        _slider.value = dayLength - timeLeft;
        if (timeLeft < 0 && !finishedDay)
        {
            finishedDay = true;
            StartAnim();
        }
    }

    public void UpdateScore(float scoreToAdd)
    {
        if (score <= scoreMax)
        {
            score += scoreToAdd;
            float widthMax = scoreUIPlaceHolder.rectTransform.sizeDelta.x;
            float width = score * widthMax / scoreMax;

            Animation anim = scoreUIPlaceHolder.GetComponent<Animation>();
            anim.Play("ScoreAnimation");
            scoreUI.rectTransform.sizeDelta = new Vector2(width, scoreUI.rectTransform.sizeDelta.y);
        }
    }

    public float GetScore()
    {
        return score;
    }

    public void UpdateMoney(int moneyToAdd)
    {
        money += moneyToAdd;
        moneyUI.text = money.ToString();
    }

    public void EarnMoney(int moneyEarned, Vector3 uiPosition)
    {
        //Debug.Log("money earned : " + moneyEarned);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(uiPosition);
        Image coin = Instantiate(coinIcon, mainCanvas.transform);
        coin.transform.localScale = Vector3.one;

        coin.rectTransform.position = new Vector3(screenPos.x, screenPos.y);
        StartCoroutine(MoveCoinToUIPoint(coin));

        money += moneyEarned;
        moneyUI.text = money.ToString();
    }

    public int GetMoney()
    {
        return money;
    }

    private IEnumerator MoveCoinToUIPoint(Image coinIcon)
    {
        while (Vector3.Distance(coinIcon.rectTransform.position, moneyUI.rectTransform.position) > 1.5f)
        {
            //coinIcon.rectTransform.position = Vector3.Lerp(coinIcon.rectTransform.position, moneyUI.rectTransform.position, 2 * Time.deltaTime);
            coinIcon.rectTransform.position = Vector3.MoveTowards(coinIcon.rectTransform.position, moneyUI.rectTransform.position, 10f);
            yield return null;
        }
        Destroy(coinIcon);

    }


    public void StartAnim()
    {

        panel_End_Of_TheDAy.SetActive(true);
        CancelInvoke("FinishAnim");
        theAnimEndOfTheDay["EndOfTheDay"].speed = 1f;

        theAnimEndOfTheDay.Play("EndOfTheDay");
        Invoke("PauseGame", theAnimEndOfTheDay["EndOfTheDay"].length);
        //Invoke("FinishAnim", theAnimEndOfTheDay["EndOfTheDay"].length);
    }

    public void PauseGame()
    {
        MenuManager.instance.PauseGame();
    }

    public void MainMenu()
    {
        MenuManager.instance.UnpauseGame();
        MenuManager.instance.LoadMainMenu();
    }

    public void FinishAnim()
    {
        //MenuManager.instance.PauseGame();
        CancelInvoke("FinishAnim");
        theAnimEndOfTheDay["EndOfTheDay"].time = theAnimEndOfTheDay["EndOfTheDay"].length;
        theAnimEndOfTheDay["EndOfTheDay"].speed = -1f;

        theAnimEndOfTheDay.Play("EndOfTheDay");
        Invoke("SwitchOff", (theAnimEndOfTheDay["EndOfTheDay"].length + 0.5f));
    }

    private void SwitchOff()
    {
        gameObject.SetActive(false);
    }
}