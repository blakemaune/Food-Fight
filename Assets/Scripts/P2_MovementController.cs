using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class P2_MovementController : MonoBehaviour {
	private bool facingRight = true;
	public bool attacking = false;
	public bool blocking = false;
	public float blockTimeRem = 2.5f;
	public float blockCoolDown = 0.0f;
	public float maxSpeed = 5.0f;
	public float jumpHeight;
	private bool projectile_cooldown = false;
	public float cooldown_time;
	public float projectile_speed;
	CharTraits stats;
	Animator anim;
	Rigidbody2D rb;
	public GameObject projectile;
	public GameObject p1;
	HealthHandler p1Health;
	public Slider shieldSlider;
	public Slider specialSlider;
	public float move;


	// Use this for initialization
	void Start () {
		gameObject.tag = "Player 2";
		FlipFacing ();
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		stats = GetComponent<CharTraits> ();
		maxSpeed = stats.walkspeed;
		jumpHeight = stats.jumpheight;
		projectile = stats.projectile;
		shieldSlider.maxValue = 5.0f;
		specialSlider.maxValue = 5.0f;
		specialSlider.value = specialSlider.maxValue;

		p1 = GameObject.FindGameObjectWithTag ("Player 1");
		if (p1 != null) {
			Debug.Log ("p1 isn't null");
			if (p1.gameObject.GetComponent<P1_MovementController> ().p2 == null) {
				Debug.Log ("Trying to manually set p1 through p2");
				p1.gameObject.GetComponent<P1_MovementController> ().p2 = gameObject;
				if (p1.gameObject.GetComponent<P1_MovementController> ().p2 == gameObject) {
					Debug.Log ("Successfully set p1's reference to p2. Manually resetting p1's p2Health");
					p1.gameObject.GetComponent<P1_MovementController> ().p2Health = gameObject.GetComponent<HealthHandler> ();
				}
			}
			p1Health = p1.GetComponent<HealthHandler> ();
			Debug.Log ("p1 health is " + p1Health);
		} else {
			Debug.Log ("p1 is null!");
		}

		// TOTAL BULL SHIT WORK AROUND
		p1.GetComponent<P1_MovementController>().p2 = gameObject;


		foreach (Transform child in transform) {
			GameObject g = (GameObject)(child.gameObject);
			// Debug.Log ("Found game object " + g.name + " in P2");


			g.tag = "Player 2 ext";
			foreach (Transform grandchild in g.transform) {
				grandchild.gameObject.tag = "Player 2 ext";
			}
		}
	}

	// Update is called once per frame
	void Update () {
		maxSpeed = stats.walkspeed;
		jumpHeight = stats.jumpheight;

		// TODO: fix characters tipping over
		if (Mathf.Abs (gameObject.transform.rotation.z) >= 30f) {
			rb.rotation = 0f;
		}
	}

	void FixedUpdate() {
		move = Input.GetAxis ("Horizontal_P2");

		// Tell animator we're moving
		anim.SetFloat ("Speed", Mathf.Abs (move));

		// Move
		rb.velocity = new Vector2 (maxSpeed * move, rb.velocity.y);

		// Jump
		if (Input.GetKeyDown(KeyCode.UpArrow) && !blocking) {
			rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
		}

		// Flip, if needed
		if ((facingRight && Input.GetKey (KeyCode.LeftArrow)) && !Input.GetKey(KeyCode.RightArrow) || (!facingRight && Input.GetKey (KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))) {
			FlipFacing ();
		}

		// Melee Attack
		if (Input.GetKey (KeyCode.RightShift) && !blocking) {
			anim.SetBool ("Punching", true);
			GetComponent<SFX> ().playSound(GetComponent<SFX>().whoosh);
			attacking = true;
		} else {
			anim.SetBool ("Punching", false);
			attacking = false;
		}

		// Projectile Attack
		if(Input.GetKey(KeyCode.Return) && !blocking) {
			if (!projectile_cooldown) {
				shoot ();
			}
		}
		if (projectile_cooldown) {
			cooldown_time -= Time.deltaTime;
			if (cooldown_time <= 0f) {
				projectile_cooldown = false;
				specialSlider.value = specialSlider.maxValue;
			} else {
				specialSlider.value = 1/cooldown_time;
			}
		}

		// Block
		if (Input.GetKey (KeyCode.DownArrow) && blockTimeRem > 0.0f && blockCoolDown <= 0) {
			blockTimeRem -= Time.deltaTime;
			shieldSlider.value = blockTimeRem;

			Debug.Log ("Player 2 blocking");
			anim.SetBool ("Punching", false);
			blocking = true;
			GetComponent<SpriteRenderer> ().color = new Color (20f, 188f, 255f, 0.65f);
			rb.velocity = new Vector2 (0.0f, rb.velocity.y);

		}else {
			if (blockTimeRem <= 0) {
				blockCoolDown = 5.0f;
			}
			if (blockTimeRem <= 5.0f) {
				blockTimeRem += (0.5f * Time.deltaTime);
				shieldSlider.value = blockTimeRem;
			}
			if (blockCoolDown >= 0){
				blockCoolDown -= Time.deltaTime;
				shieldSlider.value = 0f;
			}


			blocking = false;
			GetComponent<SpriteRenderer> ().color = new Color (256f, 256f, 256f);
		}

	}

	public void FlipFacing(){
		facingRight = !facingRight;
		Vector3 charScale = transform.localScale;
		charScale.x *= -1;
		transform.localScale = charScale;
	}

	void shoot(){
		projectile_cooldown = true;
		cooldown_time = 5.0f;
		specialSlider.value = cooldown_time;

		Debug.Log ("Should shoot projectile now");
		float facing;
		if (rb.transform.localScale.x >= 0) {
			//Character is facing forward
			facing = 1.0f;

		} else {
			//Character is facing backward
			facing = -1.0f;
		}
		GameObject bullet = Instantiate (projectile, gameObject.transform.position, gameObject.transform.rotation);
		Debug.Log ("P2 shot projectile " + bullet.name.ToString());
		bullet.tag = "Player2_Projectile";
		float vel = facing * (projectile_speed + Mathf.Abs (rb.velocity.x));
		if (facing == -1.0f) {
			bullet.GetComponent<SpriteRenderer> ().flipX = true;
		}
		bullet.GetComponent<Rigidbody2D> ().velocity = new Vector2 (vel, 0.0f);
	}

	void OnCollisionEnter2D(Collision2D c) {
		Collider2D other = c.collider;
		bool otherBlocking = p1.GetComponent<P1_MovementController> ().blocking;
		if (otherBlocking) {
			if (p1.GetComponent<ProjectileReceiver> ().buff == "Dried!") {
				Debug.Log ("Blocked while shrunk");
				p1Health.curHP -= 2.5f;
			}
			Debug.Log("Punch blocked!");
		}
		else if ((other.gameObject.tag == "Player 1" || other.gameObject.tag == "Player 1 ext") && attacking) {
			Debug.Log ("Player 2 attacked Player  1");

			p1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (5.0f, 0f);

			// Temporary way of punches pushing player back
			float facing = -1.0f * p1.transform.localScale.x;
			float x = p1.transform.position.x;
			x += facing * 1.0f; // BLOWBACK DISTANCE
			if (Mathf.Abs (x) <= 6.0f) {
				p1.transform.position = new Vector3(x, p1.transform.position.y);
			}
			// Temporary way of punches pushing player back

			if (p1Health != null) {
				p1Health.curHP -= 5.0f;
				GetComponent<SFX> ().stopSound();
				GetComponent<SFX> ().playSound(GetComponent<SFX>().punch);
			} else {
				Debug.Log ("Hit detected, but p1health is null");
			}
		}
	}
}
