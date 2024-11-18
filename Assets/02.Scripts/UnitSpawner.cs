using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unitPrefab; // 스폰할 유닛의 프리팹
    public Transform spawnPoint; // 유닛이 스폰될 위치
    public Image coolTimeBar; // 쿨타임 바 UI 이미지
    public float spawnCooldown = 15f; // 다음 유닛 스폰까지의 시간
    private float timeSinceLastSpawn = 0f; // 마지막 스폰 이후 경과 시간

    private void Start()
    {
        coolTimeBar = gameObject.transform.GetChild(0).GetChild(5).GetComponent<Image>();
        coolTimeBar.fillAmount = 0f; // 쿨타임 바 초기화
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime; // 경과 시간 업데이트

        // 쿨타임이 끝났으면 유닛 스폰
        if (timeSinceLastSpawn >= spawnCooldown)
        {
            SpawnUnit(); // 유닛 스폰
            timeSinceLastSpawn = 0f; // 경과 시간 초기화
        }

        // 쿨타임 바 업데이트
        coolTimeBar.fillAmount = timeSinceLastSpawn / spawnCooldown; // 쿨타임 바 갱신
    }

    private void SpawnUnit()
    {
        if (unitPrefab != null && spawnPoint != null)
        {
            Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
