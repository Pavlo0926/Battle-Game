// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;

using ControlFreak2;

namespace ControlFreak2.Demos.Characters
{
public class DualStickShooterCharaMotor : MonoBehaviour 
	{
	public Transform
		cameraTransform;

	public string
		moveHorzAxis	= "Horizontal",
		moveVertAxis	= "Vertical",
		aimHorzAxis		= "Horizontal2",
		aimVertAxis		= "Vertical2";

	private Rigidbody
		rb;

	public float	
		forwardSpeed	= 10,
		strafeSpeed		= 6,	
		backwardSpeed	= 4;

	public float 
		turnSpeed	= 720.0f;

	public float
		orientationOffset = 0;

	public Vector3 
		gravity = new Vector3(0, -2.0f, 0);
		

	private Vector3
		worldVel,
		localDir;

	private float 
		moveTilt;
	
	
	private float
		worldMoveAngle,
		worldOrientAngle;	
		
	private float
		inputMoveAngle,
		inputMoveTilt,
		inputAimAngle,
		inputAimTilt;



	// ----------------
	void OnEnable()
		{
		this.worldOrientAngle = this.worldMoveAngle = GetAngleFromDir(this.transform.forward, WorldPlane.XZ, 0);
		this.rb = this.GetComponent<Rigidbody>();

		this.MoveOrientation(this.worldOrientAngle);

		}



		

	// --------------------
	private void MoveOrientation(float angle)
		{
		Quaternion q = Quaternion.AngleAxis(angle + this.orientationOffset, Vector3.up);

		if (this.rb != null)
			this.rb.MoveRotation(q);
		else
			this.transform.rotation = q;
		}
		

	// -------------------
	private void MovePosition(Vector3 worldPos)
		{
		if (this.rb != null)
			this.rb.MovePosition(worldPos);
		else
			this.transform.position = worldPos;
		
		}



	// -----------------
	void Update()
		{
		// Get input...						

		this.SetCameraRelativeInput(this.cameraTransform, 
			(string.IsNullOrEmpty(this.moveHorzAxis) ? 0 : CF2Input.GetAxis(this.moveHorzAxis)),
			(string.IsNullOrEmpty(this.moveVertAxis) ? 0 : CF2Input.GetAxis(this.moveVertAxis)),
			(string.IsNullOrEmpty(this.aimHorzAxis) ? 0 : CF2Input.GetAxis(this.aimHorzAxis)),
			(string.IsNullOrEmpty(this.aimVertAxis) ? 0 : CF2Input.GetAxis(this.aimVertAxis)));
		


		}

	// --------------------
	void FixedUpdate()
		{
		float deadzone = 0.01f;

			
		float 
			aimAngle = 0, 
			aimTilt = 0;

		if (this.inputAimTilt > deadzone)
			{
			aimAngle	= this.inputAimAngle;
			aimTilt		= this.inputAimTilt;
			}
		else if (this.inputMoveTilt > deadzone)
			{
			aimAngle 	= this.inputMoveAngle;
			aimTilt		= this.inputMoveTilt;
			}
			
			
		// Turn...

		if (aimTilt > 0)
			{
			this.worldOrientAngle = Mathf.MoveTowardsAngle(this.worldOrientAngle, aimAngle, aimTilt * this.turnSpeed * Time.deltaTime);
			}

		// Move...
	
		if (this.inputMoveTilt > deadzone)
			{
			this.worldMoveAngle = this.inputMoveAngle;
			this.moveTilt = this.inputMoveTilt;			
			}
		else
			{
			this.moveTilt = 0;
			}
			
		float relativeAngle = this.worldMoveAngle - this.worldOrientAngle;
 
		this.localDir = Quaternion.AngleAxis(relativeAngle, Vector3.up) *  (new Vector3(0, 0, 1)) ;//RotateVecOnPlane(new Vector3(0, 0, 1), relativeAngle, WorldPlane.XZ);
		this.localDir *= this.moveTilt;

		Vector3 localVel = this.localDir;
		
		localVel.x *= this.strafeSpeed;
		localVel.z *= (this.localDir.z >= 0) ? this.forwardSpeed : this.backwardSpeed;

		//this.worldVel = RotateVecOnPlane(localVel, -relativeAngle + this.worldMoveAngle, WorldPlane.XZ);
		this.worldVel = Quaternion.AngleAxis(-relativeAngle + this.worldMoveAngle, Vector3.up) * localVel ; //RotateVecOnPlane(localVel, -relativeAngle + this.worldMoveAngle, WorldPlane.XZ);
		

			
		Vector3 pos = this.transform.position;
			
		pos += this.worldVel * Time.deltaTime;
		pos += this.gravity * Time.deltaTime;

		this.MovePosition(pos);
		this.MoveOrientation(this.worldOrientAngle);
		}





	// ------------------
	public float GetOrientAngle()	{ return this.worldOrientAngle; }
	public Vector3 GetLocalDir()	{ return this.localDir; }

