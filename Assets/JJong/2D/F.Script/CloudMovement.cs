using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloudMovement : MonoBehaviour
{
    public float speed = 1.0f;     // 이동 속도
    public float distance = 5.0f; // 좌우 이동 거리

    private Vector3 startPosition; // 초기 위치
    private Vector3 targetPosition; // 목표 위치

    private bool isMovingRight = true; // 현재 우측으로 이동 중인지 여부

    void Start()
    {
        startPosition = transform.position; // 초기 위치 저장
        targetPosition = CalculateTargetPosition(); // 목표 위치 계산
    }

    void Update()
    {
        // 현재 이동 방향에 따라 목표 위치를 설정
        if (isMovingRight)
        {
            targetPosition = new Vector3(startPosition.x + distance, startPosition.y, startPosition.z);
        }
        else
        {
            targetPosition = new Vector3(startPosition.x - distance, startPosition.y, startPosition.z);
        }

        // 목표 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 목표 위치에 도달하면 방향을 바꾸고 목표 위치를 업데이트
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMovingRight = !isMovingRight; // 이동 방향 반전
            targetPosition = CalculateTargetPosition(); // 목표 위치 업데이트
        }
    }

    // 목표 위치를 계산하는 함수
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