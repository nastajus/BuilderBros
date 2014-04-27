using UnityEngine;
using System.Collections;

public class LevelExitMarker : MonoBehaviour {

	private GameObject Player;
	
	void Awake(){
		Player = GameObject.FindWithTag("Player");
		if (Player == null){
			Debug.Log ("Level needs Player");
		}
	}

	void OnTriggerEnter2D(Collider2D collision){
		if ( collision.tag == "Player" ){
			GameControl.instance.SetNextState();
			Destroyer.ReloadLevel();
			/*
			if ( !GameControl.instance.ScoreAdd() && !GameControl.instance.Score.ContainsKey( GameControl.instance.CurrentPlayer ) ){
				Destroyer.ReloadLevel();
				GameControl.instance.CurrentMode = State.Results;
			}
			*/
		}
	}
}
