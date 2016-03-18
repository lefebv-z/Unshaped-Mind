using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

public enum GameState
{
	Start,
	Playing,
	EndingAnimation,
	End
}

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
	//private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.

	public GameState gameState;
	public int currentLevel;
	public int remainingTransformation;//start at max, decrease during the game until it reaches 0
	public bool isWinning = false;//enum instead ?

	public GameObject playerShape;
	private Shape shape;

	// TODO : get holes like :	ArmyUnit[] armyUnits = FindObjectsOfType(typeof(ArmyUnit)) as ArmyUnit[];


	//Awake is always called before any Start functions
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);    
		
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);		
	}
	
	// Use this for initialization
	void Start ()
	{
		currentLevel = 1;//TODO : get the good lvl from menu
		gameState = GameState.Start;
		shape = playerShape.GetComponent<Shape>();
		shape.gameManager = this;
	}

	//TODO : put gui in a seperate class
	void OnGUI()
	{
		switch (gameState)
		{
		case GameState.Start:
			//GUI.Label(new Rect(0,0,100,100), "Start !");
			print ("Start");
			gameState = GameState.Playing;
			break;
		default:
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch (gameState)
		{
		case GameState.Playing:
			//move cube ?
			break;
		case GameState.EndingAnimation:
			//WaitAndExit();//TODO play animation
			gameState = GameState.End;
			break;
		case GameState.End:
			EndGame();
			break;
		default:
			break;
		}
	}

	
//	IEnumerator WaitAndExit()
//	{
//		yield return new WaitForSeconds (1);
//	}

	void EndGame()
	{
		if (isWinning) {
			if (Application.loadedLevelName == "Level1") {
				print("load");
				gameState = GameState.Start;
				Application.LoadLevel ("Level2");//TODO levels list
			}
			else {
				print("quit");
				Application.Quit();
			}
		}
		else
			Application.LoadLevel(Application.loadedLevel);
	}
}





//
//
//public class GameManager : MonoBehaviour
//{
//	
//	public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
//	private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
////	private int level = 3;                                  //Current level number, expressed in game as "Day 1".
//	
//	//Awake is always called before any Start functions
//	void Awake()
//	{
//		//Check if instance already exists
//		if (instance == null)
//			
//			//if not, set instance to this
//			instance = this;
//		
//		//If instance already exists and it's not this:
//		else if (instance != this)
//			
//			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
//			Destroy(gameObject);    
//		
//		//Sets this to not be destroyed when reloading scene
//		DontDestroyOnLoad(gameObject);
//		
//		//Get a component reference to the attached BoardManager script
//		boardScript = GetComponent<BoardManager>();
//		
//		//Call the InitGame function to initialize the first level 
//		InitGame();
//	}
//	
//	//Initializes the game for each level.
//	void InitGame()
//	{
//		//Call the SetupScene function of the BoardManager script, pass it current level number.
//		boardScript.SetupScene(level);
//		
//	}
//	
//	
//	
//	//Update is called every frame.
//	void Update()
//	{
//		
//	}
//}