// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;

namespace ControlFreak2.Demos.Guns
{
public class Gun : MonoBehaviour 
	{
	public ParticleSystem 
		shotParticles;
	
	public AudioClip		
		shotSound,
		emptySound,
		reloadSound;
	
	private bool
		isFiring;	
	public float
		shotInterval = 0.175f;	
		
	private float
		lastShotTime;	
		
	public bool		
		unlimitedAmmo	= false;
	public int 		
		bulletCapacity 	= 40,
		bulletCount		= 40;
	
	public Transform
		projectileOrigin;		// Transform where projectiles will be fired from.
	public Bullet
		bulletPrefab;			// Bullet prefab reference. 
	
	private AudioSource
		audioSource;
	
	
	// --------------------
	private void OnEnable()
		{
		this.audioSource = this.GetComponent<AudioSource>();
		
		this.isFiring = false;
		}
	
	
	// -----------------------
	public void SetTriggerState(bool fire)
		{	
		if (fire != this.isFiring)
			{
			this.isFiring = fire;
			
			if (fire)
				{
				// Fire first bullet...
	
				this.FireBullet();	
				}
			else
				{
				}
			}
		}
	
		
	
	// --------------------
	private void FixedUpdate()
		{
		if (this.isFiring)
			this.FireBullet();
		
		}
	
	 
	
	// ------------------	
	public void Reload()
		{
		this.bulletCount = this.bulletCapacity;
		
		if ((this.audioSource != null) && (this.reloadSound != null))
			{
			this.audioSource.loop = false;
			this.audioSource.PlayOneShot(this.reloadSound);
			}
		}
	
	
	// ---------------------
	private void FireBullet()
		{
		if ((Time.time - this.lastShotTime) >= this.shotInterval)
			{
			this.lastShotTime = Time.time;
		
	
			// Shoot...
				
			
			if (this.unlimitedAmmo || (this.bulletCount > 0))
				{
				if (!this.unlimitedAmmo)
					--this.bulletCount;	
	
				// Emit particles...
					
				if ((this.shotParticles != null) )
					{
					this.shotParticles.Play();
					}
		
				// Fire projectile.
		
				if ((this.projectileOrigin != null) && (this.bulletPrefab != null))
					{
					Bullet bullet = Instantiate(this.bulletPrefab, 
						this.projectileOrigin.position, this.projectileOrigin.rotation) as Bullet;
	
					if (bullet != null)
						bullet.Init(this); 
					} 
	
		
	
				// Play sound...
		
				if ((this.audioSource != null) && (this.shotSound != null)) // && (!this.shotSoundLooped))
					{	
					this.audioSource.loop = false;
					this.audioSource.PlayOneShot(this.shotSound);	
					}
				}
	
			// No bullets left!!
	
			else
				{
				// Play sound...
		
				if ((this.audioSource != null) && (this.emptySound != null)) // && (!this.emptySoundLooped))
					{	
					this.audioSource.loop = false;
					this.audioSource.PlayOneShot(this.emptySound);	
					}
				}
	
			}
		}
	
	}
}