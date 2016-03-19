using UnityEngine;
using System.Collections;

public class MechanicScript : MonoBehaviour {
	public ShapeType type;
	bool _isActive = false;

	public void DesactivateMechanic() {
		Debug.Log("Mechanic no longer active:" + name);
		_isActive = false;
	}

	public void ActivateMechanic() {
		//TODO actually do something
		Debug.Log("Mechanic active:" + name);
		_isActive = true;
	}
}
