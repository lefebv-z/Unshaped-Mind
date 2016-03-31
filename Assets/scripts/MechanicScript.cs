using UnityEngine;
using System.Collections;

public class MechanicScript : MonoBehaviour {
	public ShapeType type;
	public GameObject[] wallsToDisapear;
	bool _isActive = false;

	public void DesactivateMechanic() {
		Debug.Log("Mechanic no longer active:" + name);
		GetComponent<SpriteRenderer>().color = Color.white;
		_isActive = false;
		foreach (GameObject obj in wallsToDisapear) {
			obj.SetActive(true);
		}
	}

	public void ActivateMechanic() {
		Debug.Log("Mechanic active:" + name);
		GetComponent<SpriteRenderer>().color = Color.gray;
		_isActive = true;
		foreach (GameObject obj in wallsToDisapear) {
			obj.SetActive(false);
		}
	}
}
