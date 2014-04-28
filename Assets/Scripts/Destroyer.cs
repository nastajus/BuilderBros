using UnityEngine;
using System.Collections;
//using UnityEditor;
using System;

public class Destroyer : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player"){
			ReloadLevel();
		}
	}
	public static void ReloadLevel(){
		int len1 = "Assets/Scripts/".Length - 1 ;
		int len2 = ".unity".Length;
		//disabled for web player
		//int len3 = EditorApplication.currentScene.Substring(len1).Length - len2;
		//string levelName = EditorApplication.currentScene.Substring(len1, len3);

		string levelName = "7.3_Matt.0.1b_level_testing_holes_blocked";
		//TODO: capture failure or determine exists for versatility
		//GameControl.instance.executedSingleAllotement = false;
		Application.LoadLevel( levelName );
	}
}
