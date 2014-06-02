using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;


public enum State { Splash, BuildMode, TestMode, ReadyToStartMode, PlayerMode, IndividualResults, FinalResults, Credits }; 
public enum SemanticAction { Jump, EnterMenu, Cancel, SwitchMode, Build, Destroy, NextItem, PrevItem, SelectItem }; //EnterMenu aka Confirm...
public enum Player { Player1, Player2, None };


public class GameControl : MonoBehaviour { 

	public static GameControl instance;
	public static Dictionary<State, string> ModeNames = new Dictionary<State, string>()
	{
		{ State.BuildMode, "BUILD MODE" },
		{ State.TestMode, "TEST MODE" },
		{ State.ReadyToStartMode, "READY TO START" },
	};
	public static Dictionary<SemanticAction, KeyCode> SemanticToKey = new Dictionary<SemanticAction, KeyCode>(){
		{ SemanticAction.Jump, KeyCode.Space },
		{ SemanticAction.EnterMenu, KeyCode.Return },
		{ SemanticAction.Cancel, KeyCode.Escape },
		{ SemanticAction.SwitchMode, KeyCode.Tab },
		{ SemanticAction.Build, KeyCode.Q },
		{ SemanticAction.Destroy, KeyCode.E },
		{ SemanticAction.NextItem, KeyCode.C },
		{ SemanticAction.PrevItem, KeyCode.Z },
		{ SemanticAction.SelectItem, KeyCode.X }
	};
	public static Dictionary<SemanticAction, string> SemanticToKeyStr = new Dictionary<SemanticAction, string>(){
		{SemanticAction.EnterMenu, "RETURN KEY" },
		{SemanticAction.Cancel, "ESCAPE KEY" }
	};
	public static Dictionary<Player, string> PlayerToString = new Dictionary<Player, string>(){
		{ Player.Player1, "PLAYER 1" },
		{ Player.Player2, "PLAYER 2" }
	};
	public static List<string> UserCreatedGONames;
	public static List<float> UserCreatedGOx;
	public static List<float> UserCreatedGOy;
	public static List<float> UserCreatedGOz;

	public State CurrentMode;
	public State PreviousMode;
	public Player CurrentPlayer = Player.Player1;
	public Player NextPlayer = Player.Player2;
	public int CurrentItem = -1;
	public int PointsRemaining;
	public const int PointsMax = 12000;
	public float StartTime;
	public float TimeUsed;
	private bool allPlayersGone = false;
	public Dictionary<Player, float> Score = new Dictionary<Player, float>();
	public List<GameObject> TileItems;
	public List<Sprite> spriteItems;
	public float penaltyTime = 0f;
	private GameObject goHolder; //TODO: this is a duplicate, remove other.
	//bool firstExecution = true;

	//public bool executedSingleAllotement = false;



	void Awake() {

		// the first time it creates the singleton
		if(instance==null){
			DontDestroyOnLoad(gameObject);
			instance = this;

			PointsRemaining = PointsMax;
			//CurrentMode = GameState.TestMode;
			
			//Load();
			
			//detect if level contains LevelStartMarker & LevelExitMarker
			GameObject exit = GameObject.Find ( "LevelExitMarker");
			GameObject start = GameObject.Find ( "LevelStartMarker");
			
			if (exit == null) Debug.Log ("Level needs Exit Marker present");
			if (start == null) Debug.Log ("Level needs Start Marker present");
			
			
			StartTime = Time.time;
			TimeUsed = 0;
			
			TileItems = new List<GameObject>();
			
			List<string> directories = new List<string>();
			directories.Add( Application.dataPath + "/Resources/Prefabs/Browning/");
			directories.Add( Application.dataPath + "/Resources/Prefabs/Special/");
			
			foreach (string directory in directories){ 
				string[] files = Directory.GetFiles( directory, "*.prefab", SearchOption.TopDirectoryOnly );
				foreach(string file in files)
				{
					string fileFixed = file.Replace('\\', '/');
					int startind = Application.dataPath.Length + "/Resources/".Length ;		//D:/Code/Others/GitHub/BuilderBros/Assets/Resources/Tiles/Mario/7-6.png
					int len = fileFixed.Length - ".prefab".Length - startind;
					string sub = fileFixed.Substring(startind, len);
					TileItems.Add (  Resources.Load<GameObject> ( sub ) );
				}
			}
			
			directories = new List<string>();
			directories.Add ( Application.dataPath + "/Resources/Tiles/Browning/" );
			directories.Add ( Application.dataPath + "/Resources/Tiles/Special/" );
			
			spriteItems = new List<Sprite>();
			foreach (string directory in directories){ 
				string[] files = Directory.GetFiles( directory, "*.png", SearchOption.TopDirectoryOnly );
				foreach(string file in files)
				{
					string fileStr = file.Replace('\\', '/');
					int startind = Application.dataPath.Length + "/Resources/".Length ;		//D:/Code/Others/GitHub/BuilderBros/Assets/Resources/Tiles/Mario/7-6.png
					int len = fileStr.Length - (".png".Length) - startind;
					string sub = fileStr.Substring(startind, len);
					spriteItems.Add ( Resources.Load<Sprite> ( sub ) );
				}
			}
			CurrentItem = 0;
			
			UserCreatedGONames = new List<string>();
			UserCreatedGOx = new List<float>();
			UserCreatedGOy = new List<float>();
			UserCreatedGOz = new List<float>();
		}
		else if (instance != this){
			Destroy(gameObject);
		}

		//DON'T PUT ANYTHING HERE AFTER IF STATEMENT, will execute every time level loaded (or more?!), DTRAN
	
	}

