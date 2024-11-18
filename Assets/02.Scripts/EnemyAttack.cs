using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;
    public float detectionRadius = 2f; // 적을 감지할 반경
    public GameObject attackMotion;
    public float attackCooldown = 1f; // 공격 쿨타임

    private float lastAttackTime; // 마지막 공격 시간

    void Start()
    {
        animator = GetComponent<Animator>();
        //attackMotion = Resources.Load<GameObject>("normalAttack");
    }

    void Update()
    {
        // 근처 적 탐지
        Transform closestEnemy = FindClosestEnemy();

        if (closestEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, closestEnemy.position);

            // 공격 범위 내에 있고, 쿨타임이 끝났다면
            if (distance <= 1f && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack(closestEnemy);
            }
            else
            {
                // 적 방향으로 바라보기
                LookAtEnemy(closestEnemy);
            }
        }
    }

    Transform FindClosestEnemy()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("PlayerUnit");
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        //거리계산해서 젤가까운적 지정
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance <= detectionRadius)//범위 2안에있으면 ㄱ
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }

        }



        return closestEnemy;
    }


    void LookAtEnemy(Transform enemy)
    {

        Vector3 direction = (enemy.position - transform.position).normalized;
        animator.SetFloat("posX", direction.x);
        animator.SetFloat("posY", direction.y);//일단 애니메이션 적 쳐다보게
    }
    void Attack(Transform enemy)
    {
        // 공격 이펙트 생성
        GameObject attackEffect = Instantiate(attackMotion, enemy.position, Quaternion.identity);
        Destroy(attackEffect, 0.1f); // 이펙트 0.1초 후 파괴

        // 적의 스크립트 가져오기
        PlayerUnitDamage enemyCtrl = enemy.GetComponent<PlayerUnitDamage>();
        if (enemyCtrl != null)
        {
            float damage = 10f; // 예시 데미지 값
            enemyCtrl.TakeDamage(damage); // 데미지 주기
        }

        // 마지막 공격 시간 업데이트
        lastAttackTime = Time.time;
    }


}
