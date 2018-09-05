using UnityEngine;
using System;
using System.Collections;

public class RS_DragCarController : MonoBehaviour {

	public event Action<float> OnEngineLaunched 		= delegate {};
	public event Action<int, float> OnGearUp 			= delegate {};
	public event Action<int, float> OnGearDown 			= delegate {};
	public event Action<float> OnThrottlePress 			= delegate {};
	public event Action<float> OnThrottleRelease 		= delegate {};

	public Transform COM;
	public AnimationCurve TorgueCureve;
	public AnimationCurve PowerCurve;

	public RS_EnigineMaxTorgue Torgue;
	public RS_EnigineMaxHorsepower Horsepower;

	public float[] Gears;
    public float MaxRPM = 8000f;
    public float _Max_RPM = 8000f;

	public Transform Body;
	public WheelCollider[] Wheels;
	public float Multiplayer;

	private int _CurrentGear = 0;
	private float _CurrnetRPM = 1000;
	private float _LaunchRMP = 0f;
	private bool _IsLaunched = false;
	private bool _IsGasPressed = false;

	private RS_VisualWheel[] _visualWheels;

	private float _Time60 = 0f;
	private float _Time100 = 0f;
	private float _LaunchTime = 0f;
	private float[] VelocityPerGear;

	private const float UNDEFINED = -1f;

	//--------------------------------------
	// Initialization
	//--------------------------------------

	void Awake() {

		_visualWheels = gameObject.GetComponentsInChildren<RS_VisualWheel> ();

        //MaxRPM = 7300f;
        //RS_PlayerData max_rpm = new RS_PlayerData();
        aMaxRPM max_rpm = new aMaxRPM();
        max_rpm.GetMaxRPM();
        MaxRPM = max_rpm.MaxRPM;

		GetComponent<Rigidbody>().centerOfMass = COM.localPosition;
		VelocityPerGear =  new float[Gears.Length];
		for(int i = 0; i < VelocityPerGear.Length; i++) {
			VelocityPerGear[i] = UNDEFINED;
		}

		Torgue.MaxTorque = Torgue.MaxTorque * Multiplayer;
		Horsepower.MaxHorsepower = Horsepower.MaxHorsepower * Multiplayer;

		UpdatePowerCurve();

	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
		
	public void Launch() {
		_CurrentGear = 1;
		_LaunchRMP = _CurrnetRPM;

		_IsLaunched = true;
		_LaunchTime = Time.time;

		OnEngineLaunched (RaceTimer.Instance.GetTimeInTicks());
	} 

	public void ShiftUp() {
		_CurrentGear++;
		ProcessGirShift();

		OnGearUp (_CurrentGear, RaceTimer.Instance.GetTimeInTicks());
	}

	public void ShiftDown() {
		_CurrentGear--;
		ProcessGirShift();

		OnGearDown (_CurrentGear, RaceTimer.Instance.GetTimeInTicks());
	}
		

	public void StepOnGas() {
		_IsGasPressed = true;

		OnThrottlePress (RaceTimer.Instance.GetTimeInTicks());
	}

	public void ReleaseGas() {
		_IsGasPressed = false;

		OnThrottleRelease (RaceTimer.Instance.GetTimeInTicks());
	}


	//--------------------------------------
	// Get / Set
	//--------------------------------------



	public float Speed {
		get {
			return GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
		}
	}

	public float CurrentTorgue {
		get {
			if(CurrentGear == 1 && _CurrnetRPM < _LaunchRMP) {
				return GetTorgue(_LaunchRMP, _CurrentGear);
			} else {
				return GetTorgue(_CurrnetRPM, _CurrentGear);
			}

		}
	}

	public float CurrentGearRatio {
		get {
			if(_CurrentGear <= 0 ) {
				return 0;
			} else {
				return Gears[_CurrentGear - 1];
			}

		}
	}

	public int CurrentGear {
		get {
			return _CurrentGear;
		}
	}

	public float CurrnetRPM {
		get {
			return _CurrnetRPM;
		}
	}

    public float Max_RPM
    {
        get
        {
            return _Max_RPM;
        }
    }

	public bool IsLaunched {
		get {
			return _IsLaunched;
		}
	}

	public float Time60 {
		get {
			return _Time60;
		}
	}

	public float Time100 {
		get {
			return _Time100;
		}
	}
		

	private float CurrentVelocity {
		get {
			return GetComponent<Rigidbody>().velocity.z;
		} 

		set {
			GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, value);
		}
	}

