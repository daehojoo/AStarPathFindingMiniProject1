//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class ArchorMoveAndAttack : MonoBehaviour
//{
//    public Transform unitTransform; // 이동시키기 위해 선택된 유닛
//    public Vector2Int bottomLeft, topRight, startPos, targetPos;
//    public List<Node> FinalNodeList; // 최종적으로 정해진 노드 리스트
//    public bool allowDiagonal, dontCrossCorner; // 대각선 이동, 코너 이동 불값

//    private int sizeX, sizeY; // x축 방향의 노드 개수, y축 방향의 노드 개수
//    [SerializeField]
//    public Node[,] NodeArray; // 노드의 배열
//    [SerializeField]
//    public Node StartNode; // 시작 노드
//    [SerializeField]
//    private Node TargetNode; // 목표 노드
//    [SerializeField]
//    private Node CurNode; // 현재 탐색 중인 노드
//    [SerializeField]
//    private List<Node> OpenList, ClosedList; // 탐색 후보 리스트, 이미 탐색 검사 끝난 리스트
//    public Coroutine moveCoroutine; // 이동 코루틴

//    //public UnitData unitData;
//    public Animator animator;
//    public GameObject attackMotion;
//    public float attackCooldown = 1f; // 공격 쿨타임
//    public Rigidbody2D rb;
//    private float lastAttackTime; // 마지막 공격 시간
//    public bool isMove = false;
//    private bool isAttacking = false;
//    public Transform closestEnemy;
//    public LineRenderer lineRenderer; // 라인 렌더러 변수 추가
//    public void Start()
//    {
//        animator = GetComponent<Animator>();
//        rb = GetComponent<Rigidbody2D>();
//        unitTransform = transform;
//        lineRenderer = GetComponent<LineRenderer>();
//        lineRenderer.positionCount = 2; // 두 점으로 이루어진 선
//        lineRenderer.enabled = false; // 처음에는 비활성화
//    }
//    void Update()
//    {
//        ClickCtrl clickCtrl = GameObject.Find("GameManager").GetComponent<ClickCtrl>();

//        // 근처 적 탐지
//        closestEnemy = FindClosestEnemy();

//        if (closestEnemy != null && !clickCtrl.isHandle)
//        {
//            float distance = Vector3.Distance(transform.position, closestEnemy.position);
//            if (closestEnemy.CompareTag("EnemyUnit"))
//            {
//                // 공격 범위 내에 있고, 쿨타임이 끝났다면
//                if (distance <= 4f)
//                {
//                    isMove = false;
//                    Debug.Log("attack");
//                    if (Time.time >= lastAttackTime + attackCooldown && !isMove && !isAttacking)
//                    {
//                        isAttacking = true; // 공격 시작
//                        Attack(closestEnemy);
//                    }
//                }
//                else if (distance >4f && distance <= 5f && !isMove)
//                {
//                    startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
//                    targetPos = new Vector2Int(Mathf.RoundToInt(closestEnemy.transform.position.x), Mathf.RoundToInt(closestEnemy.transform.position.y));

//                    PathFinding();
//                    isMove = true;
//                }
//                else
//                {

//                }
//            }
//            else if (closestEnemy.CompareTag("EnemyBuilding"))
//            {
//                // 공격 범위 내에 있고, 쿨타임이 끝났다면
//                if (distance <= 4f)
//                {
//                    isMove = false;
//                    Debug.Log("attack");
//                    if (Time.time >= lastAttackTime + attackCooldown && !isMove && !isAttacking)
//                    {
//                        isAttacking = true; // 공격 시작
//                        Attack(closestEnemy);
//                    }
//                }
//                else if (distance > 4f && distance <= 5f && !isMove)
//                {
//                    startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
//                    targetPos = new Vector2Int(Mathf.RoundToInt(closestEnemy.transform.position.x), Mathf.RoundToInt(closestEnemy.transform.position.y));

//                    PathFinding();
//                    isMove = true;
//                }
//                else
//                {

//                }
//            }
//        }
//    }

//    void Attack(Transform enemy)
//    {
//        Vector3 direction = (enemy.position - transform.position).normalized;
//        animator.SetFloat("posX", direction.x);
//        animator.SetFloat("posY", direction.y);//일단 애니메이션 적 쳐다보게
//        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
//        ClickCtrl clickCtrl = GameObject.Find("GameManager").GetComponent<ClickCtrl>();
//        clickCtrl.isHandle = false;

//        // 적의 스크립트 가져오기
//        EnemyDamage enemyCtrl = enemy.GetComponent<EnemyDamage>();
//        if (enemyCtrl != null && enemyCtrl.health > 0)
//        {
//            float damage = 20f;
//            enemyCtrl.TakeDamage(damage);
//            ShowAttackLine(enemy.position);

//            GameObject attackEffect = Instantiate(attackMotion, enemy.position, Quaternion.identity);
//            Destroy(attackEffect, 0.1f); // 이펙트 0.1초 후 파괴
//        }

