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
	private int maxLevel = 8;
	public int remainingTransformation = 15;//start at max, decrease during the game until it reaches 0
    private int maxTransfo;
	public bool isWinning = false;//enum instead ?

	public GameObject playerShape;
	private Shape shape;
    private SoundManager sm;
//	private Camera mainCamera;

    private bool endOccuring; //if the player is in thole
    private Vector3 endDestination; //position of center of hole

	void Awake()
	{
		currentLevel = PlayerPrefs.GetInt("currentLevel");
		maxLevel = PlayerPrefs.GetInt("maxLevel");
	}

	void Start ()
	{
        endOccuring = false;
        sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
//		mainCamera = (Camera)(GameObject.FindObjectOfType(typeof(Camera)));
        maxTransfo = remainingTransformation;
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
    void Update()
    {

        if (endOccuring == true && endDestination != shape.transform.position)
            shape.transform.position = Vector3.MoveTowards(shape.transform.position, endDestination, 0.5f * Time.deltaTime);
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
                            endOccuring = false;
                            gameState = GameState.Menu;
                        }
                        break;
                    }
                    else
                    {
                        //TODO: reset animation/sound
                    }
                    //mainCamera.GetComponent<Animator>().enabled = true;
                    //anim
                    gameState = GameState.End;
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
		gameState = GameState.EndingAnimation;
	}

	void EndGame() {
		if (isWinning) {
			if (currentLevel < maxLevel) {
                if (sm != null)
                    sm.PlayLvlEnd();
				gameState = GameState.Start;
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
		return PlayerPrefs.GetInt("currentPage");
	}
	
	public int getLevel()
	{
		return currentLevel;
	}

    void MoveToHole()
    {
        endOccuring = true;
        endDestination = shape.getEndPosition();
        shape.PlayParticle();
    }

}

