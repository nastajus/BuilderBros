using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public enum State { Splash, BuildMode, TestMode, ReadyToStartMode, PlayerMode, IndividualResults, FinalResults, Credits }; 
public enum SemanticAction { Jump, EnterMenu, Cancel, SwitchMode, Build, Destroy }; //EnterMenu aka Confirm...
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
		{ SemanticAction.SwitchMode, KeyCode.Z },
		{ SemanticAction.Build, KeyCode.Q },
		{ SemanticAction.Destroy, KeyCode.E }
	};
	public static Dictionary<SemanticAction, string> SemanticToKeyStr = new Dictionary<SemanticAction, string>(){
		{SemanticAction.EnterMenu, "RETURN KEY" },
		{SemanticAction.Cancel, "ESCAPE KEY" }
	};
	public static Dictionary<Player, string> PlayerToString = new Dictionary<Player, string>(){
		{ Player.Player1, "PLAYER 1" },
		{ Player.Player2, "PLAYER 2" }
	};

	public State CurrentMode;
	public State PreviousMode;
	public Player CurrentPlayer = Player.Player1;
	public Player NextPlayer = Player.Player2;
	public int PointsRemaining;
	public const int PointsMax = 12000;
	public float StartTime;
	public float TimeUsed;
	private bool allPlayersGone = false;
	public Dictionary<Player, float> Score = new Dictionary<Player, float>();
	public List<GameObject> everyDamnTile; //TODO:killmyself
	public List<GameObject> TileItems;

	//public bool executedSingleAllotement = false;


	void Awake() {
		if(instance==null){
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else if (instance != this){
			Destroy(gameObject);
		}


		PointsRemaining = PointsMax;
		//CurrentMode = GameState.TestMode;

		Load();

		//detect if level contains LevelStartMarker & LevelExitMarker
		GameObject exit = GameObject.Find ( "LevelExitMarker");
		GameObject start = GameObject.Find ( "LevelStartMarker");

		if (exit == null) Debug.Log ("Level needs Exit Marker present");
		if (start == null) Debug.Log ("Level needs Start Marker present");


		StartTime = Time.time;
		TimeUsed = 0;
		
		TileItems = new List<GameObject>();
		 
		string[] files = Directory.GetFiles( Application.dataPath + "/Resources/Prefabs/Browning/", "*.prefab", SearchOption.AllDirectories );
 		foreach(string file in files)
		{
			string fileFixed = file.Replace('\\', '/');
			int startind = Application.dataPath.Length + "/Resources/".Length ;		//D:/Code/Others/GitHub/BuilderBros/Assets/Resources/Tiles/Mario/7-6.png
			int len = fileFixed.Length - ".prefab".Length - startind;
			string sub = fileFixed.Substring(startind, len);
			TileItems.Add (  Resources.Load<GameObject> ( sub ) );
		}
	
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
			StartTime = data.StartTime;
			TimeUsed = data.TimeUsed;
			//CurrentMode = data.CurrentMode;
		}
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
	


}
