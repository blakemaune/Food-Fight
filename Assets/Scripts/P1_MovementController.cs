using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class P1_MovementController : MonoBehaviour {
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
	GameObject projectile;
	public HealthHandler p2Health;
	public Slider shieldSlider;
	public Slider specialSlider;
	public Slider healthSlider;
	public GameObject p2;
	public float move;


	// Use this for initialization
	void Start () {
		gameObject.tag = "Player 1";
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		stats = GetComponent<CharTraits> ();
		maxSpeed = stats.walkspeed;
		jumpHeight = stats.jumpheight;
		projectile = stats.projectile;
		shieldSlider.maxValue = 5.0f;
		specialSlider.maxValue = 5.0f;
		specialSlider.value = specialSlider.maxValue;

		p2 = GameObject.FindGameObjectWithTag ("Player 2");
		if (p2 != null) {
			Debug.Log ("p2 isn't null");
			p2Health = p2.GetComponent<HealthHandler> ();

			Debug.Log ("p2 health is " + p2Health);
		} else {
			Debug.Log ("p2 is null!");
		}
		//blockCoolDown = 0.0f;

		foreach (Transform child in transform) {
			GameObject g = (GameObject)(child.gameObject);
			// Debug.Log ("Found game object " + g.name + " in P1");


			g.tag = "Player 1 ext";
			foreach (Transform grandchild in g.transform) {
				grandchild.gameObject.tag = "Player 1 ext";
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
		move = Input.GetAxis ("Horizontal_P1");

		// Tell animator we're moving
		anim.SetFloat ("Speed", Mathf.Abs (move));

		// Move
		rb.velocity = new Vector2 (maxSpeed * move, rb.velocity.y);

		// Jump
		if (Input.GetKeyDown(KeyCode.W) && !blocking) {
			rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
		}

		// TODO: Determine fair way of doing blocking
		// Block
		if (Input.GetKey (KeyCode.S) && blockTimeRem > 0.0f && blockCoolDown <= 0) {
			blockTimeRem -= Time.deltaTime;
			shieldSlider.value = blockTimeRem;

			Debug.Log ("Player 1 blocking");
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

		// Flip, if needed
		if ((facingRight && Input.GetKey (KeyCode.A)) && !Input.GetKey(KeyCode.D) || (!facingRight && Input.GetKey (KeyCode.D) && !Input.GetKey(KeyCode.A))) {
			FlipFacing ();
		}

		// Melee Attack
		if (Input.GetKey (KeyCode.F) && !blocking) {
			anim.SetBool ("Punching", true);
			GetComponent<SFX> ().playSound(GetComponent<SFX>().whoosh);
			attacking = true;
		} else {
			anim.SetBool ("Punching", false);
			attacking = false;
		}

		// Projectile Attack
		if(Input.GetKeyDown(KeyCode.G) && !blocking) {
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
		Debug.Log ("P1 shot projectile " + bullet.name.ToString().Substring(0,(bullet.name.ToString().Length)-7));
		bullet.tag = "Player1_Projectile";
		float vel = facing * (projectile_speed + Mathf.Abs (rb.velocity.x));
		Debug.Log ("Bullet fired with velocity " + vel);
		bullet.transform.Rotate(new Vector3(facing,0f,0f));
		bullet.GetComponent<Rigidbody2D> ().velocity = new Vector2 (vel, 0.0f);
	}

	void OnCollisionEnter2D(Collision2D c) {
		Collider2D other = c.collider;

		bool otherBlocking = p2.GetComponent<P2_MovementController> ().blocking;
		if (otherBlocking) {
			if (p2.GetComponent<ProjectileReceiver> ().buff == "Dried!") {
				p2Health.curHP -= 2.5f;
				Debug.Log ("Blocked while shrunk");
			}
			Debug.Log("Punch blocked!");
		}
		else if ((other.gameObject.tag == "Player 2" || other.gameObject.tag == "Player 2 ext") && attacking) {
			Debug.Log ("Player 1 attacked Player  2");

			p2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (5.0f, 0f);


			float facing = -1.0f * p2.transform.localScale.x;
			float x = p2.transform.position.x;
			x += facing * 1.0f; // BLOWBACK DISTANCE
			if (Mathf.Abs (x) <= 6.0f) {
				p2.transform.position = new Vector3(x, p2.transform.position.y);
			}

			if (p2Health != null) {
				p2Health.curHP -= 5.0f;
				GetComponent<SFX> ().stopSound();
				GetComponent<SFX> ().playSound(GetComponent<SFX>().punch);
			} else {
				Debug.Log ("Hit detected, but p2health is null. Attempting to reset");
				p2Health = p2.gameObject.GetComponent<HealthHandler> ();
				if (p2Health != null) {
					Debug.Log ("p2Health no longer null");
					p2Health.curHP -= 5.0f;
				}
			}
		}
	}
}
