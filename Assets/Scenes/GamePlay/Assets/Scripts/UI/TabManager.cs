using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour {

	public Tab[] Tabs;

	private int _Size = 0;
	private int _SelectedTabIndex = 0;

	// Use this for initialization
	void Start () {
		_Size = Tabs.Length;

		for (int i = 0; i < _Size; i++) {
			Tabs [i].SetState(Tab.State.Disabled);
			Tabs [i].OnStateChanged += HandleTabStateChanged;
		}

		Tabs [_SelectedTabIndex].SetState (Tab.State.Active);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void HandleTabStateChanged (Tab tab) {
		for (int i = 0; i < _Size; i++) {
			if (!Tabs [i].Equals (tab)) {
				Tabs [i].SetState (Tab.State.Disabled);
			}
		}
	}
}
