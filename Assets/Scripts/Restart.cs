using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log ("Current timescale is " + Time.timeScale);
	}

	public void restartGame(){
		Time.timeScale = 1.0f;
		Application.LoadLevel(0);
	}
}
