using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthHandler : MonoBehaviour {
	public CharTraits stats;
	public float maxHP = 100;
	public float curHP;
	public float damageModifier;
	public P1_MovementController p1MC;
	public P2_MovementController p2MC;
	public Slider healthSlider;
	public Canvas fightUI;
	public Canvas gameOverUI;
	public Text gameOverText;
	private float lastFrameHP;

	// Use this for initialization
	void Start () {
		stats = GetComponent<CharTraits> ();
		curHP = 100;
		lastFrameHP = curHP;
		healthSlider.maxValue = maxHP;
		// Debug.Log ("Health slider for " + gameObject.name + " is " + healthSlider);

		fightUI = GameObject.FindGameObjectWithTag ("GameHUD").GetComponent<Canvas>();
		gameOverUI = GameObject.FindGameObjectWithTag ("GameOverScreen").GetComponent<Canvas>();
		gameOverText = GameObject.FindGameObjectWithTag ("GameOverText").GetComponent<Text> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		
	}
	
	// Update is called once per frame
	void Update () {
		healthSlider.value = curHP;
		if (curHP <= 0) {
			GameObject.Find ("Timer").GetComponent<Timer> ().StopTimer ();
			fightUI.enabled = false;
			gameOverUI.enabled = true;
			if (gameObject.tag == "Player 1") {
				gameOverText.text = "Player 2 Wins!";
			}
			if (gameObject.tag == "Player 2") {
				gameOverText.text = "Player 1 Wins!";
			}
			// Destroy (gameObject, 1.0f);

			GetComponent<Renderer>().enabled=false;
			transform.localScale = .001f*Vector3.one;
			if (tag == "Player 1") {
				GetComponent<P1_MovementController> ().enabled = false;
			} else if (tag == "Player 2") {
				GetComponent<P2_MovementController> ().enabled = false;
			}


			// Time.timeScale = 0;
		}

		if (lastFrameHP != curHP) {
			GetComponent<SFX> ().stopSound ();
			GetComponent<SFX> ().playSound(GetComponent<SFX>().grunt);
			lastFrameHP = curHP;
		}
	}
}
