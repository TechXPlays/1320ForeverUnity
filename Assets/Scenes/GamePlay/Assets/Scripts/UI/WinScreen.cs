using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScreen : MonoBehaviour {

	[SerializeField]
	private Text Car;

	[SerializeField]
	private Text Track;

	[SerializeField]
	private Text Tickets;

	[SerializeField]
	private Text Wins;

	[SerializeField]
	private WinScrennPlayer Winner;

	[SerializeField]
	private WinScrennPlayer Loser;

	[SerializeField]
	private Text NewRewardLabel;

	public void SetSingleplayer (UM_Player player, bool isNewRecord, float time, bool displayDelta, float delta) {
		NewRewardLabel.gameObject.SetActive (isNewRecord);
		Tickets.text = RS_PlayerData.Instance.MultiplayerTickets + "/" + RS_PlayerData.MAX_TICKETS;
		Wins.text = RS_PlayerData.Instance.MultiplayerWins.ToString();
		Car.text = RS_PlayerData.Instance.GetCarTemplate (RS_GameData.MenuCarId).Title;
		Track.text = "DESERT"; //Just the hardcode for now. Sorry...

		Winner.SetInfo (player, time, displayDelta, delta);
		Winner.gameObject.SetActive (true);
		Loser.gameObject.SetActive (false);

		gameObject.SetActive (true);
	}

	public void SetMultiplayer (UM_TBM_Match match, float localPlayerTime) {
		NewRewardLabel.gameObject.SetActive (false);
		Tickets.text = RS_PlayerData.Instance.MultiplayerTickets + "/" + RS_PlayerData.MAX_TICKETS;
		Wins.text = RS_PlayerData.Instance.MultiplayerWins.ToString();

		ST_TBM_MatchData data = new ST_TBM_MatchData (match);
		Car.text = RS_PlayerData.Instance.GetCarTemplate(data.MyCarId).Title;
		Track.text = data.TrackId;

		if (data.CompetitorReplay != null) {
			Winner.SetInfo (UM_GameServiceManager.Instance.GetPlayer (match.LocalParticipant.Playerid), localPlayerTime, true, localPlayerTime - data.CompetitorTime);
			Loser.SetInfo (UM_GameServiceManager.Instance.GetPlayer(match.Competitor.Playerid), data.CompetitorTime, false, data.CompetitorTime - localPlayerTime);
		} else {
			Winner.SetInfo (UM_GameServiceManager.Instance.GetPlayer (match.LocalParticipant.Playerid), localPlayerTime, false);

			if (match.Competitor != null) {
				Loser.SetInfo (UM_GameServiceManager.Instance.GetPlayer (match.Competitor.Playerid), data.CompetitorTime, false, data.CompetitorTime - localPlayerTime);
			} else {
				Loser.SetEmpty (null);
			}
		}

		Winner.gameObject.SetActive (true);
		Loser.gameObject.SetActive (true);

		gameObject.SetActive (true);
	}
}
