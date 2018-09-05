using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MultiplayerUI : MonoBehaviour {
	private List<MatchPanelUI> MatchList = new List<MatchPanelUI> ();

	public MatchPanelUI MatchPanelUI;
	public RectTransform ActiveMatchesHolder;
	public RectTransform FinishedMatchesHolder;
    public TopPanelUI TopPanelUI;

	private static Animator MultiplayerAnimator;

	private Dictionary<string, MatchPanelUI> matchUIs = new Dictionary<string, MatchPanelUI>();

	private const string MultiplayerShowToken = "Show";
	private const string MultiplayerHideToken = "Hide";

	void Awake() {
		MultiplayerAnimator = GetComponent<Animator> ();
	}

	void Start() {
		if (UM_GameServiceManager.Instance.ConnectionSate == UM_ConnectionState.CONNECTED) {
			TBM.Matchmaker.MatchesListLoadedEvent += HandleActionMatchesInfoLoaded;
			TBM.Matchmaker.MatchUpdatedEvent += TBM_Matchmaker_MatchUpdatedEvent;
			TBM.Matchmaker.InvitationAccepted += HandleActionInvitationAccepted;
			TBM.Matchmaker.LoadMatchesInfo ();
		} else {
			UM_GameServiceManager.OnPlayerConnected += HandleOnPlayerConnected;
		}

		ShowMultiplayer ();
	}
		
	void OnDestroy() {
		TBM.Matchmaker.MatchesListLoadedEvent -= HandleActionMatchesInfoLoaded;
		TBM.Matchmaker.MatchUpdatedEvent -= TBM_Matchmaker_MatchUpdatedEvent;
		TBM.Matchmaker.InvitationAccepted -= HandleActionInvitationAccepted;
	}

	private void HandleOnPlayerConnected() {
		UM_GameServiceManager.OnPlayerConnected -= HandleOnPlayerConnected;

		TBM.Matchmaker.MatchesListLoadedEvent += HandleActionMatchesInfoLoaded;
		TBM.Matchmaker.MatchUpdatedEvent += TBM_Matchmaker_MatchUpdatedEvent;
		TBM.Matchmaker.InvitationAccepted += HandleActionInvitationAccepted;
		TBM.Matchmaker.LoadMatchesInfo ();
	}

	private void HandleActionInvitationAccepted (UM_TBM_MatchResult result)
	{
		if (result.IsSucceeded) {
			MatchPanelUI panel = CreateNewMatchPanel (result.Match.Status == UM_TBM_MatchStatus.Ended ? FinishedMatchesHolder : ActiveMatchesHolder);
			panel.SetMatchInfo (result.Match);
			if (!matchUIs.ContainsKey (result.Match.Id)) {
				matchUIs.Add (result.Match.Id, panel);
			}
		}
	}

	private void HandleActionMatchesInfoLoaded (UM_TBM_MatchesLoadResult res) {
		if (res.IsFailed) {
			return;
		}

		foreach(UM_TBM_Invite iinvite in TBM.Matchmaker.Invitations) {
			TBM.Matchmaker.AcceptInvite (iinvite);
		}

		foreach(UM_TBM_Match mmatch in TBM.Matchmaker.Matches) {
			MatchPanelUI panel = CreateNewMatchPanel (mmatch.Status == UM_TBM_MatchStatus.Ended ? FinishedMatchesHolder : ActiveMatchesHolder);
			panel.SetMatchInfo (mmatch);
			if (!matchUIs.ContainsKey (mmatch.Id)) {
				matchUIs.Add (mmatch.Id, panel);
			}
		}
	}

	private void TBM_Matchmaker_MatchUpdatedEvent (UM_TBM_MatchResult result)
	{
		if (result.IsSucceeded) {
			if (matchUIs.ContainsKey (result.Match.Id)) {
				if (result.Match.Status == UM_TBM_MatchStatus.Ended) {
					matchUIs [result.Match.Id].transform.SetParent (FinishedMatchesHolder.transform);
					matchUIs [result.Match.Id].transform.localScale = Vector3.one;
				} else {
					matchUIs[result.Match.Id].transform.SetParent (ActiveMatchesHolder.transform);
					matchUIs[result.Match.Id].transform.localScale = Vector3.one;
				}
				matchUIs [result.Match.Id].SetMatchInfo (result.Match);
			}
		}
	}

	public static void ShowMultiplayer() {
		MultiplayerAnimator.SetBool (MultiplayerShowToken, true);
		MultiplayerAnimator.SetBool (MultiplayerHideToken, false);
	}

	public static void HideMultiplayer() {
		HideMultiplayer ();
	}

	public void RemoveMatchUI (string id) {
		if (matchUIs.ContainsKey(id)) {
			Destroy (matchUIs [id].gameObject);
			matchUIs.Remove (id);
		}
	}

	public void HideMultiplayerNonStatic() {
		MultiplayerAnimator.SetBool (MultiplayerHideToken, true);
		MultiplayerAnimator.SetBool (MultiplayerShowToken, false);
	}

	private MatchPanelUI CreateNewMatchPanel(RectTransform parent) {
		MatchPanelUI newPanel = Instantiate (MatchPanelUI);
		newPanel.gameObject.SetActive (true);
		newPanel.transform.SetParent (parent.transform);
		newPanel.transform.localScale = Vector3.one;
		newPanel.AttachToParent (this);

		MatchList.Add (newPanel);

		return newPanel;
	}

	private void ClearMatchList() {
		foreach(var match in MatchList) {
			Destroy (match.gameObject);
			MatchList [MatchList.IndexOf(match)] = null;
		}
	}

}