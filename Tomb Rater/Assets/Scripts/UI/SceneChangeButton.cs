﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeButton : MonoBehaviour {

	private GameController gameController;
	public string sceneToLoad;

	private void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}

	private void OnMouseDown () {
		switch (this.sceneToLoad) {
		case "building_map":
			if (!gameController.buildTutorialNeeded ()) {
				gameController.loadScene (sceneToLoad);
			} else {
				gameController.loadEvent (new Event_TombBuildingTutorial ());
			}
			break;
		case "annual_turnover":
			gameController.nextYear ();
			break;
		case "default":
			Debug.Log ("Couldn't handle this advisor button! " + sceneToLoad);
			break;
		}

	}

}
