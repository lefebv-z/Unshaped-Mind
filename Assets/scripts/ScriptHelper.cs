using UnityEngine;
using System.Collections;

public class ScriptHelper : MonoBehaviour {

	public static void IncCursor(ref int cursor, int length) {
		cursor++;
		if (cursor == length)
			cursor = 0;
	}

	public static void DecCursor(ref int cursor, int length) {
		cursor--;
		if (cursor == -1)
			cursor = length - 1;
	}
}
