using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    Button chapterButton;

    private void Awake()
    {
        chapterButton = this.GetComponent<Button>();
    }

    public void ButtonInhibition() // ��ư �����ϰ���
    {
        chapterButton.enabled = false;
    }

    public void ButtonPermission() // ��ư ��밡�� 
    {
        chapterButton.enabled = true;
        chapterButton.transform.GetChild(1).gameObject.SetActive(false);
    }




}
