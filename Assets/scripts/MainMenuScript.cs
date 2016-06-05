using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class MainMenuScript : MonoBehaviour {
	public GameObject[] menus; //0 == MainMenu; 1 == levels; 2 == how to play; 3 == Settings; 4 == Credits
	public int numStratums = 4;

	public int levelsPerPage = 6;
	public int[] numLevelsPerStratum;
	public string[] strateNames;

	private List<GameObject> unactivatedObjects;
	private int currentIndex = 0;
	private int currentSubIndex = 0;
	private EventSystem eventSystem;
	private Button[] buttons;
	private Button[] subButtons;
	private Text stratumNameText;

	void Start() {
		unactivatedObjects = new List<GameObject>();
		PlayerPrefs.SetInt("numStratums", numStratums);
		PlayerPrefs.SetInt("levelsPerStartum", levelsPerPage);
		for (int i = 1; i <= numStratums; i++)
			PlayerPrefs.SetInt ("levelsInStratum" + i, numLevelsPerStratum [i - 1]);

		eventSystem = GameObject.FindObjectOfType<EventSystem> ();
		buttons = menus[0].gameObject.GetComponentsInChildren<Button>();
		eventSystem.SetSelectedGameObject(buttons[currentIndex].gameObject);
	}

	// Update is called once per frame
	void Update () {

		//TODO ON LAISSE P ET N OU PAS?
//		if (menus[1].activeInHierarchy && Input.GetKeyDown(KeyCode.P)) {
//			eventSystem.SetSelectedGameObject(GameObject.Find("Left"));
//		} else if (menus[1].activeSelf && Input.GetKeyDown(KeyCode.N)) {
//			eventSystem.SetSelectedGameObject(GameObject.Find("Right"));
//		}
		if (menus [0].activeSelf) {//To detect if we are on the main page
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
			SelectButton (currentIndex);
		} else {
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				currentSubIndex++;
				if (currentSubIndex == subButtons.Length)
					currentSubIndex = 0;
			} else if (Input.GetKeyUp (KeyCode.UpArrow)) {
				currentSubIndex--;
				if (currentSubIndex == -1)
					currentSubIndex = subButtons.Length - 1;
			} else {
				return;
			}
			SelectSubButton (currentSubIndex);
		}
	}
	
	public void SelectButton(int buttonNb) {
		foreach (Button b in buttons)
			b.interactable = false;
		
		buttons [buttonNb].interactable = true;
		eventSystem.SetSelectedGameObject(buttons[buttonNb].gameObject);
		
		currentIndex = buttonNb;//Keyboard cursor go where mouse is
	}
	
	public void SelectSubButton(int buttonNb) {
		foreach (Button b in subButtons)
			b.interactable = false;
		
		subButtons [buttonNb].interactable = true;
		eventSystem.SetSelectedGameObject(subButtons[buttonNb].gameObject);
		
		currentSubIndex = buttonNb;//Keyboard cursor go where mouse is
	}

	public void PlayLevel(int stratum, int level) {
		PlayerPrefs.SetInt("currentStratum", stratum);
		PlayerPrefs.SetInt("currentLevel", level);

		Application.LoadLevel("Stratum" + stratum.ToString());
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
		subButtons = menus[1].gameObject.GetComponentsInChildren<Button>();
		foreach (Button b in subButtons) {
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
				if ((page + 1) >= numStratums){
					b.interactable = false;
				} else {
					b.interactable = true;
				}
			}
		}

		//Display stratum name
		stratumNameText = GameObject.Find ("Stratum").GetComponent<Text>();
		int stratum = page + 1;
		stratumNameText.text = "Stratum " + stratum;

		GameObject[] levels = GameObject.FindGameObjectsWithTag("Level");
		foreach (GameObject level in levels) {
			int num = int.Parse(level.name) + 1;
			if (num <= numLevelsPerStratum[page]) {
				//level.GetComponentInChildren<Text>().text = num.ToString();
				Button b = level.GetComponent<Button>();
				b.onClick.RemoveAllListeners();
				b.onClick.AddListener(delegate{PlayLevel(stratum, num);});

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.Select;
                entry.callback.AddListener((eventData) => { updateScore(stratum, num); });
                EventTrigger trigger = b.gameObject.AddComponent<EventTrigger>();
                if (trigger != null)
                    trigger.triggers.Add(entry);
			} else {
				level.SetActive(false);
				unactivatedObjects.Add(level);
			}
		}
		SelectSubButton (0);
//		eventSystem.SetSelectedGameObject(
//			subButtons[0].gameObject);
	}

    public void updateScore(int stratum, int num)
    {
        ScoreManagerScript scoring = (ScoreManagerScript)(GameObject.FindObjectOfType(typeof(ScoreManagerScript)));
        scoring.recapScore(stratum, num);
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