//        // 마지막 공격 시간 업데이트
//        lastAttackTime = Time.time;
//        StartCoroutine(ResetAttackState());
//    }
//    void ShowAttackLine(Vector3 targetPosition)
//    {
//        lineRenderer.SetPosition(0, transform.position); // 유닛 위치
//        lineRenderer.SetPosition(1, targetPosition); // 적 위치
//        lineRenderer.enabled = true; // 라인 렌더러 활성화

//        // 라인 렌더러의 색상 및 두께 설정
//        lineRenderer.startColor = Color.red; // 시작 색상
//        lineRenderer.endColor = Color.red; // 끝 색상
//        lineRenderer.startWidth = 0.1f; // 시작 두께
//        lineRenderer.endWidth = 0.1f; // 끝 두께

//        StartCoroutine(HideAttackLine());
//    }

//    // 라인 렌더러를 숨기기 위한 코루틴
//    private IEnumerator HideAttackLine()
//    {
//        yield return new WaitForSeconds(0.1f); // 잠시 대기
//        lineRenderer.enabled = false; // 라인 렌더러 비활성화
//    }
//    private IEnumerator ResetAttackState()
//    {
//        yield return new WaitForSeconds(attackCooldown); // 쿨타임 대기
//        isAttacking = false; // 공격 상태 초기화
//    }
//    Transform FindClosestEnemy()
//    {

//        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyUnit");
//        Transform closestEnemy = null;
//        float closestDistance = 5f;

//        //거리계산해서 젤가까운적 지정
//        foreach (GameObject enemy in enemies)
//        {
//            float distance = Vector3.Distance(transform.position, enemy.transform.position);
//            if (distance < closestDistance && distance <= 5)//범위 5안에있으면 ㄱ
//            {
//                closestDistance = distance;
//                closestEnemy = enemy.transform;
//                //Debug.Log(enemy.name);
//            }

//        }
//        if (closestEnemy == null)
//        {
//            GameObject[] enemies1 = GameObject.FindGameObjectsWithTag("EnemyBuilding");



//            //거리계산해서 젤가까운적 지정
//            foreach (GameObject enemy in enemies1)
//            {
//                float distance = Vector3.Distance(transform.position, enemy.transform.position);
//                if (distance < 10 && distance <= 10)//범위 5안에있으면 ㄱ
//                {
//                    closestDistance = distance;
//                    closestEnemy = enemy.transform;
//                    //Debug.Log(enemy.name);
//                }

//            }

//        }


//        return closestEnemy;
//    }
//    public void PathFinding()
//    {
//        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

//        //배열 초기화 벽감지
//        sizeX = topRight.x - bottomLeft.x + 1;
//        sizeY = topRight.y - bottomLeft.y + 1;
//        NodeArray = new Node[sizeX, sizeY];

//        for (int i = 0; i < sizeX; i++)
//        {
//            for (int j = 0; j < sizeY; j++)
//            {
//                bool isWall = false;
//                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
//                {
//                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
//                    {
//                        isWall = true;
//                    }
//                }
//                //NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
//            }
//        }


//        // 시작과 끝 노드 초기화
//        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];

//        //시작 노드(칸) = 노드배열[시작지점.x - 좌하단.x, 시작지점.y - 좌하단.y]
//        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];
//        //Debug.Log(TargetNode.x);

//        //탐색후보리스트에 스타트 노드 대입
//        OpenList = new List<Node>() { StartNode };
//        //탐색끝난리스트를 미리 생성
//        ClosedList = new List<Node>();
//        //최종리스트 미리 생성
//        FinalNodeList = new List<Node>();

//        foreach (Node node in NodeArray)
//        {
//            node.G = int.MaxValue; // 무한대
//            node.H = 0; // 초기화
//            node.ParentNode = null; // 부모 노드 초기화
//        }

//        StartNode.G = 0;
//        StartNode.H = (Mathf.Abs(StartNode.x - TargetNode.x) + Mathf.Abs(StartNode.y - TargetNode.y)) * 10;

//        // A* 알고리즘을 통해 경로 찾기
//        // OpenList가 비어있지 않은 동안 반복
//        while (OpenList.Count > 0)
//        {
//            // OpenList에서 첫 번째 노드를 CurNode로 설정
//            CurNode = OpenList[0];//시작위치 노드를 현재노드로

//            // OpenList에서 가장 낮은 F 값을 가진 노드를 찾기 위해 반복
//            for (int i = 1; i < OpenList.Count; i++)
//            {
//                // 현재 노드의 F 값보다 작거나 같고 H 값이 더 작은 경우 CurNode를 업데이트
//                if (OpenList[i].F < CurNode.F || (OpenList[i].F == CurNode.F && OpenList[i].H < CurNode.H))
//                {
//                    CurNode = OpenList[i];
//                }
//            }