	public State OppositeMode(){
		if ( CurrentMode == State.BuildMode ){
			CurrentMode = State.TestMode;
		}
		else if ( CurrentMode == State.TestMode ){
			CurrentMode = State.BuildMode;
		}
		return CurrentMode; 
	}

	public Player SetNextPlayer(){
		if ( CurrentPlayer == Player.Player1 ){
			CurrentPlayer = Player.Player2;
			NextPlayer = Player.Player1;
		}
		else if ( CurrentPlayer == Player.Player2 ){
			CurrentPlayer = Player.Player1;
			NextPlayer = Player.None;
		}
		return CurrentPlayer; 

	}

	public State SetNextState(){
		if ( CurrentMode == State.PlayerMode ) {
			SetNextPlayer();
			CurrentMode = State.IndividualResults;
		}
		else if ( CurrentMode == State.IndividualResults && NextPlayer != Player.None) {
			SetNextPlayer();
			CurrentMode = State.BuildMode;
		}
		else if ( CurrentMode == State.IndividualResults && NextPlayer == Player.None) {
			CurrentMode = State.FinalResults;
		}
		else if ( CurrentMode == State.FinalResults) {
			CurrentMode = State.Credits;
		}
		return CurrentMode;
	}

	public bool ScoreAdd(){ 
		bool successState;
		if (CurrentPlayer != Player.None && !Score.ContainsKey( CurrentPlayer ) ){
			Score.Add(CurrentPlayer, TimeUsed);
			successState = true;
		}
		else 
			successState = false;
		return successState;
	}

	public void NextItem(){
		if (CurrentItem + 1 < spriteItems.Count && CurrentItem + 1 < TileItems.Count){
			CurrentItem++;
		}
	}

	public void PrevItem(){
		if (CurrentItem - 1 >= 0){
			CurrentItem--;
		}
	}

	//push an individual GO's raw data because GO not serializeable
	public void PushUserGO(GameObject go){
		UserCreatedGONames.Add(go.name);
		UserCreatedGOx.Add (go.transform.position.x);
		UserCreatedGOy.Add (go.transform.position.y);
		UserCreatedGOz.Add (go.transform.position.z);
	}




	//TODO: possibly remove this
	void OnApplicationQuit() {
		Debug.Log ("OnApplicationQuit triggered");
		Save();
	}


	public void Save(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");
		
		GameData data = new GameData();
		data.PointsRemaining = PointsRemaining;
		data.StartTime = StartTime;
		data.TimeUsed = TimeUsed;
		data.UserCreatedGONames = UserCreatedGONames;
		data.UserCreatedGOx = UserCreatedGOx;
		data.UserCreatedGOy = UserCreatedGOy;
		data.UserCreatedGOz = UserCreatedGOz;
		//data.CurrentMode = CurrentMode;
		
		bf.Serialize(file, data);
		file.Close();
	}

	public void OnLevelWasLoaded()
	{
		GameControl.instance.Load();
	}

	public void Load(){
		if (  File.Exists (Application.persistentDataPath + "/gameInfo.dat") ){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
			GameData data = (GameData)bf.Deserialize(file);
			file.Close ();
			
			PointsRemaining = data.PointsRemaining;
			StartTime = data.StartTime;
			TimeUsed = data.TimeUsed;
			UserCreatedGONames = data.UserCreatedGONames;
			UserCreatedGOx = data.UserCreatedGOx;
			UserCreatedGOy = data.UserCreatedGOy;
			UserCreatedGOz = data.UserCreatedGOz;
			//CurrentMode = data.CurrentMode;

			//instantiate list based off raw data because GO not serializeable

			goHolder = GameObject.Find("HoldUserBlocks");
			if (goHolder==null){
				goHolder = new GameObject();
				goHolder.name = "HoldUserBlocks";
			}

			if ( UserCreatedGONames != null ){
				for (int i = 0; i < UserCreatedGONames.Count; i++){

					float x = UserCreatedGOx[i];
					float y = UserCreatedGOy[i];
					float z = UserCreatedGOz[i];
					string folder = "Prefabs/Browning/";
					string loadme = (folder + UserCreatedGONames[i]).Substring(0, (folder + UserCreatedGONames[i]).Length - "(Clone)".Length ) ;
					GameObject go = (GameObject)Instantiate( Resources.Load ( loadme ) , new Vector3( x,y,z) , Quaternion.identity  );  
					//go.transform.parent = goHolder.transform;

				}
			}
			else {
				UserCreatedGONames = new List<string>();
				UserCreatedGOx = new List<float>();
				UserCreatedGOy = new List<float>();
				UserCreatedGOz = new List<float>();
			}
		}
	}


	public static void ReloadLevel(){
		int len1 = "Assets/Scripts/".Length - 1 ;
		int len2 = ".unity".Length;
		int len3 = EditorApplication.currentScene.Substring(len1).Length - len2;
		string levelName = EditorApplication.currentScene.Substring(len1, len3);
		//TODO: capture failure or determine exists for versatility
		//GameControl.instance.executedSingleAllotement = false;
		GameControl.instance.Save();
		Application.LoadLevel( levelName );
		//the tomb of bug
	}
}

[Serializable]
class GameData{
	public State CurrentMode;
	public State PreviousMode;
	public int PointsRemaining;
	public float StartTime;
	public float TimeUsed;
	public Dictionary<Player, float> Score;
	public List<string> UserCreatedGONames;
	public List<float> UserCreatedGOx;
	public List<float> UserCreatedGOy;
	public List<float> UserCreatedGOz;


}
