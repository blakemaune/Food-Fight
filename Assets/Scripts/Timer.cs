using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour {
	public float startingTime;
	public float remainingTime;
	public bool running = false;
	public Text timeText;
	Canvas fightUI;
	Canvas gameOverUI;
	Text gameOverText;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1.0f;

		fightUI = GameObject.FindGameObjectWithTag ("GameHUD").GetComponent<Canvas>();
		gameOverUI = GameObject.FindGameObjectWithTag ("GameOverScreen").GetComponent<Canvas>();
		gameOverText = GameObject.FindGameObjectWithTag ("GameOverText").GetComponent<Text> ();
	}

	public void StartTimer(){
		running = true;
		remainingTime = startingTime;
	}
	public void StopTimer(){
		running = false;
	}
	public void ResumeTimer(){
		running = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (running && remainingTime > 0) {
			timeText.text = remainingTime.ToString("0.00");

			remainingTime -= Time.deltaTime;
		}
		if (remainingTime <= 0) {
			HealthHandler p1Health = GameObject.FindGameObjectWithTag ("Player 1").GetComponent<HealthHandler> ();
			HealthHandler p2Health = GameObject.FindGameObjectWithTag ("Player 2").GetComponent<HealthHandler> ();
			fightUI.enabled = false;
			gameOverUI.enabled = true;

			if (p1Health.curHP > p2Health.curHP) {
				gameOverText.text = "Player 1 Wins!";
			} else if (p1Health.curHP < p2Health.curHP) {
				gameOverText.text = "Player 2 Wins!";
			} else {
				gameOverText.text = "Tie!";
			}

			Time.timeScale = 0;
		}
	}
}
