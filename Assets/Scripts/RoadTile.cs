using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;

#endif

public class RoadTile : Tile
{
    public Color start_color;
    public Color end_color;

    // This determines which sprite is used based on the RoadTiles that are adjacent to it and rotates it to fit the other tiles.
    // As the rotation is determined by the RoadTile, the TileFlags.OverrideTransform is set for the tile.
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.color = Color.Lerp(start_color, end_color, Random.value);
    }
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Road Tile")]
    public static void CreateRoadTile()
    {
        string path =
            EditorUtility.SaveFilePanelInProject("Save Road Tile", "New Road Tile", "Asset", "Save Road Tile",
                "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(CreateInstance<RoadTile>(), path);
    }
#endif
}