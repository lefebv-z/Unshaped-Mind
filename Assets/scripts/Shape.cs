using UnityEngine;
using System.Collections;

public enum ShapeType {
	Square = 0,
	Triangle = 1,
	Circle = 2,
	Hexagon = 3
}

//TODO : separate playercontroller & shape attributes in 2 classes
public class Shape : MonoBehaviour {
	private float speed = 10f;
	public GameManager gameManager;
	public Material colorFilterMat;
	static Sprite[] sprites;
	public ShapeType currentType = ShapeType.Square;
	
	Color[] _colors = new Color[4] { //Same order than ShapeType
		new Color(0, 255, 0),  //Square color
		new Color(0, 0, 255),  //Triangle color
		new Color(255, 0, 0),  //Circle color
		new Color(0, 255, 255) //Hegagon color
	};
	KeyCode[] shapeButton = new KeyCode[] { //Same order than ShapeType
		KeyCode.LeftArrow,
		KeyCode.UpArrow,
		KeyCode.RightArrow,
		KeyCode.DownArrow
	};

	void Start() {
		sprites = Resources.LoadAll<Sprite>("SpriteSheet");
		colorFilterMat.color = _colors[(int)currentType];
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();//TODO : find why initialization in the GameManager fails
	}

	public ShapeType GetShape() {
		return currentType;
	}

	void ChangeShape(ShapeType newShape) {
		currentType = newShape;
		colorFilterMat.color = _colors[(int)newShape];
		gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)newShape];
	}

	void Update () {
		if (gameManager.gameState != GameState.Playing) {
			return;
		}

		if (Input.GetKeyDown(shapeButton[(int)ShapeType.Square])) {
			ChangeShape(ShapeType.Square);
		} else if (Input.GetKeyDown(shapeButton[(int)ShapeType.Triangle])) {
			ChangeShape(ShapeType.Triangle);
		} else if (Input.GetKeyDown(shapeButton[(int)ShapeType.Circle])) {
			ChangeShape(ShapeType.Circle);
		} else if (Input.GetKeyDown(shapeButton[(int)ShapeType.Hexagon])) {
			ChangeShape(ShapeType.Hexagon);
		}

		float v = Input.GetAxisRaw("Vertical");
		float v2 = Input.GetAxisRaw("Horizontal");

		GetComponent<Rigidbody2D>().velocity = new Vector2(v2, v) * speed;
	}

	void OnTriggerStay2D (Collider2D other) {
		if (gameManager.gameState != GameState.Playing) {
			return;
		}
		if (!gameManager.isWinning && other.gameObject.tag == (currentType.ToString () + "Hole")) {
			Debug.Log("win");
			gameManager.gameState = GameState.EndingAnimation;
			gameManager.isWinning = true;
		}
		if (other.gameObject.tag == "Mechanism") {
			MechanicScript mechScript = other.GetComponent<MechanicScript>();
			if (mechScript.type == currentType) {
				mechScript.ActivateMechanic();
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (gameManager.gameState != GameState.Playing) {
			return;
		}

		if (other.gameObject.tag == (currentType.ToString () + "Hole")) {
			Debug.Log("win");
			gameManager.gameState = GameState.EndingAnimation;
			gameManager.isWinning = true;
		} else if (other.gameObject.tag.Contains("Hole")) {
			Debug.Log("wrong hole");
			//TODO: death gestion?
		}
	}

	/*void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Mechanism") {
			other.GetComponent<MechanicScript>().DesactivateMechanic();
		}
	}*/
}
