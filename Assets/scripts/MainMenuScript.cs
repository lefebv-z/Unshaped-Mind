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

	public GameObject[] arrowDisabledSprites;

	private List<GameObject> unactivatedObjects;
	private int currentIndex = 0;
	private int currentSubIndex = 0;
	private EventSystem eventSystem;
	private Button[] buttons;
	private Button[] subButtons;
	private Text stratumNameText;
	private bool AllowQuitting = false;
    private SoundManager sm;

	void Start() {
		unactivatedObjects = new List<GameObject>();
		PlayerPrefs.SetInt("numStratums", numStratums);
		PlayerPrefs.SetInt("levelsPerStartum", levelsPerPage);
		for (int i = 1; i <= numStratums; i++)
			PlayerPrefs.SetInt ("levelsInStratum" + i, numLevelsPerStratum [i - 1]);

		eventSystem = GameObject.FindObjectOfType<EventSystem> ();
		buttons = menus[0].gameObject.GetComponentsInChildren<Button>();
		eventSystem.SetSelectedGameObject(buttons[currentIndex].gameObject);
		Cursor.visible = true;
        sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
	}

	// Update is called once per frame
	void Update () {

		//TODO ON LAISSE P ET N OU PAS?
//		if (menus[1].activeInHierarchy && Input.GetKeyDown(KeyCode.P)) {
//			eventSystem.SetSelectedGameObject(GameObject.Find("Left"));
//		} else if (menus[1].activeSelf && Input.GetKeyDown(KeyCode.N)) {
//			eventSystem.SetSelectedGameObject(GameObject.Find("Right"));
//		}
		if (menus [0].activeSelf) {//If we are on the main page
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				ScriptHelper.IncCursor(ref currentIndex, menus.Length);
			} else if (Input.GetKeyUp (KeyCode.UpArrow)) {
				ScriptHelper.DecCursor(ref currentIndex, menus.Length);
			} else {
				return;
			}
			SelectButton (currentIndex);
		} else if (menus [1].activeSelf) {//If we are in the level selection
			//TODO add up and down arrows
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				ScriptHelper.IncCursor(ref currentSubIndex, subButtons.Length);
				while (subButtons[currentSubIndex].IsActive() == false)//skip inactive level buttons
					ScriptHelper.IncCursor(ref currentSubIndex, subButtons.Length);
				//TODO REFACTOR CEST DEGUELASSE !!!!
				if ((subButtons[currentSubIndex].name == "Left" &&
				    arrowDisabledSprites[0].activeSelf == true)
				    ||(subButtons[currentSubIndex].name == "Right" &&
				   		arrowDisabledSprites[1].activeSelf == true)) {
					ScriptHelper.IncCursor(ref currentSubIndex, subButtons.Length);
					while (subButtons[currentSubIndex].IsActive() == false)//skip inactive level buttons
						ScriptHelper.IncCursor(ref currentSubIndex, subButtons.Length);
				}
			} else if (Input.GetKeyUp (KeyCode.LeftArrow)) {
				ScriptHelper.DecCursor(ref currentSubIndex, subButtons.Length);
				while (subButtons[currentSubIndex].IsActive() == false)//skip inactive level buttons
					ScriptHelper.DecCursor(ref currentSubIndex, subButtons.Length);
				//TODO REFACTOR CEST DEGUELASSE !!!!
				if ((subButtons[currentSubIndex].name == "Left" &&
				     arrowDisabledSprites[0].activeSelf == true)
				    ||(subButtons[currentSubIndex].name == "Right" &&
				   arrowDisabledSprites[1].activeSelf == true)) {
					ScriptHelper.DecCursor(ref currentSubIndex, subButtons.Length);
					while (subButtons[currentSubIndex].IsActive() == false)//skip inactive level buttons
						ScriptHelper.DecCursor(ref currentSubIndex, subButtons.Length);
				}
			} else {
				return;
			}
			SelectSubButton (currentSubIndex);
		} else {
			//TODO fix weird behavior in settings menu
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				ScriptHelper.IncCursor(ref currentSubIndex, subButtons.Length);
			} else if (Input.GetKeyUp (KeyCode.UpArrow)) {
				ScriptHelper.DecCursor(ref currentSubIndex, subButtons.Length);
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
        if (sm != null)
            sm.addBSound();
		subButtons = menus[1].gameObject.GetComponentsInChildren<Button>();
		foreach (Button b in subButtons) {
			if (b.name == "Left") {
				b.onClick.RemoveAllListeners();
				b.onClick.AddListener(() => {LevelPicker(page - 1);});
				if (page == 0) {
					b.interactable = false;
					arrowDisabledSprites[0].SetActive(true);
				} else {
					b.interactable = true;
					arrowDisabledSprites[0].SetActive(false);
				}
			} else if (b.name == "Right") {
				b.onClick.RemoveAllListeners();
				b.onClick.AddListener(() => {LevelPicker(page + 1);});
				if ((page + 1) >= numStratums){
					b.interactable = false;
					arrowDisabledSprites[1].SetActive(true);
				} else {
					b.interactable = true;
					arrowDisabledSprites[1].SetActive(false);
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
                //add score reaction

                EventTrigger.Entry entrys = new EventTrigger.Entry();
                entrys.eventID = EventTriggerType.UpdateSelected;
                entrys.callback.AddListener((eventData) => { updateScore(stratum, num); });

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.Select;
                entry.callback.AddListener((eventData) => { updateScore(stratum, num); });
                EventTrigger trigger = b.gameObject.AddComponent<EventTrigger>();
                if (trigger != null)
                {
                    trigger.triggers.Add(entry);
                    trigger.triggers.Add(entrys);
                }
			} else {
				level.SetActive(false);
				unactivatedObjects.Add(level);
			}
		}
		SelectSubButton (1);
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
		if (Screen.fullScreen != menus[3].GetComponentInChildren<Toggle>().isOn) {
			menus[3].GetComponentInChildren<Toggle>().isOn = Screen.fullScreen;
		}
	}

	public void Credits() {
		Debug.Log("Credits");
		menus[0].SetActive(false);
		menus[4].SetActive(true);
		eventSystem.SetSelectedGameObject(
			menus[4].gameObject.GetComponentInChildren<Button>().gameObject);
	}

	public void Exit() {
		menus[5].SetActive(true);
		eventSystem.SetSelectedGameObject(menus[5].GetComponentInChildren<Button>().gameObject);
		menus[5].transform.FindChild("Title").GetComponent<Text>().text = "Quit game";
		menus[5].transform.FindChild("Yes").GetComponent<Button>().onClick.AddListener(() => {AllowQuitting = true; Application.Quit();});
	}

	public void ToggleFullscreen() {
		Screen.fullScreen = !Screen.fullScreen;
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

	void OnApplicationQuit() {
		if (!AllowQuitting) {
			Application.CancelQuit();
			Exit();
		}
	}
}
