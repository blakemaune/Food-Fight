using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public GameObject character;
	public GameObject[] characterArray;
	P1_MovementController p1Controls;
	P2_MovementController p2Controls;
	public Slider healthSlider_ref;
	public Slider bufferSlider;
	public Slider shieldSlider;
	public Slider specialSlider;
	public Text bufferText;

	// Use this for initialization
	void Start () {
		
	}

	public void spawnCharacter(){
		GameObject g = Instantiate (character, gameObject.transform.position, gameObject.transform.rotation);
		p1Controls = g.GetComponent<P1_MovementController> ();
		p2Controls = g.GetComponent<P2_MovementController> ();
		ProjectileReceiver pr = g.GetComponent<ProjectileReceiver> ();
		HealthHandler hh = g.GetComponent<HealthHandler> ();

		if (gameObject.name == "P1Spawner") {
			p1Controls.enabled = true;

			p1Controls.specialSlider = specialSlider;
			p1Controls.shieldSlider = shieldSlider;

			p2Controls.enabled = false;
			Destroy(g.GetComponent<P2_MovementController>());
		}
		if (gameObject.name == "P2Spawner") {
			p1Controls.enabled = false;

			p2Controls.specialSlider = specialSlider;
			p2Controls.shieldSlider = shieldSlider;

			p2Controls.enabled = true;
			Destroy(g.GetComponent<P1_MovementController>());
		}
		pr.buffSlide = bufferSlider;
		pr.bufferText = bufferText;
		hh.healthSlider = healthSlider_ref;
	}

	public void setCharacter(GameObject g){
		character = g;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
