using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;


public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public int nowStage;
    public PathFinder pathFinder;
    public BoardMaker boardMaker; // json���� ���� �����͸� ������ �ִ� ����
    public Timer stageTimer;
    public GameObject nowPlayingBoard;
    UIController uiController;
    [SerializeField] public List<Sprite> spriteList;

    [Header("BlackTile����")]
    public float becomeBlackTime;
    public float returnOriColorTime;
    [Header("MoveTile����")]
    public float moveTileMovingTime;

    public Timer moveTimer; // ����Ÿ���� �����ϸ� ��������ϴ� Ÿ�̸�
    [Header("����Tile����")]
    public float illSpreadTime;
    [HideInInspector]
    public Timer infectionTimer; // ������ü�� �����ϸ� ��������ϴ� Ÿ�̸�

    CTile illGenerator; // ����Ÿ���� ��ü

    public ETileType missionColor = ETileType.Any;

    [Header("Clear����ƼŬ")]
    public ParticleSystem[] clearParticlePrefabs;

    [Header("��� ����Ʈ")]
    public List<GameObject> backGroundObject;

    [Range(0, 3)]
    public int usingParticleIndex;

    public GameObject startEndParticle;

    const int maxChapter = 7;

    public StarLevelArr[] stageStartLevel = new StarLevelArr[maxChapter];

    Coroutine runningCoroutineMove = null;
    Coroutine runningCoroutineInfection = null;
    Coroutine runningCoroutineStageTimer = null;

    public bool isClearChapter = false;

    public bool clickChapterStart = false; // é�� ��ư Ŭ���ϸ� ClearChapterCheck() ���� �ٷ� clearó���ǹǷ� ����ó���� 

    private void Awake()
    {
        boardMaker = GameObject.Find("BoardMaker").GetComponent<BoardMaker>();

        uiController = FindObjectOfType<UIController>();
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM();
    }

    public void StageChange()
    {
        nowStage++;

        uiController.GetMissionUI.SetActive(true);


        /// ���� Ŭ���� ����üũ
        if (ClearChapterCheck() && clickChapterStart == true) // true���� é�͸� Ŭ�����ߴٴ� ���̴�
        {
            //Destroy(nowPlayingBoard); // Ŭ������ ���������� ���带 �����Ѵ�.

            Invoke("PopUpClear", 1.5f); // ��� ���� ��Ų��.
            //uiController.PopUp_GameClear();

            isClearChapter = true;

            clickChapterStart = false;
            return;
        }
        else
        {
            Destroy(nowPlayingBoard); // Ŭ������ ���������� ���带 �����Ѵ�.

            boardMaker.GenerateBoardFromJson(nowStage);

            InitGameManager();

            MissionColorSpriteChange();
        }

        clickChapterStart = true;
    }

    void MissionColorSpriteChange()
    {
        if (missionColor == ETileType.Any) uiController.GetMissionColorUI.sprite = spriteList[(int)ETileType.None];
        else if (missionColor == ETileType.Random) /// �̼ǿ� ������ �ΰ��Ϸ��ϱ⿡ �־���;�� 
        {
            int rnd = Random.Range((int)ETileType.Red, (int)ETileType.Purple + 1);

            missionColor = (ETileType)rnd;

            uiController.GetMissionColorUI.sprite = spriteList[rnd];
        }
        else uiController.GetMissionColorUI.sprite = spriteList[(int)missionColor];
    }

    bool ClearChapterCheck()
    {
        if (nowStage % 10 == 0 && nowStage != 0) return true;
        else return false;
    }
    public void DestroyBoard()
    {
        Destroy(nowPlayingBoard); // Ŭ������ ���������� ���带 �����Ѵ�.
        nowStage = 0;
    }

    public void PopUpClear()
    {
        uiController.PopUp_GameClear();
    }
    public void InitGameManager()
    {
        boardMaker = GameObject.Find("BoardMaker").GetComponent<BoardMaker>();
        pathFinder = new PathFinder(boardMaker);
        pathFinder.InitPathFinder();
        infectionTimer = new Timer();
        moveTimer = new Timer();
        isClearChapter = false;

        if (ExistMoveTile()) StartMoveTiles();
        if (ExistInfectionTile()) StartInfectionTiles();
    }

    public void NewTimer(int _givenTime) // é�͹�ư Ŭ���ϸ� ����Ǵ� �Լ�
    {
        stageTimer = new Timer(_givenTime)
        {
            timerText = uiController.GetTimerText,
            gameOver = () => uiController.PopUp_GameOver()
        };
        /// ���� ���� ������ stageTimer�� ��������
        //         stageTimer.gameOver = () => uiController.PopUp_GameOver();
        //         stageTimer.timerText = uiController.GetTimerText;

        uiController.GetReTryTime = _givenTime;

        if (runningCoroutineStageTimer != null) StopCoroutine(runningCoroutineStageTimer); // ������ ���� Ÿ�̸� ����

        runningCoroutineStageTimer = StartCoroutine(stageTimer.UpdateTimer()); // ���ο� Ÿ�̸� ����
    }
    public void ClearEffect()
    {
        //PathFinder.ClearCondition cc = pathFinder.CheckPathBFS();
        PathFinder.ClearCondition cc = pathFinder.CheckPathBFS_New();

        if (cc.isClear)
        {
            Debug.Log("Ŭ���� ����" + cc.clearColor);

            foreach (var t in boardMaker.GetBoardData)
            {
                /// ���� �� ��鸸 ����Ʈ ����ֱ�
                if (t.colorType == cc.clearColor)
                {
                    t.colorType = ETileType.None;
                    t.RefreshColorForMaker();
                    t.PlayClearEffect();
                    t.isUsedEffect = true;
                }

                /// ������ t.colorType != ETileType.None �̺κ� ����Ʈ ���� ���ΰ� �ƴҰ��ΰ� �ִ´ٸ� Ctile�� 56�� �����ؾ���
                //                 if (t.colorType != cc.clearColor && t.colorType != ETileType.Start && t.colorType != ETileType.End /*&& t.colorType != ETileType.None*/)
                //                 {
                //                     t.colorType = ETileType.None;
                //                     t.RefreshColorForMaker();
                //                     t.PlayClearEffect();
                //                     t.isUsedEffect = true;
                //                     /// Ŭ���� ����
                //                 }
            }

            ClearRemainingParticle(); // �ܿ� ��ƼŬ ����

            SoundManager.Instance.PlayClearEffect();

            StageChange();
        }
    }

    public void ClearRemainingParticle() // ������Ҷ� �ܿ� ��ƼŬ ����
    {
        foreach (var t2 in boardMaker.GetBoardData)
        {
            t2.RemainingParticle();
        }
    }

    void GenerateBoard(int _nowStage)
    {
        boardMaker.GenerateBoardData();
    }

    public void StartInfectionTiles() //���� Ÿ�� ����
    {
        StopInFectionTiles();

        runningCoroutineInfection = StartCoroutine(infectionTimer.InfectionTimer(illSpreadTime, CheckInfectionTile));
    }

    public void StartMoveTiles() // ����Ÿ�� ����
    {
        StopMoveTiles();

        runningCoroutineMove = StartCoroutine(moveTimer.MoveTimer(moveTileMovingTime, CheckMoveTile));
    }

    public void StopMoveTiles()
    {
        if (runningCoroutineMove == null) return;

        Debug.Log("���� ���� �ڷ�ƾ");

        StopCoroutine(runningCoroutineMove);
    }


    public void StopInFectionTiles()
    {
        if (runningCoroutineInfection == null) return;

        Debug.Log("���� ���� �ڷ�ƾ");

        StopCoroutine(runningCoroutineInfection);
    }

    void CheckMoveTile() // ��� Ÿ���� �����ؼ� ����Ÿ���� ã�´�.
    {
        if (moveTimer == null) { Debug.Log("����Ÿ�̸� ����"); return; }

        List<CTile> moveTile = new List<CTile>();

        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.colorType == ETileType.Move)
            {
                moveTile.Add(tile);
            }
        }

        foreach (var t in moveTile)
        {
            t.MovingTile();
        }
    }

    bool ExistMoveTile()
    {
        List<CTile> moveTile = new List<CTile>();

        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.colorType == ETileType.Move)
            {
                moveTile.Add(tile);
            }
        }
        if (moveTile.Count <= 0) return false;
        else return true;
    }
    bool ExistInfectionTile()
    {
        List<CTile> infectedTile = new List<CTile>();

        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.colorType == ETileType.Infection || tile.colorType == ETileType.InfectionMother)
            {
                infectedTile.Add(tile);
            }
        }
        if (infectedTile.Count <= 0) return false;
        else return true;
    }

    void CheckInfectionTile() // ��� Ÿ���� �����ؼ� ����ü�� ã�´�.
    {
        if (infectionTimer == null) { Debug.Log("����Ÿ�̸� ����"); return; }

        List<CTile> infectedTile = new List<CTile>();

        foreach (var tile in boardMaker.GetBoardData)
        {
            if (tile.colorType == ETileType.Infection || tile.colorType == ETileType.InfectionMother)
            {
                infectedTile.Add(tile);
            }
        }

        if (infectedTile.Count <= 0) return;

        foreach (var t in infectedTile)
        {
            t.InfectTile();
        }
    }
}
