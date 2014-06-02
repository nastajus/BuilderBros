using UnityEngine;
using System.Collections;
//using UnityEditor;
using System;

public class Destroyer : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player"){
			GameControl.ReloadLevel();
		}
	}
	//TODO: this seems excessively stupid having level loading execute for each enemy. should really refactor || rethink this
	void Start()
	{

	}
}
