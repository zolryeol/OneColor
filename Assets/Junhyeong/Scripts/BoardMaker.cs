using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보드를 생성하는 코드
public class BoardMaker : MonoBehaviour
{
    [SerializeField]
    int stage = 0;
    int widthSize = 5;
    //[SerializeField]
    int heightSize = 7;
    [SerializeField]
    ETileType mission = ETileType.Any;
    public int GetWidthSize => widthSize;
    public int GetHeightSize => heightSize;
    public int GetStage => stage;
    public ETileType GetMission => mission;

    [SerializeField] GameObject tilePrefab;
    CTile[,] boardData; // x,y 첫번째 줄이 가로다
    public CTile[,] GetBoardData
    {
        get => boardData;
        set => boardData = value;
    }

    private void Awake()
    {
        boardData = new CTile[widthSize, heightSize];
        //ReGetBoardData();
    }

    public void GenerateBoardData(int _heightSize = CBoardMaxSize.MaxHeight, int _widthSize = CBoardMaxSize.MaxWidth)
    /// 데이터 생성과 동시에 인스턴싱해준다.
    {
        boardData = new CTile[_widthSize, _heightSize];

        var parent = new GameObject("Board");

        for (int i = 0; i < _heightSize; ++i)
        {
            for (int j = 0; j < _widthSize; ++j)
            {
                boardData[j, i] = Instantiate(tilePrefab, new Vector3(j - 2, i - 4, 1), Quaternion.identity, parent.transform).AddComponent<CTile>();
                boardData[j, i].colorType = ETileType.None;
                boardData[j, i].SetTilePosition(j, i);
            }
        }

        RefreshTile();
    }

    public void GenerateBoardFromJson(int _nowStage)
    {
        JsonForBoardData jsonLoader = new JsonForBoardData();

        //var data = jsonLoader.LoadDataFromJson(_nowStage, out int _outHeight);

        var data = jsonLoader.LoadDataFromJsonMobile(_nowStage, out int _outHeight);

        heightSize = _outHeight;

        if (data == null)
        {
            Debug.LogError("Json에 데이터 없음");
            return;
        }

        boardData = new CTile[widthSize, heightSize];

        var parent = new GameObject("Board");

        GameManager.Instance.nowPlayingBoard = parent;

        GameManager.Instance.missionColor = data.missionColor;


        int index = 0;
        for (int i = 0; i < heightSize; ++i)
        {
            for (int j = 0; j < widthSize; ++j)
            {
                boardData[j, i] = Instantiate(tilePrefab, new Vector3(j - 2, i - 4, 1), Quaternion.identity, parent.transform).AddComponent<CTile>();

                boardData[j, i].colorType = data.arrColorType[index];
                //                 boardData[j, i].isBlackTile = data.isBlack[index];
                //                 boardData[j, i].isIllGeneratorTile = data.isIllGenerator[index];
                //                 boardData[j, i].isMoveTile = data.isMove[index];

                index++;

                boardData[j, i].SetTilePosition(j, i);
            }
        }


        int backGroundIndex = 0;

        switch (GameManager.Instance.nowStage)
        {
            case int v when v <= (int)EStageRange.Spring:
                backGroundIndex = (int)EBackGroundType.Spring;
                break;

            case int v when v <= (int)EStageRange.Summer:
                backGroundIndex = (int)EBackGroundType.Summer;
                break;

            case int v when v <= (int)EStageRange.Fall:
                backGroundIndex = (int)EBackGroundType.Fall;
                break;

            case int v when v <= (int)EStageRange.Winter:
                backGroundIndex = (int)EBackGroundType.Winter;
                break;

            default:
                Debug.Log("스테이지 백그라운드 인덱스에 문제가 생김");
                break;
        }

        /// 뒷배경 설정해주는 부분
        var _backGround = Instantiate<GameObject>(GameManager.Instance.backGroundObject[backGroundIndex], parent.transform);

        _backGround.transform.position = new Vector3(_backGround.transform.position.x, _backGround.transform.position.y, _backGround.transform.position.z + 3);

        RefreshTile();
    }

    public void ReGetBoardData()
    {
        GameObject _board = GameManager.Instance.nowPlayingBoard;
        if (_board == null) _board = GameObject.Find("Board").gameObject;

        widthSize = 5;
        heightSize = _board.transform.childCount / 5;

        if (boardData == null) boardData = new CTile[widthSize, heightSize];

        int k = 0;

        for (int i = 0; i < heightSize; ++i)
        {
            for (int j = 0; j < widthSize; ++j)
            {
                boardData[j, i] = _board.transform.GetChild(k).GetComponent<CTile>();
                k++;
            }
        }
    }

    public void ChangeTileType(CTile _tile)
    {
        boardData[(int)_tile.GetTilePosition.x, (int)_tile.GetTilePosition.y].colorType = _tile.colorType;
    }

    public void DestroyBoard()
    {
        foreach (var tile in boardData)
        {
            DestroyImmediate(tile);
        }

        DestroyImmediate(GameObject.Find("Board"));
    }

    public void RefreshTile()
    {
        if (boardData == null)
        {
            ReGetBoardData();
        }

        foreach (var tile in boardData)
        {
            tile.RefreshColorForMaker();
        }
    }

    public void SaveBoardDataToJson()
    {
        JsonForBoardData jsonData = new JsonForBoardData();

        if (boardData == null)
        {
            ReGetBoardData();
        }

        jsonData.AddDataToJson(this);
    }
}
