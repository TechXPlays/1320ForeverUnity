using UnityEngine;
using UnityEngine.SceneManagement;
using System;

using System.Collections;

public class RS_GamePlayController : MonoBehaviour {

	public static RS_GameMode Mode;
	public static UM_TBM_Match ActiveMatch;
	public static ST_TBM_MatchData ActiveMatchData;

	private RS_GhostCarManager _ghost;
	private RS_DragCarController _player;
	private MultiplayerGameController _netController = null;

	public event Action OnLevelStartedAction = delegate{};
	public event Action OnLevelFinishedAction = delegate{};
	public event Action OnPlayerSpawned = delegate{};

	private static RS_GamePlayController _instance;

	void Awake() {
		
	}

	void Start() {
		_ghost = gameObject.AddComponent<RS_GhostCarManager> ();

		RS_CarFactory.CarLoadedAction += RS_CarFactory_CarLoadedAction;
		if (Mode == RS_GameMode.Multiplayer) {
			ActiveMatchData = new ST_TBM_MatchData (ActiveMatch);
			RS_CarFactory.CreateCar(ActiveMatchData.CompetitorReplay != null ? ActiveMatchData.CompetitorReplay.CarId : RS_GameData.MenuCarId , RS_CarCreationMode.Garage);

			_netController = gameObject.AddComponent<MultiplayerGameController> ();
			#if !UNITY_EDITOR
			_netController.InitMatchData (ActiveMatch);
			#endif
		} else {
			RS_CarFactory.CreateCar(RS_GameData.MenuCarId, RS_CarCreationMode.Garage);
		}
	}

	void OnDestroy() {
		_instance = null;
	}

	public void SetupCar() {

		GameObject.FindObjectOfType<RS_GamePlayCamera>().FollowTarget = _player.transform;

		UIController.ButtonGasPressedEvent += delegate {
			_player.StepOnGas();
		};

		UIController.ButtonGasReleasedEvent += delegate() {
			_player.ReleaseGas();
		};

		UIController.ButtonGearUpEvent += GearUp;
		UIController.ButtonGearDownEvent += GearDown;
		UIController.ButtonRestartEvent += Restart;



		UIController.SetupUI(_player);
		UIController.StartTimer.StartCountDown();
		UIController.StartTimer.OnCountDownFinished += delegate {
			LaunchEngines();
		};

	}


	private RS_GameUIController _UIController = null;
	public  RS_GameUIController UIController {
		get {
			if(_UIController == null) {
				_UIController = GameObject.FindObjectOfType<RS_GameUIController>();
			}

			return _UIController;
		}
	}

	public static RS_GamePlayController Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType(typeof(RS_GamePlayController)) as RS_GamePlayController;
			}
			return _instance;
		}
	}

	public MultiplayerGameController Multiplayer {
		get {
			return _netController;
		}
	}

	public RS_GhostCarManager GhostManager {
		get { 
			return _ghost;
		}
	}

	public RS_DragCarController Player {
		get {
			return _player;
		}
	}

	private void LaunchEngines () {
		OnLevelStartedAction ();
		_player.Launch();
	}

	private void GearUp() {
		_player.ShiftUp();
	}

	private void GearDown() {
		_player.ShiftDown();
	}

	private void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void RS_CarFactory_CarLoadedAction (GameObject car) {
		RS_CarFactory.CarLoadedAction -= RS_CarFactory_CarLoadedAction;
		PlayerSpawnPoint spawnPoint = GameObject.FindObjectOfType<PlayerSpawnPoint>();
		TrackFinishPoint finishPoint = GameObject.FindObjectOfType<TrackFinishPoint> ();
		finishPoint.OnPointTriggered += delegate() {
			OnLevelFinishedAction();
			Debug.Log("!!!Drag Race Fuinished!!!");

			UIController.ButtonGoToMainMenuEvent += UIController_ButtonGoToMainMenuEvent;

			if (Mode == RS_GameMode.SinglePlayer) {
				UIController.ShowForSingleplayer(UM_GameServiceManager.Instance.Player,
					_ghost.IsNewRecord(),
					_ghost.GetCurrentTime(),
					_ghost.SavedDataExists(),
					_ghost.GetCurrentTime() - _ghost.GetRecordTime());
			} else {
				foreach (UM_TBM_Match match in TBM.Matchmaker.Matches) { //Just shitty lines of code. Refactor needed
					if (match.Id.Equals(ActiveMatch.Id)) {
						UIController.ShowForMultiplayer(match, _ghost.GetCurrentTime());
					}
				}
			}
		};

		_player = car.GetComponent<RS_DragCarController>();
		_player.transform.position = spawnPoint.transform.position;

		SetupCar();
		OnPlayerSpawned ();
	}

	private void UIController_ButtonGoToMainMenuEvent ()
	{
		RS_LevelLoader.LoadLevel (RS_Scene.RS_Garage);
	}

}
