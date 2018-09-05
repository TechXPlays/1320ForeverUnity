using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RS_UserPanel : MonoBehaviour {

	public Image UserAvatar;
	public Text UserName;
	public Texture2D defaultAvatart;
	private Sprite defaultAvatrtSprite;


	private bool IsTextureSet = false;

	//--------------------------------------
	// Initialization
	//--------------------------------------

	void Awake() {

		defaultAvatrtSprite = Sprite.Create (defaultAvatart, new Rect (0, 0, defaultAvatart.width, defaultAvatart.height), new Vector2 (0.5f, 0.5f));

		UpdateUserPanel();
		UM_GameServiceManager.OnConnectionStateChnaged += HandleOnConnectionStateChnaged;
	}
								

	void OnDestroy() {
		UM_GameServiceManager.OnConnectionStateChnaged -= HandleOnConnectionStateChnaged;
	}

	void FixedUpdate() {
		if(UM_GameServiceManager.Instance.IsConnected && !IsTextureSet) {
			UpdateUserPanel();
		}
	}


	//--------------------------------------
	// Handlers
	//--------------------------------------


	void HandleOnConnectionStateChnaged (UM_ConnectionState state){
		if(UM_GameServiceManager.Instance.IsConnected) {
			UM_Player player = UM_GameServiceManager.Instance.Player;
			if(player.BigPhoto ==  null) {
				player.LoadBigPhoto();
			}

		}
		UpdateUserPanel();
	}
	


	//--------------------------------------
	// Private Methods
	//--------------------------------------


	private void UpdateUserPanel() {

		if(UM_GameServiceManager.Instance.IsConnected) {
			UM_Player player = UM_GameServiceManager.Instance.Player;
			UserName.text = player.Name;
			if(player.BigPhoto != null) {
				Debug.Log("RS_UserPanel User phto was set");
				UserAvatar.sprite = Sprite.Create (player.BigPhoto, new Rect (0, 0, player.BigPhoto.width, player.BigPhoto.height), new Vector2 (0.5f, 0.5f));
				IsTextureSet = true;
			} else {
				UserAvatar.sprite =  defaultAvatrtSprite;
				IsTextureSet = false;
			}
		} else {
			UserName.text = "Unknown User";
			UserAvatar.sprite =  defaultAvatrtSprite;
			IsTextureSet = false;
		}

	}








}
