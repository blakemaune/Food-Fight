using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Handler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Abs(gameObject.transform.position.x) > 10){
			Destroy(gameObject);
		}

		if (gameObject.transform.position.y <= -5.0f) {
			Debug.Log ("Egg should disappear now");
			Destroy (gameObject);
		}
		
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log (gameObject.name + " hit " + other.name);
		if(other.gameObject.tag == "Boundary"){
			// May not be working because boundary is not a trigger
			Destroy (gameObject);
		}
		if(gameObject.tag == "Player1_Projectile" && other.gameObject.tag == "Player 2"){
			// May not be working because boundary is not a trigger
			Debug.Log("SHOULD DISAPPEAR NOW");
			Destroy (gameObject);
		}
		if (gameObject.tag == "Player2_Projectile" && other.gameObject.tag == "Player 1") {
			// May not be working because boundary is not a trigger
			Debug.Log ("SHOULD DISAPPEAR NOW");
			Destroy (gameObject);
		}
	}
}
