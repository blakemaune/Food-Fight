using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ProjectileReceiver : MonoBehaviour {
	Rigidbody2D rb;
	CharTraits stats;
	public bool cooling = false;
	public float cooltime = 5.0f;
	public float coolRemain;
	private float ogWalkSpeed;
	private float ogJumpHeight;
	private Vector3 ogScale;
	public string buff;
	public Slider buffSlide;
	public Text bufferText;
	public bool flipped = false;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		stats = GetComponent<CharTraits> ();
		ogWalkSpeed = stats.walkspeed;
		ogJumpHeight = stats.jumpheight;
		// ogScale = gameObject.transform.localScale;
		buffSlide.maxValue = cooltime;
		buffSlide.value = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		float scaleModifier = 1.0f;
		bufferText.text = buff;
		if (cooling) {
			coolRemain -= Time.deltaTime;
			buffSlide.value = coolRemain;
			if (buff == "Spicy!") {
				GetComponent<HealthHandler> ().curHP *= 0.999f;
			}
			if (buff == "Dried!") {
//				gameObject.transform.localScale = new Vector3 (gameObject.transform.localScale.x, 0.3f, 1.0f);
//				GetComponent<Rigidbody2D> ().gravityScale = 0.5f;
			}
		}
		if (coolRemain <= 0f) {
			cooling = false;
			buff = "";
			rb.gravityScale = 1;


			if (flipped) {
				flipped = false;
				float xscale = rb.transform.localScale.x;
				float yscale = rb.transform.localScale.y;
				rb.transform.localScale = new Vector3 (-1.0f * xscale, yscale);
			}
			stats.walkspeed = ogWalkSpeed;
			stats.jumpheight = ogJumpHeight;
			gameObject.transform.localScale = new Vector3 (gameObject.transform.localScale.x, 0.6f, 1.0f);
			GetComponent<AudioSource> ().pitch = 1.0f;
			GetComponent<Rigidbody2D> ().gravityScale = 1.0f;

		}
	}

	void OnTriggerEnter2D(Collider2D other){
		bool blocking = false;
		if (tag == "Player 1") {
			blocking = GetComponent<P1_MovementController> ().blocking;
		} else if (tag == "Player 2") {
			blocking = GetComponent<P2_MovementController> ().blocking;
		}

		if (other.tag.Contains ("Projectile")) {
			string name = other.name.ToString ().Substring (0, (other.name.ToString ().Length) - 7);
			Debug.Log (gameObject.name + " hit by " + name);
			//CoffeeDrop, Jalapeno, Raisin, Takeout_Projectile, Wasabi, Cheese_Projectile

			if (blocking || (gameObject.tag == "Player 1" && other.tag == "Player1_Projectile") || (gameObject.tag == "Player 2" && other.tag == "Player2_Projectile")) {
				return;
			}

			if (name == "Egg") {
				GetComponent<HealthHandler> ().curHP -= 1.0f;
			}

			if (!cooling) {
				cooling = true;
				if (name == "CoffeeDrop") {
					buff = "Caffinated!";
					stats.walkspeed = 3.0f * ogWalkSpeed;
					stats.jumpheight = 2.0f * ogJumpHeight;
					coolRemain = cooltime;
					buffSlide.value = cooltime;

				}
				if (name == "Jalapeno") {
					buff = "Spicy!";
					coolRemain = cooltime;
					buffSlide.value = cooltime;
				}
				if (name == "Raisin") {
					// Shrinks size and blocks only take away half of damage
					buff = "Dried!";

					gameObject.GetComponent<AudioSource> ().pitch = 1.5f;
					gameObject.transform.localScale = new Vector3 (gameObject.transform.localScale.x, 0.3f, 1.0f);
					GetComponent<Rigidbody2D> ().gravityScale = 0.5f;
					stats.jumpheight = 0.5f * ogJumpHeight;
					coolRemain = cooltime;
				}
				if (name == "Takeout_Projectile") {
					buff = "Fat!";
					rb.gravityScale *= 3;
					coolRemain = cooltime;


//					Vector3 v = rb.velocity;
//					v.x = other.attachedRigidbody.velocity.x;
//					v.y = 15.0f;
//					rb.velocity = v;
				}
				if (name == "Wasabi") {
					buff = "Wasabi'd!";


					// GetComponent<SpriteRenderer> ().flipX = true;
					flipped = true;
					float xscale = rb.transform.localScale.x;
					float yscale = rb.transform.localScale.y;
					rb.transform.localScale = new Vector3 (-1.0f * xscale, yscale);


					coolRemain = cooltime;
				}
				if (name == "Cheese_Projectile") {

					buff = "Speed halfed!";
					stats.walkspeed = 0.5f * ogWalkSpeed;
					stats.jumpheight = 0.5f * ogJumpHeight;
					coolRemain = cooltime;
					buffSlide.value = cooltime;
				}
				Destroy (other.gameObject);
			} else {
				return;
			}
		} else {
			Debug.Log ("Collision happened, not a projectile!");
		}
	}
}
