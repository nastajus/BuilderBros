using UnityEngine;
using System.Collections;

public class RewardTime : MonoBehaviour {
	
	private GameObject Player;
	private bool inside = false;
	
	void OnTriggerEnter2D(Collider2D collision){
		if ( GameControl.instance.CurrentMode == State.PlayerMode && inside == false ){
			inside = true;
			GameControl.instance.rewardTime += GameControl.rewardAmount;
			
		}
	}
	
	void OnTriggerExit2D(Collider2D collision){
		inside = false;
		if ( GameControl.instance.CurrentMode == State.PlayerMode ){
			Destroy(gameObject);
		}
	}
}