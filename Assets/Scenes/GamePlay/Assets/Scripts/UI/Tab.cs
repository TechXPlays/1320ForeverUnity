using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour {

	public enum State {
		Active = 0,
		Disabled = 1
	}

	public Toggle TabToggle;
	public Text TabHeader;

	public ScrollRect ScrollRect;
	public GameObject Content;

	public Color EnabledHeaderColor;
	public Color DisabledHeaderColor;

	public event Action<Tab> OnStateChanged = delegate {};

	private State _State = State.Disabled;

	// Use this for initialization
	void Start () {
		TabToggle.onValueChanged.AddListener (OnTabBtnClick);
		_State = TabToggle.isOn ? State.Active : State.Disabled;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnValidate() {
		TabHeader.color = TabToggle.isOn ? EnabledHeaderColor : DisabledHeaderColor;
	}

	public void SetState(State state) {
		_State = state;
		if (state == State.Active) {
			Content.SetActive (true);
		} else {
			Content.SetActive (false);
		}
	}

	public void OnTabBtnClick(bool enabled) {
		State state = (State)Mathf.Abs ((int)_State - 1);
		SetState (state);
		OnStateChanged (this);
		OnValidate ();
	}

	public State CurrentState {
		get {
			return _State;
		}
	}

}
