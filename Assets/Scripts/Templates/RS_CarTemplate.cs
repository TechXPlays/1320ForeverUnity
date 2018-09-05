using UnityEngine;
using System.Collections;

public class RS_CarTemplate  {
	
	private string _Id;
	private int _Index = 0;
	private int _WinsToUnlock = 0;

	public string Title;
	public int EngineValue;
	public int GearBoxValue;
	public int GripValue;
    public float MaxRPM;


	//--------------------------------------
	// Initialization
	//--------------------------------------

	public RS_CarTemplate(string carId, int winsToUnlock) {
		_Id = carId;
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------


	public void SetIndex(int index) {
		_Index = index;
	}

	//--------------------------------------
	// Get / Set
	//--------------------------------------


	public string Id {
		get {
			return _Id;
		}
	}

	public int Index{
		get {
			return _Index;
		}
	}

	public int WinsToUnlock {
		get {
			return _WinsToUnlock;
		}
	}
}
