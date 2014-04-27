using UnityEngine;
using System.Collections;

public class LevelStartMarker : MonoBehaviour {	

	private GameObject Player;

	void Awake(){

		Player = GameObject.FindWithTag("Player");
		if (Player != null){
			Player.transform.position = transform.position;
		}
		else{
			Debug.Log ("Level needs Player");
		}
		/*
		GameObject go = (GameObject)Instantiate(Resources.Load ("Prefabs/Characters/Runner"));
		go.tag = "Player";
		*/
	}

	// Update is called once per frame
	void Update () {
		if (GameControl.instance.CurrentMode == State.BuildMode || GameControl.instance.CurrentMode == State.TestMode )
			active = true;
		else
			active = false;
	}
}
