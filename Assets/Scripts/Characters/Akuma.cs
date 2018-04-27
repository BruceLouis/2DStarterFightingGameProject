﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akuma : MonoBehaviour {

	public GameObject projectile;
	public AudioClip hadoukenSound, shoryukenSound, flameSound;
	public AudioClip hurricaneKickSound, hadoukenCreatedSound;
	
	private Character character;
	private Animator animator; 
	private Rigidbody2D physicsbody;
	
	private int shoryukenType;
	private float hadoukenAngle;
	private bool hurricaneActive, diveKickActive;	
	
	// Use this for initialization
	void Start () {
		character = GetComponent<Character>();
		animator = GetComponent<Animator>();
		physicsbody = GetComponent<Rigidbody2D>();
		hurricaneActive = animator.GetBool("hurricaneKickActive");
		diveKickActive = animator.GetBool("diveKickActive");
	}
	
	// Update is called once per frame	
	void FixedUpdate (){
		hurricaneActive = animator.GetBool("hurricaneKickActive");
		diveKickActive = animator.GetBool("diveKickActive");
		if (hurricaneActive){
			physicsbody.isKinematic = true;
			if (character.GetRightEdgeDistance() < 0.5f && character.side == Character.Side.P1){ 			
				physicsbody.velocity = new Vector2(0f, physicsbody.velocity.y);
			}
			if (character.GetLeftEdgeDistance() < 0.5f && character.side == Character.Side.P2){	
				physicsbody.velocity = new Vector2(0f, physicsbody.velocity.y);
			}
		}
		else if (diveKickActive){
			physicsbody.isKinematic = true;
		}
		else{
			physicsbody.isKinematic = false;
		}
	}
	
	public void HadoukenRelease(){
		Vector3 offset = new Vector3(0.75f, 0f, 0f);
		GameObject hadouken = Instantiate(projectile);
		Rigidbody2D rigidbody = hadouken.GetComponent<Rigidbody2D>();
		SpriteRenderer hadoukenSprite = hadouken.GetComponentInChildren<SpriteRenderer>();		
//		AudioSource.PlayClipAtPoint(hadoukenCreatedSound, transform.position);
		HadoukenInitialize (offset, hadouken); 
		switch(animator.GetInteger("hadoukenPunchType")){
		case 0:
			if (character.side == Character.Side.P1){
				rigidbody.velocity = new Vector2(2.25f, 0f);
			}
			else {
				rigidbody.velocity = new Vector2(-2.25f, 0f);	
				hadoukenSprite.flipX = true;
			}
			break;
			
		case 1:
			if (character.side == Character.Side.P1){
				rigidbody.velocity = new Vector2(2.75f, 0f);
			}
			else {
				rigidbody.velocity = new Vector2(-2.75f, 0f);	
				hadoukenSprite.flipX = true;
			}
			break;
			
		default:
			if (character.side == Character.Side.P1){
				rigidbody.velocity = new Vector2(3.25f, 0f);
			}
			else {
				rigidbody.velocity = new Vector2(-3.25f, 0f);	
				hadoukenSprite.flipX = true;
			}
			break;
		}
		
	}
	
	public void AirHadoukenRelease(){
		Vector3 offset = new Vector3(0.75f, 0f, 0f);
		GameObject hadouken = Instantiate(projectile);
		Rigidbody2D rigidbody = hadouken.GetComponent<Rigidbody2D>();
		SpriteRenderer hadoukenSprite = hadouken.GetComponentInChildren<SpriteRenderer>();		
//		AudioSource.PlayClipAtPoint(hadoukenCreatedSound, transform.position);
		HadoukenInitialize (offset, hadouken); 
		switch(animator.GetInteger("hadoukenPunchType")){
		case 0:
			if (character.transform.localScale.x == 1){
				rigidbody.velocity = new Vector2(0.5f, -1.5f);
			}
			else {
				rigidbody.velocity = new Vector2(-0.5f, -1.5f);	
			}
			break;
			
		case 1:
			if (character.transform.localScale.x == 1){
				rigidbody.velocity = new Vector2(1f, -1.5f);
			}
			else {
				rigidbody.velocity = new Vector2(-1f, -1.5f);	
			}
			break;
			
		default:
			if (character.transform.localScale.x == 1){
				rigidbody.velocity = new Vector2(1.5f, -1.5f);
			}
			else {
				rigidbody.velocity = new Vector2(-1.5f, -1.5f);	
			}
			break;
		}
		//air hadouken angle calculated using tan2 		
		hadoukenAngle = Mathf.Atan2(rigidbody.velocity.y, rigidbody.velocity.x) * Mathf.Rad2Deg;
		hadouken.transform.rotation = Quaternion.AngleAxis(hadoukenAngle, Vector3.forward);
	}	

	public void AkumaShoryuken(){
		shoryukenType = animator.GetInteger("shoryukenPunchType");
		if (animator.GetBool("isLiftingOff") == false){				
			switch(animator.GetInteger("shoryukenPunchType")){
			case 0:
				character.MoveProperties(30f, 20f, 5f, 75f, 2, 3);
				break;
			case 1:
				character.MoveProperties(40f, 25f, 7.5f, 80f, 2, 3);
				break;
			default:
				character.MoveProperties(60f, 25f, 10f, 85f, 2, 3);
				break;
			}
		}
	}	
	
	public void ShoryukenLiftOff(){		
		switch(animator.GetInteger("shoryukenPunchType")){
		case 0:
			AkumaTakeOffVelocity(1f, 3f);
			break;
			
		case 1:			
			AkumaTakeOffVelocity(1.5f, 3.75f);
			break;
			
		default:	
			AkumaTakeOffVelocity(2f, 4.5f);			
			break;	
		}				
		AudioSource.PlayClipAtPoint(character.normalAttackSound,transform.position);
	}
	
	public void AkumaHurricaneKickLiftOff(){		
		AkumaTakeOffVelocity(1.25f, 1.25f);
	}
	
	public void AkumaHurricaneKickFloat(float timerHitStun){		
		physicsbody.velocity = new Vector2 (physicsbody.velocity.x, 0f);
		character.MoveProperties(timerHitStun, 20f, 10f, 30f, 2, 7);
	}	
	
	public void AkumaHurricaneLanding(){
		animator.SetBool("hurricaneKickActive", false);
	}
	
	public void AkumaHyakkishuLiftOff(){
		if (animator.GetBool("isAirborne") == false && animator.GetBool("isInHitStun") == false 
		    && animator.GetBool("isKnockedDown") == false && animator.GetBool("isInBlockStun") == false
		    && animator.GetBool("isThrown") == false && animator.GetBool("isMidAirRecovering") == false
		    && animator.GetBool("isLiftingOff") == true){			
			switch(animator.GetInteger("hyakkishuKickType")){
				case 0:
					AkumaTakeOffVelocity(1f, 4.5f);
					break;
				case 1:
					AkumaTakeOffVelocity(1.5f, 4.5f);
					break;
				default:
					AkumaTakeOffVelocity(2f, 4.5f);
					break;
			}
		}
	}
	
	public void AkumaHyakkiGozanProperties(){
		AkumaTakeOffVelocity(1.5f, 0f);
		character.MoveProperties(40f, 20f, 10f, 25f, 2, 1);
	}
	
	public void AkumaHyakkiGoshoProperties(){
		character.MoveProperties(40f, 20f, 10f, 30f, 1, 2);
	}
	
	public void AkumaDiveKickProperties(){
		AkumaTakeOffVelocity(0f, 0f);
		character.MoveProperties(40f, 20f, 10f, 20f, 2, 4);
	}
	
	public void AkumaDiveKick(){
		AkumaTakeOffVelocity(1.25f, -3.25f);
	}
	
	void HadoukenInitialize (Vector3 offset, GameObject hadouken){
		if (animator.GetInteger ("hadoukenOwner") == 1) {
			hadouken.gameObject.layer = LayerMask.NameToLayer ("ProjectileP1");
			hadouken.gameObject.tag = "Player1";
			hadouken.transform.parent = GameObject.Find ("ProjectileP1Parent").transform;
		}
		else {
			hadouken.gameObject.layer = LayerMask.NameToLayer ("ProjectileP2");
			hadouken.gameObject.tag = "Player2";
			hadouken.transform.parent = GameObject.Find ("ProjectileP2Parent").transform;
		}
		if (character.transform.localScale.x == 1) {
			hadouken.transform.position = transform.position + offset;
		}
		else {
			hadouken.transform.position = transform.position - offset;
		}
	}	
	
	void AkumaTakeOffVelocity (float x, float y){
		if (character.transform.localScale.x == 1) {
			physicsbody.velocity = new Vector2 (x, y);
		}
		else {
			physicsbody.velocity = new Vector2 (-x, y);
		}
	}	
	
}
