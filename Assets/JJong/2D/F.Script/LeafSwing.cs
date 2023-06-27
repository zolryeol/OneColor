using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LeafSwing : MonoBehaviour
{
    public float swingSpeed = 1.0f;    // ���� �ӵ�
    public float swingRange = 30.0f;   // ���� ����

    private Vector3 originalPosition;  // �ʱ� ��ġ ��
    private Quaternion originalRotation;  // �ʱ� ȸ�� ��

    void Start()
    {
        originalPosition = transform.position; // �ʱ� ��ġ ���� ����
        originalRotation = transform.rotation; // �ʱ� ȸ�� ���� ����
    }

    void Update()
    {
        // ���� ȿ�� ���
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingRange; // ���� ���� ���
        Quaternion rotation = originalRotation * Quaternion.Euler(0f, 0f, angle); // �ʱ� ȸ�� ���� ���� ������ �����Ͽ� ȸ��
        transform.rotation = rotation; // ȸ�� �� ����

        // ���� ������ ���� ��ġ ����
        Vector3 offset = (rotation * Vector3.up) * angle * 0.1f;
        transform.position = originalPosition + offset; // �ʱ� ��ġ ���� ���� ������ �����Ͽ� ��ġ ����
    }
}