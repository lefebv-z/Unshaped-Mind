using UnityEngine;
using System.Collections;

public class MechanicScript : MonoBehaviour {
	public ShapeType type;
	public GameObject[] wallsToDisapear;
	bool _isActive = false;

	public void DesactivateMechanic() {
		Debug.Log("Mechanic no longer active:" + name);
		_isActive = false;
		foreach (GameObject obj in wallsToDisapear) {
			obj.SetActive(true);
		}
	}

	public void ActivateMechanic() {
		//TODO actually do something
		Debug.Log("Mechanic active:" + name);
		_isActive = true;
		foreach (GameObject obj in wallsToDisapear) {
			obj.SetActive(false);
		}
	}
}
