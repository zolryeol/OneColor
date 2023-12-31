using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 로고에 쓸 페이드아웃
/// </summary>

public class LogoFadeInOut : MonoBehaviour
{
    public Image logo;

    private void Awake()
    {
        logo = GameObject.Find("Logo").GetComponent<Image>();
    }

    private void Start()
    {
        Invoke("FadeIn", 0.5f);
    }

    public void GoTitle()
    {
    }

    public void FadeOut()
    {
        StartCoroutine(LogoFadeOutCoroutine());

    }

    public void FadeIn()
    {
        StartCoroutine(LogoFadeInCoroutine());
    }

    IEnumerator LogoFadeInCoroutine()
    {
        float fadeCount = 0; // 알파값

        var logoColor = logo.color;

        WaitForSeconds wfs = new WaitForSeconds(0.01f);

        while (fadeCount <= 1.0f) // 255 가아니라 0~1사이값으로 쓴다.
        {
            fadeCount += 0.01f;
            yield return wfs; // 0.01 초마다 수행시킬것이다.

            logo.color = new Color(logoColor.r, logoColor.g, logoColor.b, fadeCount);
        }
        FadeOut();
    }

    IEnumerator LogoFadeOutCoroutine()
    {
        float fadeCount = 1; // 알파값

        var logoColor = logo.color;

        WaitForSeconds wfs = new WaitForSeconds(0.01f);

        while (fadeCount >= 0.0f) // 255 가아니라 0~1사이값으로 쓴다.
        {
            fadeCount -= 0.01f;
            yield return wfs; // 0.01 초마다 수행시킬것이다.

            logo.color = new Color(logoColor.r, logoColor.g, logoColor.b, fadeCount);
        }

        transform.parent.gameObject.SetActive(false);
    }
}
