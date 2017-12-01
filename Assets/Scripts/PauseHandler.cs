using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour {
	public Canvas pauseScreen;
	public Canvas gameUI;
	public bool paused;

	// Use this for initialization
	void Start () {
		paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Debug.Log ("PauseToggle");
			paused = !paused;
			if (paused) {
				Debug.Log ("PauseHandler: set timescale to 0");
				Time.timeScale = 0f;
				pauseScreen.enabled = true;
				gameUI.enabled = false;
			} else {
				Debug.Log ("PauseHandler: set timescale to 1");
				Time.timeScale = 1f;
				pauseScreen.enabled = false;
				gameUI.enabled = true;
			}
		}
	}
}
