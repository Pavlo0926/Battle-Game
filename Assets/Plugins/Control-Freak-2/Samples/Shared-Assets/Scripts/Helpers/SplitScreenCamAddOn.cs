// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;
using ControlFreak2;

namespace ControlFreak2.Demos.Helpers
{
[ExecuteInEditMode()]
public class SplitScreenCamAddOn : MonoBehaviour 
	{
	public float 
		rotationAngle;

	private Camera 
		cam;

	// --------------------
	void OnEnable()
		{	
		this.cam = this.GetComponent<Camera>();
		}

	void OnDisable()
		{
		if (this.cam != null)	
			this.cam.ResetProjectionMatrix();
		}

	// -------------------
	void LateUpdate()
		{
		if (this.cam == null)
			return;
			
		float
			w = this.cam.rect.width * Screen.width,
			h = this.cam.rect.height * Screen.height;

		Vector2 upVec = new Vector3(Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * this.rotationAngle)), Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * this.rotationAngle)));

		Vector2 orientedSize = new Vector2(((w * upVec.x) + (h * upVec.y)),  ((h * upVec.x) + (w * upVec.y)));

		float aspectRatio = (orientedSize.x / orientedSize.y);
	
		Matrix4x4 projMatrix;

		if (this.cam.orthographic)
			{
			projMatrix = Matrix4x4.Ortho(-orientedSize.x * this.cam.orthographicSize, orientedSize.x * this.cam.orthographicSize, 
				-orientedSize.y * this.cam.orthographicSize, orientedSize.y * this.cam.orthographicSize, this.cam.nearClipPlane, this.cam.farClipPlane);
			}
		else
			{
			projMatrix = Matrix4x4.Perspective(this.cam.fieldOfView, aspectRatio, this.cam.nearClipPlane, this.cam.farClipPlane); //this.cam.projectionMatrix =
			}
			
		if (this.preMult)
			projMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, this.rotationAngle), Vector3.one) * projMatrix;
		else
			projMatrix = projMatrix * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, this.rotationAngle), Vector3.one);
	
		this.cam.projectionMatrix = projMatrix;
		}

public bool preMult = true;

	}
}
