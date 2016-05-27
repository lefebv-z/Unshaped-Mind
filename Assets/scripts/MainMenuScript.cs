using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class MainMenuScript : MonoBehaviour {
	public GameObject[] menus; //0 == MainMenu; 1 == levels; 2 == how to play; 3 == Settings; 4 == Credits
	public int numStratums = 1;
	public int numLevels = 6;

	public int levelsPerPage = 6;
	public int[] numLevelsPerStratum;
	public string[] strateNames;

	private List<GameObject> unactivatedObjects;
	private int currentIndex = 0;
	private EventSystem eventSystem;
	private Button[] buttons;
	private Text stratumNameText;

	void Start() {
		unactivatedObjects = new List<GameObject>();
		PlayerPrefs.SetInt("maxLevel", numLevels);
		PlayerPrefs.SetInt("levelsPerStartum", levelsPerPage);

		eventSystem = GameObject.FindObjectOfType<EventSystem> ();
		buttons = menus[0].gameObject.GetComponentsInChildren<Button>();
		eventSystem.SetSelectedGameObject(buttons[currentIndex].gameObject);
	}

	// Update is called once per frame
	void Update () {
		if (menus[1].activeInHierarchy && Input.GetKeyDown(KeyCode.P)) {
			eventSystem.SetSelectedGameObject(GameObject.Find("Left"));
		} else if (menus[1].activeSelf && Input.GetKeyDown(KeyCode.N)) {
			eventSystem.SetSelectedGameObject(GameObject.Find("Right"));
		}
		if (!menus[0].activeSelf)//TODO: cleaner way to do this ?
			return;
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			currentIndex++;
			if (currentIndex == menus.Length)
				currentIndex = 0;
		} else if (Input.GetKeyUp (KeyCode.UpArrow)) {
			currentIndex--;
			if (currentIndex == -1)
				currentIndex = menus.Length - 1;
		} else {
			return;
		}
		eventSystem.SetSelectedGameObject(buttons[currentIndex].gameObject);
	}

	public void PlayLevel(int page, int level) {
		PlayerPrefs.SetInt("currentPage", page);
		PlayerPrefs.SetInt("currentLevel", level);
//		int i = 1;
//		while (level >= i)
//			i += levelsPerPage;
//		i -= levelsPerPage;

		page++;
		//TODO rename to Stratum_
		Application.LoadLevel("Stratum" + page.ToString());// + "_" + level.ToString());
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
				if ((page + 1) * levelsPerPage + 1 > numLevels) {//TODO: change so we can have less than 6 levels
					b.interactable = false;
				} else {
					b.interactable = true;
				}
			}
		}

		//Display stratum name
		stratumNameText = GameObject.Find ("Stratum").GetComponent<Text>();
		int pageDisplay = page + 1;
		stratumNameText.text = "Stratum " + pageDisplay;

		GameObject[] levels = GameObject.FindGameObjectsWithTag("Level");//TODO change to "Stratum"
		foreach (GameObject level in levels) {
			int num = /*page * levelsPerPage +*/ int.Parse(level.name) + 1;
			if (num <= numLevelsPerStratum[page]) {
				//level.GetComponentInChildren<Text>().text = num.ToString();
				Button b = level.GetComponent<Button>();
				b.onClick.RemoveAllListeners();
				b.onClick.AddListener(delegate{PlayLevel(page, num);});
			} else {
				level.SetActive(false);
				unactivatedObjects.Add(level);
			}
		}
		eventSystem.SetSelectedGameObject(
			buttons[0].gameObject);
	}

	public void HowToPlay() {
		Debug.Log("How to play");
		menus[0].SetActive(false);
		menus[2].SetActive(true);
		eventSystem.SetSelectedGameObject(
			menus[2].gameObject.GetComponentInChildren<Button>().gameObject);
	}
	
	public void Settings() {
		Debug.Log("Settings");
		menus[0].SetActive(false);
		menus[3].SetActive(true);
		eventSystem.SetSelectedGameObject(
			menus[3].gameObject.GetComponentInChildren<Button>().gameObject);
	}

	public void Credits() {
		Debug.Log("Credits");
		menus[0].SetActive(false);
		menus[4].SetActive(true);
		eventSystem.SetSelectedGameObject(
			menus[4].gameObject.GetComponentInChildren<Button>().gameObject);
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
		eventSystem.SetSelectedGameObject(buttons[currentIndex].gameObject);
	}
}
