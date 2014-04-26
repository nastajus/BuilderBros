using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform player;
	
	void Update () {
		transform.position = new Vector3(player.position.x + 6, 4, -10);
	}
}
