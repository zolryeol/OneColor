using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloudMovement : MonoBehaviour
{
    public float speed = 1.0f;     // �̵� �ӵ�
    public float distance = 5.0f; // �¿� �̵� �Ÿ�

    private Vector3 startPosition; // �ʱ� ��ġ
    private Vector3 targetPosition; // ��ǥ ��ġ

    private bool isMovingRight = true; // ���� �������� �̵� ������ ����

    void Start()
    {
        startPosition = transform.position; // �ʱ� ��ġ ����
        targetPosition = CalculateTargetPosition(); // ��ǥ ��ġ ���
    }

    void Update()
    {
        // ���� �̵� ���⿡ ���� ��ǥ ��ġ�� ����
        if (isMovingRight)
        {
            targetPosition = new Vector3(startPosition.x + distance, startPosition.y, startPosition.z);
        }
        else
        {
            targetPosition = new Vector3(startPosition.x - distance, startPosition.y, startPosition.z);
        }

        // ��ǥ ��ġ�� �̵�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ��ǥ ��ġ�� �����ϸ� ������ �ٲٰ� ��ǥ ��ġ�� ������Ʈ
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMovingRight = !isMovingRight; // �̵� ���� ����
            targetPosition = CalculateTargetPosition(); // ��ǥ ��ġ ������Ʈ
        }
    }

    // ��ǥ ��ġ�� ����ϴ� �Լ�
    Vector3 CalculateTargetPosition()
    {
        if (isMovingRight)
        {
            return new Vector3(startPosition.x + distance, startPosition.y, startPosition.z);
        }
        else
        {
            return new Vector3(startPosition.x - distance, startPosition.y, startPosition.z);
        }
    }
}