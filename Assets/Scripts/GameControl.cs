using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public enum GameState { Splash, BuildMode, TestMode, PlayerMode, Results, Credits }; 



public class GameControl : MonoBehaviour {

	public static GameControl instance;
	public static Dictionary<GameState, string> ModeNames = new Dictionary<GameState, string>()
	{
		{ GameState.BuildMode, "BUILD MODE" },
		{ GameState.TestMode, "TEST MODE" },
	};
	public GameState CurrentMode;
	public int PointsRemaining;
	public const int PointsMax = 12000;

	void Awake() {
		if(instance==null){
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else if (instance != this){
			Destroy(gameObject);
		}


		//PointsRemaining = PointsMax;
		//CurrentMode = GameState.TestMode;

		Load();

	}

	void OnApplicationQuit() {
		Debug.Log ("saving should be happening.");
		Save();
	}


	public void Save(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");
		
		GameData data = new GameData();
		data.PointsRemaining = PointsRemaining;
		//data.CurrentMode = CurrentMode;
		
		bf.Serialize(file, data);
		file.Close();
	}
	
	public void Load(){
		if (  File.Exists (Application.persistentDataPath + "/gameInfo.dat") ){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
			GameData data = (GameData)bf.Deserialize(file);
			file.Close ();
			
			PointsRemaining = data.PointsRemaining;
			//CurrentMode = data.CurrentMode;
		}
	}
}

[Serializable]
class GameData{

	public GameState CurrentMode;
	public int PointsRemaining;
}
