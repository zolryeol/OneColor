using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    public float moveSpeed = 1.0f; // ���� �̵� �ӵ�
    public float spawnInterval = 5.0f; // ���� ���� ����
    public float despawnDelay = 3.0f; // ���� ���� ������

    private Vector3 initialPosition; // ���� �ʱ� ��ġ
    private bool isMoving = true; // ���� �̵� ����

    private void Start()
    {
        // ���� �ʱ� ��ġ ����
        initialPosition = transform.position;

        // ���� �̵� �ڷ�ƾ ����
        StartCoroutine(MoveCloud());
    }

    IEnumerator MoveCloud()
    {
        while (isMoving)
        {
            // ���� �̵�
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    void Update()
    {
        // ������ ȭ���� ����� ����
        float cloudWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        float screenLeftX = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        float screenRightX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        if (transform.position.x + cloudWidth / 2 < screenLeftX || transform.position.x - cloudWidth / 2 > screenRightX)
        {
            // ���� ����
            StartCoroutine(DespawnCloud());
        }
    }

    IEnumerator DespawnCloud()
    {
        // ���� ���� ������ �Ŀ� ���� ����
        yield return new WaitForSeconds(despawnDelay);
        Destroy(gameObject);

        // ���� �ð� �Ŀ� ���� �����
        yield return new WaitForSeconds(spawnInterval);

        // ���� �����
        GameObject newCloud = Instantiate(gameObject, initialPosition, Quaternion.identity);

        // ���� �̵� �ڷ�ƾ ����
        CloudController newCloudController = newCloud.GetComponent<CloudController>();
        newCloudController.isMoving = true;
        StartCoroutine(newCloudController.MoveCloud());
    }
}