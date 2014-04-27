using UnityEngine;
using System.Collections;

public class BGFollow : MonoBehaviour {
	
	public Transform player;

	void Update () {
		transform.position = new Vector3(player.position.x + 6, 4, 0);
	}
}
