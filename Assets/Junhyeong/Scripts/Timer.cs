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

    public Text timerText; // UI에서 받아 올 것
    public float totalTime = 10; // 주어질 시간
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
            Debug.Log(_givenTime + "초뒤 퍼짐");

            yield return wfs;

            _ac();
        }

        Debug.Log("감염타이머 끝");
    }

    public IEnumerator MoveTimer(float _givenTime, System.Action _ac)
    {
        WaitForSeconds wfs = new WaitForSeconds(_givenTime);

        while (GameManager.Instance.infectionTimer != null)
        {
            Debug.Log(_givenTime + "초뒤 이동");

            yield return wfs;

            _ac();
        }

        Debug.Log("무브타이머 끝");
    }

    public IEnumerator SpectialTileTimer(float _givenTime, System.Action _ac)
    {
        float remainingTime = _givenTime;

        int beforeTime = (int)remainingTime;

        while (0 <= remainingTime)
        {
            if ((int)remainingTime < beforeTime)
            {
                Debug.Log("남은시간" + (int)remainingTime);
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

        // 시간이 0이하로 되었을 경우
        if (!isClear)
        {
            formattedTime = string.Format("{0:0}:{1:00}", 0, 0);
            timerText.text = formattedTime;

            StopTime();
        }
        else
        {
            // 클리어 한경우;
            Debug.Log("타이머상에서 클리어");
        }
    }
}
