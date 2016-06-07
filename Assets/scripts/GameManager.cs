using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

public enum GameState
{
	Start = 0,
	Playing,
	EndingAnimation,
	End,
	Menu,
	Fullscreen,
	GameStateCount,
	PortalAnimation
}

public class GameManager : MonoBehaviour {
	public GameState gameState;
	public static int currentLevel = 0;
	//private int maxLevel = 8;
	public int remainingTransformation = 15;//start at max, decrease during the game until it reaches 0
    private int maxTransfo;
	public bool isWinning = false;//enum instead ?

	public GameObject playerShape;
	private Shape shape;
    private SoundManager sm;
	private InGameMenus	menu;
    private ScoreManagerScript scoring;
//	private Camera mainCamera;

    private bool endOccuring; //if the player is in thole
    private Vector3 endDestination; //position of center of hole

	void Awake()
	{
		currentLevel = PlayerPrefs.GetInt("currentLevel");
		menu = gameObject.GetComponent<InGameMenus> ();
        
		//maxLevel = PlayerPrefs.GetInt("maxLevel");
	}

	void Start ()
	{
        endOccuring = false;
        sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
//		mainCamera = (Camera)(GameObject.FindObjectOfType(typeof(Camera)));
        maxTransfo = remainingTransformation;
		changeGameState(GameState.Start);
		shape = playerShape.GetComponent<Shape>();
		shape.gameManager = this;
	}

	//TODO : put gui in a seperate class
	void OnGUI()
	{
		switch (gameState) {
			case GameState.Start:
				Debug.Log("Start");
				changeGameState(GameState.Playing);
				break;
			default:
				break;
		}
	}
	
	// Update is called once per frame
    void Update()
    {

        if (endOccuring == true && endDestination != shape.transform.position)
		{
			shape.transform.position = Vector3.MoveTowards (shape.transform.position, endDestination, 1f * Time.deltaTime);
		}
        else
        {
            switch (gameState)
            {
                case GameState.Playing:
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        RestartLevel();
                    }
                    break;
                case GameState.EndingAnimation:
                    if (isWinning)
                    {
                        
                        MoveToHole();
                        if (endOccuring == true && endDestination == shape.transform.position)
                        {
                            if (sm != null)
                                sm.PlayLvlEnd();
                            
							menu.Win();
                            endOccuring = false;
                            changeGameState(GameState.Menu);
                            scoring = (ScoreManagerScript)(GameObject.FindObjectOfType(typeof(ScoreManagerScript)));
                            if (scoring != null)
                                scoring.starScore();
                            
					}
                        break;
                    }
                    else
                    {
                        //TODO: reset animation/sound
                    }
                    //mainCamera.GetComponent<Animator>().enabled = true;
                    //anim
                    changeGameState(GameState.End);
                    break;
                case GameState.End:
                    EndGame();
                    break;
                default:
                    break;
            }
        }
    }

	public void RestartLevel()
	{
		if (sm != null)
			sm.PlayRestart ();
		isWinning = false;
		changeGameState(GameState.EndingAnimation);
	}

	void EndGame() {
		if (isWinning) {
			if (nextLevelExists()) {
                if (sm != null)
                    sm.PlayLvlEnd();
				changeGameState(GameState.Start);
				InGameMenus.GoToNextLevel();
			} else {
				Application.LoadLevel("Menu");
			}
		} else {
			Application.LoadLevel(Application.loadedLevel);
		}
	}

    public int getMaxTransfo()
    {
        return maxTransfo;
    }
	
	public int getStratum()
	{
		return PlayerPrefs.GetInt("currentStratum");
	}
	
	public int getLevel()
	{
		return currentLevel;
	}

	public int getLevelsInCurrentStratum() {
		return PlayerPrefs.GetInt ("levelsInStratum" + PlayerPrefs.GetInt("currentStratum"));
	}

	//Check if next level exist.
	//if so and saveNext is true, then put next level in pref
	public static bool nextLevelExists(bool saveNext = false) {
		int level = PlayerPrefs.GetInt("currentLevel");
		int stratum = PlayerPrefs.GetInt("currentStratum");
		//int levelsPerStartum = PlayerPrefs.GetInt("levelsPerStartum");
		level++;
		if (level > PlayerPrefs.GetInt ("levelsInStratum" + stratum)) {
			level = 1;
			stratum++;
		}
		if (stratum > PlayerPrefs.GetInt ("numStratums")) {
			return false;
		}
		if (saveNext) {
			PlayerPrefs.SetInt("currentLevel", level);
			PlayerPrefs.SetInt("currentStratum", stratum);
		}
		return true;
	}

    void MoveToHole()
    {
        endOccuring = true;
        endDestination = shape.getEndPosition();
        shape.PlayParticle();
    }

	public void changeGameState(GameState newState)
	{
		gameState = newState;
		switch (gameState) {
		case GameState.Playing:
			Cursor.visible = false;
			break;
		}
	}

}

