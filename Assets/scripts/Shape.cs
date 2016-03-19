using UnityEngine;
using System.Collections;

public enum ShapeType {
	Square = 0,
	Triangle = 1,
	Circle = 2
}

//TODO : separate playercontroller & shape attributes in 2 classes
public class Shape : MonoBehaviour {
	private float speed = 10f;
	public GameManager gameManager;
	public Material colorFilterMat;
	static Sprite[] sprites;
	static public ShapeType currentType = ShapeType.Square;
	
	Color[] _colors = new Color[3] { //Same order than ShapeType
		new Color(0, 255, 0), //Square color
		new Color(0, 0, 255), //Triangle color
		new Color(255, 0, 0)  //Circle color
	};
	KeyCode[] shapeButton = new KeyCode[] { //Same order than ShapeType
		KeyCode.LeftArrow,
		KeyCode.UpArrow,
		KeyCode.RightArrow
	};

	void Start()
	{
		sprites = Resources.LoadAll<Sprite>("SpriteSheet");
		colorFilterMat.color = _colors[(int)currentType];
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();//TODO : find why initialization in the GameManager fails
	}

	void ChangeShape(ShapeType newShape) {
		currentType = newShape;
		colorFilterMat.color = _colors[(int)newShape];
		gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)newShape];
	}

	void Update ()
	{
		if (gameManager.gameState != GameState.Playing) {
			return;
		}

		if (Input.GetKeyDown(shapeButton[(int)ShapeType.Square])) {
			ChangeShape(ShapeType.Square);
		} else if (Input.GetKeyDown(shapeButton[(int)ShapeType.Triangle])) {
			ChangeShape(ShapeType.Triangle);
		} else if (Input.GetKeyDown(shapeButton[(int)ShapeType.Circle])) {
			ChangeShape(ShapeType.Circle);
		}

		float v = Input.GetAxisRaw("Vertical");
		float v2 = Input.GetAxisRaw("Horizontal");

		GetComponent<Rigidbody2D>().velocity = new Vector2(v2, v) * speed;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		//print ("TRIGGER ENTER ");
		if (gameManager.gameState != GameState.Playing) {
			return;
		}

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
