using UnityEngine;
using System.Collections;

public enum ColorType {
	Green = 0,
	Blue = 1,
	Red = 2,
	Cyan = 3,
	White = 4
}

public class WallManager : MonoBehaviour {

	public ColorType		color;

	private bool			isOn = true;
	private Collider2D		wallCollider;
	private SpriteRenderer	wallSprite;

	void Start () {
		wallCollider = gameObject.GetComponent<Collider2D>();
		wallSprite = gameObject.GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		if (isOn && (int)Shape.currentType == (int)color || color == ColorType.White)
			TurnOn(false);

		else if (!isOn && (int)Shape.currentType != (int)color && color != ColorType.White)
			TurnOn(true);
	}

	private void TurnOn (bool on) {
		wallCollider.enabled = on;
		wallSprite.enabled = on;
		isOn = on;
	}
}
