//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class PlayerUnitCtrl : MonoBehaviour
//{
//    private float detectionRadius = 3f; // Ž�� �ݰ�
//    public float moveSpeed = 3f; // �̵� �ӵ�
//    public Animator animator;
//    public Transform closestPlayer;
    
//    public UnitMoveToTarget unitManager;

//    public ArchorMoveAndAttack archorMoveAndAttack;
//    public bool isTrace = false;
//    public bool isHandle = false;
//    void Start()
//    {
//        if (this.gameObject.transform.GetComponent<UnitMoveToTarget>())
//        unitManager = transform.GetComponent<UnitMoveToTarget>();
//        if (this.gameObject.transform.GetComponent<ArchorMoveAndAttack>())
//            archorMoveAndAttack = transform.GetComponent<ArchorMoveAndAttack>();
//        animator = GetComponent<Animator>();

//    }

//    void Update()
//    {
//        // ����� �÷��̾� Ž��
//        closestPlayer = FindClosestPlayer();

//        if (closestPlayer != null)
//        {
//            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);

//            // Ž�� �ݰ� ���� ���� ���
//            if (distanceToPlayer <= detectionRadius)
//            {
//                if (distanceToPlayer > 1f)
//                {

//                    if ( !isTrace && !isHandle)
//                    {
//                        if (unitManager != null)
//                        {
//                            isTrace = true;
//                            unitManager.tr = transform;
//                            unitManager.startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
//                            unitManager.targetPos = new Vector2Int(Mathf.RoundToInt(closestPlayer.position.x), Mathf.RoundToInt(closestPlayer.position.y));
//                            unitManager.PathFinding(); // ��� ã�� ȣ��
//                        }
//                        else if (archorMoveAndAttack != null)
//                        {
//                            isTrace = true;
//                            archorMoveAndAttack.unitTransform = transform;
//                            archorMoveAndAttack.startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
//                            archorMoveAndAttack.targetPos = new Vector2Int(Mathf.RoundToInt(closestPlayer.position.x), Mathf.RoundToInt(closestPlayer.position.y));
//                            archorMoveAndAttack.PathFinding(); // ��� ã�� ȣ��

//                        }
                      

//                    }
//                }
//                else if (distanceToPlayer <= 1)
//                {
//                    //if (unitManager.moveCoroutine != null)
//                    //{
//                    //    unitManager.StopCoroutine(unitManager.moveCoroutine);
//                    //    isTrace = false;
//                    //}
//                    //if (archorMoveAndAttack.moveCoroutine != null)
//                    //{
//                    //    archorMoveAndAttack.StopCoroutine(archorMoveAndAttack.moveCoroutine);
//                    //    isTrace = false;
//                    //}
//                }
//            }
//        }

//        Transform FindClosestPlayer()
//        {
//            GameObject[] players = GameObject.FindGameObjectsWithTag("EnemyUnit");
//            Transform closestPlayer = null;
//            float closestDistance = Mathf.Infinity;

//            foreach (GameObject player in players)
//            {
//                float distance = Vector3.Distance(transform.position, player.transform.position);
//                if (distance < closestDistance)
//                {
//                    closestDistance = distance;
//                    closestPlayer = player.transform;
//                }
//            }

//            return closestPlayer;
//        }
//    }
//}




