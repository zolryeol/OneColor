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

    [SerializeField] Image[] ClearStar = new Image[3]; // ���â�� ��� ��
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

    void SetClearUIStar() // ClearUI�� ��
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

    public void ClearStartResult() // Ŭ����������� �� ĥ���ִ� ��
    {
        int _stage = GameManager.Instance.nowStage;

        int _starLevel = (int)CheckClearStar(_stage, GameManager.Instance.stageTimer.GetRemainingTime); // �� � �������ϴ°�

        ///checkclearstar���� ��� �޾ƿͼ� ���ʺ��� ���ο�� �ٲپ��ش�.
        for (int i = 0; i < _starLevel; ++i)
        {
            ClearStar[i].sprite = yellowStar;
        }


        /// �÷��̾� ���������� �� ��� ������ �����Ѵ�; 0-9 �� ����� 0�� �� ����, 1-9�� ����� 1�� 0 ����
        int _Chapter = _stage - 10; // �÷����� é��
        int _ChapterNum = (int)(_Chapter * 0.1f);

        SetChapterStar(_ChapterNum, _starLevel); // �������� ����â�� ���� ����ش�

        string _stageKey = _ChapterNum.ToString() + "ChapterClearLevel";

        PlayerPrefs.SetInt(_stageKey, _starLevel);
        PlayerPrefs.Save();

        // ����é�� ������
        UnlockChapter(_ChapterNum);
    }

    void UnlockChapter(int _UnlockChapter)
    {
        _UnlockChapter++; // �Ķ���ʹ� ����é�� �ٲ� ���� ����é�Ͷ� +1 ���ִ°��̴�

        if (6 < _UnlockChapter)
        {
            Debug.Log("é�ʹ� 0~6 ������ ���ķ� �����Ϸ��ߴ�");
            return;
        }

        chapterButtons[_UnlockChapter].ButtonPermission(); // ��ư ��밡���ϰ��ϰ� ü���̹��� ����
    }
    void LoadPlayerStarData() // ���� ���������� �ѹ� �ҷ����� �Լ� ������ ����Ǿ��ִ� �����ͷ� �ر� �� ����� ���ش�
    {
        string ccl = "ChapterClearLevel";

        for (int i = 0; i < 7; ++i)
        {
            var v = i.ToString() + ccl;

            if (PlayerPrefs.HasKey(v) == false) continue; // �����Ͱ� ������ �Ѿ

            if ((int)EClearStar.Zero <= PlayerPrefs.GetInt(v)) // �����Ͱ� 0���� ũ�ٸ� �ش罺�������� ���� �ر� �� ��
            {
                UnlockChapter(i); // ��� �����ִ� �κ�

                SetChapterStar(i, PlayerPrefs.GetInt(v)); // 0é�� ���� �����Ͱ� �����ϴ� é���� ���� ����ش�
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
    public void OnBoard(int chapter_stage) // ���� ����
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
        /// ���� �ڸ�
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
        // �����ִ°��
        if (popUp_Ingame.activeSelf)
        {
            Time.timeScale = 1;

            popUp_Ingame.SetActive(false);
        }
        else // �����ִ°��
        {
            Time.timeScale = 0;

            popUp_Ingame.SetActive(true);
        }
    }
    public void PopUp_Exit() // �ڷΰ��� �� ��� �˾���Ų��;
    {
        // �����ִ°��
        if (popUp_Exit.activeSelf)
        {
            Time.timeScale = 1;

            popUp_Exit.SetActive(false);
        }
        else // �����ִ°��
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
        // �����ִ°��
        if (popUp_GameOver.activeSelf)
        {
            Time.timeScale = 1;

            popUp_GameOver.SetActive(false);
        }
        else // �����ִ°��
        {
            Time.timeScale = 0;

            popUp_GameOver.SetActive(true);
        }
    }
    public void PopUp_GameClear()
    {
        // �����ִ°�� ����
        if (popUp_GameClear.activeSelf)
        {
            Time.timeScale = 1;

            popUp_GameClear.SetActive(false);
        }
        else // �����ִ°��
        {
            Time.timeScale = 0;

            popUp_GameClear.SetActive(true);

            ClearStartResult();
        }
    }

    public EClearStar CheckClearStar(int _nowStage, float _remainingTime)
    {
        switch (_nowStage) // é���� ������ ��������
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
