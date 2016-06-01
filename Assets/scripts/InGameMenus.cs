using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameMenus : MonoBehaviour {
	public GameObject pauseMenuFirstObject;
	public GameObject endMenuFirstObject;
	public GameObject confirmMenuFirstObject;

	public GameObject _pauseMenu;
	public GameObject _endLevelMenu;
	public GameObject _confirmMenu;
	public GameManager _manager;
	GameState _oldState;
	EventSystem _es;

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
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
			if (!_pauseMenu.activeSelf) {
				_oldState = _manager.gameState;
				_manager.changeGameState(GameState.Menu);
				_pauseMenu.SetActive(true);
				_es.SetSelectedGameObject(pauseMenuFirstObject);
			} else {
				_manager.changeGameState(_oldState);
				_pauseMenu.SetActive(false);
			}
		}
		if (_manager.gameState == GameState.Menu && !_pauseMenu.activeSelf && !_endLevelMenu.activeSelf) {
			_oldState = _manager.gameState;
			_endLevelMenu.SetActive(true);
			_es.SetSelectedGameObject(endMenuFirstObject);
		}
	}

	public void Win() {
		_endLevelMenu.SetActive (true);
		_es.SetSelectedGameObject (endMenuFirstObject);
	}

	public void Continue() {
		_manager.changeGameState(_oldState);
		_pauseMenu.SetActive(false);
	}

	public static void GoToNextLevel() {
		if (GameManager.nextLevelExists (true)) {
			Application.LoadLevel("Stratum" + PlayerPrefs.GetInt("currentStratum").ToString());
		} else {
			Application.LoadLevel("Menu");
		}
	}

	public void ExitConfirm() {
		_confirmMenu.SetActive(false);
		_pauseMenu.SetActive(true);
		_endLevelMenu.SetActive(false);
		_es.SetSelectedGameObject(pauseMenuFirstObject);
	}

	public void NextLevel() {
		GoToNextLevel();
	}

	public void Restart() {
		_manager.RestartLevel();
	}

	public void MainMenu() {
		_confirmMenu.SetActive(true);
		_pauseMenu.SetActive(false);
		_endLevelMenu.SetActive(false);
		_es.SetSelectedGameObject(confirmMenuFirstObject);
		_confirmMenu.transform.FindChild("Title").GetComponent<Text>().text = "Return to main menu";
		_confirmMenu.transform.FindChild("Yes").GetComponent<Button>().onClick.AddListener(() => {Application.LoadLevel("Menu");});
	}

	public void Exit() {
		_confirmMenu.SetActive(true);
		_pauseMenu.SetActive(false);
		_endLevelMenu.SetActive(false);
		_es.SetSelectedGameObject(confirmMenuFirstObject);
		_confirmMenu.transform.FindChild("Title").GetComponent<Text>().text = "Quit game";
		_confirmMenu.transform.FindChild("Yes").GetComponent<Button>().onClick.AddListener(() => {Application.Quit();});
	}
}