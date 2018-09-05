using UnityEngine;
using System.Collections;

public class RS_GarageController : MonoBehaviour {

	public GameObject SpawnObject;

	private GameObject CurrentCar = null;
	private int CurrentCarIndex;


	//--------------------------------------
	// Initialization
	//--------------------------------------


	void Start() {
		RS_GameData.MenuCarId = RS_GameData.PlayerCarId;
		CurrentCarIndex =  RS_PlayerData.Instance.GetCarTemplate(RS_GameData.MenuCarId).Index; 

		if (UM_GameServiceManager.Instance.ConnectionSate == UM_ConnectionState.UNDEFINED) {
			UM_GameServiceManager.Instance.Connect ();
		}

		UpdateCar ();
	}


	void OnDestroy() {
		RS_CarFactory.CarLoadedAction -= RS_CarFactory_CarLoadedAction;
	}



	//--------------------------------------
	// Public Methods
	//--------------------------------------

	public void SpawnNextCar() {
		
		int NextIndex = CurrentCarIndex + 1;

		if(NextIndex  < RS_PlayerData.Instance.Cars.Count) {
			RS_CarTemplate tpl = RS_PlayerData.Instance.CarsList[NextIndex];
			RS_GameData.MenuCarId = tpl.Id;

			CurrentCarIndex = NextIndex;		
			UpdateCar ();
		}
	}

	public void SpawnPrevCar() {

		int PrevIndex = CurrentCarIndex - 1;

		if(PrevIndex < 0) {
			return;
		}

		RS_CarTemplate tpl = RS_PlayerData.Instance.CarsList[PrevIndex];


		RS_GameData.MenuCarId = tpl.Id;
		CurrentCarIndex = PrevIndex;
		UpdateCar ();
	}



	//--------------------------------------
	// Private Methods
	//--------------------------------------

	private void UpdateCar() {
		if(CurrentCar != null) {
			DestroyImmediate(CurrentCar);
		}

		RS_CarFactory.CarLoadedAction += RS_CarFactory_CarLoadedAction;
		RS_CarFactory.CreateCar(RS_GameData.MenuCarId, RS_CarCreationMode.Garage);

	}


	//--------------------------------------
	// Action Handlers
	//--------------------------------------

	void RS_CarFactory_CarLoadedAction (GameObject car) {

		RS_CarFactory.CarLoadedAction -= RS_CarFactory_CarLoadedAction;

		CurrentCar = car;

		CurrentCar.transform.parent = SpawnObject.transform;
		CurrentCar.transform.localPosition =  Vector3.zero;
		CurrentCar.transform.localRotation = Quaternion.identity;
	}
		
}
