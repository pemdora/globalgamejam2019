using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour {

	public List<AudioClip> musicList;

    public static MusicManager instance = null;        
    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        
    }

    private void OnEnable(){
		SceneManager.sceneLoaded += ManageAudio;
	}

	private void OnDisable(){
		SceneManager.sceneLoaded -= ManageAudio;
	}

	public void SwapMusic(int music){
        /*
		//fade out current+fade in new music=
		fadeOut();
		GetComponent<AudioSource>().clip = musicList[music];
		GetComponent<AudioSource>().Play();
		fadeIn();*/
	}


	private void fadeIn() {
		if ( GetComponent<AudioSource>().volume < 1f) {
			GetComponent<AudioSource>().volume += 0.1f * Time.deltaTime;
		}
	}
	
	private void fadeOut() {
		if(GetComponent<AudioSource>().volume > 0.1f){
			GetComponent<AudioSource>().volume -= 0.1f * Time.deltaTime;
		}
	}

	private void ManageAudio(Scene scene, LoadSceneMode mode){
		//main menu
		if(scene.buildIndex == 0){
			SwapMusic (0);
		}
		//level selection
		else if(scene.buildIndex == 1){
			SwapMusic (1);
		}
		//Demo_environement
		else if(scene.buildIndex == 2){
			SwapMusic (2);
		}
		//quizz
		else if(scene.buildIndex == 3){
			SwapMusic (3);
		}
		//Lvl01
		else if(scene.buildIndex == 4){
			SwapMusic (4);
		}
		//Tutorial
		else if(scene.buildIndex == 5){
			SwapMusic (5);
		}
	}
}
