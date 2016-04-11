using UnityEngine;
using System.Collections;

public class InGameMenus : MonoBehaviour {
	GameObject _pauseMenu;
	GameObject _endLevelMenu;
	GameManager _manager;
	GameState _oldState;

	// Use this for initialization
	void Start () {
		_pauseMenu = GameObject.Find("PauseMenu");
		_endLevelMenu = GameObject.Find("EndLevelMenu");
		_manager = GameObject.FindObjectOfType<GameManager>();

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
			} else {
				_manager.gameState = _oldState;
				_pauseMenu.SetActive(false);
			}
		}
		if (_manager.gameState == GameState.Menu && !_pauseMenu.activeSelf && !_endLevelMenu.activeSelf) {
			_oldState = _manager.gameState;
			_endLevelMenu.SetActive(true);
		}
	}

	public void Continue() {
		Debug.Log("continue");
		_manager.gameState = _oldState;
		_pauseMenu.SetActive(false);
	}

	public void NextLevel() {
		Debug.Log("next level");
		int level = PlayerPrefs.GetInt("currentLevel");
		level++;
		PlayerPrefs.SetInt("currentLevel", level);
		Application.LoadLevel("Level" + level.ToString());
	}

	public void Restart() {
		Debug.Log("restart");
		Application.LoadLevel(Application.loadedLevel);
	}

	public void MainMenu() {
		Debug.Log("main menu");
		Application.LoadLevel("Menu");
	}
}