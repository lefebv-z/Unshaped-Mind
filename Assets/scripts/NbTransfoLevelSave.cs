using UnityEngine;
using System.Collections;

public class NbTransfoLevelSave : MonoBehaviour {
	public int nbTransformations = 10;
    public int bestRemainingTransfo = 1;
    public float bestTime = 40.0f;
	public bool[] shapeAvailable = new bool[] {
		true,//square
		true,//triangle
		true,//circle
		true//hexagon
	};
	public ShapeType StartingType = ShapeType.Square;
}