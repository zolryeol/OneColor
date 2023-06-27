using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    [SerializeField] GameObject stageSelect;

    [SerializeField] GameObject title;
    [SerializeField] GameObject popUp_Exit;
    [SerializeField] GameObject popUp_Ingame;
    [SerializeField] GameObject popUp_GameOver;
    [SerializeField] GameObject popUp_GameClear;

    [SerializeField] Text timerText;
    [SerializeField] GameObject missionUI;
    [SerializeField] Transform missionSet;
    Image missionColorUI;

    [SerializeField] Image[] ClearStar = new Image[3]; // 결과창에 띄울 별
    [SerializeField] Sprite grayStar;
    [SerializeField] Sprite yellowStar;

    public GameObject GetPopUp_Exit => popUp_Exit;
    public GameObject GetPopUp_Ingame => popUp_Ingame;
    public GameObject GetGameOver => popUp_GameOver;
    public GameObject GetGameClear => popUp_GameClear;
    public GameObject GetStageSelect => stageSelect;
    public GameObject GetMissionUI => missionUI;
    public Image GetMissionColorUI => missionColorUI;

    ButtonController[] chapterButtons;

    int nowPlayingChapter;
    int nowPlayingChapterMaxTime;

    public int GetReTryTime
    {
        get => nowPlayingChapterMaxTime;
        set => nowPlayingChapterMaxTime = value;
    }


    public GameObject GetTitle => title;

    public Text GetTimerText => timerText;

    private void Awake()
    {
        chapterButtons = new ButtonController[7];
        missionColorUI = missionUI.transform.GetChild(0).GetComponent<Image>();
    }

    //     private void OnEnable()
    //     {
    //         for (int i = 0; i < 7; ++i)
    //         {
    //             string n = "Chapter" + i;
    //             chapterButtons[i] = GameObject.Find(n).GetComponent<ButtonController>();
    // 
    //             chapterButtons[i].ButtonInhibition();
    //         }
    // 
    //         chapterButtons[0].ButtonPermission();
    // 
    //         SetClearUIStar();
    // 
    //         LoadPlayerStarData();
    //     }

    void Start()
    {
        for (int i = 0; i < 7; ++i)
        {
            string n = "Chapter" + i;
            chapterButtons[i] = GameObject.Find(n).GetComponent<ButtonController>();

            chapterButtons[i].ButtonInhibition();
        }

        chapterButtons[0].ButtonPermission();
        //         chapterButtons[1].ButtonPermission();

        SetClearUIStar();

        LoadPlayerStarData();
    }

    void SetClearUIStar() // ClearUI의 별
    {
        var startSet = popUp_GameClear.transform.GetChild(3);
        ClearStar = startSet.transform.GetComponentsInChildren<Image>();
    }

    void SetChapterStar(int _chapter, int _starCount)
    {

        var starSet = chapterButtons[_chapter].transform.Find("StarSet").gameObject;

        Image[] starImages = new Image[starSet.transform.childCount];

        for (int i = 0; i < starSet.transform.childCount; ++i)
        {
            starImages[i] = starSet.transform.GetChild(i).GetComponent<Image>();
        }

        for (int i = 0; i < _starCount; ++i)
        {
            starImages[i].sprite = yellowStar;
        }
    }
    public void OnMissionUI()
    {
        missionSet.gameObject.SetActive(true);
    }
    public void OFFMissionUI()
    {
        missionSet.gameObject.SetActive(false);
    }
    public void InitStarResult()
    {
        foreach (var star in ClearStar)
        {
            star.sprite = grayStar;
        }
    }

    public void ClearStartResult() // 클리어했을경우 별 칠해주는 곳
    {
        int _stage = GameManager.Instance.nowStage;

        int _starLevel = (int)CheckClearStar(_stage, GameManager.Instance.stageTimer.GetRemainingTime); // 별 몇개 찍어줘야하는가

        ///checkclearstar에서 등급 받아와서 왼쪽부터 엘로우로 바꾸어준다.
        for (int i = 0; i < _starLevel; ++i)
        {
            ClearStar[i].sprite = yellowStar;
        }


        /// 플레이어 프리팹으로 별 몇개로 깼는지 저장한다; 0-9 를 깬경우 0에 별 저장, 1-9를 깬경우 1에 0 저장
        int _Chapter = _stage - 10; // 플레이한 챕터
        int _ChapterNum = (int)(_Chapter * 0.1f);

        SetChapterStar(_ChapterNum, _starLevel); // 스테이지 선택창의 별도 찍어준다

        string _stageKey = _ChapterNum.ToString() + "ChapterClearLevel";

        PlayerPrefs.SetInt(_stageKey, _starLevel);
        PlayerPrefs.Save();

        // 다음챕터 언락기능
        UnlockChapter(_ChapterNum);
    }

    void UnlockChapter(int _UnlockChapter)
    {
        _UnlockChapter++; // 파라미터는 현재챕터 바꿀 것은 다음챕터라서 +1 해주는것이다

        if (6 < _UnlockChapter)
        {
            Debug.Log("챕터는 0~6 까지다 이후로 접근하려했다");
            return;
        }

        chapterButtons[_UnlockChapter].ButtonPermission(); // 버튼 사용가능하게하고 체인이미지 끈다
    }
    void LoadPlayerStarData() // 게임 실행했을때 한번 불러오는 함수 기존에 저장되어있는 데이터로 해금 및 별찍기 해준다
    {
        string ccl = "ChapterClearLevel";

        for (int i = 0; i < 7; ++i)
        {
            var v = i.ToString() + ccl;

            if (PlayerPrefs.HasKey(v) == false) continue; // 데이터가 없으면 넘어감

            if ((int)EClearStar.Zero <= PlayerPrefs.GetInt(v)) // 데이터가 0보다 크다면 해당스테이지의 다음 해금 및 별
            {
                UnlockChapter(i); // 언락 시켜주는 부분

                SetChapterStar(i, PlayerPrefs.GetInt(v)); // 0챕터 부터 데이터가 존재하는 챕터의 별을 찍어준다
            }
        }
    }

    public void OffTitle()
    {
        title.SetActive(false);
    }
    public void OnTitle()
    {
        title.SetActive(true);
    }
    public void OnStageSelect()
    {
        stageSelect.SetActive(true);
    }
    public void OffStageSelect()
    {
        stageSelect.SetActive(false);

        if (popUp_Exit.activeSelf)
        {
            PopUp_Exit();
        }
    }
    public void OnBoard(int chapter_stage) // 게임 시작
    {
        GameManager.Instance.nowStage = chapter_stage;

        GameManager.Instance.nowStage -= 1;

        GameManager.Instance.StageChange();

        OffStageSelect();

        OnMissionUI();

        nowPlayingChapter = chapter_stage;
    }
    public void ReStartNowStage()
    {
        GameManager.Instance.ClearRemainingParticle();

        GameManager.Instance.clickChapterStart = false;

        OnBoard(GameManager.Instance.nowStage);
    }
    public void RestartNowChapter()
    {
        /// 광고 자리
        GameManager.Instance.ClearRemainingParticle();

        GameManager.Instance.NewTimer(nowPlayingChapterMaxTime);

        OnBoard(nowPlayingChapter);
    }
    public void ReturnToStageSelect()
    {
        GameManager.Instance.DestroyBoard();

        GameManager.Instance.ClearRemainingParticle();

        PopUp_Ingame();

        OnStageSelect();

        OFFMissionUI();

        GameManager.Instance.clickChapterStart = false;
    }
    public void PopUp_Ingame()
    {
        // 켜져있는경우
        if (popUp_Ingame.activeSelf)
        {
            Time.timeScale = 1;

            popUp_Ingame.SetActive(false);
        }
        else // 꺼져있는경우
        {
            Time.timeScale = 0;

            popUp_Ingame.SetActive(true);
        }
    }
    public void PopUp_Exit() // 뒤로가기 할 경우 팝업시킨다;
    {
        // 켜져있는경우
        if (popUp_Exit.activeSelf)
        {
            Time.timeScale = 1;

            popUp_Exit.SetActive(false);
        }
        else // 꺼져있는경우
        {
            Time.timeScale = 0;

            popUp_Exit.SetActive(true);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PopUp_GameOver()
    {
        // 켜져있는경우
        if (popUp_GameOver.activeSelf)
        {
            Time.timeScale = 1;

            popUp_GameOver.SetActive(false);
        }
        else // 꺼져있는경우
        {
            Time.timeScale = 0;

            popUp_GameOver.SetActive(true);
        }
    }
    public void PopUp_GameClear()
    {
        // 켜져있는경우 끈다
        if (popUp_GameClear.activeSelf)
        {
            Time.timeScale = 1;

            popUp_GameClear.SetActive(false);
        }
        else // 꺼져있는경우
        {
            Time.timeScale = 0;

            popUp_GameClear.SetActive(true);

            ClearStartResult();
        }
    }

    public EClearStar CheckClearStar(int _nowStage, float _remainingTime)
    {
        switch (_nowStage) // 챕터의 마지막 스테이지
        {
            case 10:
                {
                    switch ((int)_remainingTime)
                    {
                        case var value when value >= GameManager.Instance.stageStartLevel[0].StarLevel[2]:
                            return EClearStar.Three;
                        case var value when value >= GameManager.Instance.stageStartLevel[0].StarLevel[1]:
                            return EClearStar.Two;
                        case var value when value >= GameManager.Instance.stageStartLevel[0].StarLevel[0]:
                            return EClearStar.One;
                        case var value when value >= 0:
                            return EClearStar.Zero;
                    }
                    break;
                }

            case 20:
                {
                    switch ((int)_remainingTime)
                    {
                        case var value when value >= GameManager.Instance.stageStartLevel[1].StarLevel[2]:
                            return EClearStar.Three;
                        case var value when value >= GameManager.Instance.stageStartLevel[1].StarLevel[1]:
                            return EClearStar.Two;
                        case var value when value >= GameManager.Instance.stageStartLevel[1].StarLevel[0]:
                            return EClearStar.One;
                        case var value when value >= 0:
                            return EClearStar.Zero;
                    }
                    break;
                }
            case 30:
                {
                    switch ((int)_remainingTime)
                    {
                        case var value when value >= GameManager.Instance.stageStartLevel[2].StarLevel[2]:
                            return EClearStar.Three;
                        case var value when value >= GameManager.Instance.stageStartLevel[2].StarLevel[1]:
                            return EClearStar.Two;
                        case var value when value >= GameManager.Instance.stageStartLevel[2].StarLevel[0]:
                            return EClearStar.One;
                        case var value when value >= 0:
                            return EClearStar.Zero;
                    }
                    break;
                }
            case 40:
                {
                    switch ((int)_remainingTime)
                    {
                        case var value when value >= GameManager.Instance.stageStartLevel[3].StarLevel[2]:
                            return EClearStar.Three;
                        case var value when value >= GameManager.Instance.stageStartLevel[3].StarLevel[1]:
                            return EClearStar.Two;
                        case var value when value >= GameManager.Instance.stageStartLevel[3].StarLevel[0]:
                            return EClearStar.One;
                        case var value when value >= 0:
                            return EClearStar.Zero;
                    }
                    break;
                }
            case 50:
                {
                    switch ((int)_remainingTime)
                    {
                        case var value when value >= GameManager.Instance.stageStartLevel[4].StarLevel[2]:
                            return EClearStar.Three;
                        case var value when value >= GameManager.Instance.stageStartLevel[4].StarLevel[1]:
                            return EClearStar.Two;
                        case var value when value >= GameManager.Instance.stageStartLevel[4].StarLevel[0]:
                            return EClearStar.One;
                        case var value when value >= 0:
                            return EClearStar.Zero;
                    }
                    break;
                }
            case 60:
                {
                    switch ((int)_remainingTime)
                    {
                        case var value when value >= GameManager.Instance.stageStartLevel[5].StarLevel[2]:
                            return EClearStar.Three;
                        case var value when value >= GameManager.Instance.stageStartLevel[5].StarLevel[1]:
                            return EClearStar.Two;
                        case var value when value >= GameManager.Instance.stageStartLevel[5].StarLevel[0]:
                            return EClearStar.One;
                        case var value when value >= 0:
                            return EClearStar.Zero;
                    }
                    break;
                }
            case 70:
                {
                    switch ((int)_remainingTime)
                    {
                        case var value when value >= GameManager.Instance.stageStartLevel[6].StarLevel[2]:
                            return EClearStar.Three;
                        case var value when value >= GameManager.Instance.stageStartLevel[6].StarLevel[1]:
                            return EClearStar.Two;
                        case var value when value >= GameManager.Instance.stageStartLevel[6].StarLevel[0]:
                            return EClearStar.One;
                        case var value when value >= 0:
                            return EClearStar.Zero;
                    }
                    break;
                }

            default:
                break;
        }
        return EClearStar.Zero;
    }

}
