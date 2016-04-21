using UnityEngine;
using System.Collections;

public class NbTransfoLevelSave : MonoBehaviour {
	public int nbTransformations = 10;
	public bool[] shapeAvailable = new bool[] {
		true,//square
		true,//triangle
		true,//circle
		true//hexagon
	};
	public ShapeType StartingType = ShapeType.Square;
}