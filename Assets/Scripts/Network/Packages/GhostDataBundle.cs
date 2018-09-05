using UnityEngine;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class GhostDataBundle : MNT_NetworkPackage {

	public sealed class EventType
	{
		public const int ThrottlePress = 0;
		public const int ThrottleRelease = 1;
		public const int GearUp = 2;
		public const int GearDown = 3;

		private EventType() {}
	}

	private Vector3 _position = Vector3.zero;
	private Vector3 _rotation = Vector3.zero;

	private float _throttle = 0.0f;

	private float _time = 0.0f;
	private int _eventType;

	private List<Vector3> _wheelsPositions = new List<Vector3> ();
	private List<float> _wheelsAngularSpeeds = new List<float> ();

	public GhostDataBundle(float time, int eventType) : base(2) {
		_time = time;
		_eventType = eventType;

		BuildPackage ();
	}

	public GhostDataBundle(byte[] data) : base(data){
		DecomposePackage ();
	}

	private void DecomposePackage() {
		_time = ReadValue<float> ();
		_eventType = ReadValue<int> ();
	}

	private void BuildPackage() {
		WriteValue<float> (_time);
		WriteValue<int> (_eventType);
	}

	public Vector3 Position {
		get {
			return _position;
		}
	}

	public Vector3 Rotation {
		get {
			return _rotation;
		}
	}

	public List<Vector3> WheelsPositions {
		get {
			return _wheelsPositions;
		}
	}

	public List<float> WheelsAngularSpeeds {
		get {
			return _wheelsAngularSpeeds;
		}
	}

	public float Throttle {
		get {
			return _throttle;
		}
	}

	public float Time {
		get {
			return _time;
		}
	}

	public int Event {
		get {
			return _eventType;
		}
	}

}
