using UnityEngine;
using System.Collections;

public class PenaltyTime : MonoBehaviour {

	private GameObject Player;
	private bool inside = false;
	
	void OnTriggerEnter2D(Collider2D collision){
		if ( GameControl.instance.CurrentMode == State.PlayerMode && inside == false ){
			inside = true;
			GameControl.instance.penaltyTime = GameControl.instance.penaltyTime + 5;
				
		}
	}

	void OnTriggerExit2D(Collider2D collision){
		inside = false;
	}
}