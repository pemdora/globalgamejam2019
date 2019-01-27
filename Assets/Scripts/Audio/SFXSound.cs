using System.Collections.Generic;
using UnityEngine;

public class SFXSound : MonoBehaviour {

	public List<AudioClip> theSound;

	private void Start () {

	}

	public void PlayTheSound(int i, float _pitch)
    {
        GetComponent<AudioSource>().clip = theSound[i];
        GetComponent<AudioSource>().pitch = _pitch;
        GetComponent<AudioSource>().Play();
	}
}
