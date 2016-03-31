﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

public enum GameState
{
	Start = 0,
	Playing,
	EndingAnimation,
	End,
	Menu,
	GameStateCount
}

public class GameManager : MonoBehaviour {
	public GameState gameState;
	public static int currentLevel = 0;
	private int maxLevel = 8;
	public int remainingTransformation = 15;//start at max, decrease during the game until it reaches 0
	public bool isWinning = false;//enum instead ?

	public GameObject playerShape;
	private Shape shape;

	// TODO : get holes like :	ArmyUnit[] armyUnits = FindObjectsOfType(typeof(ArmyUnit)) as ArmyUnit[];

	void Awake()
	{
		Debug.Log("load current level =" + currentLevel);
		currentLevel = PlayerPrefs.GetInt("currentLevel");
		maxLevel = PlayerPrefs.GetInt("maxLevel");
	}

	void Start ()
	{
		gameState = GameState.Start;
		shape = playerShape.GetComponent<Shape>();
		shape.gameManager = this;
	}

	//TODO : put gui in a seperate class
	void OnGUI()
	{
		switch (gameState) {
			case GameState.Start:
				Debug.Log("Start");
				gameState = GameState.Playing;
				break;
			default:
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch (gameState) {
			case GameState.Playing:
				if (Input.GetKeyDown(KeyCode.R))
					gameState = GameState.EndingAnimation;
				break;
			case GameState.EndingAnimation:
				if (isWinning) {
					gameState = GameState.Menu;
					break;
				}
				else {
					//TODO: reset animation/sound
				}
				gameState = GameState.End;
				break;
			case GameState.End:
				EndGame();
				break;
			default:
				break;
		}
	}

	void EndGame() {
		if (isWinning) {
			if (currentLevel < maxLevel) {
                SoundManager sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
                if (sm != null)
                    sm.PlayLvlEnd();
				gameState = GameState.Start;
				currentLevel++;
				PlayerPrefs.SetInt("currentLevel", currentLevel);
				Debug.Log("load next level =" + currentLevel + " max=" + maxLevel);
				Application.LoadLevel ("Level" + currentLevel.ToString());
			} else {
				Application.LoadLevel("Menu");
			}
		} else {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}

