using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyMoveAndAttack : MonoBehaviour
{
    public Transform unitTransform; // 이동시키기 위해 선택된 유닛
    public Vector2Int bottomLeft,  startPos, targetPos;
    public Vector2Int topRight = new Vector2Int(60,60);
    public List<Node> FinalNodeList; // 최종적으로 정해진 노드 리스트
    public bool allowDiagonal, dontCrossCorner; // 대각선 이동, 코너 이동 불값

    private int sizeX, sizeY; // x축 방향의 노드 개수, y축 방향의 노드 개수
    [SerializeField]
    public Node[,] NodeArray; // 노드의 배열
    [SerializeField]
    public Node StartNode; // 시작 노드
    [SerializeField]
    private Node TargetNode; // 목표 노드
    [SerializeField]
    private Node CurNode; // 현재 탐색 중인 노드
    [SerializeField]
    private List<Node> OpenList, ClosedList; // 탐색 후보 리스트, 이미 탐색 검사 끝난 리스트
    public Coroutine moveCoroutine; // 이동 코루틴

    //public UnitData unitData;
    public Animator animator;
    public GameObject attackMotion;
    public float attackCooldown = 2f; // 공격 쿨타임
    public Rigidbody2D rb;
    private float lastAttackTime; // 마지막 공격 시간
    public bool isMove = false;
    public Transform closestEnemy;

    public void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        unitTransform = transform;
    }
    void Update()
    {
        

        // 근처 적 탐지
        closestEnemy = FindClosestEnemy();

        if (closestEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, closestEnemy.position);

            // 공격 범위 내에 있고, 쿨타임이 끝났다면
            if (distance <= 2f && Time.time >= lastAttackTime + attackCooldown && !isMove)
            {
                Debug.Log("EnemyAttack");
                
                Attack(closestEnemy);
                isMove = false;
            }
            else if (distance > 1.5f && distance <=30f && !isMove)
            {
                startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                targetPos = new Vector2Int(Mathf.RoundToInt(closestEnemy.transform.position.x), Mathf.RoundToInt(closestEnemy.transform.position.y));

                PathFinding();
                isMove = true;
            }
            else
            {
                isMove = false;
            }
        }
    }

    void Attack(Transform enemy)
    {
        Vector3 direction = (enemy.position - transform.position).normalized;
        animator.SetFloat("posX", direction.x);
        animator.SetFloat("posY", direction.y);//일단 애니메이션 적 쳐다보게
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;


        // 적의 스크립트 가져오기
        PlayerUnitDamage enemyCtrl = enemy.GetComponent<PlayerUnitDamage>();
        if (enemyCtrl != null && enemyCtrl.health > 0)
        {
            float damage = 20f;
            enemyCtrl.TakeDamage(damage);
            GameObject attackEffect = Instantiate(attackMotion, enemy.position, Quaternion.identity);
            Destroy(attackEffect, 0.1f); // 이펙트 0.1초 후 파괴
        }

        // 마지막 공격 시간 업데이트
        lastAttackTime = Time.time;
    }
    Transform FindClosestEnemy()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("PlayerUnit");
        Transform closestEnemy = null;
        float closestDistance = 30f;

        //거리계산해서 젤가까운적 지정
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance <= 30)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
                //Debug.Log(enemy.name);
            }

        }



        return closestEnemy;
    }
    public void PathFinding()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

        //배열 초기화 벽감지
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                {
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        isWall = true;
                    }
                }
                //NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }

        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];
        //Debug.Log(TargetNode.x);
        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();
        foreach (Node node in NodeArray)
        {
            node.G = int.MaxValue;
            node.H = 0; // 초기화
            node.ParentNode = null;
        }

        StartNode.G = 0;
        StartNode.H = (Mathf.Abs(StartNode.x - TargetNode.x) + Mathf.Abs(StartNode.y - TargetNode.y)) * 10;
        while (OpenList.Count > 0)
        {
            CurNode = OpenList[0];//시작위치 노드를 현재노드로
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F < CurNode.F || (OpenList[i].F == CurNode.F && OpenList[i].H < CurNode.H))
                {
                    CurNode = OpenList[i];
                }
            }
            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);
            //만약 현재 노드가 목표 노드와 같다면
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;              
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode); 
                    TargetCurNode = TargetCurNode.ParentNode; //부모 노드로 이동
                }
                FinalNodeList.Add(StartNode);//시작 노드 추가
                FinalNodeList.Reverse(); //리스트를 역순으로 뒤집음 (시작에서 목표까지의 경로)
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);
                }
                moveCoroutine = StartCoroutine(MoveAlongPath());
                return;
            }
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1); 
                OpenListAdd(CurNode.x - 1, CurNode.y + 1); 
                OpenListAdd(CurNode.x - 1, CurNode.y - 1); 
                OpenListAdd(CurNode.x + 1, CurNode.y - 1); 
            }
            OpenListAdd(CurNode.x, CurNode.y + 1); 
            OpenListAdd(CurNode.x + 1, CurNode.y); 
            OpenListAdd(CurNode.x, CurNode.y - 1); 
            OpenListAdd(CurNode.x - 1, CurNode.y); 
        }

    }

    void OpenListAdd(int checkX, int checkY)
    {
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            if (allowDiagonal)
                if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall)
                    return;

            if (dontCrossCorner)
                if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall)
                    return;

            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);//루트2 )1:루트2:1)
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }

    private IEnumerator MoveAlongPath()//이동로직
    {
        foreach (Node node in FinalNodeList)
        {
            if (this.gameObject.CompareTag("EnemyUnit"))
            {

                Vector3 targetPosition = new Vector3(node.x, node.y, 0);
                while (Vector3.Distance(unitTransform.position, targetPosition) > 1f)
                {

                    Vector3 roundedTargetPosition = new Vector3(Mathf.Round(targetPosition.x), Mathf.Round(targetPosition.y), targetPosition.z);
                    animator.SetFloat("posX", targetPosition.x - unitTransform.position.x);
                    animator.SetFloat("posY", targetPosition.y - unitTransform.position.y);


                    unitTransform.position = Vector3.MoveTowards(unitTransform.position, roundedTargetPosition, Time.deltaTime * 2f);
                    yield return null;
                }
            }

        }
        isMove = false;
        startPos = new Vector2Int(Mathf.RoundToInt(unitTransform.position.x), Mathf.RoundToInt(unitTransform.position.y));
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        FinalNodeList.Clear();
        StartCoroutine(DelayForSplite());

    }
    public IEnumerator DelayForSplite()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;


        yield return new WaitForSeconds(0.3f);
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

    }

    void OnDrawGizmos()
    {
        if (FinalNodeList != null && FinalNodeList.Count != 0)
        {
            for (int i = 0; i < FinalNodeList.Count - 1; i++)
            {
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
            }
        }
    }
}
