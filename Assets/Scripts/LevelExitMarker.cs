using UnityEngine;
using System.Collections;

public class LevelExitMarker : MonoBehaviour {

	public GameObject Player;
	
	void Awake(){
		if (Player == null){
			Debug.Log ("Level Exit Marker needs Player attached");
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
