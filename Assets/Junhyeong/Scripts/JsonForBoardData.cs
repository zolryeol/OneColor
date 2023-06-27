using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class JsonForBoardData
{
    public class SBoardData // json에 저장하기 위한 board의 정보
    {
        public int stage;

        public List<ETileType> arrColorType; // 2차원 배열의 값을 저장할 것이다.

        public int widthSize;
        public int heightSize;
        public ETileType missionColor;

        //         public List<bool> isBlack;
        //         public List<bool> isIllGenerator;
        //         public List<bool> isMove;
    }

    List<string> listBoardData = new List<string>();

    string jsonFileName = "BoardData.json";

    string DataToString(BoardMaker _boardMaker)
    {
        SBoardData data = new SBoardData
        {
            arrColorType = new List<ETileType>(),
            //             isBlack = new List<bool>(),
            //             isIllGenerator = new List<bool>(),
            //             isMove = new List<bool>(),
            missionColor = _boardMaker.GetMission,
            stage = _boardMaker.GetStage,
            heightSize = _boardMaker.GetHeightSize,
            widthSize = _boardMaker.GetWidthSize
        };

        InsertTileInfo(data, _boardMaker);

        string ToJsonData = JsonUtility.ToJson(data);

        return ToJsonData;
    }

    void InsertTileInfo(SBoardData data, BoardMaker _boardMaker) // board메이커의 타일 정보를 data에 하나하나 넣어준다
    {
        data.stage = _boardMaker.GetStage;
        data.heightSize = _boardMaker.GetHeightSize;
        data.widthSize = _boardMaker.GetWidthSize;

        for (int i = 0; i < data.heightSize; ++i)
        {
            for (int j = 0; j < data.widthSize; ++j)
            {
                data.arrColorType.Add(_boardMaker.GetBoardData[j, i].colorType);
                //                 data.isBlack.Add(_boardMaker.GetBoardData[j, i].isBlackTile);
                //                 data.isIllGenerator.Add(_boardMaker.GetBoardData[j, i].isIllGeneratorTile);
                //                 data.isMove.Add(_boardMaker.GetBoardData[j, i].isMoveTile);
            }
        }
    }

    public void SaveDataToJson(int _Stage, BoardMaker _boardMaker)
    {
        string path = Application.dataPath + "/Resources/JsonData/" + jsonFileName;

        var d = DataToString(_boardMaker);

        listBoardData.Add(d);

        File.AppendAllLines(path, listBoardData);
    }

    public void AddDataToJson(BoardMaker _boardMaker)
    {
        string path = Application.dataPath + "/Resources/JsonData/" + jsonFileName;

        string[] arrExistData = File.ReadAllLines(path); // 한줄씩 읽어서 배열에 저장해둔다.

        List<SBoardData> existdataList = new List<SBoardData>(); // 기존에 있던 내용들을 sBoardData형식으로 저장하기위한 리스트

        foreach (var j in arrExistData) // arr에 존재하는 내용을 sBoardData로 변환후 리스트에 넣는다.
        {
            var data = JsonUtility.FromJson<SBoardData>(j);
            existdataList.Add(data);
        }

        bool isNew = true; // 기존 데이터에 덮어쓸것인지 새로 추가할 것인지 확인하는 변수

        for (int i = 0; i < existdataList.Count; ++i) // 기존데이터를 들고와서 stage가 이미 존재하는가 확인한다.
        {
            if (existdataList[i].stage == _boardMaker.GetStage)
            {
                existdataList[i].arrColorType.Clear();

                existdataList[i].missionColor = _boardMaker.GetMission;

                //                 existdataList[i].isBlack.Clear();
                //                 existdataList[i].isIllGenerator.Clear();
                //                 existdataList[i].isMove.Clear();

                InsertTileInfo(existdataList[i], _boardMaker);

                isNew = false;

                break;
            }
        }

        if (isNew) // 새로운 스테이지 값이라면
        {
            SBoardData newData = new SBoardData
            {
                arrColorType = new List<ETileType>(),
                //                 isBlack = new List<bool>(),
                //                 isIllGenerator = new List<bool>(),
                //                 isMove = new List<bool>(),
                missionColor = _boardMaker.GetMission,
                stage = _boardMaker.GetStage,
                heightSize = _boardMaker.GetHeightSize,
                widthSize = _boardMaker.GetWidthSize
            };
            InsertTileInfo(newData, _boardMaker);

            existdataList.Add(newData);
        }

        //  기존에 json을 지우고 새로 만들어 넣어준다.
        File.Delete(path);

        List<string> newJsonList = new List<string>();

        foreach (var sboardD in existdataList)
        {
            var j = JsonUtility.ToJson(sboardD);
            newJsonList.Add(j);
        }

        File.AppendAllLines(path, newJsonList);
    }

    public SBoardData LoadDataFromJson(int _nowStage, out int _heightSize)
    {
        // 제이슨을 읽어 들여 해당 데이터를 기반으로 스테이지를 만든다.
        string path = Application.dataPath + "/Resources/JsonData/" + "BoardData.json";

        string[] arrExistData = File.ReadAllLines(path); // 경로에 있는 string을 한줄씩 읽어들여서 배열에 넣는다.

        List<SBoardData> existdataList = new List<SBoardData>(); // 기존에 있던 내용들을 sBoardData형식으로 저장하기위한 리스트

        foreach (var j in arrExistData) // arr에 존재하는 내용을 sBoardData로 변환후 리스트에 넣는다.
        {
            var data = JsonUtility.FromJson<SBoardData>(j);
            existdataList.Add(data);
        }

        bool isExist = false;

        foreach (var existData in existdataList)
        {
            if (existData.stage == _nowStage) // 현재 스테이지의 내용이라면
            {
                _heightSize = existData.heightSize;
                Debug.Log("기존의 높이" + existData.heightSize);

                isExist = true;
                return existData;
            }
        }

        if (isExist)
        {
            Debug.Log("Json에 해당 스테이지의 데이터가 없습니다");
            _heightSize = 1;
            return null;
        }

        _heightSize = 1;
        return null;
    }

    public SBoardData LoadDataFromJsonMobile(int _nowStage, out int _heightSize)
    {
        // 제이슨을 읽어 들여 해당 데이터를 기반으로 스테이지를 만든다.
        string path = "JsonData/BoardData";

        TextAsset jsonTextFile = Resources.Load<TextAsset>(path);

        if (jsonTextFile == null)
        {
            Debug.LogError("Failed to load JSON file from Resources folder at path: " + path);
            _heightSize = 7;
            return null;
        }

        string[] arrExistData = jsonTextFile.text.Split('\n');

        List<SBoardData> existdataList = new List<SBoardData>(); // 기존에 있던 내용들을 sBoardData형식으로 저장하기위한 리스트

        foreach (var j in arrExistData) // arr에 존재하는 내용을 sBoardData로 변환후 리스트에 넣는다.
        {
            var data = JsonUtility.FromJson<SBoardData>(j);
            existdataList.Add(data);
        }

        bool isExist = false;

        foreach (var existData in existdataList)
        {
            if (existData.stage == _nowStage) // 현재 스테이지의 내용이라면
            {
                _heightSize = existData.heightSize;
                //Debug.Log("기존의 높이" + existData.heightSize);

                isExist = true;
                return existData;
            }
        }

        if (!isExist)
        {
            Debug.Log("Json에 해당 스테이지의 데이터가 없습니다");
            _heightSize = 1;
            return null;
        }

        _heightSize = 1;
        return null;
    }
}

