using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ShapeType {
	Square = 0,
	Triangle = 1,
	Circle = 2,
	Hexagon = 3,
	ShapeTypeCount = 4
}

//TODO : separate playercontroller & shape attributes in 2 classes
public class Shape : MonoBehaviour {
	private float speed = 10f;
	public GameManager gameManager;
	public Material colorFilterMat;
	static Sprite[] sprites;
	public ShapeType currentType = ShapeType.Square;
	private ParticleSystem _particleSystem;
    private SoundManager sm;
	public bool[] shapeAvailable = new bool[] {
		true,//square
		true,//triangle
		true,//circle
		true//hexagon
	};

	Color[] _colors = new Color[4] { //Same order than ShapeType
		new Color(0, 255, 0),  //Square color
		new Color(0, 0, 255),  //Triangle color
		new Color(255, 0, 0),  //Circle color
		new Color(0, 255, 255) //Hegagon color
	};

	string[] shapeButtons = new string[] { //Same order than ShapeType
		"Square",
		"Triangle",
		"Circle",
		"Hexagon"
	};

	void Start() {
        sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
		_particleSystem = gameObject.GetComponent<ParticleSystem>();
		sprites = Resources.LoadAll<Sprite>("SpriteSheet");
		colorFilterMat.color = _colors[(int)currentType];
		gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)currentType];
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();//TODO : find why initialization in the GameManager fails
	}

	public ShapeType GetShape() {
		return currentType;
	}

	void ChangeShape(ShapeType newShape) {
		currentType = newShape;
		colorFilterMat.color = _colors[(int)newShape];
		_particleSystem.GetComponent<ParticleSystemRenderer>().material.mainTexture = Resources.Load<Texture2D>(currentType.ToString() + "Hole");
		gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)newShape];
		_particleSystem.Play();
	}

	void Update () {
		if (gameManager.gameState != GameState.Playing) {
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			return;
		}

		for (int i = 0; i < (int)ShapeType.ShapeTypeCount; i++) {
			if (Input.GetButtonDown(shapeButtons[i])
			    && shapeAvailable[i]
			    && (ShapeType)(i) != currentType) {
				if (gameManager.remainingTransformation > 0) {
					ChangeShape((ShapeType)(i));
					gameManager.remainingTransformation--;
                    if (sm != null)
                        sm.PlayChange();
					print ("remaining transfo: " + gameManager.remainingTransformation);
				} else {
                    if (sm != null)
                        sm.PlayNoChange();
					print ("No more transformation possible");//TODO: real feedback
				}
				break;
			}
            else if (Input.GetButtonDown(shapeButtons[i]) && shapeAvailable[i])
                if (sm != null)
                    sm.PlayNoChange();
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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            if (sm != null)
                sm.PlayHitWall();
        }
    }

	/*void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Mechanism") {
			other.GetComponent<MechanicScript>().DesactivateMechanic();
		}
	}*/
}
