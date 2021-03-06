﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DumpBody : MonoBehaviour {

	float killWeight = 0.2f; //how much each kill pleases the voices
	float silenceMod;
	public Slider slider;
	public Image sliderfill;
	List<GameObject> inRange;

	// Use this for initialization
	void Start () {
		inRange = new List<GameObject> ();
		slider.value = 1;
		silenceMod = GameInfo.silencingModifier;
	}
	
	public bool kill(GameObject human) {
		bool contains = inRange.Contains (human);
		if (contains) {
			inRange.Remove(human);
			GameInfo.bodyCount ++;
			slider.value -= (killWeight * silenceMod);
			slider.value = Mathf.Max(0, slider.value);
			if (slider.value <= 0.5) {
				sliderfill.color = Color.yellow;
			}
		}
		return contains;
	}

	void Update() {
		for (int i = inRange.Count-1; i >= 0; i--) {
			if (inRange[i] == null) {
				inRange.RemoveAt (i);
			}
		}
		ActionContext.canDumpBody = inRange.Count > 0;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Body")) {
			inRange.Add (other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag ("Body") && inRange.Contains (other.gameObject)) {
			inRange.Remove(other.gameObject);
		}
	}
}
