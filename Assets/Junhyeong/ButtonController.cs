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

    public void ButtonInhibition() // 버튼 사용못하게함
    {
        chapterButton.enabled = false;
    }

    public void ButtonPermission() // 버튼 사용가능 
    {
        chapterButton.enabled = true;
        chapterButton.transform.GetChild(1).gameObject.SetActive(false);
    }




}
