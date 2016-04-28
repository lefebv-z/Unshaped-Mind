using UnityEngine;
using System.Collections;

public class MechanicScript : MonoBehaviour {
	public ShapeType type;
	public GameObject[] wallsToDisapear;
	private GameManager _gameManager;
	bool _isActive = false;
	Color[] _colors = new Color[5] { //Same order than ShapeType
		new Color(0, 1, 0, 0.5f),   //green
		new Color(0, 0, 1, 0.5f),   //blue
		new Color(1, 0, 0, 0.5f),   //red
		new Color(0, 1, 1, 0.5f),   //cyan
		new Color(0.25f, 0.25f, 0.25f, 0.5f) //grey
	};

	void Start() {
		_gameManager = GameObject.FindObjectOfType<GameManager>();
		foreach (GameObject obj in wallsToDisapear) {
			GameObject line = new GameObject("LineToMechanism");
			line.transform.SetParent(obj.transform);
			line.gameObject.transform.parent = obj.transform;
			LineRenderer renderer = line.AddComponent<LineRenderer>();
			renderer.SetPosition(0, obj.transform.position);
			renderer.SetPosition(1, transform.position);
			ColorType cType = obj.GetComponent<WallManager>().color;
			Material wallMat = new Material(Shader.Find("Unlit/Color"));
			wallMat.color = _colors[(int)cType];
			renderer.material = wallMat;
			renderer.SetWidth(0.05f, 0.05f);
			renderer.enabled = false;
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
		if (_gameManager.gameState == GameState.Fullscreen &&
		    wallsToDisapear.Length > 0 &&
		    wallsToDisapear[0].GetComponentInChildren<LineRenderer>().enabled == false) {
			ActivateLines();
		} else if (_gameManager.gameState == GameState.Playing &&
		    wallsToDisapear.Length > 0 &&
		    wallsToDisapear[0].GetComponentInChildren<LineRenderer>().enabled == true) {
			DesactivateLines();
		}
	}

	public void DesactivateMechanic() {
        SoundManager sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
        if ( _isActive == true)
        {
            Debug.Log("Mechanic no longer active:" + name);
            GetComponent<SpriteRenderer>().color = Color.white;
            if (sm != null)
                sm.PlayUnlocking();
            _isActive = false;
            foreach (GameObject obj in wallsToDisapear)
                obj.SetActive(true);
        }
	}

    public void ActivateMechanic()
    {
        SoundManager sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
        if (_isActive == false)
        {
            Debug.Log("Mechanic active:" + name);
            GetComponent<SpriteRenderer>().color = Color.gray;
            if (sm != null)
                sm.PlayUnlocking();
            _isActive = true;
            foreach (GameObject obj in wallsToDisapear)
                obj.SetActive(false);
        }
    }
}
