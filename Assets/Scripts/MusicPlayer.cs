using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
	private AudioSource themeMusic;
	// Use this for initialization
	void Start () {
		themeMusic = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.M)) {
			if (themeMusic.isPlaying) {
				themeMusic.Stop();
			}
			else{
				themeMusic.Play();
			}
		}

		GameObject p1 = GameObject.FindGameObjectWithTag ("Player 1");
		GameObject p2 = GameObject.FindGameObjectWithTag ("Player 2");
		if(p1 != null && p2 != null){
			if (p1.gameObject.GetComponent<ProjectileReceiver> ().buff == "Caffinated!" || p2.gameObject.GetComponent<ProjectileReceiver> ().buff == "Caffinated!") {
				themeMusic.pitch = 1.5f;
			} else {
				themeMusic.pitch = 1.0f;
			}
		}

	}
}