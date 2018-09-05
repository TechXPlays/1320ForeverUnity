using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScrennPlayer : MonoBehaviour {

	[SerializeField]
	private Image PlayerAvatar;

	[SerializeField]
	private Text PlayerName;

	[SerializeField]
	private Text PlayerTime;

	[SerializeField]
	private Text DeltaTime;

	[SerializeField]
	private Image WinnerImage;

	[SerializeField]
	private Image LoserImage;

	UM_Player _participant;

	public void SetInfo(UM_Player participant, float time, bool displayDelta, float delta = 0.0f) {
		_participant = participant;
		if (_participant != null) {
			_participant.SmallPhotoLoaded += PhotoLoaded;
			if (_participant.SmallPhoto != null) {
				PhotoLoaded (_participant.SmallPhoto);
			} else {				
				_participant.LoadSmallPhoto ();
			}

			PlayerName.text = _participant.Name;
		} else {
			PlayerName.text = "Matching...";
		}

		PlayerTime.text = GetTimeFormated (time);

		string sign = delta >= 0.0f ? "+" : "-";
		DeltaTime.text = sign + GetTimeFormated (Mathf.Abs(delta));
		DeltaTime.gameObject.SetActive (displayDelta);

		DeltaTime.color = delta >= 0.0f ? Color.red : Color.green;

		WinnerImage.gameObject.SetActive (delta <= 0.0f);
		LoserImage.gameObject.SetActive (delta > 0.0f);
	}

	public void SetEmpty(UM_Player participant) {
		_participant = participant;
		if (_participant != null) {
			_participant.SmallPhotoLoaded += PhotoLoaded;
			if (_participant.SmallPhoto != null) {
				PhotoLoaded (_participant.SmallPhoto);
			} else {				
				_participant.LoadSmallPhoto ();
			}

			PlayerName.text = participant.Name;
		} else {
			PlayerName.text = "Matching...";
		}

		PlayerTime.text = "-:-:-";
		DeltaTime.gameObject.SetActive (false);
		WinnerImage.gameObject.SetActive (false);
		LoserImage.gameObject.SetActive (false);
	}

	private string GetTimeFormated (float time) {
		int minutes = (int) time / 60;
		int seconds = (int) time % 60;
		int miliseconds = (int)(time * 100 % 100);

		return string.Format ("{0:d2}:{1:d2}:{2:d3}", minutes, seconds, miliseconds);
	}

	private void PhotoLoaded(Texture2D tex) {
		if (_participant != null) {
			_participant.SmallPhotoLoaded += PhotoLoaded;
			if (tex != null) {
				PlayerAvatar.sprite = Sprite.Create (tex, new Rect (0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
			}
		}
	}
}