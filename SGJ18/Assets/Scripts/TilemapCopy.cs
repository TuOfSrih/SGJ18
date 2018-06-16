using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCopy : MonoBehaviour {

	public Tilemap other;
	Tilemap tm;
	public static Tilemap copy;
	// Use this for initialization
	void Start () {
		tm = GetComponent<Tilemap>();
		copy = tm;
		for (int i = other.cellBounds.xMin; i <= other.cellBounds.xMax; i++) {
			for (int j = other.cellBounds.yMin; j <= other.cellBounds.yMax; j++) {
				TileBase t = other.GetTile(new Vector3Int(i, j, 0));
				tm.SetTile(new Vector3Int(i, j, 5), t);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
