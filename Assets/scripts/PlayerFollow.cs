using UnityEngine;
using System.Collections;

public class PlayerFollow : MonoBehaviour {
	private Camera cam;
	private float camSize = 10;
	private float fullscreenSize = 10;
	private bool isFullscreen;
	private Vector3 middlePos;
	public	GameObject	player;

	void Start () {
		float[] levelSize = new float[4] { 0, 0, 0, 0 }; //0=left 1=top 2=right 3=bottom
		cam = gameObject.GetComponent<Camera>();
		Transform[] walls = GameObject.Find("walls").GetComponentsInChildren<Transform>();
		foreach (Transform w in walls) {
			if (w.transform.position.x < levelSize[0]) {
				levelSize[0] = w.transform.position.x;
			} else if (w.transform.position.x > levelSize[2]) {
				levelSize[2] = w.transform.position.x;
			}
			if (w.transform.position.y > levelSize[1]) {
				levelSize[1] = w.transform.position.y;
			} else if (w.transform.position.y < levelSize[3]) {
				levelSize[3] = w.transform.position.y;
			}
		}
		if ((levelSize[1] - levelSize[3]) / (levelSize[2] - levelSize[0]) < 2/3) {
			fullscreenSize = (levelSize[1] - levelSize[3]) - 10;
		} else {
			fullscreenSize = (levelSize[2] - levelSize[0]) - 10;
		}
		if (fullscreenSize < 10) {
			fullscreenSize = 10;
		}
		middlePos = new Vector3((levelSize[2] + levelSize[0]) / 2, (levelSize[1] + levelSize[3]) / 2, -10);
	}

	//TODO: do it better (get level size and only move when necessary)
	void Update () {
		if (!isFullscreen) {
			transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
		} else {
			transform.position = new Vector3(middlePos.x, middlePos.y, middlePos.z);
		}
		if (Input.GetKeyDown(KeyCode.Tab)) {
			if (isFullscreen) {
				isFullscreen = false;
				cam.orthographicSize = camSize;
			} else {
				isFullscreen = true;
				cam.orthographicSize = fullscreenSize;
			}
		}
	}
}
