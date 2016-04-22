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
	//public Shape			playerShape;
	public ColorType		color;

	private bool			isOn = true;
	private Collider2D		wallCollider;
	private MeshRenderer	wallSprite;
	private Shape			playerShape;
	private Texture			tex = Resources.LoadAll<Texture>("walltile")[0];
	private GameObject		childGO;

	void Start () {
		childGO = transform.Find("WallGraphics").gameObject;

		wallCollider = gameObject.GetComponent<Collider2D>();
		wallSprite = childGO.GetComponent<MeshRenderer>();
		playerShape = GameObject.Find ("PlayerShape").GetComponent<Shape> ();
		if (color == ColorType.Green)
			tex = Resources.LoadAll<Texture>("wallTileSqu")[0];
		if (color == ColorType.Blue)
			tex = Resources.LoadAll<Texture>("wallTileTri")[0];
		if (color == ColorType.Red)
			tex = Resources.LoadAll<Texture>("wallTileCir")[0];
		if (color == ColorType.Cyan)
			tex = Resources.LoadAll<Texture>("wallTileHex")[0];
		childGO.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", tex);
		childGO.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(transform.localScale.x / 2, 1);
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
