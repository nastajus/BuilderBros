//source: http://forum.unity3d.com/threads/113055-Creating-Custom-GUI-Skins-PART-ONE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GUIScript : MonoBehaviour {

	public GUISkin MenuSkin;// = new GUISkin(); 
	//MyScriptableObject someInstance = ScriptableObject.CreateInstance("MyScriptableObject") as MyScriptableObject;
	//GUISkin MenuSkin = (GUISkin)ScriptableObject.CreateInstance("MyGUISkin");
	public bool toggleText;
	public int toolbarInt = 3;
	public string[] toolBarStrings = {"Toolbar1", "Toolbar2", "Toolbar3"};
	private Vector2[] toolBarStringsSizes;
	public int selGridInt = 4;
	public string[] selStrings = {"Grid 1", "Grid 2", "Grid 3", "Grid 4"};
	private Vector2[] selStringsSizes;
	public float hSliderValue = 0.0f;
	public float hSbarValue;

	private RectOffset marginDefaultUnity;
	private RectOffset paddingDefaultUnity;
	private RectOffset myMP;
	
	private Sprite tex1, tex2, tex3, tex4, tex5, tex6, tex11, tex12, tex13, tex14, tex15, tex16;
	private Sprite[] textures; //TODO change to List


	int outerMargin = 30;
	Vector2 itemboxSize;
	Vector2 toolboxSize;
	Vector2 toolboxPosition;
	Vector2 toolboxContentsSize;
	string toolboxTitle = "ITEM SELECTOR";
	int innerMargin = 10;
	int borderSize = 7;

	private Vector2 scrollPosition = Vector2.zero;
	private float scrollMovement;

	void Start (){

		//various initializations
		toolBarStringsSizes = new Vector2[toolBarStrings.Length];
		marginDefaultUnity = new RectOffset(4,4,4,4);
		paddingDefaultUnity = new RectOffset(6,6,3,3);
		myMP = new RectOffset( marginDefaultUnity.left + paddingDefaultUnity.left, marginDefaultUnity.right + paddingDefaultUnity.right, 
		                       marginDefaultUnity.top + paddingDefaultUnity.top, marginDefaultUnity.bottom + paddingDefaultUnity.bottom );

		//iterate over the set of all styles in the skin
		foreach ( GUIStyle childStyle in MenuSkin){
			childStyle.font = MenuSkin.font;
		}
	}

	void OnGUI(){

		GUI.skin = MenuSkin;
		GUI.skin.box.fontSize = 20; 

		Vector2 metricPosition = new Vector2( outerMargin, outerMargin );
		Vector2 metricSize = GUI.skin.box.CalcSize ( new GUIContent( "POINTS: " + GameControl.PointsMax ) );	//this is the larger of this or time
		metricSize = new Vector2( metricSize.x + (float)(myMP.left + myMP.right)*2, metricSize.y );

		if ( GameControl.instance.CurrentMode == State.Splash ){
			GUI.DrawTexture( new Rect(0,0,Screen.width,Screen.height), Resources.Load<Sprite> ("Tiles/Meta/Splashscreen" ).texture, ScaleMode.ScaleToFit );
		}
		else if (GameControl.instance.CurrentMode == State.BuildMode || GameControl.instance.CurrentMode == State.TestMode) {

			//top middle
			string modeText =  GameControl.PlayerToString[GameControl.instance.CurrentPlayer] + "\n" + GameControl.ModeNames[GameControl.instance.CurrentMode];
			Vector2 modeSize = GUI.skin.box.CalcSize ( new GUIContent( modeText ) ); 
			modeSize = new Vector2( modeSize.x + (float)(myMP.left + myMP.right)*2, modeSize.y );
			Vector2 modePosition = new Vector2( Screen.width/2 - modeSize.x/2, outerMargin );
			GUI.Box (new Rect(modePosition.x, modePosition.y, modeSize.x, modeSize.y), modeText);


			if ( GameControl.instance.CurrentMode == State.BuildMode ){


				//top right

				itemboxSize = new Vector2( 100, 100 );
				toolboxSize = new Vector2( 350, itemboxSize.y + 100 );
				toolboxPosition = new Vector2 ( Screen.width - toolboxSize.x - outerMargin, (float)outerMargin ); 
				toolboxContentsSize = new Vector2( GameControl.instance.spriteItems.Count * itemboxSize.x, itemboxSize.y );
				scrollMovement = (itemboxSize.x + innerMargin) *3.33f; //80f;

				///int borderThickness = 5;

				//GUIStyle = GUI.skin.button.

				//add correct padding visually
				for ( int i = 0; i < toolBarStrings.Length; i++ ){
					toolBarStringsSizes[i] = GUI.skin.label.CalcSize ( new GUIContent( toolBarStrings[i] ) );
					toolBarStringsSizes[i] = new Vector2( toolBarStringsSizes[i].x + (float)(myMP.left + myMP.right)*2, toolBarStringsSizes[i].y );
				}
				//TODO REENABLE IF NEEDED: int tbWidth = (int)toolBarStringsSizes[0].x * toolBarStringsSizes.Length;

				GUI.Box (new Rect(toolboxPosition.x, toolboxPosition.y, toolboxSize.x, toolboxSize.y), toolboxTitle);
				GUI.BeginGroup( new Rect(toolboxPosition.x + innerMargin, (Screen.height - toolboxSize.y - outerMargin) + innerMargin, toolboxSize.x - innerMargin*2, toolboxSize.y - innerMargin*2));
				GUI.EndGroup();


				scrollPosition = GUI.BeginScrollView(new Rect(toolboxPosition.x + innerMargin, toolboxPosition.y + innerMargin*3, toolboxSize.x-innerMargin*2, toolboxSize.y-innerMargin*4), scrollPosition, new Rect(0, 0, toolboxContentsSize.x, toolboxContentsSize.y));
				for (int i = 0; i < GameControl.instance.spriteItems.Count; i++){
					GUI.Box (new Rect( i*(itemboxSize.x+innerMargin), 0, itemboxSize.x, itemboxSize.y), "");
					//check size?
					Vector2 scaledSize = ScaleToFit( new Vector2(GameControl.instance.spriteItems[i].texture.width, GameControl.instance.spriteItems[i].texture.height), new Vector2(itemboxSize.x - borderSize*2, itemboxSize.y - borderSize*2) );

					//Vector2 scaledSize = ScaleToFit( new Vector2(textures[i].texture.width, textures[i].texture.height), new Vector2(itemboxSize.x - borderSize*2, itemboxSize.y - borderSize*2) );
					GUI.DrawTexture(new Rect( (i*(itemboxSize.x+innerMargin))+(itemboxSize.x/2-GameControl.instance.spriteItems[i].texture.width/2/4), itemboxSize.y/2-GameControl.instance.spriteItems[i].texture.height/2/4, scaledSize.x/4, scaledSize.y/4 ), GameControl.instance.spriteItems[i].texture, ScaleMode.ScaleToFit );
					//GUI.DrawTexture(new Rect( i*(itemboxSize.x+innerMargin), 0, itemboxSize.x, itemboxSize.y), textures[i].texture, ScaleMode.ScaleAndCrop );
					Vector2 v = GUI.skin.label.CalcSize( new GUIContent( "-" + GameValues.items[i] ));
					GUI.Label(new Rect( i*(itemboxSize.x+innerMargin)+itemboxSize.x/2 - v.x/2, itemboxSize.y+innerMargin, itemboxSize.x, itemboxSize.y), "-" + GameValues.items[i] );
				}
				GUI.EndScrollView();


				float result = Input.GetAxis("Mouse ScrollWheel");
				if (result!=0){
					scrollPosition = new Vector2(scrollPosition.x + result*scrollMovement, scrollPosition.y);
				}



				//scrollPosition = GUI.BeginScrollView(new Rect(toolboxPosition.x + innerMargin, toolboxPosition.y + innerMargin*3, toolboxSize.x-innerMargin*2, toolboxSize.y-innerMargin*5), scrollPosition, new Rect(0, 0, 220, 200));
				//	GUI.Button(new Rect(0, 0, 100, 20), "Top-left");
				//	GUI.Button(new Rect(120, 0, 100, 20), "Top-right");
				//	GUI.Button(new Rect(0, 180, 100, 20), "Bottom-left");
				//	GUI.Button(new Rect(120, 180, 100, 20), "Bottom-right");
				//GUI.EndScrollView();



				//top left
				GUI.Box (new Rect(metricPosition.x, metricPosition.y, metricSize.x, metricSize.y), "POINTS: " + GameControl.instance.PointsRemaining );


				//bottom right
				Vector2 testButtonSize = GUI.skin.button.CalcSize( new GUIContent( "SWITCH TO: \n" + GameControl.ModeNames[ State.TestMode ] ));
				testButtonSize = new Vector2( testButtonSize.x + (float)(myMP.left + myMP.right)*2, testButtonSize.y );
				Vector2 buildButtonSize = GUI.skin.button.CalcSize( new GUIContent( "SWITCH TO: \n" + GameControl.ModeNames[ State.BuildMode ] ));
				buildButtonSize = new Vector2( buildButtonSize.x + (float)(myMP.left + myMP.right)*2, buildButtonSize.y );
				Vector2 modeSwitchSize;
				if ( GameControl.instance.CurrentMode == State.BuildMode ){
					modeSwitchSize = testButtonSize;
				}
				else if ( GameControl.instance.CurrentMode == State.TestMode ){
					modeSwitchSize = buildButtonSize;
				}
				else { 
					modeSwitchSize = testButtonSize; 
				}
				Vector2 modeSwitchPosition = new Vector2( Screen.width - modeSwitchSize.x - outerMargin, Screen.height - modeSwitchSize.y - outerMargin );
				if ( GUI.Button (new Rect(modeSwitchPosition.x, modeSwitchPosition.y, modeSwitchSize.x, modeSwitchSize.y), "SWITCH TO: \n" + GameControl.ModeNames[GameControl.instance.CurrentMode]) ) {
					GameControl.instance.OppositeMode();
				}

				/*
				if(rectModeSwitch.Contains(Event.current.mouseDown))
					//Debug.Log("rectModeSwitch");
					Debug.Log ( Event.current );
				else
					Debug.Log("no rect");

				*/

				//bottom left
				//save mods
				//delete mods
				//toggle on/off mods
				//mods == modifications
				string[] modBoxStrings = { "SAVE\nMOD", "LOAD\nMOD", "DESTROY\nMOD", "TOGGLE\nMOD" };

				Vector2 modBoxItemSize = GUI.skin.button.CalcSize( new GUIContent( modBoxStrings[2] ) ); //TODO: MINOR: make utility method to algorithmically get largest string automatically here. hardcoded so don't care.
				//modBoxItemSize += new Vector2( (float)(myMP.left + myMP.right)*0, (float)(myMP.top + myMP.bottom)*0 );
				Vector2 modBoxSize = new Vector2 ( modBoxItemSize.x + innerMargin, modBoxItemSize.y*modBoxStrings.Length + innerMargin*(modBoxStrings.Length+1));
				Vector2 modBoxPosition = new Vector2 ( outerMargin, Screen.height - outerMargin - modBoxSize.y );

				//vertically grouped along left side
				GUI.BeginGroup(new Rect(modBoxPosition.x, modBoxPosition.y, modBoxSize.x, modBoxSize.y));
				//TODO: MEDIUM: Bind these to SemanticActions
				if ( GUI.Button(new Rect(innerMargin*1, innerMargin*1, modBoxItemSize.x, modBoxItemSize.y), modBoxStrings[0]) ) { 
					GameControl.instance.Save(); 
					//needs fullscreen message here... should i use game states??? nahhh..
				}
				if ( GUI.Button(new Rect(innerMargin*1, innerMargin*2 + modBoxItemSize.y*1, modBoxItemSize.x, modBoxItemSize.y), modBoxStrings[1]) ) { //load
					GameControl.instance.Load(); 
				}
				if ( GUI.Button(new Rect(innerMargin*1, innerMargin*3 + modBoxItemSize.y*2, modBoxItemSize.x, modBoxItemSize.y), modBoxStrings[2]) ) { //destroy
					Destroy ( GameControl.goHolder );
					//TODO: STOP DISTASTEFUL COUPLING: now I have another static object outside this class, the "goHolder". YUCK. BUT THIS IS LAZY & QUICK.
					//magic here... umm... pretend file doesn't exist? 
				}
				if ( GUI.Button(new Rect(innerMargin*1, innerMargin*4 + modBoxItemSize.y*3, modBoxItemSize.x, modBoxItemSize.y), modBoxStrings[3]) ) { //toggle
					//GameControl.instance.Toggle(); //why doesn't recognize this?
					//magic here... umm... pretend file doesn't exist? 
				}
				GUI.EndGroup();
			}

		}

		else if ( GameControl.instance.CurrentMode == State.ReadyToStartMode ){
			//Vector2 readyToStartBoxSize = new Vector2 ( 300, 200 ); 
			string textBlahBlah = GameControl.ModeNames[ State.ReadyToStartMode ] + "\n" + GameControl.PlayerToString[GameControl.instance.NextPlayer] + "? \n\n" + "PRESS " + GameControl.SemanticToKeyStr[ SemanticAction.EnterMenu ] + "\nWHEN READY\n" + GameControl.SemanticToKeyStr[ SemanticAction.Cancel ] + " TO CANCEL";
			Vector2 readyToStartBoxSize = GUI.skin.box.CalcSize( new GUIContent( textBlahBlah ));
			readyToStartBoxSize = new Vector2( readyToStartBoxSize.x + (float)(myMP.left + myMP.right)*2, readyToStartBoxSize.y );
			Vector2 readyToStartBoxPosition = new Vector2 ( Screen.width/2 - readyToStartBoxSize.x/2, Screen.height/2 - readyToStartBoxSize.y/2 );
			GUI.Box (new Rect(readyToStartBoxPosition.x, readyToStartBoxPosition.y, readyToStartBoxSize.x, readyToStartBoxSize.y), textBlahBlah );
		}

		else if ( GameControl.instance.CurrentMode == State.PlayerMode ) {
			int roundedUsedSeconds = Mathf.FloorToInt(GameControl.instance.TimeUsed);
			int displaySeconds = roundedUsedSeconds % 60;
			int displayMinutes = roundedUsedSeconds / 60; 
			string text = string.Format ("{0}:{1:00}", displayMinutes, displaySeconds); 
			GUI.Box (new Rect(metricPosition.x, metricPosition.y, metricSize.x, metricSize.y), "TIME: " + text );

			string modeText =  "GO: " + GameControl.PlayerToString[GameControl.instance.CurrentPlayer] + "!!";
			Vector2 modeSize = GUI.skin.box.CalcSize ( new GUIContent( modeText ) ); 
			modeSize = new Vector2( modeSize.x + (float)(myMP.left + myMP.right)*2, modeSize.y );
			Vector2 modePosition = new Vector2( Screen.width/2 - modeSize.x/2, outerMargin );
			GUI.Box (new Rect(modePosition.x, modePosition.y, modeSize.x, modeSize.y), modeText);

		}

		/*
		else if ( GameControl.instance.CurrentMode == State.IndividualResults  ) {

			int roundedUsedSeconds = Mathf.FloorToInt(GameControl.instance.TimeUsed);
			int displaySeconds = roundedUsedSeconds % 60;
			int displayMinutes = roundedUsedSeconds / 60; 
			string leTime = string.Format ("{0}:{1:00}", displayMinutes, displaySeconds); 
			string modeText =  "LEVEL FINISHED, " + GameControl.PlayerToString[GameControl.instance.CurrentPlayer] + "!!" + "\n" + "TIME TAKEN: " + leTime ;
			Vector2 modeSize = GUI.skin.box.CalcSize ( new GUIContent( modeText ) ); 
			modeSize = new Vector2( modeSize.x + (float)(myMP.left + myMP.right)*2, modeSize.y );
			Vector2 modePosition = new Vector2( Screen.width/2 - modeSize.x/2, outerMargin );
			GUI.Box (new Rect(modePosition.x, modePosition.y, modeSize.x, modeSize.y), modeText);


		}
		*/
		/*
		else if ( GameControl.instance.CurrentMode == State.FinalResults ){
			GUI.DrawTexture( new Rect(0,0,Screen.width,Screen.height), Resources.Load<Sprite> ("Prefabs/Meta/Clipboard" ).texture, ScaleMode.ScaleToFit );
		}
		*/

//		else if ( GameControl.instance.CurrentMode == State.BuildMode && Player.None ) {
//
//
//
	}


	void Update(){

		//GameControl.instance.TimeUsed = Time.time - GameControl.instance.StartTime;

		//conditions that affect this OnGUI on next frame
		if (Input.GetKeyDown( GameControl.SemanticToKey[ SemanticAction.EnterMenu ] ) && GameControl.instance.CurrentMode == State.Splash ){
			GameControl.instance.PreviousMode = GameControl.instance.CurrentMode;
			GameControl.instance.CurrentMode = State.BuildMode;
		}
		else if (Input.GetKeyDown( GameControl.SemanticToKey[ SemanticAction.EnterMenu ] ) && GameControl.instance.CurrentMode != State.ReadyToStartMode && GameControl.instance.NextPlayer != Player.None ){
			GameControl.instance.PreviousMode = GameControl.instance.CurrentMode;
			GameControl.instance.CurrentMode = State.ReadyToStartMode;
		}
		else if (Input.GetKeyDown( GameControl.SemanticToKey[ SemanticAction.EnterMenu ] ) && GameControl.instance.CurrentMode == State.ReadyToStartMode ){
			GameControl.instance.SetNextPlayer();
			GameControl.instance.CurrentMode = State.PlayerMode;
			GameControl.instance.StartTime = Time.time;
			GameControl.ReloadLevel();
		}
		else if (Input.GetKeyDown( GameControl.SemanticToKey[ SemanticAction.Cancel ] ) && GameControl.instance.CurrentMode == State.ReadyToStartMode ){
			GameControl.instance.CurrentMode = GameControl.instance.PreviousMode;
		}
		else if ( Input.GetKeyDown( GameControl.SemanticToKey[ SemanticAction.SwitchMode ] )  && (GameControl.instance.CurrentMode == State.BuildMode || GameControl.instance.CurrentMode == State.TestMode) ){
			GameControl.instance.OppositeMode();
		}
		else if ( Input.GetKeyDown ( KeyCode.P ) ){
			GameControl.instance.CurrentMode = State.FinalResults;
		}


	}

	private Vector2 ScaleToFit(Vector2 possiblyOversized, Vector2 maxSize){
		float reductionRatio1 = 1f, reductionRatio2 = 1f;
		if (possiblyOversized.x > maxSize.x){	//say 180 for 100 ... 
			reductionRatio1 = maxSize.x / possiblyOversized.x;
		}
		if (possiblyOversized.y > maxSize.y){
			reductionRatio2 = maxSize.y / possiblyOversized.y;
		}

		Vector2 result; 

		if (reductionRatio1 < reductionRatio2){	//TODO: not good logic here.
			result = new Vector2( possiblyOversized.x * reductionRatio1, possiblyOversized.y * reductionRatio1);
		}
		else if (reductionRatio1 > reductionRatio2){
			result = new Vector2( possiblyOversized.x * reductionRatio2, possiblyOversized.y * reductionRatio2);
		}
		else 
			result = possiblyOversized; //isn't

		return result;
	}
}








