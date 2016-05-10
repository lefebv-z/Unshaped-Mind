using UnityEngine;
using System.Collections;

public class MechanicScript : MonoBehaviour {
	public ShapeType type;
	public GameObject[] wallsToDisapear;
	public GameObject[] wallsToAppear;
	public GameObject VFX;
	public GameManager gameManager;
	public GameObject lineToMechPref;
	private Sprite[]	spritesOn;
	private Sprite[]	spritesOff;
	bool _isActive = false;
	Color[] _colors = new Color[5] { //Same order than ShapeType
		new Color(0, 1, 0, 0.5f),   //green
		new Color(0, 0, 1, 0.5f),   //blue
		new Color(1, 0, 0, 0.5f),   //red
		new Color(0, 1, 1, 0.5f),   //cyan
		new Color(0.25f, 0.25f, 0.25f, 0.5f) //grey
	};

	void Start() {
		spritesOn = Resources.LoadAll<Sprite>("mechanismOn");
		spritesOff = Resources.LoadAll<Sprite>("mechanisms");
		if (!gameManager) {
			gameManager = GameObject.FindObjectOfType<GameManager>();
		}
		foreach (GameObject obj in wallsToDisapear) {
			GameObject line = Instantiate(lineToMechPref);
			LineRenderer renderer = line.GetComponent<LineRenderer>();
			renderer.SetPosition(0, obj.transform.position);
			renderer.SetPosition(1, transform.position);
			line.transform.SetParent(obj.transform);
			line.gameObject.transform.parent = obj.transform;
			renderer.enabled = false;
			obj.GetComponent<WallManager>().setDisappear();
		}
	}

	public void ActivateLines() {
		foreach (GameObject obj in wallsToDisapear) {
			obj.GetComponentInChildren<LineRenderer>().enabled = true;
		}
	}
	
	public void DesactivateLines() {
		foreach (GameObject obj in wallsToDisapear) {
			obj.GetComponentInChildren<LineRenderer>().enabled = false;
		}
	}

	void Update() {
		if (gameManager.gameState == GameState.Fullscreen &&
		    wallsToDisapear.Length > 0 && wallsToDisapear[0].activeSelf == true &&
		    wallsToDisapear[0].GetComponentInChildren<LineRenderer>().enabled == false) {
			ActivateLines();
		} else if (gameManager.gameState == GameState.Playing &&
	        wallsToDisapear.Length > 0 && wallsToDisapear[0].activeSelf == true &&
		    wallsToDisapear[0].GetComponentInChildren<LineRenderer>().enabled == true) {
			DesactivateLines();
		}
	}

	public void DesactivateMechanic() {
        SoundManager sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
        if ( _isActive == true)
        {
            Debug.Log("Mechanic no longer active:" + name);
			GetComponent<SpriteRenderer>().sprite = spritesOff[(int)type];
            if (sm != null)
                sm.PlayUnlocking();
            _isActive = false;
            foreach (GameObject obj in wallsToDisapear)
                obj.SetActive(true);
			foreach (GameObject obj in wallsToAppear)
				obj.SetActive(false);
		}
	}

    public void ActivateMechanic()
    {
        SoundManager sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
        if (_isActive == false)
        {
            Debug.Log("Mechanic active:" + name);
			GetComponent<SpriteRenderer>().sprite = spritesOn[(int)type];
            if (sm != null)
                sm.PlayUnlocking();
            _isActive = true;
            foreach (GameObject obj in wallsToDisapear) {
				/*GameObject vfx = Instantiate(VFX);
				vfx.transform.SetParent(transform);
				vfx.GetComponentInChildren<ParticleSystem>().startColor = _colors[(int)obj.GetComponentInChildren<WallManager>().color];
				vfx.transform.localPosition = Vector3.zero;
				float angle = Mathf.Atan2(obj.transform.position.y - transform.position.y, obj.transform.position.x - transform.position.x) * 180;
				vfx.transform.Rotate(0.0f, 0.0f, angle - 90.0f);*/
                obj.SetActive(false);
			}
			foreach (GameObject obj in wallsToAppear)
				obj.SetActive(true);
		}
    }
}
