using UnityEngine;
using System.Collections;

public class CheatCode : MonoBehaviour {
	void Update () {
		if (Input.GetButtonDown("Win")) {
			Shape _playerShape = GameObject.FindObjectOfType<Shape>();
			string[] tagNames = new string[] {"SquareHole", "TriangleHole", "CircleHole", "HexagonHole"};
			int i = 0;
			foreach (string tagName in tagNames) {
				GameObject[] holes = GameObject.FindGameObjectsWithTag(tagName);
				if (holes.Length > 0) {
					_playerShape.ChangeShape((ShapeType)i);
					_playerShape.transform.position = holes[0].transform.position + new Vector3(0.5f, 0.5f, 0.0f);
					return;
				}
				i++;
			}
		} else if (Input.GetButtonDown("Lose")) {
			Debug.Log("lose");
			GameObject.FindObjectOfType<GameManager>().remainingTransformation = 0;
		} else if (Input.GetButtonDown("NextLevel")) {
			InGameMenus inGameMenu = GameObject.FindObjectOfType<InGameMenus>();
			inGameMenu.NextLevel();
		} else if (Input.GetButtonDown("PreviousLevel")) {
			if (PreviousLevelExist()) {
				Application.LoadLevel("Stratum" + PlayerPrefs.GetInt("currentStratum").ToString());
			} else {
				Application.LoadLevel("Menu");
			}
		}
	}

	bool PreviousLevelExist() {
		int level = PlayerPrefs.GetInt("currentLevel");
		int stratum = PlayerPrefs.GetInt("currentStratum");
		level--;
		if (level < 1) {
			stratum--;
			if (stratum < 1) {
				return false;
			}
			level = PlayerPrefs.GetInt ("levelsInStratum" + stratum);
		}
		PlayerPrefs.SetInt("currentLevel", level);
		PlayerPrefs.SetInt("currentStratum", stratum);
		return true;
	}
}
