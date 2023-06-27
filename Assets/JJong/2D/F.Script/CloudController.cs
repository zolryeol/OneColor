using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    public float moveSpeed = 1.0f; // 구름 이동 속도
    public float spawnInterval = 5.0f; // 구름 생성 간격
    public float despawnDelay = 3.0f; // 구름 삭제 딜레이

    private Vector3 initialPosition; // 구름 초기 위치
    private bool isMoving = true; // 구름 이동 상태

    private void Start()
    {
        // 구름 초기 위치 저장
        initialPosition = transform.position;

        // 구름 이동 코루틴 실행
        StartCoroutine(MoveCloud());
    }

    IEnumerator MoveCloud()
    {
        while (isMoving)
        {
            // 구름 이동
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    void Update()
    {
        // 구름이 화면을 벗어나면 삭제
        float cloudWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        float screenLeftX = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        float screenRightX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        if (transform.position.x + cloudWidth / 2 < screenLeftX || transform.position.x - cloudWidth / 2 > screenRightX)
        {
            // 구름 삭제
            StartCoroutine(DespawnCloud());
        }
    }

    IEnumerator DespawnCloud()
    {
        // 구름 삭제 딜레이 후에 구름 삭제
        yield return new WaitForSeconds(despawnDelay);
        Destroy(gameObject);

        // 일정 시간 후에 구름 재생성
        yield return new WaitForSeconds(spawnInterval);

        // 구름 재생성
        GameObject newCloud = Instantiate(gameObject, initialPosition, Quaternion.identity);

        // 구름 이동 코루틴 실행
        CloudController newCloudController = newCloud.GetComponent<CloudController>();
        newCloudController.isMoving = true;
        StartCoroutine(newCloudController.MoveCloud());
    }
}