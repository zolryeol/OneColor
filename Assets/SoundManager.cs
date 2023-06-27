using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  사운드매니저 사운드 종류가 많지 않기때문에 브루탈하게 사용한다.
/// </summary>
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }
            return instance;
        }
    }

    [SerializeField] List<AudioClip> audioClip = new List<AudioClip>();

    [SerializeField]
    GameObject bgmPlayer;
    AudioSource bgmP;
    [SerializeField]
    GameObject tabSoundPlayer;
    [SerializeField]
    GameObject etcPlayer;
    AudioSource[] etcP = new AudioSource[2];

    [SerializeField] AudioSource[] tabSound = new AudioSource[3];
    [SerializeField] List<AudioClip> bgmList = new List<AudioClip>();
    [SerializeField] List<AudioClip> etcList = new List<AudioClip>();
    [SerializeField] int etcIndex = 0;

    int nowBgmIndex = 0;

    int audioSourceIndex = 0;

    private void Awake()
    {
        bgmPlayer = transform.GetChild(0).gameObject;
        tabSoundPlayer = transform.GetChild(1).gameObject;
        etcPlayer = transform.GetChild(2).gameObject;
    }

    private void Start()
    {
        for (int i = 0; i < tabSound.Length; ++i)
        {
            var audioS = tabSoundPlayer.gameObject.GetComponent<AudioSource>();
            tabSound[i] = audioS;
        }

        bgmP = bgmPlayer.gameObject.GetComponent<AudioSource>();

        etcP[0] = etcPlayer.gameObject.transform.GetChild(0).GetComponent<AudioSource>();
        etcP[0].clip = etcList[0];
        etcP[1] = etcPlayer.gameObject.transform.GetChild(1).GetComponent<AudioSource>();
        etcP[1].clip = etcList[1];
    }
    public void PlayTapSound()
    {
        tabSound[audioSourceIndex].PlayOneShot(audioClip[(int)ESoundType.TabEffect]);
        audioSourceIndex++;
        if (tabSound.Length <= audioSourceIndex) audioSourceIndex = 0;
    }
    public void PlayBGM()
    {
        if (bgmList.Count == 0)
        {
            Debug.LogWarning("BGM 클립이 없습니다.");
            return;
        }

        if (bgmList.Count <= nowBgmIndex)
        {
            nowBgmIndex = 0;
        }

        bgmP.clip = bgmList[nowBgmIndex];

        bgmP.Play();

        // OnComplete 이벤트에 다음 클립 재생 함수를 등록
        StartCoroutine(VerifyPlaying());
    }

    public void PlayClearEffect(int soundIndex = 0)
    {
        etcP[0].Play();
        etcP[1].Play();
    }

    // 다음 클립 재생 함수
    private void PlayNextClip()
    {
        nowBgmIndex++;
        PlayBGM();
    }
    IEnumerator VerifyPlaying() // 곡이 끝났는가 확인
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (!bgmP.isPlaying)
            {
                PlayNextClip();
            }
        }
    }
}
