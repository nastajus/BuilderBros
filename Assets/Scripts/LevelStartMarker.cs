using UnityEngine;
using System.Collections;

public class LevelStartMarker : MonoBehaviour {	

	public GameObject Player;

	void Awake(){
		if (Player != null){
			Player.transform.position = transform.position;
		}
		else{
			Debug.Log ("Level Start Marker needs Player attached");
		}
	}

	// Update is called once per frame
	void Update () {
		if (GameControl.instance.CurrentMode == State.BuildMode || GameControl.instance.CurrentMode == State.TestMode )
			active = true;
		else
			active = false;
	}
}
