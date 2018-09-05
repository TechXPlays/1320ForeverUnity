using UnityEngine;
using System.Collections;

public class RS_RaceDataPackage : MNT_NetworkPackage {

	public float Time;


	public RS_RaceDataPackage():base(1) {

	}
	
	public RS_RaceDataPackage(byte[] data):base(data) {
		Time = ReadValue<float>();
	}


	public void BuildPackage() {
		WriteValue<float>(Time);
	}

}
