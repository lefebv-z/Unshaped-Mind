using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameMenus : MonoBehaviour {
	public static GameObject	nextScreen;

	public GameObject	 pauseMenuFirstObject;
	public GameObject 	endMenuFirstObject;
	public GameObject 	confirmMenuFirstObject;

	public GameObject 	_pauseMenu;
	public GameObject	_endLevelMenu;
	public GameObject 	_confirmMenu;

	public Sprite		returnMenuSprite;
	public Sprite		quitGameSprite;

	private GameObject	HowToMenu;
	private GameObject	OptionsMenu;
	private GameObject	CreditsMenu;
	private GameObject 	HowToFirstObject;
	private GameObject 	OptionsFirstObject;
	private GameObject 	CreditsFirstObject;

	private SoundManager	SoundManager;
	static public float		pauseVolumeDivider = 3.0f;

	public GameManager _manager;
	GameState _oldState;
	static EventSystem _es;

	private bool AllowQuitting = false;

	// Use this for initialization
	void Start () {
		_es = EventSystem.current;
		
		/*if (PlayerPrefs.GetInt("currentLevel") >= PlayerPrefs.GetInt("maxLevel")) {
			Button[] buttons = _pauseMenu.GetComponentsInChildren<Button>();
			foreach (Button b in buttons) {
				if (b.name == "NextLevel") {
					b.interactable = false;
				}
			}
			buttons = _endLevelMenu.GetComponentsInChildren<Button>();
			foreach (Button b in buttons) {
				if (b.name == "NextLevel") {
					b.interactable = false;
				}
			}
		}*/
        /* adding new sound*/
        //SoundManager sm = (SoundManager)FindObjectOfType(typeof(SoundManager));
        //get all button and add play song event
		_pauseMenu.SetActive(false);
		_endLevelMenu.SetActive(false);

		SoundManager = GameObject.Find("SoundSystemManager").GetComponent<SoundManager>();

		HowToMenu = GameObject.Find("How to play");
		CreditsMenu = GameObject.Find("Credits");
		OptionsMenu = GameObject.Find("Settings");

		HowToFirstObject = HowToMenu.transform.Find("ReturnButton").gameObject;
		CreditsFirstObject = CreditsMenu.transform.Find("ReturnButton").gameObject;
		OptionsFirstObject = OptionsMenu.transform.Find("ReturnButton").gameObject;

		ResetVolumeSliders();

		HowToMenu.SetActive(false);
		CreditsMenu.SetActive(false);
		OptionsMenu.SetActive(false);

		nextScreen = GameObject.Find("NextStartScreen");
		nextScreen.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (!_pauseMenu.activeSelf) {
				if (_manager.gameState != GameState.Menu) {
					_oldState = _manager.gameState;
					_manager.changeGameState(GameState.Menu);
					_pauseMenu.SetActive(true);
					_es.SetSelectedGameObject(pauseMenuFirstObject);
					//Decrease volume on pause
					SoundManager.BGMVolumeChange(SoundManager.getBGMVolume() / pauseVolumeDivider);
				}
			} else {
				_manager.changeGameState(_oldState);
				_pauseMenu.SetActive(false);
				SoundManager.BGMVolumeChange(SoundManager.getBGMVolume() * pauseVolumeDivider);
			}
		}
		/*
		if (_manager.gameState == GameState.Menu && !_pauseMenu.activeSelf && !_endLevelMenu.activeSelf) {
			_oldState = _manager.gameState;
			_endLevelMenu.SetActive(true);
			_es.SetSelectedGameObject(endMenuFirstObject);
		}
		*/
	}

	void ResetVolumeSliders() {
		Slider bgmSlider = OptionsMenu.transform.Find("BGMVolumeSlider").GetComponent<Slider>();
		Slider effectSlider = OptionsMenu.transform.Find("VolumeSlider").GetComponent<Slider>();

		bgmSlider.value = SoundManager.getBGMVolume();
		effectSlider.value = SoundManager.getEffectsVolume();

		bgmSlider.onValueChanged.AddListener(delegate{SoundManager.BGMVolumeChange(bgmSlider.value);});
		effectSlider.onValueChanged.AddListener(delegate{SoundManager.EffectsVolumeChange(effectSlider.value);});
	}

	public void Win() {
		SoundManager.BGMVolumeChange(SoundManager.getBGMVolume() / pauseVolumeDivider);
		_endLevelMenu.SetActive (true);
		_es.SetSelectedGameObject (endMenuFirstObject);
	}

	public void Continue() {
		SoundManager.BGMVolumeChange(SoundManager.getBGMVolume() * pauseVolumeDivider);
		_manager.changeGameState(_oldState);
		_pauseMenu.SetActive(false);
	}

	public static void GoToNextLevel() {
		SoundManager sm = GameObject.Find ("SoundSystemManager").GetComponent<SoundManager> ();
		sm.BGMVolumeChange(sm.getBGMVolume() * pauseVolumeDivider);
		if (GameManager.nextLevelExists (true) && !GameManager.nextStratum) {
			Application.LoadLevel("Stratum" + PlayerPrefs.GetInt("currentStratum").ToString());
		} else if (GameManager.nextStratum) {
			_es.SetSelectedGameObject(null);
			GameManager.nextStratum = false;
			nextScreen.SetActive(true);
		} else {
			Application.LoadLevel("Menu");
		}
	}

	//Used to quit all submenus because there's no way i'm doing 3 more functions for the same thing
	public void ExitConfirm() {
		CreditsMenu.SetActive(false);
		HowToMenu.SetActive(false);
		OptionsMenu.SetActive(false);
		_confirmMenu.SetActive(false);
		_pauseMenu.SetActive(true);
		_es.SetSelectedGameObject(pauseMenuFirstObject);
	}

	public void NextLevel() {
		GoToNextLevel();
	}

	public void Restart() {
		SoundManager.BGMVolumeChange(SoundManager.getBGMVolume() * pauseVolumeDivider);
		_manager.RestartLevel();
	}

	public void HowTo() {
		HowToMenu.SetActive(true);
		_pauseMenu.SetActive(false);
		_es.SetSelectedGameObject(HowToFirstObject);
	}

	public void Credits() {
		CreditsMenu.SetActive(true);
		_pauseMenu.SetActive(false);
		_es.SetSelectedGameObject(CreditsFirstObject);
	}

	public void Options() {
		OptionsMenu.SetActive(true);
		_pauseMenu.SetActive(false);
		_es.SetSelectedGameObject(OptionsFirstObject);
	}

	public void MainMenu() {
		_confirmMenu.SetActive(true);
		_pauseMenu.SetActive(false);
		_es.SetSelectedGameObject(confirmMenuFirstObject);
		_confirmMenu.transform.FindChild("Title").GetComponent<Image>().sprite = returnMenuSprite;
		_confirmMenu.transform.FindChild("Yes").GetComponent<Button>().onClick.AddListener(() => {
			SoundManager.BGMVolumeChange(SoundManager.getBGMVolume() * pauseVolumeDivider);
			Application.LoadLevel("Menu");});
	}

	public void Exit() {
		_confirmMenu.SetActive(true);
		_pauseMenu.SetActive(false);
		_es.SetSelectedGameObject(confirmMenuFirstObject);
		_confirmMenu.transform.FindChild("Title").GetComponent<Image>().sprite = quitGameSprite;
		_confirmMenu.transform.FindChild("Yes").GetComponent<Button>().onClick.AddListener(() => {AllowQuitting = true; Application.Quit();});
	}
		

	void OnApplicationQuit() {
		if (!AllowQuitting) {
			Application.CancelQuit();
			Exit();
		}
	}
}