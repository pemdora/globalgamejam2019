using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySprites : MonoBehaviour {

    public List<Sprite> tutoSprites;
    public GameObject objToActive;
    public Image img;
    private int index;

	// Use this for initialization
	void Start () {
        index = 0;
        img.sprite = tutoSprites[index];
        index++;
    }
	

    public void NextSlide()
    {
        if(index< tutoSprites.Count)
        {
            img.sprite = tutoSprites[index];
            index++;
        }
        else
        {
            index = 0;
            img.sprite = tutoSprites[index];
            index++;
            objToActive.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
