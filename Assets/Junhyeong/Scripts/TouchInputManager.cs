using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchInputManager : MonoBehaviour
{
    Camera mainCamera;

    Dictionary<int, bool> fingerState = new Dictionary<int, bool>();

    InputAction backAction;

    UIController uiController;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        backAction = new InputAction("ESC", InputActionType.Button, "<Keyboard>/escape");
        backAction.Enable();
        uiController = FindObjectOfType<UIController>();
    }

    private void OnEnable()
    {
        backAction.performed += ESC;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void MultipleTouch()
    {
        var activeTouches = Touch.activeFingers;

        if (activeTouches.Count > 0)
        {
            // 각 터치 정보를 가져와서 처리
            foreach (var finger in activeTouches)
            {
                int fingerId = finger.index;
                // 현재 터치 중인 Touch 구조체 가져오기
                Touch touch = finger.currentTouch;

                if (!fingerState.ContainsKey(fingerId) || fingerState[fingerId] == false)
                {
                    fingerState[fingerId] = true;
                    // 터치한 지점의 좌표 계산
                    Vector3 touchPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.screenPosition.x, touch.screenPosition.y, 0f));

                    // Raycast를 사용하여 터치한 오브젝트 판별
                    RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                    // 오브젝트가 있을 경우
                    if (hit.collider != null && hit.transform.CompareTag("Tile"))
                    {
                        if (!uiController.GetPopUp_Ingame.activeSelf) /// 팝업창 켜져있을때 예외처리
                        {
                            var t = hit.collider.gameObject.GetComponent<CTile>();

                            t.TouchedFunction();

                            //Debug.Log(hit.transform.name + "을 클릭함 복수터치");
                        }
                    }
                }
                if (finger.currentTouch.ended) // 터치를 뗀 경우
                {
                    fingerState[fingerId] = false;
                }
            }
        }
    }

    void ESC(InputAction.CallbackContext context)
    {
        Debug.Log("ESC를 눌렀다");

        // 게임 실행중이면 실행용 팝업

        // 스테이지 셀렉트 화면이라면 셀렉트용 팝업

        if (uiController.GetTitle.activeSelf || uiController.GetGameClear.activeSelf || uiController.GetGameOver.activeSelf)
        {
            return;
        }

        if (uiController.GetStageSelect.activeSelf)
        {
            uiController.PopUp_Exit();
            return;
        }
        else if (!uiController.GetStageSelect.activeSelf)
        {
            uiController.PopUp_Ingame();
            return;
        }
    }

    void Update()
    {
        MultipleTouch();
    }
}