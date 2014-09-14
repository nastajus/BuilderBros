using UnityEngine;
using UnityEditor;
using System.Collections;


//removes "(Clone)" from all prefabs in hierarchy when run from the Editor.
public class RenameAll : EditorWindow {


	[MenuItem("Window/RenameAll")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(RenameAll));
	}

	public void OnGUI()
	{
		if (GUILayout.Button("removes \"(Clone)\" from all prefabs in hierarchy \n when run from the Editor"))
		{
			RemoveZeeClones();
		}
	}

	private static void RemoveZeeClones(){
		GameObject[] gameObjs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach(GameObject go in gameObjs)
		{
			if ( go.name.Contains("(Clone)") ){
				string targetName = (go.name).Substring(0, (go.name).Length - "(Clone)".Length ) ;
				go.name = targetName;
			}
		}

	}
}
