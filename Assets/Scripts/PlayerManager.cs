using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance;

    public Image scoreUIPlaceHolder;
    public Image scoreUI;

    public Text moneyUI;
    public Image coinIcon;

    private float score;
    private float scoreMax;

    private int money;

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

        score = 2.6f;
        scoreMax = 5.0f;
        UpdateScore(0.0f);

        money = 100;
        UpdateMoney(0);
    }

    public void UpdateScore(float scoreToAdd)
    {
        score += scoreToAdd;
        float widthMax = scoreUIPlaceHolder.rectTransform.sizeDelta.x;
        float width = score * widthMax / scoreMax;

        Animation anim = scoreUIPlaceHolder.GetComponent<Animation>();
        anim.Play("ScoreAnimation");
        scoreUI.rectTransform.sizeDelta = new Vector2(width, scoreUI.rectTransform.sizeDelta.y);
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

    public void EarnMoney(int moneyEarned, Vector3 worldPosition)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        coinIcon.rectTransform.position = new Vector3(screenPos.x, screenPos.y);
        Debug.Log("money earned : " + moneyEarned);

        StartCoroutine(MoveCoinToUIPoint(coinIcon.rectTransform.position));

        money += moneyEarned;
        moneyUI.text = money.ToString();
    }

    public int GetMoney()
    {
        return money;
    }

    private IEnumerator MoveCoinToUIPoint(Vector3 currentPos)
    {
        Debug.Log("Start Coroutine");
        Debug.Log(Vector3.Distance(currentPos, moneyUI.rectTransform.position));
        while(Vector3.Distance(currentPos, moneyUI.rectTransform.position) > 0.05f)
        {
            Debug.Log(currentPos + " ; " + moneyUI.rectTransform.position);
            Vector3.Lerp(currentPos, moneyUI.rectTransform.position, 2 * Time.deltaTime);
            yield return null;
        }
        Debug.Log("target reached");

    }
}
