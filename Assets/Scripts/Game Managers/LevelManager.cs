﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public float timeBetweenSpawns;
	public float timeSinceSpawns;
	public GameObject person;
	public GameObject police;
	public Slider slider;
	public Image blackout;
	float loseValue = .5f;
	int spawnLayer = 1;
	Color color;
	bool isFaded = false;
	bool fadeOut = false;

	public Text timer;
	public float levelTime = 5 * 60; //seconds before level ends


	bool transition;

	// Use this for initialization
	void Start () {
		levelTime += 5;
		color = blackout.color;
		color.a = 1f;
		blackout.color = color;
		timeSinceSpawns = GameInfo.spawnInterval;
	}


	public void endLevel() {
		GameInfo.nightNumber++;
		GameInfo.silencingModifier *= .75f;
		if (slider.value > loseValue) {
			GameInfo.loseCondition = GameInfo.LoseCondition.Insane;
			lose ();
		} else {
			//load next level
			Application.LoadLevel (1);
		}
	}

	public void lose() {
		Application.LoadLevel ("Gameover");
	}

	
	// Update is called once per frame
	void Update () {
		timeSinceSpawns += Time.deltaTime;
		if (timeSinceSpawns > timeBetweenSpawns) {
			SpawnNPC ();
			timeSinceSpawns = 0f;
		}
		levelTime -= Time.deltaTime;
		if (levelTime <= 0) {
			endLevel ();
		}
		int mins = (int)(levelTime / 60);
		int secs = (int)(levelTime % 60);
		string txt = mins + ":" + secs;
		if (secs < 10) {
			txt = mins + ":0" + secs;
		}
		timer.text = txt;
		if (!isFaded) {
			color = blackout.color;
			if (color.a > .01f) {
				color.a -= .2f * Time.deltaTime;
			} else {
				color.a = 0f;
			}
			blackout.color = color;
		} else if (fadeOut) {
			color = blackout.color;
			if (color.a < .99f) {
				color.a *= .2f * Time.deltaTime;
			} else {
				color.a = 1f;
				endLevel ();
			}
			blackout.color = color;
		}
	}
	
	void SpawnNPC(){
		int random = Random.Range (0, 10);
		GameObject npc;
		if (random == 0) {
			npc = Instantiate (police);
			if (npc.GetComponent<MoveBetweenPoints>().moveType == MoveBetweenPoints.MoveType.wait) {
				npc.GetComponent<MoveBetweenPoints>().moveType = MoveBetweenPoints.MoveType.patrol;
			}
		} else {
			npc = Instantiate (person);
		}
		npc.GetComponent<SpriteRenderer>().sortingOrder = spawnLayer;
		spawnLayer++;
	}
}
