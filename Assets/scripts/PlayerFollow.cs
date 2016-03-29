using UnityEngine;
using System.Collections;

public class PlayerFollow : MonoBehaviour {

	public	GameObject	player;

	void Start () {
	
	}

	//TODO: do it better (get level size and only move when necessary)
	void Update () {
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
	}
}
