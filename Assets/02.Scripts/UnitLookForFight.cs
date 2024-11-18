using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLookForFight : MonoBehaviour
{
    public Animator animator;
    public float detectionRadius = 2f; // ���� ������ �ݰ�
    public GameObject attackMotion;
    public float attackCooldown = 1f; // ���� ��Ÿ��
    public Rigidbody2D rb;
    private float lastAttackTime; // ������ ���� �ð�

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //attackMotion = Resources.Load<GameObject>("normalAttack");
    }

    void Update()
    {
        // ��ó �� Ž��
        Transform closestEnemy = FindClosestEnemy();

        if (closestEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, closestEnemy.position);

            // ���� ���� ���� �ְ�, ��Ÿ���� �����ٸ�
            if (distance <= 1f && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack(closestEnemy);
            }
            else
            {
                // �� �������� �ٶ󺸱�
                LookAtEnemy(closestEnemy);
            }
        }
    }

    Transform FindClosestEnemy()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyUnit");
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

       //�Ÿ�����ؼ� ��������� ����
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance <= detectionRadius)//���� 2�ȿ������� ��
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
        animator.SetFloat("posX",direction.x);
        animator.SetFloat("posY", direction.y);//�ϴ� �ִϸ��̼� �� �Ĵٺ���
    }
    void Attack(Transform enemy)
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        // ���� ����Ʈ ����
        GameObject attackEffect = Instantiate(attackMotion, enemy.position, Quaternion.identity);
        Destroy(attackEffect, 0.1f); // ����Ʈ 0.1�� �� �ı�

        // ���� ��ũ��Ʈ ��������
        EnemyDamage enemyCtrl = enemy.GetComponent<EnemyDamage>();
        if (enemyCtrl != null)
        {
            float damage = 20f; // ���� ������ ��
            enemyCtrl.TakeDamage(damage); // ������ �ֱ�
        }

        // ������ ���� �ð� ������Ʈ
        lastAttackTime = Time.time;
    }


}
