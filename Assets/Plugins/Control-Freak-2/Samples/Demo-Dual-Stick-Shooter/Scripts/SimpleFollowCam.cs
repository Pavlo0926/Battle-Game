// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;

namespace ControlFreak2.Demos.Cameras
{
public class SimpleFollowCam : MonoBehaviour 
	{
	public Transform 
		targetTransform;
		
	public float 
		smoothingTime = 0.1f;

	private Vector3 
		targetOfs,
		smoothPosVel;

	private float
		camZoomFactor;

	public float 
		perspZoomOutOffset = 10.0f,	
		
		orthoZoomInSize	 	= 2,
		orthoZoomOutSize	= 5;

	public string
		camZoomDeltaAxis = "Cam-Zoom-Delta";


	private Camera 
		cam;

	// -------------------
	void OnEnable()
		{
		this.cam = this.GetComponent<Camera>();

		if ((this.cam != null) && (this.cam.orthographic))
			this.camZoomFactor = 0.5f;

		if (this.targetTransform != null)
			this.targetOfs = this.transform.position - this.targetTransform.position;
		}


	// ------------------
	void Update()
		{
		if (!string.IsNullOrEmpty(this.camZoomDeltaAxis))
			this.camZoomFactor += CF2Input.GetAxis(this.camZoomDeltaAxis);

		this.camZoomFactor = Mathf.Clamp01(this.camZoomFactor);
		}

	// --------------------
	void FixedUpdate()
		{
		if (this.targetTransform == null)
			return;

		Vector3 pos = 
			this.targetTransform.position + this.targetOfs;


		if ((this.cam != null) && cam.orthographic)
			{
			cam.orthographicSize = CFUtils.SmoothTowards(this.cam.orthographicSize, 
				Mathf.Lerp(this.orthoZoomInSize, this.orthoZoomOutSize, this.camZoomFactor), this.smoothingTime, CFUtils.realDeltaTimeClamped, 0.0001f);
			}
			
		else
			pos -= (this.transform.forward * (this.camZoomFactor * this.perspZoomOutOffset));
	
		if (this.smoothingTime > 0.001f)
			pos = Vector3.SmoothDamp(this.transform.position, pos, ref this.smoothPosVel, this.smoothingTime);

		this.transform.position = pos;
		}

	}
}
