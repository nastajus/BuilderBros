using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log (other);
		if (other.tag == "Player"){
			//reload the stage.
			Debug.Break ();
			return;
		}
	}
}
