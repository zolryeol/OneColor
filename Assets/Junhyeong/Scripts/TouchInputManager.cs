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
            // �� ��ġ ������ �����ͼ� ó��
            foreach (var finger in activeTouches)
            {
                int fingerId = finger.index;
                // ���� ��ġ ���� Touch ����ü ��������
                Touch touch = finger.currentTouch;

                if (!fingerState.ContainsKey(fingerId) || fingerState[fingerId] == false)
                {
                    fingerState[fingerId] = true;
                    // ��ġ�� ������ ��ǥ ���
                    Vector3 touchPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.screenPosition.x, touch.screenPosition.y, 0f));

                    // Raycast�� ����Ͽ� ��ġ�� ������Ʈ �Ǻ�
                    RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                    // ������Ʈ�� ���� ���
                    if (hit.collider != null && hit.transform.CompareTag("Tile"))
                    {
                        if (!uiController.GetPopUp_Ingame.activeSelf) /// �˾�â ���������� ����ó��
                        {
                            var t = hit.collider.gameObject.GetComponent<CTile>();

                            t.TouchedFunction();

                            //Debug.Log(hit.transform.name + "�� Ŭ���� ������ġ");
                        }
                    }
                }
                if (finger.currentTouch.ended) // ��ġ�� �� ���
                {
                    fingerState[fingerId] = false;
                }
            }
        }
    }

    void ESC(InputAction.CallbackContext context)
    {
        Debug.Log("ESC�� ������");

        // ���� �������̸� ����� �˾�

        // �������� ����Ʈ ȭ���̶�� ����Ʈ�� �˾�

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