	public RS_VisualWheel[] VisualWheels {
		get {
			return _visualWheels;
		}
	}

	//--------------------------------------
	// Physics Calculation
	//--------------------------------------


	void FixedUpdate() {

		if(!_IsLaunched) {
			IdleCarBehaviour();
		} else {
			LaunchedCarBehaviour();
		}


		if(_CurrnetRPM < 750f) {
			_CurrnetRPM = 750f;
		} 

		if(_CurrnetRPM > MaxRPM) {
			_CurrnetRPM = MaxRPM;
		}
	}

	private void IdleCarBehaviour() {
		if(_IsGasPressed) {
			_CurrnetRPM += 200;
		} else {
			_CurrnetRPM -= 100;
		}
	}

	private void LaunchedCarBehaviour() {
		ApplayTorgue(CurrentTorgue);
		float WheelsRMP =  Mathf.Clamp( Wheels[0].rpm * CurrentGearRatio * 2, 1000, MaxRPM);

		if(_CurrnetRPM > WheelsRMP ) {
			float diff = _CurrnetRPM - WheelsRMP;
			_CurrnetRPM = _CurrnetRPM - diff / 70f;
		} else {
			_CurrnetRPM = WheelsRMP;
		}

		TrackCarPerfomance();
		ApplaySpeedLimitation();

	}

	private void TrackCarPerfomance() {
		if(Speed >= 60 && _Time60 == 0f) {
			_Time60 = Time.time - _LaunchTime;
		}

		if(Speed >= 100 && _Time100 == 0f) {
			_Time100 = Time.time - _LaunchTime;
		}
	}

	private void ProcessGirShift() {
		_CurrentGear  = Mathf.Clamp(_CurrentGear, 1, Gears.Length);
		_CurrnetRPM =  Mathf.Clamp( Wheels[0].rpm * CurrentGearRatio * 2, 1000, MaxRPM);
	}		

	private void ApplaySpeedLimitation() {		

		if(_CurrnetRPM == MaxRPM && VelocityPerGear[CurrentGear - 1] == UNDEFINED) {
			VelocityPerGear[CurrentGear - 1] = CurrentVelocity;
		}

		float MaxVelocity = VelocityPerGear[CurrentGear - 1];
		if(MaxVelocity != UNDEFINED) {

			if(CurrentVelocity > MaxVelocity) {
				_CurrnetRPM -= 200f;
				CurrentVelocity -= 1f;
			}
		}

	}

	private void ApplayTorgue(float torgue) {
		foreach(WheelCollider w in Wheels) {
			w.motorTorque = torgue;
		}
	}

	private float GetTorgue(float rmp, int gear) {
		
		if(gear <= 0) {
			return 0f;
		} else {
			return TorgueCureve.Evaluate(rmp) * Gears[gear - 1];
		}

	}

	public void UpdatePowerCurve() {

		TorgueCureve =  new AnimationCurve();

		TorgueCureve.AddKey(new Keyframe(0, 0));
		TorgueCureve.AddKey(new Keyframe(Torgue.RPM, Torgue.MaxTorque));

		PowerCurve =  new AnimationCurve();

		PowerCurve.AddKey(new Keyframe(0, 0));
		PowerCurve.AddKey(new Keyframe(Horsepower.RPM, Horsepower.MaxHorsepower));

		float t = PowerCurve.Evaluate(5252);
		TorgueCureve.AddKey(new Keyframe(5252 ,t));

		TorgueCureve.AddKey(new Keyframe(1000, Torgue.MaxTorque  * 0.6f));
		TorgueCureve.AddKey(new Keyframe(8000, t / 1.5f));

	}
		
}