//            // 현재 선택된 노드를 OpenList에서 제거
//            OpenList.Remove(CurNode);
//            // 현재 노드를 ClosedList에 추가 (탐색이 끝난 노드)
//            ClosedList.Add(CurNode);

//            // 만약 현재 노드가 목표 노드와 같다면
//            if (CurNode == TargetNode)
//            {
//                Node TargetCurNode = TargetNode; // 목표 노드를 설정
//                                                 // 시작 노드까지 부모 노드를 거슬러 올라가면서 최종 노드 리스트를 생성
//                while (TargetCurNode != StartNode)
//                {
//                    FinalNodeList.Add(TargetCurNode); // 최종 노드 리스트에 현재 노드 추가
//                    TargetCurNode = TargetCurNode.ParentNode; // 부모 노드로 이동
//                }
//                FinalNodeList.Add(StartNode); // 시작 노드 추가
//                FinalNodeList.Reverse(); // 리스트를 역순으로 뒤집음 (시작에서 목표까지의 경로)

//                // 최종 노드 리스트의 각 노드 좌표 출력
//                //for (int i = 0; i < FinalNodeList.Count; i++)
//                //    print(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);

//                // 경로 시각화를 위한 메서드 호출

//                // 이동 중인 코루틴이 있으면 중단
//                if (moveCoroutine != null)
//                {
//                    StopCoroutine(moveCoroutine);
//                }
//                // 새로운 경로를 따라 이동하는 코루틴 시작
//                moveCoroutine = StartCoroutine(MoveAlongPath());
//                return; // 경로 찾기 종료
//            }


//            // 대각선 이동이 허용되면 대각선 방향으로 이웃 노드 추가
//            if (allowDiagonal)
//            {
//                OpenListAdd(CurNode.x + 1, CurNode.y + 1); // 오른쪽 위
//                OpenListAdd(CurNode.x - 1, CurNode.y + 1); // 왼쪽 위
//                OpenListAdd(CurNode.x - 1, CurNode.y - 1); // 왼쪽 아래
//                OpenListAdd(CurNode.x + 1, CurNode.y - 1); // 오른쪽 아래
//            }

//            // 상하좌우 이웃 노드 추가
//            OpenListAdd(CurNode.x, CurNode.y + 1); // 위쪽
//            OpenListAdd(CurNode.x + 1, CurNode.y); // 오른쪽
//            OpenListAdd(CurNode.x, CurNode.y - 1); // 아래쪽
//            OpenListAdd(CurNode.x - 1, CurNode.y); // 왼쪽
//        }

//    }

//    void OpenListAdd(int checkX, int checkY)
//    {
//        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
//        {
//            if (allowDiagonal)
//                if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall)
//                    return;

//            if (dontCrossCorner)
//                if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall)
//                    return;

//            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
//            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

//            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
//            {
//                NeighborNode.G = MoveCost;
//                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
//                NeighborNode.ParentNode = CurNode;

//                OpenList.Add(NeighborNode);
//            }
//        }
//    }

//    private IEnumerator MoveAlongPath()
//    {


//        foreach (Node node in FinalNodeList)
//        {
//            if (this.gameObject.CompareTag("PlayerUnit"))
//            {

//                Vector3 targetPosition = new Vector3(node.x, node.y, 0);
//                //float detectionRange = closestEnemy.gameObject.CompareTag("EnemyUnit") ? 1.8f : 5.0f;

//                while (Vector3.Distance(unitTransform.position, targetPosition) > 2f)
//                {

//                    Vector3 roundedTargetPosition = new Vector3(Mathf.Round(targetPosition.x), Mathf.Round(targetPosition.y), targetPosition.z);
//                    animator.SetFloat("posX", targetPosition.x - unitTransform.position.x);
//                    animator.SetFloat("posY", targetPosition.y - unitTransform.position.y);


//                    unitTransform.position = Vector3.MoveTowards(unitTransform.position, roundedTargetPosition, Time.deltaTime * 4f);
//                    yield return null;
//                }

//            }

//        }
//        isMove = false;
//        startPos = new Vector2Int(Mathf.RoundToInt(unitTransform.position.x), Mathf.RoundToInt(unitTransform.position.y));
//        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
//        FinalNodeList.Clear();
//        StartCoroutine(DelayForSplite());

//    }
//    public IEnumerator DelayForSplite()
//    {
//        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

//        ClickCtrl clickCtrl = GameObject.Find("GameManager").GetComponent<ClickCtrl>();
//        clickCtrl.isHandle = false;

//        yield return new WaitForSeconds(0.3f);
//        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

//    }

//    void OnDrawGizmos()
//    {
//        if (FinalNodeList != null && FinalNodeList.Count != 0)
//        {
//            for (int i = 0; i < FinalNodeList.Count - 1; i++)
//            {
//                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
//            }
//        }
//    }
//}
