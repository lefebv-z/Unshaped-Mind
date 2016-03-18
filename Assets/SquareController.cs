using UnityEngine;
using System.Collections;

public class SquareController : MonoBehaviour {

	private float speed = 10f;
	private bool inverseAxis = false;

	void Start()
	{
//		GUILayout.Label("Press [space] to change form");
		//print("Press [space] to change form");
	}

	void OnGUI() {

		if (Event.current.Equals (Event.KeyboardEvent (KeyCode.Space.ToString ()))) {
			Debug.Log ("Space key is pressed.");
			print("change form");
			//todo circle shape
			inverseAxis = !inverseAxis;
		}

	}

	void Update () {
		float v = Input.GetAxisRaw("Vertical");
		float v2 = Input.GetAxisRaw("Horizontal");

		if (inverseAxis)
			GetComponent<Rigidbody2D>().velocity = new Vector2(v, v2) * speed;
		else
			GetComponent<Rigidbody2D>().velocity = new Vector2(v2, v) * speed;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		print ("TRIGGER ENTER ");
		if (Application.loadedLevelName == "Level2"
			&& inverseAxis
			&& other.gameObject.tag == "CircleHole") {
			print ("win");
			StartCoroutine("WaitAndExit", true);
		}
		else if (other.gameObject.tag == "SquareHole") {
			print ("win");
			StartCoroutine("WaitAndExit", true);
		}
		else if (other.gameObject.tag == "CircleHole"
		         || other.gameObject.tag == "TriangleHole") {
			print ("not this way");
			StartCoroutine("WaitAndExit", false);
			//			// ... if the triggering collider is a capsule collider...
//			if(other is CapsuleCollider)
//				// ... increase the count of triggering objects.
//				count++;
		}
	}

	IEnumerator WaitAndExit(bool next)
	{
		yield return new WaitForSeconds(1);
		if (next) {
			if (Application.loadedLevelName == "Level1") {
				print("load");
				Application.LoadLevel ("Level2");//TODO levels list
			}
			else {
				print("quit");
				Application.Quit();
			}
		}
		else
			Application.LoadLevel(Application.loadedLevel);
	}
	
	
	void OnTriggerExit2D (Collider2D other)
	{
		//print ("TRIGGER EXIT ");
		//		// If the leaving gameobject is the player or an enemy and the collider is a capsule collider...
//		if(other.gameObject == player || (other.gameObject.tag == Tags.enemy && other is CapsuleCollider))
//			// decrease the count of triggering objects.
//			count = Mathf.Max(0, count-1);
	}
}
