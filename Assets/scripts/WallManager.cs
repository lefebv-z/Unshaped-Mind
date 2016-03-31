﻿using UnityEngine;
using System.Collections;

public enum ColorType {
	Green = 0,
	Blue = 1,
	Red = 2,
	Cyan = 3,
	White = 4
}

public class WallManager : MonoBehaviour {
	//public Shape			playerShape;
	public ColorType		color;

	private bool			isOn = true;
	private Collider2D		wallCollider;
	private SpriteRenderer	wallSprite;
	private Shape			playerShape;
	
	void Start () {
		wallCollider = gameObject.GetComponent<Collider2D>();
		wallSprite = gameObject.GetComponent<SpriteRenderer>();
		playerShape = GameObject.Find ("PlayerShape").GetComponent<Shape> ();
	}
	
	void Update () {
		if (isOn && (int)playerShape.GetShape() != (int)color && (int)color != (int)ColorType.White) {
			TurnOn(false);
		}
		if (!isOn && ((int)playerShape.GetShape() == (int)color || (int)color == (int)ColorType.White)) {
			TurnOn(true);
		}
	}

	private void TurnOn (bool on) {
		wallCollider.enabled = on;
		wallSprite.enabled = on;
		isOn = on;
	}
}
