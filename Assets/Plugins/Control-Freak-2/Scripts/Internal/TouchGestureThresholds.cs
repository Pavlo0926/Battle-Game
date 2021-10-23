// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

//! \cond

using UnityEngine;

namespace ControlFreak2.Internal
{
	
[System.Serializable]
public class TouchGestureThresholds 
	{


	public float
		tapMoveThreshCm,
		tapPosThreshCm,
		dragThreshCm,
		scrollThreshCm,
		scrollMagnetFactor,

		swipeSegLenCm,
		swipeJoystickRadCm,
	
		tapMaxDur,
		multiTapMaxTimeGap,

		longPressMinTime,
		longTapMaxDuration;

	
	private float
		_tapMoveThreshPx,
		_tapPosThreshPx,
		_dragThreshPx,
		_swipeSegLenPx,
		_swipeJoystickRadPx,

		_scrollThreshPx;

	private float
		storedDpi;
		
	public float tapMoveThreshPx 	{ get { return this._tapMoveThreshPx; } }
	public float tapMoveThreshPxSq 	{ get { return this._tapMoveThreshPx * this._tapMoveThreshPx; } }
	public float tapPosThreshPx 	{ get { return this._tapPosThreshPx; } }
	public float tapPosThreshPxSq 	{ get { return this._tapPosThreshPx * this._tapPosThreshPx; } }
	public float dragThreshPx		{ get { return this._dragThreshPx; } }
	public float dragThreshPxSq		{ get { return this._dragThreshPx * this._dragThreshPx; } }

	public float swipeSegLenPx{ get { return this._swipeSegLenPx; } }
	public float swipeSegLenPxSq{ get { return (this._swipeSegLenPx * this._swipeSegLenPx); } }
	public float swipeJoystickRadPx{ get { return this._swipeJoystickRadPx; } }
	public float swipeJoystickRadPxSq{ get { return (this._swipeJoystickRadPx * this._swipeJoystickRadPx); } }
	public float scrollThreshPx 	{ get { return this._scrollThreshPx; } }
	public float scrollThreshPxSq 	{ get { return this._scrollThreshPx * this._scrollThreshPx; } }


	// ---------------
	public TouchGestureThresholds()
		{
		this.storedDpi					= -1;

		this.tapMoveThreshCm 		= 0.1f;
		this._tapMoveThreshPx 		= 10;
		this.tapPosThreshCm 			= 0.5f;
		this._tapPosThreshPx 		= 30;

		this.dragThreshCm				= 0.25f;
		this._dragThreshPx			= 10;
			
		this.swipeSegLenCm			= 0.5f;
		this._swipeSegLenPx 			= 10;

		this.swipeJoystickRadCm 	= 1.5f;
		this._swipeJoystickRadPx 	= 15;

		this.scrollMagnetFactor		= 0.1f;
		this.scrollThreshCm			= 0.5f;
		this._scrollThreshPx		= 30;

		this.tapMaxDur 				= 0.3f;
		this.multiTapMaxTimeGap 	= 0.4f;
		this.longPressMinTime		= 0.5f;
		this.longTapMaxDuration		= 1.0f;
		}
		

	// ------------------
	public void Recalc(float dpi)
		{
		if (dpi <= 0.0001f)
			dpi = 96;

		if (this.storedDpi == dpi)
			return;

		this.storedDpi = dpi;
			
		this.OnRecalc((dpi / 2.54f));
		}

	// -------------------
	virtual protected void OnRecalc(float dpcm)
		{
		this._tapMoveThreshPx		= Mathf.Max(2, (this.tapMoveThreshCm	* dpcm));
		this._tapPosThreshPx		= Mathf.Max(2, (this.tapPosThreshCm 	* dpcm));
		this._dragThreshPx			= Mathf.Max(2, (this.dragThreshCm		* dpcm));
		this._swipeSegLenPx			= Mathf.Max(2, (this.swipeSegLenCm		* dpcm));
		this._swipeJoystickRadPx	= Mathf.Max(2, (this.swipeJoystickRadCm	* dpcm));
		this._scrollThreshPx		= Mathf.Max(2, (this.scrollThreshCm 	* dpcm));
		}
		

			
	}
	



// ----------------------
[System.Serializable]
public class MultiTouchGestureThresholds : TouchGestureThresholds
	{
	public float
		pinchDistThreshCm,
		pinchAnalogRangeCm,	
		//pinchAnalogDeadzone,
		pinchDeltaRangeCm,
		pinchDigitalThreshCm,
		pinchScrollStepCm,
		pinchScrollMagnet,

