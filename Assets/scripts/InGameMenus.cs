using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

//var es : EventSystem = EventSystemManager.currentSystem;
//es.SetSelectGameObject(go, null);

public class InGameMenus : MonoBehaviour {
	public GameObject pauseMenuFirstObject;
	public GameObject endMenuFirstObject;

	public GameObject _pauseMenu;
	public GameObject _endLevelMenu;
	public GameManager _manager;
	GameState _oldState;
	EventSystem _es;

	// Use this for initialization
	void Start () {
		_es = EventSystem.current;

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
				_manager.gameState = GameState.Menu;
				_pauseMenu.SetActive(true);
				_es.SetSelectedGameObject(pauseMenuFirstObject);
			} else {
				_manager.gameState = _oldState;
				_pauseMenu.SetActive(false);
			}
		}
		if (_manager.gameState == GameState.Menu && !_pauseMenu.activeSelf && !_endLevelMenu.activeSelf) {
			_oldState = _manager.gameState;
			_endLevelMenu.SetActive(true);
			_es.SetSelectedGameObject(endMenuFirstObject);
		}
	}

	public void Continue() {
		_manager.gameState = _oldState;
		_pauseMenu.SetActive(false);
	}

	public static void GoToNextLevel() {
		int level = PlayerPrefs.GetInt("currentLevel");
		int levelsPerStartum = PlayerPrefs.GetInt("levelsPerStartum");
		level++;
		PlayerPrefs.SetInt("currentLevel", level);
		int i = 1;
		while (level >= i)
			i += levelsPerStartum;
		i -= levelsPerStartum;
		Debug.Log(level + " " + PlayerPrefs.GetInt("maxLevel"));
		if (level < PlayerPrefs.GetInt("maxLevel")) {
			Application.LoadLevel("Level" + i.ToString());
		} else {
			Application.LoadLevel("Menu");
		}
	}

	public void NextLevel() {
		GoToNextLevel();
	}

	public void Restart() {
		_manager.RestartLevel();
	}

	public void MainMenu() {
		Application.LoadLevel("Menu");
	}
}