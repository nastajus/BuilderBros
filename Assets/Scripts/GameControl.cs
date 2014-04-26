using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public enum GameState { Splash, BuildMode, PlayMode, Results, Credits }; 



public class GameControl : MonoBehaviour {

	public static GameControl control;
	public GameState mode = GameState.Splash;
	public Dictionary<GameState, string> ModeNames;
	public float health;
	public float experience;
	
	void Awake() {
		if(control==null){
			DontDestroyOnLoad(gameObject);
			control = this;
		}
		else if (control != this){
			Destroy(gameObject);
		}

		ModeNames = new Dictionary<GameState, string>()
		{
			{ GameState.BuildMode, "BUILD MODE" },
			{ GameState.PlayMode, "PLAY MODE" },
		};
	}
	
	void OnGUI(){
		//update mode here???? ??? 
		GUI.Label(new Rect(10,10,100,30), "Health: " + health);
		GUI.Label(new Rect(10,40,150,30), "Experience: " + experience);
	}
	
	public void Save(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
		
		GameData data = new GameData();
		data.health = health;
		data.experience = experience;
		
		bf.Serialize(file, data);
		file.Close();
	}
	
	public void Load(){
		if (  File.Exists (Application.persistentDataPath + "/playerInfo.dat") ){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			GameData data = (GameData)bf.Deserialize(file);
			file.Close ();
			
			health = data.health;
			experience = data.experience;
		}
	}
}

[Serializable]
class GameData{

	public float health;
	public float experience;
}