		twistMinDistCm,
		twistAngleThresh,
		twistAnalogRange,
		//twistAnalogDeadzone,
		twistDeltaRange,
		twistDigitalThresh,
		twistScrollStep,
		twistScrollMagnet,

		multiFingerTapMaxFingerDistCm;

	private float
		_pinchDistThreshPx,
		_pinchAnalogRangePx,
		_pinchDeltaRangePx,
		_pinchDigitalThreshPx,
		_pinchScrollStepPx,
		_twistMinDistPx,
		_multiFingerTapMaxFingerDistPx;
		
	
	public float pinchDistThreshPx 		{ get { return this._pinchDistThreshPx; } }
	public float pinchDistThreshPxSq 	{ get { return this._pinchDistThreshPx * this._pinchDistThreshPx; } }

	public float pinchAnalogRangePx		{ get { return this._pinchAnalogRangePx; } }
	public float pinchAnalogRangePxSq	{ get { return (this._pinchAnalogRangePx * this._pinchAnalogRangePx); } }

	public float pinchDeltaRangePx		{ get { return this._pinchDeltaRangePx; } }
	public float pinchDeltaRangePxSq	{ get { return (this._pinchDeltaRangePx * this._pinchDeltaRangePx); } }

	public float pinchDigitalThreshPx 	{ get { return this._pinchDigitalThreshPx; } }
	public float pinchDigitalThreshPxSq	{ get { return this._pinchDigitalThreshPx * this._pinchDigitalThreshPx; } }

	public float pinchScrollStepPx 		{ get { return this._pinchScrollStepPx; } }
	public float pinchScrollStepPxSq	{ get { return this._pinchScrollStepPx * this._pinchScrollStepPx; } }


	public float twistMinDistPx 	{ get { return this._twistMinDistPx; } }
	public float twistMinDistPxSq 	{ get { return this._twistMinDistPx * this._twistMinDistPx; } }
	

	public float multiFingerTapMaxFingerDistPx		{ get { return this._multiFingerTapMaxFingerDistPx; } }
	public float multiFingerTapMaxFingerDistPxSq	{ get { return (this._multiFingerTapMaxFingerDistPx * this._multiFingerTapMaxFingerDistPx); } }

		
	// --------------------
	public MultiTouchGestureThresholds() : base()
		{
		this.twistMinDistCm = 0.5f;
		this._twistMinDistPx = 2;
		this.twistAngleThresh = 2.0f;
		this.twistDigitalThresh = 25;	
		//this.twistAnalogDeadzone = 0.2f;

		this.twistAnalogRange = 45;
		this.twistDeltaRange = 90;
			
		//this.pinchAnalogDeadzone = 0.2f;
		this.pinchAnalogRangeCm = 1.5f;
		this._pinchAnalogRangePx = 30;

		this.pinchDeltaRangeCm = 2.5f;
		this._pinchDeltaRangePx = 60;

		this.pinchDistThreshCm = 0.2f;
		this._pinchDistThreshPx = 2;	
			
		this.pinchDigitalThreshCm = 1.0f;
		this._pinchDigitalThreshPx = 20;	

		this.pinchScrollStepCm = 0.5f;
		this._pinchScrollStepPx = 10;

		this.twistScrollStep = 30;

		this.multiFingerTapMaxFingerDistCm = 3.0f;
		this._multiFingerTapMaxFingerDistPx = 90;

		}


	// ---------------------
	override protected void OnRecalc(float dpcm)
		{
		base.OnRecalc(dpcm);

		this._pinchDistThreshPx		= Mathf.Max(2, this.pinchDistThreshCm * dpcm);
		this._pinchDigitalThreshPx	= Mathf.Max(2, this.pinchDigitalThreshCm * dpcm);
		this._twistMinDistPx 		= Mathf.Max(2, this.twistMinDistCm * dpcm);
		this._pinchAnalogRangePx 	= Mathf.Max(2, this.pinchAnalogRangeCm * dpcm);
		this._pinchDeltaRangePx 	= Mathf.Max(2, this.pinchDeltaRangeCm * dpcm);
		this._multiFingerTapMaxFingerDistPx = Mathf.Max(2, this.multiFingerTapMaxFingerDistCm * dpcm);
		this._pinchScrollStepPx 	= Mathf.Max(2, this.pinchScrollStepCm * dpcm);

		}
	

	}

}

//! \endcond
