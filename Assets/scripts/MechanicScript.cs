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
			if (transform.parent.name.Contains(PlayerPrefs.GetInt("currentLevel").ToString())) {
				GameObject vfx = Instantiate(VFX);
				vfx.transform.SetParent(transform);
				ParticleSystem particleSys = vfx.GetComponentInChildren<ParticleSystem>();
				particleSys.startColor = _colors[(int)obj.GetComponentInChildren<WallManager>().color];
				vfx.transform.localPosition = Vector3.zero;
				
				Vector3 from = Vector3.right;
				Vector3 to = obj.transform.position - transform.position;
				to.z = 0.0f;
				float angle = Mathf.Acos(Vector3.Dot(from, to) / (from.magnitude * to.magnitude)) * Mathf.Rad2Deg;
				if (from.y > to.y) {
					angle *= -1;
				}
				angle -= 90.0f;
				vfx.transform.Rotate(Vector3.forward, angle);
				particleSys.startLifetime = Vector3.Distance(from, to) / particleSys.startSpeed;
			}
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
			if (obj.activeSelf) {
				obj.GetComponentsInChildren<LineRenderer>(true)[0].enabled = true;
			}
		}
	}
	
	public void DesactivateLines() {
		foreach (GameObject obj in wallsToDisapear) {
			if (obj.activeSelf) {
				obj.GetComponentInChildren<LineRenderer>().enabled = false;
			}
		}
	}

	void Update() {
		if (gameManager.gameState == GameState.Fullscreen &&
		    wallsToDisapear.Length > 0 && wallsToDisapear[0].activeSelf == true &&
		    wallsToDisapear[0].GetComponentInChildren<LineRenderer>().enabled == false) {
			ActivateLines();
		} else if (gameManager.gameState == GameState.Playing && !Shape.isHelpActive &&
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
            foreach (GameObject obj in wallsToDisapear) {
                obj.SetActive(true);
			}
			ParticleSystem[] particleSys = GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem ps in particleSys) {
				ps.Play();
			}
			foreach (GameObject obj in wallsToAppear)
				obj.SetActive(false);
		}
	}

    public void ActivateMechanic()
    {
		SoundManager sm = (SoundManager)(GameObject.FindObjectOfType (typeof(SoundManager)));
		if (_isActive == false) {
			Debug.Log ("Mechanic active:" + name);
			GetComponent<SpriteRenderer> ().sprite = spritesOn [(int)type];
			if (sm != null)
				sm.PlayUnlocking ();
			_isActive = true;
			ParticleSystem[] particleSys = GetComponentsInChildren<ParticleSystem> ();
			foreach (ParticleSystem ps in particleSys) {
				ps.loop = false;
				Debug.Log(ps.name);
				ps.Stop ();
				if (ps.name.Contains("VFX")) {
					ps.gameObject.SetActive(false);
				}
			}

			//Mech particles
			ParticleSystem mechPs = GetComponent<ParticleSystem> ();
			mechPs.Play();

			//Walls particles and (dis)appearance
			foreach (GameObject obj in wallsToDisapear) {
				ParticleSystem ps = obj.GetComponent<ParticleSystem> ();
				ps.Play ();
				StartCoroutine (WaitAndMakeWallDisappear (ps.duration, obj));
			}
			foreach (GameObject obj in wallsToAppear) {
				ParticleSystem ps = obj.GetComponent<ParticleSystem> ();
				ps.Play ();
				StartCoroutine (WaitAndMakeWallAppear (ps.duration, obj));
			}
		}
	}

	IEnumerator WaitAndMakeWallDisappear(float waitTime, GameObject wall) {
		yield return new WaitForSeconds(waitTime);
		wall.SetActive (false);
	}
	IEnumerator WaitAndMakeWallAppear(float waitTime, GameObject wall) {
		yield return new WaitForSeconds(waitTime);
		wall.SetActive (true);
	}
}
