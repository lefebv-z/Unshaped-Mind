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

    private Vector3 endDestination;

	void Start() {
		sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
		_particleSystem = gameObject.GetComponent<ParticleSystem>();
		sprites = Resources.LoadAll<Sprite>("SpriteSheet_crayon");
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		GameObject[] levels = GameObject.FindGameObjectsWithTag("Level");
		foreach (GameObject level in levels) {
			if (level.name == "Level" + GameManager.currentLevel.ToString()) {
				Transform[] positions = level.gameObject.GetComponentsInChildren<Transform>();
				foreach (Transform p in positions) {
					if (p.name == "PlayerPos") {
						transform.position = p.localPosition;
					}
				}
				NbTransfoLevelSave levelData = level.gameObject.GetComponentInChildren<NbTransfoLevelSave>();
				gameManager.remainingTransformation = levelData.nbTransformations;
				shapeAvailable = levelData.shapeAvailable;
				currentType = levelData.StartingType;
			} else {
				level.SetActive(false);
			}
		}
		colorFilterMat.color = _colors[(int)currentType];
		gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)currentType];
		Transform[] availableShapes = GameObject.Find("InGameInfos").GetComponentsInChildren<Transform>(true);
		foreach (Transform availableShape in availableShapes) {
			int i = 0;
			foreach (bool shAvail in shapeAvailable) {
				SetAvailableShape(shAvail, shapeButtons[i], availableShape.gameObject);
				i++;
			}
		}
	}

	void SetAvailableShape(bool avShape, string shName, GameObject availableShape) {
		if (avShape) {
			if (availableShape.name.Contains(shName)) {
				if (availableShape.name.Contains("Unavailable")) {
					availableShape.SetActive(false);
				} else {
					availableShape.SetActive(true);
				}
			}
		} else {
			if (availableShape.name.Contains(shName)) {
				if (availableShape.name.Contains("Available")) {
					availableShape.SetActive(false);
				} else {
					availableShape.SetActive(true);
				}
			}
		}
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
            endDestination = other.gameObject.transform.position;
			gameManager.gameState = GameState.EndingAnimation;
			gameManager.isWinning = true;
		}
		if (other.gameObject.tag == "Mechanism") {
			MechanicScript mechScript = other.GetComponent<MechanicScript>();
			if (mechScript.type == currentType) {
				mechScript.ActivateMechanic();
			}
		}
		if (other.gameObject.tag == "Portal") {
			PortalScript portalScript = other.GetComponent<PortalScript>();
			if (portalScript.type == currentType) {
				portalScript.UsePortal(this.gameObject);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (gameManager.gameState != GameState.Playing) {
			return;
		}

		if (other.gameObject.tag == (currentType.ToString () + "Hole")) {
			Debug.Log("win");
            endDestination = other.gameObject.transform.position;
			gameManager.gameState = GameState.EndingAnimation;
			gameManager.isWinning = true;
		} else if (other.gameObject.tag.Contains("Hole")) {
			Debug.Log("wrong hole");
			//TODO: death gestion?
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		if (gameManager.gameState != GameState.Playing) {
			return;
		}

		if (other.gameObject.tag == "Portal") {
			PortalScript portalScript = other.GetComponent<PortalScript>();
//			if (portalScript.type == currentType) {
				portalScript.UnlockPortal();
//			}
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

    public Vector3 getEndPosition()
    {
        return endDestination;
    }

    public void PlayParticle()
    {
    _particleSystem.Play();
    }
}
