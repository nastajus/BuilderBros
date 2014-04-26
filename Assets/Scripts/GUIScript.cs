//source: http://forum.unity3d.com/threads/113055-Creating-Custom-GUI-Skins-PART-ONE

using UnityEngine;
using System.Collections;

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
	private Sprite[] textures;

	int outerMargin = 30;
	Vector2 itemboxSize;
	Vector2 toolboxSize;
	Vector2 toolboxPosition;
	Vector2 toolboxContentsSize;
	string toolboxTitle = "ITEM SELECTOR";
	string modeMode = "BUILD MODE"; //TODO move
	int innerMargin = 10;
	int borderSize = 7;

	private Vector2 scrollPosition = Vector2.zero;
	private float scrollMovement;

	
	void Awake (){

		//various initializations
		toolBarStringsSizes = new Vector2[toolBarStrings.Length];
		marginDefaultUnity = new RectOffset(4,4,4,4);
		paddingDefaultUnity = new RectOffset(6,6,3,3);
		myMP = new RectOffset( marginDefaultUnity.left + paddingDefaultUnity.left, marginDefaultUnity.right + paddingDefaultUnity.right, 
		                       marginDefaultUnity.top + paddingDefaultUnity.top, marginDefaultUnity.bottom + paddingDefaultUnity.bottom );
		//tex1 = Resources.Load<Sprite> ( "Tiles/Textures/7-6" );
		//tex2 = Resources.Load<Sprite> ( "Tiles/Textures/7-7" );
		tex1 = Resources.Load<Sprite> ( "Special/CastleWide" );
		tex2 = Resources.Load<Sprite> ( "Special/CastleTall" );
		tex3 = Resources.Load<Sprite> ( "Special/Castle86x86" );
		tex4 = Resources.Load<Sprite> ( "Tiles/8-6" );
		tex5 = Resources.Load<Sprite> ( "Tiles/8-7" );
		tex6 = Resources.Load<Sprite> ( "Tiles/8-9" );
		tex11 = Resources.Load<Sprite> ( "Tiles/7-6" );
		tex12 = Resources.Load<Sprite> ( "Tiles/7-7" );
		tex13 = Resources.Load<Sprite> ( "Special/Castle86x86" );
		tex14 = Resources.Load<Sprite> ( "Tiles/8-6" );
		tex15 = Resources.Load<Sprite> ( "Tiles/8-7" );
		tex16 = Resources.Load<Sprite> ( "Tiles/8-9" );

		//Debug.Log ( tex1.height + " " + tex1.width ) ;

		textures = new Sprite[12] { tex1, tex2, tex3, tex4, tex5, tex6,  tex11, tex12, tex13, tex14, tex15, tex16 };

		itemboxSize = new Vector2( 100, 100 );
		toolboxSize = new Vector2( 350, itemboxSize.y + 100 );
		toolboxPosition = new Vector2 ( (float)outerMargin, Screen.height - toolboxSize.y - outerMargin );
		toolboxContentsSize = new Vector2( textures.Length * itemboxSize.x, itemboxSize.y );
		scrollMovement = (itemboxSize.x + innerMargin) *3.33f; //80f;


		//iterate over the set of all styles in the skin
		foreach ( GUIStyle childStyle in MenuSkin){
			childStyle.font = MenuSkin.font;
		}
	}

	void OnGUI(){
		GUI.skin = MenuSkin;
		GUI.skin.box.fontSize = 20;


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
			//toolbarInt = GUI.Toolbar(new Rect(0,10, tbWidth ,30), toolbarInt, toolBarStrings);
			//GUI.Toolbar( new Rect(), 
			//selGridInt = GUI.SelectionGrid(new Rect(0,50,200,40), selGridInt, selStrings, 2);
			//hSliderValue = GUI.HorizontalSlider(new Rect(0,110,100,30), hSliderValue, 0f, 1f);
			//hSbarValue = GUI.HorizontalSlider(new Rect(0,130,100,30), hSbarValue, 0f, 10f); //1f first

			//GUI.SelectionGrid(new Rect(0,10, tbWidth ,100), 2, textures, 

			///GUI.SelectionGrid vs GUILayout.SelectionGrid.
						
			//GUI.DrawTexture(new Rect(0, 70, 100, 100), tex1, ScaleMode.ScaleAndCrop );
			//GUI.DrawTexture(new Rect(110, 70, 100, 100), tex2, ScaleMode.ScaleAndCrop );
			//GUI.DrawTexture(new Rect(220, 70, 100, 100), tex3, ScaleMode.ScaleAndCrop );
		GUI.EndGroup();


		scrollPosition = GUI.BeginScrollView(new Rect(toolboxPosition.x + innerMargin, toolboxPosition.y + innerMargin*3, toolboxSize.x-innerMargin*2, toolboxSize.y-innerMargin*4), scrollPosition, new Rect(0, 0, toolboxContentsSize.x, toolboxContentsSize.y));
			for (int i = 0; i < textures.Length; i++){
				GUI.Box (new Rect( i*(itemboxSize.x+innerMargin), 0, itemboxSize.x, itemboxSize.y), "");
				//check size?
				Vector2 scaledSize = ScaleToFit( new Vector2(textures[i].texture.width, textures[i].texture.height), new Vector2(itemboxSize.x - borderSize*2, itemboxSize.y - borderSize*2) );
				GUI.DrawTexture(new Rect( (i*(itemboxSize.x+innerMargin))+(itemboxSize.x/2-textures[i].texture.width/2), itemboxSize.y/2-textures[i].texture.height/2, scaledSize.x, scaledSize.y ), textures[i].texture, ScaleMode.ScaleToFit );
				//GUI.DrawTexture(new Rect( i*(itemboxSize.x+innerMargin), 0, itemboxSize.x, itemboxSize.y), textures[i].texture, ScaleMode.ScaleAndCrop );
				Vector2 v = GUI.skin.label.CalcSize( new GUIContent( "-4000" ));
				GUI.Label(new Rect( i*(itemboxSize.x+innerMargin)+itemboxSize.x/2 - v.x/2, itemboxSize.y+innerMargin, itemboxSize.x, itemboxSize.y), "-4000" );
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



		Vector2 modeSize = GUI.skin.label.CalcSize ( new GUIContent( modeMode ) );
		modeSize = new Vector2( modeSize.x + (float)(myMP.left + myMP.right)*2, modeSize.y );
		Vector2 modePosition = new Vector2( Screen.width/2 - modeSize.x/2, outerMargin );
		GUI.Box (new Rect(modePosition.x, modePosition.y, modeSize.x, modeSize.y), modeMode);



		Vector2 pointsSize = GUI.skin.label.CalcSize ( new GUIContent( "POINTS: 8000" ) );
		pointsSize = new Vector2( pointsSize.x + (float)(myMP.left + myMP.right)*2, pointsSize.y );
		Vector2 pointsPosition = new Vector2( outerMargin, outerMargin );
		GUI.Box (new Rect(pointsPosition.x, pointsPosition.y, pointsSize.x, pointsSize.y), "POINTS: 8000");


	}

	private Vector2 ScaleToFit(Vector2 possiblyOversized, Vector2 maxSize){
		float reductionRatio1 = 0f, reductionRatio2 = 0f;
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








