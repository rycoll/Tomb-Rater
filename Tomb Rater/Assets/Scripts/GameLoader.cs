﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {

	public GameObject gameControllerPrefab;
	private GameController gameController;

	private void Start () {
		GameObject[] gcList = GameObject.FindGameObjectsWithTag ("GameController");
		if (gcList.Length == 0) {
			GameObject obj = (GameObject)Instantiate (gameControllerPrefab, this.transform.position, Quaternion.identity);
			gameController = obj.GetComponent<GameController> ();
		} else if (gcList.Length == 1) {
			gameController = gcList [0].GetComponent<GameController> ();
		} else {
			Debug.Log ("There are too many GameControllers! " + gcList.Length);
			exitApplication ();
		}
	}

	public void startNewGame () {
		gameController.loadEvent(new Event_Introduction());
	}

	public void continueGame () {
		Debug.Log ("Continuing a previous game!");
		gameController.setBuildTutorialNeeded (false);
		gameController.setOvermenuTutorialNeeded (false);
		gameController.setWorkTutorialNeeded (false);
		gameController.loadScene ("menu");
	}

	public void exitApplication () {
		Application.Quit ();
	}
}
