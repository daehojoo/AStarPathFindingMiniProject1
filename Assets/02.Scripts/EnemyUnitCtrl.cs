//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyUnitCtrl : MonoBehaviour
//{
//    private float detectionRadius = 3f; // 탐지 반경
//    public float moveSpeed = 3f; // 이동 속도
//    public Animator animator;
//    public Transform closestPlayer;
//    public UnitMoveToTarget unitManager;
//    public bool isTrace =false;
//    void Start()
//    {
//         unitManager = transform.GetComponent<UnitMoveToTarget>();
//        animator = GetComponent<Animator>();

//    }

//    void Update()
//    {
//        // 가까운 플레이어 탐지
//        closestPlayer = FindClosestPlayer();

//        if (closestPlayer != null)
//        {
//            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);
            
//            // 탐지 반경 내에 있을 경우
//            if (distanceToPlayer <= detectionRadius)
//            {
//                if (distanceToPlayer > 1f)
//                {
                    
//                    if (unitManager != null && !isTrace)
//                    {
//                        isTrace = true;
//                        unitManager.unitTransform = transform;
//                        unitManager.startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
//                        unitManager.targetPos = new Vector2Int(Mathf.RoundToInt(closestPlayer.position.x), Mathf.RoundToInt(closestPlayer.position.y));
//                        //Debug.Log(unitManager.targetPos);
//                        //if (unitManager.moveCoroutine != null)
//                        //{
//                        //    unitManager.StopCoroutine(unitManager.moveCoroutine);
//                        //}
//                        unitManager.PathFinding(); // 경로 찾기 호출
//                        Debug.Log("EnemyPathFindingStart");
//                    }
//                }
//                else if (distanceToPlayer <= 1)
//                {
//                    if (unitManager.moveCoroutine != null)
//                    {
//                        unitManager.StopCoroutine(unitManager.moveCoroutine);
//                        isTrace = false;
//                    }
//                }
//            }
//        }

//        Transform FindClosestPlayer()
//        {
//            GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerUnit");
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
   



