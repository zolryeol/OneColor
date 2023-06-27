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
    public BoardMaker boardMaker; // json으로 들고온 데이터를 가지고 있는 변수
    public Timer stageTimer;
    public GameObject nowPlayingBoard;
    UIController uiController;
    [SerializeField] public List<Sprite> spriteList;

    [Header("BlackTile관련")]
    public float becomeBlackTime;
    public float returnOriColorTime;
    [Header("MoveTile관련")]
    public float moveTileMovingTime;

    public Timer moveTimer; // 무브타일이 존재하면 돌기시작하는 타이머
    [Header("감염Tile관련")]
    public float illSpreadTime;
    [HideInInspector]
    public Timer infectionTimer; // 감염모체가 존재하면 돌기시작하는 타이머

    CTile illGenerator; // 감염타일의 모체

    public ETileType missionColor = ETileType.Any;

    [Header("Clear시파티클")]
    public ParticleSystem[] clearParticlePrefabs;

    [Header("배경 리스트")]
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

    public bool clickChapterStart = false; // 챕터 버튼 클릭하면 ClearChapterCheck() 에서 바로 clear처리되므로 예외처리용 

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


        /// 게임 클리어 판정체크
        if (ClearChapterCheck() && clickChapterStart == true) // true값은 챕터를 클리어했다는 뜻이다
        {
            //Destroy(nowPlayingBoard); // 클리어한 스테이지의 보드를 제거한다.

            Invoke("PopUpClear", 1.5f); // 잠시 지연 시킨다.
            //uiController.PopUp_GameClear();

            isClearChapter = true;

            clickChapterStart = false;
            return;
        }
        else
        {
            Destroy(nowPlayingBoard); // 클리어한 스테이지의 보드를 제거한다.

            boardMaker.GenerateBoardFromJson(nowStage);

            InitGameManager();

            MissionColorSpriteChange();
        }

        clickChapterStart = true;
    }

    void MissionColorSpriteChange()
    {
        if (missionColor == ETileType.Any) uiController.GetMissionColorUI.sprite = spriteList[(int)ETileType.None];
        else if (missionColor == ETileType.Random) /// 미션에 랜덤을 부과하려하기에 넣었다;ㅡ 
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
        Destroy(nowPlayingBoard); // 클리어한 스테이지의 보드를 제거한다.
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

    public void NewTimer(int _givenTime) // 챕터버튼 클릭하면 실행되는 함수
    {
        stageTimer = new Timer(_givenTime)
        {
            timerText = uiController.GetTimerText,
            gameOver = () => uiController.PopUp_GameOver()
        };
        /// 위와 같은 내용임 stageTimer를 생략가능
        //         stageTimer.gameOver = () => uiController.PopUp_GameOver();
        //         stageTimer.timerText = uiController.GetTimerText;

        uiController.GetReTryTime = _givenTime;

        if (runningCoroutineStageTimer != null) StopCoroutine(runningCoroutineStageTimer); // 기존에 돌던 타이머 정지

        runningCoroutineStageTimer = StartCoroutine(stageTimer.UpdateTimer()); // 새로운 타이머 시작
    }
    public void ClearEffect()
    {
        //PathFinder.ClearCondition cc = pathFinder.CheckPathBFS();
        PathFinder.ClearCondition cc = pathFinder.CheckPathBFS_New();

        if (cc.isClear)
        {
            Debug.Log("클리어 색은" + cc.clearColor);

            foreach (var t in boardMaker.GetBoardData)
            {
                /// 길이 된 놈들만 이펙트 집어넣기
                if (t.colorType == cc.clearColor)
                {
                    t.colorType = ETileType.None;
                    t.RefreshColorForMaker();
                    t.PlayClearEffect();
                    t.isUsedEffect = true;
                }

                /// 마지막 t.colorType != ETileType.None 이부분 이펙트 넣을 것인가 아닐것인가 넣는다면 Ctile의 56줄 제거해야함
                //                 if (t.colorType != cc.clearColor && t.colorType != ETileType.Start && t.colorType != ETileType.End /*&& t.colorType != ETileType.None*/)
                //                 {
                //                     t.colorType = ETileType.None;
                //                     t.RefreshColorForMaker();
                //                     t.PlayClearEffect();
                //                     t.isUsedEffect = true;
                //                     /// 클리어 이후
                //                 }
            }

            ClearRemainingParticle(); // 잔여 파티클 제거

            SoundManager.Instance.PlayClearEffect();

            StageChange();
        }
    }

    public void ClearRemainingParticle() // 재시작할때 잔여 파티클 제거
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

    public void StartInfectionTiles() //감염 타일 실행
    {
        StopInFectionTiles();

        runningCoroutineInfection = StartCoroutine(infectionTimer.InfectionTimer(illSpreadTime, CheckInfectionTile));
    }

    public void StartMoveTiles() // 무브타일 실행
    {
        StopMoveTiles();

        runningCoroutineMove = StartCoroutine(moveTimer.MoveTimer(moveTileMovingTime, CheckMoveTile));
    }

    public void StopMoveTiles()
    {
        if (runningCoroutineMove == null) return;

        Debug.Log("정지 감염 코루틴");

        StopCoroutine(runningCoroutineMove);
    }


    public void StopInFectionTiles()
    {
        if (runningCoroutineInfection == null) return;

        Debug.Log("정지 감염 코루틴");

        StopCoroutine(runningCoroutineInfection);
    }

    void CheckMoveTile() // 모든 타일을 조사해서 무브타일을 찾는다.
    {
        if (moveTimer == null) { Debug.Log("무브타이머 없음"); return; }

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

    void CheckInfectionTile() // 모든 타일을 조사해서 감염체를 찾는다.
    {
        if (infectionTimer == null) { Debug.Log("감염타이머 없음"); return; }

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