	// -----------------
	public void SetWorldInput(float moveAngle, float moveTilt, float aimAngle, float aimTilt)
		{
		this.inputMoveAngle	= moveAngle;
		this.inputMoveTilt	= moveTilt;
		this.inputAimAngle	= aimAngle;
		this.inputAimTilt	= aimTilt;
		}
		

	// ------------------
	public void SetCameraRelativeInput(Transform camTr, float moveX, float moveY, float aimX, float aimY)
		{
		float 
			moveAngle	= 0,
			moveTilt	= 0,
			aimAngle	= 0,
			aimTilt		= 0;

		moveAngle	= GetJoyAngleAndTilt(camTr, moveX, moveY, WorldPlane.XZ, out moveTilt);
		aimAngle	= GetJoyAngleAndTilt(camTr, aimX, aimY, WorldPlane.XZ, out aimTilt);
		
		this.SetWorldInput(moveAngle, moveTilt, aimAngle, aimTilt);
		}
	


	// --------------------
	public enum WorldPlane
		{
		XZ,
		XY,
		ZY
		}
		

	// ----------------------	
	static public float GetAngleFromDir(Vector3 dir, WorldPlane plane, float fallbackAngle)
		{
		Vector2 v = ((plane == WorldPlane.XY) ? new Vector2(dir.x, dir.y) : (plane == WorldPlane.XZ) ? new Vector2(dir.x, dir.z) : new Vector2(dir.z, dir.y));
		if (v.sqrMagnitude < 0.000001f)	
			{
			return fallbackAngle;
			}

		v.Normalize();
		
		return (Mathf.Rad2Deg * Mathf.Atan2(v.x, v.y));
		}



	


	// ----------------------
	// Transform 2d vector from camera space onto world plane...
	// -----------------------
	static public Vector3 GetWorldVecFromCamera(Transform camTr, float x, float y, WorldPlane plane)
		{
		Vector3 xDir;
		Vector3 yDir;

		switch (plane)
			{
			case WorldPlane.XZ :
				xDir = camTr.right;
				yDir = camTr.forward; 
				xDir.y = xDir.z;
				yDir.y = yDir.z;
				break;

			case WorldPlane.XY :
				xDir = camTr.right;
				yDir = camTr.up;
				xDir.z = 0;
				yDir.z = 0;
				break;

			case WorldPlane.ZY :
			default:
				xDir = camTr.forward;
				yDir = camTr.up;
				xDir.x = xDir.z;
				yDir.x = yDir.z;
				//xDir.x = 0;
				//yDir.x = 0;
				break;
			}

		xDir.z = 0;
		yDir.z = 0;

		xDir.Normalize();
		yDir.Normalize();


		return ((xDir * x) + (yDir * y));
		}


	// -------------------------
	/// Calculate joystick's angle and tilt...
	// -------------------------
	static public float GetJoyAngleAndTilt(Transform camTr, float x, float y, WorldPlane plane, out float tilt)
		{
		Vector2 inputv = new Vector2(x, y);
		if (inputv.sqrMagnitude < 0.000001f)	
			{
			tilt = 0;
			return 0;
			}

		Vector2 v = GetWorldVecFromCamera(camTr, x, y, plane);
		float d = v.magnitude;
		if (d < 0.0001f)
			{
			tilt = 0;
			return 0;
			}
		
		v /= d;
		
		tilt = Mathf.Min(d, 1.0f);
		
		return (Mathf.Rad2Deg * Mathf.Atan2(v.x, v.y));
		}
	

	// -----------------
	static public Vector3 AngleToWorldVec(float angle, WorldPlane plane)
		{
		angle *= Mathf.Deg2Rad;
		float sinv = Mathf.Sin(angle);
		float cosv = Mathf.Cos(angle);

		switch (plane)
			{
			case WorldPlane.XY : 
				return new Vector3(sinv, cosv, 0); 

			case WorldPlane.XZ : 	
				return new Vector3(sinv, 0, cosv); 

			case WorldPlane.ZY :
			default:
				return new Vector3(0, cosv, sinv); 
			}
		}


	// -------------------
	static public Vector3 RotateVecOnPlane(Vector3 v, float angle, WorldPlane plane)
		{
		angle *= Mathf.Deg2Rad;
		float sinv = Mathf.Sin(angle);
		float cosv = Mathf.Cos(angle);

		switch (plane)
			{
			case WorldPlane.XY : 
				return new Vector3(
					(v.x * cosv) - (v.y * sinv), 
					(v.x * sinv) + (v.y * cosv), 
					v.z); 

			case WorldPlane.XZ : 	
				return new Vector3(
					(v.x * cosv) - (v.z * sinv),  
					v.y, 
					(v.x * sinv) + (v.z * cosv)); 

			case WorldPlane.ZY :
			default:
				return new Vector3(
					v.x, 
					(v.z * sinv) + (v.y * cosv), 
					(v.z * cosv) - (v.y * sinv));  
			}
		}

	}
}
