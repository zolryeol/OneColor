using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer
{
    public Timer() { }
    public Timer(int _givenTime)
    {
        totalTime = _givenTime;
    }

    public Text timerText; // UI���� �޾� �� ��
    public float totalTime = 10; // �־��� �ð�
    float remainingTime;
    public bool isClear = false;
    public System.Action gameOver;

    public float GetRemainingTime => remainingTime;
    public void StopTime()
    {
        gameOver();
    }

    public IEnumerator InfectionTimer(float _givenTime, System.Action _ac)
    {
        WaitForSeconds wfs = new WaitForSeconds(_givenTime);

        while (GameManager.Instance.infectionTimer != null)
        {
            Debug.Log(_givenTime + "�ʵ� ����");

            yield return wfs;

            _ac();
        }

        Debug.Log("����Ÿ�̸� ��");
    }

    public IEnumerator MoveTimer(float _givenTime, System.Action _ac)
    {
        WaitForSeconds wfs = new WaitForSeconds(_givenTime);

        while (GameManager.Instance.infectionTimer != null)
        {
            Debug.Log(_givenTime + "�ʵ� �̵�");

            yield return wfs;

            _ac();
        }

        Debug.Log("����Ÿ�̸� ��");
    }

    public IEnumerator SpectialTileTimer(float _givenTime, System.Action _ac)
    {
        float remainingTime = _givenTime;

        int beforeTime = (int)remainingTime;

        while (0 <= remainingTime)
        {
            if ((int)remainingTime < beforeTime)
            {
                Debug.Log("�����ð�" + (int)remainingTime);
                beforeTime = (int)remainingTime;
            }

            remainingTime -= Time.deltaTime;

            yield return null;
        }

        _ac();
    }
    public IEnumerator UpdateTimer()
    {
        remainingTime = totalTime;

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        string formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerText.text = formattedTime;

        yield return new WaitForSeconds(1f);

        while (remainingTime > 0f && !isClear)
        {
            remainingTime -= Time.deltaTime;

            minutes = Mathf.FloorToInt(remainingTime / 60);
            seconds = Mathf.FloorToInt(remainingTime % 60);

            formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            timerText.text = formattedTime;

            yield return null;
        }

        // �ð��� 0���Ϸ� �Ǿ��� ���
        if (!isClear)
        {
            formattedTime = string.Format("{0:0}:{1:00}", 0, 0);
            timerText.text = formattedTime;

            StopTime();
        }
        else
        {
            // Ŭ���� �Ѱ��;
            Debug.Log("Ÿ�̸ӻ󿡼� Ŭ����");
        }
    }
}
