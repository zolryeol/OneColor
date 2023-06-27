using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LeafSwing : MonoBehaviour
{
    public float swingSpeed = 1.0f;    // 스윙 속도
    public float swingRange = 30.0f;   // 스윙 범위

    private Vector3 originalPosition;  // 초기 위치 값
    private Quaternion originalRotation;  // 초기 회전 값

    void Start()
    {
        originalPosition = transform.position; // 초기 위치 값을 저장
        originalRotation = transform.rotation; // 초기 회전 값을 저장
    }

    void Update()
    {
        // 스윙 효과 계산
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingRange; // 스윙 각도 계산
        Quaternion rotation = originalRotation * Quaternion.Euler(0f, 0f, angle); // 초기 회전 값에 스윙 각도를 적용하여 회전
        transform.rotation = rotation; // 회전 값 적용

        // 스윙 각도에 따라 위치 조정
        Vector3 offset = (rotation * Vector3.up) * angle * 0.1f;
        transform.position = originalPosition + offset; // 초기 위치 값에 스윙 각도를 적용하여 위치 조정
    }
}