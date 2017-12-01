using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharTraits : MonoBehaviour {
	public float walkspeed;
	public float jumpheight;
	public GameObject projectile;
	public int maxHp;

	void Start(){
	}

	float setWalkSpeed(float w){
		this.walkspeed = w;
		return this.walkspeed;
	}

	float setJumpHeight(float h){
		this.jumpheight = h;
		return this.jumpheight;
	}

	GameObject setProjectile(GameObject p){
		this.projectile = p;
		return this.projectile;
	}

	float getWalkSpeed(){
		return this.walkspeed;
	}

	float getJumpHeight(){
		return this.jumpheight;
	}

	GameObject getProjectile(){
		return this.projectile;
	}
}
