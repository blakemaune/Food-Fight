using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggShoot : MonoBehaviour {
	private P1_MovementController p1;
	private P2_MovementController p2;
	public GameObject egg;

	// Use this for initialization
	void Start () {
		p1 = GetComponent<P1_MovementController> ();
		p2 = GetComponent<P2_MovementController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (p1.blocking || p2.blocking) {
			return;
		}
		if (gameObject.tag == "Player 1")
		{
			if(Input.GetKeyDown(KeyCode.F)){
				GameObject bullet = Instantiate (egg, gameObject.transform.position, gameObject.transform.rotation);
				bullet.tag = "Player1_Projectile";
				bullet.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.0f, -5.0f);
			}
		} 
		else if (gameObject.tag == "Player 2")
		{
			if(Input.GetKeyDown(KeyCode.RightShift)){
				GameObject bullet = Instantiate (egg, gameObject.transform.position, gameObject.transform.rotation);
				bullet.tag = "Player2_Projectile";
				bullet.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.0f, -5.0f);
			}
		}
	}
}
