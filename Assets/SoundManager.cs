using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ����Ŵ��� ���� ������ ���� �ʱ⶧���� ���Ż�ϰ� ����Ѵ�.
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
            Debug.LogWarning("BGM Ŭ���� �����ϴ�.");
            return;
        }

        if (bgmList.Count <= nowBgmIndex)
        {
            nowBgmIndex = 0;
        }

        bgmP.clip = bgmList[nowBgmIndex];

        bgmP.Play();

        // OnComplete �̺�Ʈ�� ���� Ŭ�� ��� �Լ��� ���
        StartCoroutine(VerifyPlaying());
    }

    public void PlayClearEffect(int soundIndex = 0)
    {
        etcP[0].Play();
        etcP[1].Play();
    }

    // ���� Ŭ�� ��� �Լ�
    private void PlayNextClip()
    {
        nowBgmIndex++;
        PlayBGM();
    }
    IEnumerator VerifyPlaying() // ���� �����°� Ȯ��
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
