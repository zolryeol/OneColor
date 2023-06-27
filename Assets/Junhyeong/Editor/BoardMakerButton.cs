using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardMaker))]
public class BoardMakerButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BoardMaker maker = (BoardMaker)target;

        if (GUILayout.Button("New GenerateBoard"))
        {
            maker.GenerateBoardData(maker.GetHeightSize, maker.GetWidthSize);
        }

        if (GUILayout.Button("GenerateBoardFromJson"))
        {
            maker.GenerateBoardFromJson(maker.GetStage);
        }

        if (GUILayout.Button("DestroyBoard"))
        {
            maker.DestroyBoard();
        }

        if (GUILayout.Button("RefreshColor"))
        {
            maker.RefreshTile();
        }

        if (GUILayout.Button("SaveToJson"))
        {
            maker.SaveBoardDataToJson();
        }
    }
}

[CustomEditor(typeof(CTile))]
public class TileButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CTile tile = (CTile)target;

        if (GUILayout.Button("Start"))
        {
            tile.SetStartTile();
        }

        if (GUILayout.Button("End"))
        {
            tile.SetEndTile();
        }
    }
}
