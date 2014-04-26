using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelDesigner))]
public class LevelDesignerEditor : Editor {
	
	LevelDesigner script;
	Vector2 oldTilePos = new Vector2();
	private GameObject goLevel;


	void OnEnable(){
		script = (LevelDesigner) target;
		goLevel = GameObject.Find("Level");
	}

	/*
	public override void OnInspectorGUI(){
		Texture myTexture = (Texture)Resources.Load("Tiles/0-0");
		GUILayout.Label(myTexture);
		Debug.Log (  GUI.skin.window.border.horizontal  + " " + GUI.skin.window.border.vertical );
		Debug.Log ("");
   }
	*/
	
	void OnSceneGUI(){
		
		Ray ray = HandleUtility.GUIPointToWorldRay ( Event.current.mousePosition);
		Vector2 tilePos = new Vector2();
		Vector2 downTilePos = new Vector2();
		tilePos.x = Mathf.RoundToInt(ray.origin.x);
		tilePos.y = Mathf.RoundToInt(ray.origin.y); 
		
		if (tilePos != oldTilePos){
			script.gizmoPosition = tilePos;
			SceneView.RepaintAll();
			oldTilePos = tilePos;
		}

		if (Event.current.type == EventType.MouseDown && Event.current.button == 0){
			downTilePos = tilePos;

			if (script.prefab != null){
				GameObject go = (GameObject)Instantiate(script.prefab, new Vector2(tilePos.x, tilePos.y), Quaternion.identity);
				float tiltAroundX=0, tiltAroundY=0, tiltAroundZ=0;
				go.transform.rotation = Quaternion.Euler(tiltAroundX, tiltAroundY, tiltAroundZ);
				if (goLevel==null){
					goLevel = new GameObject();
					goLevel.name = "Level";
				}
				go.transform.parent = goLevel.transform;
			}
		}


		//script.prefab != null && 
		if (Event.current.type == EventType.MouseUp){// && Event.current.button == 0){

			Debug.Log ( downTilePos);
			Debug.Log ("boogya");
			int xDir, yDir;
			xDir= (downTilePos.x <= tilePos.x) ? 1 : -1;
			yDir= (downTilePos.y <= tilePos.y) ? 1 : -1;
			
			for(int x=0; x<Mathf.Abs(downTilePos.x-tilePos.x); x=x+xDir){
				Debug.Log ( x );
				for(int y=0; y<Mathf.Abs(downTilePos.y-tilePos.y); y=y+yDir){
					Debug.Log ( y );
					Instantiate(script.prefab, new Vector2(x, y), Quaternion.identity);
				}
			}
		}

		Selection.activeGameObject = GameObject.Find ("LevelDesignerGO");

		if (GUI.changed)
			EditorUtility.SetDirty(target);



	} 


}
