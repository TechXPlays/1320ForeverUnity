using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RS_MatchesListPanel : MonoBehaviour {



	private List<RS_TBM_MatchUI> NodesList = new List<RS_TBM_MatchUI>();


	public ScrollRect MatchesScroll;
	public Texture2D DefaultOponentIcon;

	//--------------------------------------
	// Initialization
	//--------------------------------------


	void Awake() {

		TBM.Matchmaker.MatchesListUpdated  += HandleMatchesListUpdated;


		UM_GameServiceManager.OnPlayerConnected += HandleOnPlayerConnected;

		if(UM_GameServiceManager.Instance.IsConnected) {
			HandleOnPlayerConnected();
		}


	}



	
	void OnDestroy() {
		TBM.Matchmaker.MatchesListUpdated -= HandleMatchesListUpdated;

		UM_GameServiceManager.OnPlayerConnected -= HandleOnPlayerConnected;
	}


	//--------------------------------------
	// Handlers
	//--------------------------------------

	

	void HandleMatchesListUpdated () {
		UpdateMatceshUI();
	}


	void HandleOnPlayerConnected () {
		Debug.Log("Player commected loading matches");
		TBM.Matchmaker.LoadMatchesInfo ();
	}


	//--------------------------------------
	// Private Methods
	//--------------------------------------




	private RS_TBM_MatchUI CreateContent(int index) {
		RS_TBM_MatchUI matchUI = RS_AssetsLoader.LoadGameObject (RS_TBM_MatchUI.AssetName).GetComponent<RS_TBM_MatchUI>();

		matchUI.transform.SetParent(MatchesScroll.content.transform);
		matchUI.transform.localScale = Vector3.one;



		
		RectTransform PanelRect = matchUI.GetComponent<RectTransform> ();
		PanelRect.anchoredPosition = new Vector2 (0f, index * (-PanelRect.rect.height)  );
		
		NodesList.Add (matchUI);

		return matchUI;

	}
	

	private void UpdateMatceshUI () {
		DestroyContent ();
		DiscardPositions ();

		Debug.Log("TBM.Matchmaker.Matches.Count: "+ TBM.Matchmaker.Matches.Count);
		Debug.Log("TBM.Matchmaker.Invitations.Count: "+ TBM.Matchmaker.Invitations.Count);
		
		int index = 0;

		foreach(UM_TBM_Invite invite in TBM.Matchmaker.Invitations) {
			RS_TBM_MatchUI ui = CreateContent(index);
			ui.SetInvite(invite);
			index += 1;
		}
		
		foreach(UM_TBM_Match match in TBM.Matchmaker.Matches) {
			RS_TBM_MatchUI ui = CreateContent(index);
			ui.SetMatch(match);
			index += 1;
		}
		
		SetPositionAndResize ();
	}

	private void SetPositionAndResize() {
		if (NodesList.Count > 0) {

			RectTransform RectTr = MatchesScroll.content;
			
			float ElementHeight = NodesList[0].GetComponent<RectTransform>().rect.height;
			float PanelHeight = ElementHeight * NodesList.Count;
			
			RectTr.sizeDelta = new Vector2(RectTr.sizeDelta.x, PanelHeight);
		}
	}

	private void DiscardPositions() {
		RectTransform RectTr = MatchesScroll.content;
		RectTr.sizeDelta = new Vector2(0f, MatchesScroll.GetComponent<RectTransform>().rect.height);
	}


	private void DestroyContent() {
		foreach(RS_TBM_MatchUI matchUI in NodesList) {
			Destroy(matchUI.gameObject);
		}
		
		NodesList.Clear ();
	}
	
}
