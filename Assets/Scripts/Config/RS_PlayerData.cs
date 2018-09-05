using System;
using UnityEngine;
using System.Collections.Generic;

public class RS_PlayerData : SA.Common.Pattern.NonMonoSingleton<RS_PlayerData> {
	
	private const string GHOST_DATA_BUNDLE = "GHOST_DATA_BUNDLE";
	private const string MULTIPLAYER_TICKETS = "MULTIPLAYER_TICKETS";
    private const string MULTIPLAYER_TICKETS_TIME = "MULTIPLAYER_TICKETS_TIME";
	private const string MULTIPLAYER_WINS = "MULTIPLAYER_WINS";
    private const string ADS_CD = "ADS_CD";

	public const int MAX_TICKETS = 90;
    private const int START_MULTIPLAYER_TICKETS = 90;
    private const int RECOVERY_TIME = 120;
    private const int ADS = 0;



	private IPlayerDataLoader _DataLoader = new RS_PrefsDataLoader ();
	private Dictionary<string, RS_CarTemplate> _Cars =  new Dictionary<string, RS_CarTemplate>();

	public RS_PlayerData () {
		LoadCarsConfigurations();
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	
	public bool HasGhostData(string track) {
		return _DataLoader.HasKey (GHOST_DATA_BUNDLE + track);
	}	
	
	public string GetGhostData(string track) {
		if (HasGhostData(track)) {
			return _DataLoader.GetString(GHOST_DATA_BUNDLE + track);
		}
		return string.Empty;
	}	
	
	public void SaveGhostData(string track, string data) {
		_DataLoader.SetString (GHOST_DATA_BUNDLE + track, data);
	}

	public bool HasKey(string key) {
		return _DataLoader.HasKey(key);
	}
	
	public void SetKey(string key) {
		_DataLoader.SetString(key, key);
	}

	public RS_CarTemplate GetCarTemplate(string CarId) {
		if(_Cars.ContainsKey(CarId)) {
			return _Cars[CarId];
		} else {
			return _Cars[RS_GameConfig.DEFAULT_CAR_ID];
		}
	}

	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public int MultiplayerTickets {
		get {
            int tmp;
            if (_DataLoader.HasKey(MULTIPLAYER_TICKETS))
                tmp = _DataLoader.GetInt(MULTIPLAYER_TICKETS);
            else 
                return START_MULTIPLAYER_TICKETS;
            if (tmp < MAX_TICKETS) {
                TimeSpan span = DateTime.Now - LastTimeMultiplayerTicket;
                tmp += (int)(span.TotalSeconds / RECOVERY_TIME);
                if (tmp < MAX_TICKETS) {
                    LastTimeMultiplayerTicket = DateTime.Now.AddSeconds(-(span.TotalSeconds % RECOVERY_TIME));
                } else {
                    LastTimeMultiplayerTicket = DateTime.MinValue;
                }
                MultiplayerTickets = tmp;
            }
            return _DataLoader.GetInt(MULTIPLAYER_TICKETS);
		}
		set {
            value = Mathf.Clamp(value, 0, MAX_TICKETS);
			_DataLoader.SetInt (MULTIPLAYER_TICKETS, value);
            if (value < MAX_TICKETS && LastTimeMultiplayerTicket == DateTime.MinValue)
                LastTimeMultiplayerTicket = DateTime.Now;
		}
	}

    public DateTime LastTimeMultiplayerTicket {
        get {
            if(_DataLoader.HasKey(MULTIPLAYER_TICKETS_TIME)) {
                return DateTime.Parse(_DataLoader.GetString(MULTIPLAYER_TICKETS_TIME));
            } else {
                return DateTime.MinValue;
            }
        }

        set {
            _DataLoader.SetString(MULTIPLAYER_TICKETS_TIME, value.ToString());
        }
    }

	public int MultiplayerWins {
		get { 
			return _DataLoader.HasKey(MULTIPLAYER_WINS) ? _DataLoader.GetInt(MULTIPLAYER_WINS) : 0;
		}
		set { 
			_DataLoader.SetInt (MULTIPLAYER_WINS, value);
		}
	}

    public int AdsCD {
        get {
            return _DataLoader.HasKey(ADS_CD) ? _DataLoader.GetInt(ADS_CD) : ADS;
        }
        set {
            if (value < 0)
                value = ADS;
            _DataLoader.SetInt(ADS_CD, value);
        }
    }

	public IPlayerDataLoader DataLoader {
		get {
			return _DataLoader;
		}
	}

	public Dictionary<string, RS_CarTemplate> Cars {
		get {
			return _Cars;
		}
	}

	public List<RS_CarTemplate> CarsList {
		get {
			List<RS_CarTemplate> _list =  new List<RS_CarTemplate>();

			foreach(KeyValuePair<string, RS_CarTemplate> pair in _Cars) {
				_list.Add(pair.Value);
			}

			return _list;
		}
	}




	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------

	public void LoadCarsConfigurations() {
		RS_CarTemplate car;

		car =  new RS_CarTemplate("Black_Car", 0);
		car.SetIndex(0);
		car.Title = "Ford Mustang";
		car.EngineValue = 375;
		car.GearBoxValue = 112;
		car.GripValue = 321;
        car.MaxRPM = 6750f;
		_Cars.Add(car.Id, car);

		car =  new RS_CarTemplate("Yellow_Car", 10);
		car.SetIndex(1);
		car.Title = "Chevy Camaro";
		car.EngineValue = 425;
		car.GearBoxValue = 386;
		car.GripValue = 320;
        car.MaxRPM = 7000f;
		_Cars.Add(car.Id, car);

		car =  new RS_CarTemplate("Red_Car", 20);
		car.SetIndex(2);
		car.Title = "Dodge Challenger";
		car.EngineValue = 777;
		car.GearBoxValue = 707;
		car.GripValue = 300;
        car.MaxRPM = 8000f;
		_Cars.Add(car.Id, car);

		car =  new RS_CarTemplate("White_Car", 30);
		car.SetIndex(3);
		car.Title = "Dodge Viper";
		car.EngineValue = 835;
		car.GearBoxValue = 743;
		car.GripValue = 456;
        car.MaxRPM = 6900f;
		_Cars.Add(car.Id, car);

		car =  new RS_CarTemplate("Yellow_FastCar", 40);
		car.SetIndex(4);
		car.Title = "Chevy Corvette";
		car.EngineValue = 950;
		car.GearBoxValue = 888;
		car.GripValue = 438;
        car.MaxRPM = 9000f;
		_Cars.Add(car.Id, car);
	}

}

public class aMaxRPM
{
    RS_CarTemplate car;
    private Dictionary<string, RS_CarTemplate> _Cars = new Dictionary<string, RS_CarTemplate>();
    public float MaxRPM
    {
        get { return car.MaxRPM; }
    }

    public int carID
    {
        get { return carID; }
    }

    public void GetMaxRPM()
    {
        var carID = 0;
            car = new RS_CarTemplate("Black_Car", 0);
            carID = 1;
            car.MaxRPM = 2000f;

            car = new RS_CarTemplate("Yellow_Car", 10);
            carID = 2;
            car.MaxRPM = 6900f;

            car = new RS_CarTemplate("Red_Car", 20);
            carID = 3;
            car.MaxRPM = 8000f;

            car = new RS_CarTemplate("White_Car", 30);
            carID = 4;
            car.MaxRPM = 6900f;

            car = new RS_CarTemplate("Yellow_FastCar", 40);
            carID = 5;
            car.MaxRPM = 8100f;
    }
}

