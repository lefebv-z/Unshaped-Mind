using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenuScript : MonoBehaviour {
	public GameObject[] menus; //0 == MainMenu; 1 == levels; 2 == Settings; 3 == Credits
	public int numLevels = 2;
	public int levelsPerPage = 6;
	List<GameObject> unactivatedObjects;

	void Start() {
		unactivatedObjects = new List<GameObject>();
	}

	public void PlayLevel(int level) {
		PlayerPrefs.SetInt("currentLevel", level);
		Application.LoadLevel("Level" + level.ToString());
	}

	void reactivateObjects() {
		if (unactivatedObjects.Count > 0) {
			foreach (GameObject o in unactivatedObjects) {
				o.SetActive(true);
			}
			unactivatedObjects.Clear();
		}
	}

	public void LevelPicker(int page) {
		menus[0].SetActive(false);
		reactivateObjects();
		menus[1].SetActive(true);
		Button[] buttons = menus[1].gameObject.GetComponentsInChildren<Button>();
		foreach (Button b in buttons) {
			if (b.name == "Left") {
				b.onClick.RemoveAllListeners();
				b.onClick.AddListener(() => {LevelPicker(page - 1);});
				if (page == 0) {
					b.interactable = false;
				} else {
					b.interactable = true;
				}
			} else if (b.name == "Right") {
				b.onClick.RemoveAllListeners();
				b.onClick.AddListener(() => {LevelPicker(page + 1);});
				if ((page + 1) * levelsPerPage + 1 > numLevels) {
					b.interactable = false;
				} else {
					b.interactable = true;
				}
			}
		}
		GameObject[] levels = GameObject.FindGameObjectsWithTag("Level");
		foreach (GameObject level in levels) {
			int num = page * levelsPerPage + int.Parse(level.name) + 1;
			if (num <= numLevels) {
				level.GetComponentInChildren<Text>().text = num.ToString();
				Button b = level.GetComponent<Button>();
				b.onClick.RemoveAllListeners();
				b.onClick.AddListener(delegate{PlayLevel(num);});
			} else {
				level.SetActive(false);
				unactivatedObjects.Add(level);
			}
		}
	}

	public void Settings() {
		Debug.Log("Settings");
		menus[0].SetActive(false);
		menus[2].SetActive(true);
	}
	
	public void Credits() {
		Debug.Log("Credits");
		menus[0].SetActive(false);
		menus[3].SetActive(true);
	}
	
	public void Exit() {
		Debug.Log("Exit");
		Application.Quit();
	}

	public void Return() {
		Debug.Log("Return");
		foreach (GameObject o in menus) {
			o.SetActive(false);
		}
		reactivateObjects();
		menus[0].SetActive(true);
	}
}
