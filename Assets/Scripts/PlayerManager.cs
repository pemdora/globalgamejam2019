using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private bool gameOver;

    [SerializeField]
    private Slider _slider;
    private float timeLeft;
    private bool finishedDay;

    private Animation theAnimEndOfTheDay;
    [SerializeField]
    private GameObject panel_End_Of_TheDAy;
    private Animation gameOverAnim;
    [SerializeField]
    private GameObject panel_GameOver;

    [Header("Game param")]
    public float dayLength;
    public float scoreStart;
    public int moneyStart;
    public int maxDay;
    private int dayCounter;

    private Color daySky;
    private Color nightSky;
    private Color dayLight;
    private Color nightLight;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
            theAnimEndOfTheDay = panel_End_Of_TheDAy.GetComponent<Animation>();
            gameOverAnim = panel_GameOver.GetComponent<Animation>();
            dayCounter = 0;
        }
        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);


        score = scoreStart;
        money = moneyStart;

        scoreMax = 5.0f;
        UpdateScore(0.0f);

        UpdateMoney(0);

        // init colors
        daySky = new Color(74, 129, 161, 0) / 255;
        nightSky = new Color(35, 56, 67, 0) / 255;
        dayLight = new Color(255, 255, 255, 1) / 255;
        nightLight = new Color(250, 148, 48, 1) / 255;
}

    void Start()
    {
        ResetSlider();
        finishedDay = false;
        gameOver = false;
    }

    public void ResetSlider()
    {
        _slider.minValue = 0f;
        _slider.maxValue = dayLength;
        timeLeft = dayLength;
    }

    void Update()
    {
        if (!gameOver && score <= 0)
        {
            gameOver = true;
            StartAnimGameOver();
        }
        else
        {
            timeLeft -= Time.deltaTime;
            _slider.value = dayLength - timeLeft;

            // handle the sun light direction
            float ratio = _slider.value / dayLength;

            float rSky = daySky.r + (nightSky.r - daySky.r) * ratio;
            float gSky = daySky.g + (nightSky.g - daySky.g) * ratio;
            float bSky = daySky.b + (nightSky.b - daySky.b) * ratio;
            Color skyColor = new Color(rSky, gSky, bSky);

            float rLight = dayLight.r + (nightLight.r - dayLight.r) * ratio;
            float gLight = dayLight.g + (nightLight.g - dayLight.g) * ratio;
            float bLight = dayLight.b + (nightLight.b - dayLight.b) * ratio;
            Color lightColor = new Color(rLight, gLight, bLight);

            Camera.main.backgroundColor = skyColor;
            Light dirLight = GameObject.Find("Directional Light").GetComponent<Light>();
            dirLight.color = lightColor;


            if (timeLeft <= 0 && !finishedDay)
            {
                dayCounter++;
                panel_End_Of_TheDAy.transform.Find("Day").GetComponent<TextMeshProUGUI>().text = this.dayCounter.ToString() + "/" + this.maxDay;
                finishedDay = true;
                panel_End_Of_TheDAy.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = this.score.ToString();
                int moneyEarned = (int)Mathf.Floor(this.score) * 10;
                this.money += moneyEarned;
                panel_End_Of_TheDAy.transform.Find("Money").GetComponent<TextMeshProUGUI>().text = (this.money-moneyEarned) + "+ (" + moneyEarned.ToString() + ")";
                StartAnimEndDay();

                if (dayCounter>= maxDay)
                {
                    panel_End_Of_TheDAy.transform.Find("ButtonNext").gameObject.SetActive(false);
                }
            }
        }
    }

    public void NextDay()
    {
        MenuManager.instance.UnpauseGame();
        FinishAnimEndDay();
        ResetSlider();
        finishedDay = false;
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


    public void StartAnimEndDay()
    {

        panel_End_Of_TheDAy.SetActive(true);
        CancelInvoke("FinishAnimEndDay");
        theAnimEndOfTheDay["EndOfTheDay"].speed = 1f;

        theAnimEndOfTheDay.Play("EndOfTheDay");
        Invoke("PauseGame", theAnimEndOfTheDay["EndOfTheDay"].length);
        //Invoke("FinishAnim", theAnimEndOfTheDay["EndOfTheDay"].length);
    }

    public void FinishAnimEndDay()
    {
        //MenuManager.instance.PauseGame();
        CancelInvoke("FinishAnimEndDay");
        theAnimEndOfTheDay["EndOfTheDay"].time = theAnimEndOfTheDay["EndOfTheDay"].length;
        theAnimEndOfTheDay["EndOfTheDay"].speed = -1f;

        theAnimEndOfTheDay.Play("EndOfTheDay");
    }



    public void StartAnimGameOver()
    {
        panel_GameOver.SetActive(true);
        CancelInvoke("FinishAnimGameOver");
        gameOverAnim["GameOver"].speed = 1f;

        gameOverAnim.Play("GameOver");
        Invoke("PauseGame", gameOverAnim["GameOver"].length);
        //Invoke("FinishAnim", theAnimEndOfTheDay["EndOfTheDay"].length);
    }

    public void FinishAnimGameOver()
    {
        //MenuManager.instance.PauseGame();
        CancelInvoke("FinishAnimGameOver");
        gameOverAnim["GameOver"].time = gameOverAnim["GameOver"].length;
        gameOverAnim["GameOver"].speed = -1f;

        gameOverAnim.Play("GameOver");
        //Invoke("SwitchOff", (theAnimEndOfTheDay["EndOfTheDay"].length + 0.5f));
    }

    public void ReloadScene(string scene)
    {
        MenuManager.instance.UnpauseGame();
        MenuManager.instance.LoadScene(scene);
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

    private void SwitchOff()
    {
        gameObject.SetActive(false);
    }
}