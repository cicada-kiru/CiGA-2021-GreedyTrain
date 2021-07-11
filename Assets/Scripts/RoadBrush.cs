#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomGridBrush(true, false, false, "Road Brush")]
public class RoadBrush : GridBrush
{
    public static Vector2Int[][] oddr_directions =
    {
        new[]
        {
            new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, -1),
            new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1)
        },
        new[]
        {
            new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1),
            new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1)
        }
    };

    public Vector3Int OddrOffsetNeighbor(Vector2Int hex, int direction)
    {
        var parity = hex.y & 1;
        var dir = oddr_directions[parity][direction];
        return new Vector3Int(hex.x + dir[0], hex.y + dir[1], 0);
    }

    public override void Paint(GridLayout grid, GameObject brush_target, Vector3Int hex)
    {
        base.Paint(grid, brush_target, hex);
        for (var i = 0; i < 6; i++)
        {
            var neighbor_hex = OddrOffsetNeighbor((Vector2Int) hex, i);
            base.Paint(grid, brush_target, neighbor_hex);
            for (var j = 0; j < 6; j++)
            {
                base.Paint(grid, brush_target, OddrOffsetNeighbor((Vector2Int) neighbor_hex, j));
            }
        }
    }
    [MenuItem("Assets/Create/Road Brush")]
    public static void CreateBrush()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Road Brush", "New Road Brush", "Asset",
            "Save Road Brush", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(CreateInstance<RoadBrush>(), path);
    }
}

[CustomEditor(typeof(RoadBrush))]
public class RoadBrushEditor : GridBrushEditor
{
    private RoadBrush road_brush => target as RoadBrush;

    public override void OnPaintSceneGUI(GridLayout grid, GameObject brush_target, BoundsInt position,
        GridBrushBase.Tool tool, bool executing)
    {
        base.OnPaintSceneGUI(grid, brush_target, position, tool, executing);
    }
}
#endif