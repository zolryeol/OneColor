using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �ΰ� �� ���̵�ƿ�
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
        float fadeCount = 0; // ���İ�

        var logoColor = logo.color;

        WaitForSeconds wfs = new WaitForSeconds(0.01f);

        while (fadeCount <= 1.0f) // 255 ���ƴ϶� 0~1���̰����� ����.
        {
            fadeCount += 0.01f;
            yield return wfs; // 0.01 �ʸ��� �����ų���̴�.

            logo.color = new Color(logoColor.r, logoColor.g, logoColor.b, fadeCount);
        }
        FadeOut();
    }

    IEnumerator LogoFadeOutCoroutine()
    {
        float fadeCount = 1; // ���İ�

        var logoColor = logo.color;

        WaitForSeconds wfs = new WaitForSeconds(0.01f);

        while (fadeCount >= 0.0f) // 255 ���ƴ϶� 0~1���̰����� ����.
        {
            fadeCount -= 0.01f;
            yield return wfs; // 0.01 �ʸ��� �����ų���̴�.

            logo.color = new Color(logoColor.r, logoColor.g, logoColor.b, fadeCount);
        }

        transform.parent.gameObject.SetActive(false);
    }
}
