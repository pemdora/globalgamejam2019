using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance;

    public Image scoreUIPlaceHolder;
    public Image scoreUI;

    private float score;
    private float scoreMax;

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
    }

    public void UpdateScore(float scoreToAdd)
    {
        score += scoreToAdd;
        float widthMax = scoreUIPlaceHolder.rectTransform.sizeDelta.x;
        float width = score * widthMax / scoreMax;
        scoreUI.rectTransform.sizeDelta = new Vector2(width, scoreUI.rectTransform.sizeDelta.y);
    }

    public float GetScore()
    {
        return score;
    }

}
