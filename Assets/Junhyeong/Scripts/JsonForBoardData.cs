using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class JsonForBoardData
{
    public class SBoardData // json�� �����ϱ� ���� board�� ����
    {
        public int stage;

        public List<ETileType> arrColorType; // 2���� �迭�� ���� ������ ���̴�.

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

    void InsertTileInfo(SBoardData data, BoardMaker _boardMaker) // board����Ŀ�� Ÿ�� ������ data�� �ϳ��ϳ� �־��ش�
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

        string[] arrExistData = File.ReadAllLines(path); // ���پ� �о �迭�� �����صд�.

        List<SBoardData> existdataList = new List<SBoardData>(); // ������ �ִ� ������� sBoardData�������� �����ϱ����� ����Ʈ

        foreach (var j in arrExistData) // arr�� �����ϴ� ������ sBoardData�� ��ȯ�� ����Ʈ�� �ִ´�.
        {
            var data = JsonUtility.FromJson<SBoardData>(j);
            existdataList.Add(data);
        }

        bool isNew = true; // ���� �����Ϳ� ��������� ���� �߰��� ������ Ȯ���ϴ� ����

        for (int i = 0; i < existdataList.Count; ++i) // ���������͸� ���ͼ� stage�� �̹� �����ϴ°� Ȯ���Ѵ�.
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

        if (isNew) // ���ο� �������� ���̶��
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

        //  ������ json�� ����� ���� ����� �־��ش�.
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
        // ���̽��� �о� �鿩 �ش� �����͸� ������� ���������� �����.
        string path = Application.dataPath + "/Resources/JsonData/" + "BoardData.json";

        string[] arrExistData = File.ReadAllLines(path); // ��ο� �ִ� string�� ���پ� �о�鿩�� �迭�� �ִ´�.

        List<SBoardData> existdataList = new List<SBoardData>(); // ������ �ִ� ������� sBoardData�������� �����ϱ����� ����Ʈ

        foreach (var j in arrExistData) // arr�� �����ϴ� ������ sBoardData�� ��ȯ�� ����Ʈ�� �ִ´�.
        {
            var data = JsonUtility.FromJson<SBoardData>(j);
            existdataList.Add(data);
        }

        bool isExist = false;

        foreach (var existData in existdataList)
        {
            if (existData.stage == _nowStage) // ���� ���������� �����̶��
            {
                _heightSize = existData.heightSize;
                Debug.Log("������ ����" + existData.heightSize);

                isExist = true;
                return existData;
            }
        }

        if (isExist)
        {
            Debug.Log("Json�� �ش� ���������� �����Ͱ� �����ϴ�");
            _heightSize = 1;
            return null;
        }

        _heightSize = 1;
        return null;
    }

    public SBoardData LoadDataFromJsonMobile(int _nowStage, out int _heightSize)
    {
        // ���̽��� �о� �鿩 �ش� �����͸� ������� ���������� �����.
        string path = "JsonData/BoardData";

        TextAsset jsonTextFile = Resources.Load<TextAsset>(path);

        if (jsonTextFile == null)
        {
            Debug.LogError("Failed to load JSON file from Resources folder at path: " + path);
            _heightSize = 7;
            return null;
        }

        string[] arrExistData = jsonTextFile.text.Split('\n');

        List<SBoardData> existdataList = new List<SBoardData>(); // ������ �ִ� ������� sBoardData�������� �����ϱ����� ����Ʈ

        foreach (var j in arrExistData) // arr�� �����ϴ� ������ sBoardData�� ��ȯ�� ����Ʈ�� �ִ´�.
        {
            var data = JsonUtility.FromJson<SBoardData>(j);
            existdataList.Add(data);
        }

        bool isExist = false;

        foreach (var existData in existdataList)
        {
            if (existData.stage == _nowStage) // ���� ���������� �����̶��
            {
                _heightSize = existData.heightSize;
                //Debug.Log("������ ����" + existData.heightSize);

                isExist = true;
                return existData;
            }
        }

        if (!isExist)
        {
            Debug.Log("Json�� �ش� ���������� �����Ͱ� �����ϴ�");
            _heightSize = 1;
            return null;
        }

        _heightSize = 1;
        return null;
    }
}

