using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NormalTile : Tile {

	public Sprite heightMap;

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
		base.GetTileData(position, tilemap, ref tileData);
		if (position.z == 5) {
			tileData.sprite = heightMap;
		}
	}

#if UNITY_EDITOR
	// The following is a helper that adds a menu item to create a RoadTile Asset
	[MenuItem("Assets/Create/NormalTile")]
	public static void CreateRoadTile() {
		string path = EditorUtility.SaveFilePanelInProject("Save NormalTile", "New NormalTile", "Asset", "Save NormalTile", "Assets");
		if (path == "")
			return;
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NormalTile>(), path);
	}
#endif
}
