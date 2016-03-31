﻿using UnityEngine;
using System.Collections;

public class MechanicScript : MonoBehaviour {
	public ShapeType type;
	public GameObject[] wallsToDisapear;
	bool _isActive = false;

	public void DesactivateMechanic() {
        SoundManager sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
        if (sm != null && _isActive == true)
        {
            Debug.Log("Mechanic no longer active:" + name);
            GetComponent<SpriteRenderer>().color = Color.white;
            sm.PlayUnlocking();
            _isActive = false;
            foreach (GameObject obj in wallsToDisapear)
                obj.SetActive(true);
        }
	}

    public void ActivateMechanic()
    {
        SoundManager sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
        if (sm != null && _isActive == false)
        {
            Debug.Log("Mechanic active:" + name);
            GetComponent<SpriteRenderer>().color = Color.gray;
            sm.PlayUnlocking();
            _isActive = true;
            foreach (GameObject obj in wallsToDisapear)
                obj.SetActive(false);
        }
    }
}
