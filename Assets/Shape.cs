using UnityEngine;
using System.Collections;

public enum ShapeType {
	Square,
	Circle,
	Triangle,
	Hexagone
}

//TODO : separate playercontroller & shape attributes in 2 classes
public class Shape : MonoBehaviour {
	
	public ShapeType currentType;
	private float speed = 10f;
	public GameManager gameManager;

	//TODO spritesheet

	void Start()
	{
		currentType = ShapeType.Square; //TODO : get shape of last level's end 
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();//TODO : find why initialization in the GameManager fails
	}
	
	void Update ()
	{
		if (gameManager.gameState != GameState.Playing)
			return;

		float v = Input.GetAxisRaw("Vertical");
		float v2 = Input.GetAxisRaw("Horizontal");

		GetComponent<Rigidbody2D>().velocity = new Vector2(v2, v) * speed;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		//print ("TRIGGER ENTER ");
		if (gameManager.gameState != GameState.Playing)
			return;

		if (other.gameObject.tag == (currentType.ToString () + "Hole")) {
			print ("win");
			gameManager.gameState = GameState.EndingAnimation;
			gameManager.isWinning = true;
		} else {
			print ("wrong hole");
			//TODO: death gestion
		}
	}
	
}
