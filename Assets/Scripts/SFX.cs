using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour {
	public AudioClip whoosh;
	public AudioClip punch;
	public AudioClip grunt;
	private AudioSource source;
	

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}

	public void stopSound(){
		source.Stop ();
	}

	public void playSound(AudioClip sound){
		if (!source.isPlaying){
			source.clip = sound;
			source.Play ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
