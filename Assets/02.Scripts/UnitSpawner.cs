using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unitPrefab; // ������ ������ ������
    public Transform spawnPoint; // ������ ������ ��ġ
    public Image coolTimeBar; // ��Ÿ�� �� UI �̹���
    public float spawnCooldown = 15f; // ���� ���� ���������� �ð�
    private float timeSinceLastSpawn = 0f; // ������ ���� ���� ��� �ð�

    private void Start()
    {
        coolTimeBar = gameObject.transform.GetChild(0).GetChild(5).GetComponent<Image>();
        coolTimeBar.fillAmount = 0f; // ��Ÿ�� �� �ʱ�ȭ
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime; // ��� �ð� ������Ʈ

        // ��Ÿ���� �������� ���� ����
        if (timeSinceLastSpawn >= spawnCooldown)
        {
            SpawnUnit(); // ���� ����
            timeSinceLastSpawn = 0f; // ��� �ð� �ʱ�ȭ
        }

        // ��Ÿ�� �� ������Ʈ
        coolTimeBar.fillAmount = timeSinceLastSpawn / spawnCooldown; // ��Ÿ�� �� ����
    }

    private void SpawnUnit()
    {
        if (unitPrefab != null && spawnPoint != null)
        {
            Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
