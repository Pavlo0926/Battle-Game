// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;

namespace ControlFreak2.Demos.Guns
{
public class Bullet : MonoBehaviour 
	{
	private Gun
		gun;		// reference to a gun that fired this bullet.
	

	private Rigidbody
		rigidBody;

	public float
		maxLifetime = 5.0f;
	private float
		lifetime;


	public float 
		initialSpeed = 20,
		maxSpeed		= 30,
		accel			= 20;


	private float
		speed;


	// ------------------
	void OnEnable()
		{	
		this.rigidBody = this.GetComponent<Rigidbody>();
		}

	// --------------------
	public void Init(Gun gun)
		{
		this.gun = gun;
		this.lifetime = 0;
		this.speed = Mathf.Max(0, this.initialSpeed);
		}
		

	// ---------------
	public Gun GetGun()
		{ return this.gun; }

	
	// ---------------
	void FixedUpdate()
		{
		this.speed += Time.deltaTime * this.accel;
		if (this.speed > this.maxSpeed)
			this.speed = this.maxSpeed;


		Transform tr = this.transform;
		Vector3 newPos = (tr.position + ((tr.forward * this.speed) * Time.deltaTime));

		if (this.rigidBody != null)
			this.rigidBody.MovePosition(newPos);
		else
			tr.position = newPos;
		

		// Destroy this bullet if it didn't hit anything...

		if ((this.lifetime += Time.deltaTime) > this.maxLifetime)
			Destroy(this.gameObject);
		}


//	// ------------------
//	void OnTriggerEnter(Collider objectHit)
//		{
//		// TODO : explode, inflict damage, etc.
//		
//		if (this.gun != null)
//			{
//			// ...
//			}
//		
////#if UNITY_EDITOR
////		Debug.Log("Fr[" + Time.frameCount + "] bullet ["+this.name+"] hit [" + objectHit.name + "]!");
////#endif	
//	
//		// Destroy on impact.
//
//		Destroy(this.gameObject);
//		} 
	}
}
