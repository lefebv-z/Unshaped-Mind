using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {
	public ShapeType type;
	public GameObject exitPortal;
	bool _isActive = true;
	
	public void UsePortal(GameObject playerShape)
	{
		if (_isActive == true)
		{
			Debug.Log("Portal used:" + name);
			playerShape.transform.position = exitPortal.transform.position;
			exitPortal.GetComponent<PortalScript>().LockPortal();//Temporary lock the portal to prevent infinite teleportation
		}
	}

	public void UnlockPortal()
	{
		GetComponent<SpriteRenderer>().color = Color.white;
		_isActive = true;
	}
	public void LockPortal()
	{
		SoundManager sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
		if (sm != null)
			sm.PlayUnlocking();
		GetComponent<SpriteRenderer>().color = Color.gray;
		_isActive = false;
	}
}
