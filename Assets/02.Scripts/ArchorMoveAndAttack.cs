//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class ArchorMoveAndAttack : MonoBehaviour
//{
//    public Transform unitTransform; // �̵���Ű�� ���� ���õ� ����
//    public Vector2Int bottomLeft, topRight, startPos, targetPos;
//    public List<Node> FinalNodeList; // ���������� ������ ��� ����Ʈ
//    public bool allowDiagonal, dontCrossCorner; // �밢�� �̵�, �ڳ� �̵� �Ұ�

//    private int sizeX, sizeY; // x�� ������ ��� ����, y�� ������ ��� ����
//    [SerializeField]
//    public Node[,] NodeArray; // ����� �迭
//    [SerializeField]
//    public Node StartNode; // ���� ���
//    [SerializeField]
//    private Node TargetNode; // ��ǥ ���
//    [SerializeField]
//    private Node CurNode; // ���� Ž�� ���� ���
//    [SerializeField]
//    private List<Node> OpenList, ClosedList; // Ž�� �ĺ� ����Ʈ, �̹� Ž�� �˻� ���� ����Ʈ
//    public Coroutine moveCoroutine; // �̵� �ڷ�ƾ

//    //public UnitData unitData;
//    public Animator animator;
//    public GameObject attackMotion;
//    public float attackCooldown = 1f; // ���� ��Ÿ��
//    public Rigidbody2D rb;
//    private float lastAttackTime; // ������ ���� �ð�
//    public bool isMove = false;
//    private bool isAttacking = false;
//    public Transform closestEnemy;
//    public LineRenderer lineRenderer; // ���� ������ ���� �߰�
//    public void Start()
//    {
//        animator = GetComponent<Animator>();
//        rb = GetComponent<Rigidbody2D>();
//        unitTransform = transform;
//        lineRenderer = GetComponent<LineRenderer>();
//        lineRenderer.positionCount = 2; // �� ������ �̷���� ��
//        lineRenderer.enabled = false; // ó������ ��Ȱ��ȭ
//    }
//    void Update()
//    {
//        ClickCtrl clickCtrl = GameObject.Find("GameManager").GetComponent<ClickCtrl>();

//        // ��ó �� Ž��
//        closestEnemy = FindClosestEnemy();

//        if (closestEnemy != null && !clickCtrl.isHandle)
//        {
//            float distance = Vector3.Distance(transform.position, closestEnemy.position);
//            if (closestEnemy.CompareTag("EnemyUnit"))
//            {
//                // ���� ���� ���� �ְ�, ��Ÿ���� �����ٸ�
//                if (distance <= 4f)
//                {
//                    isMove = false;
//                    Debug.Log("attack");
//                    if (Time.time >= lastAttackTime + attackCooldown && !isMove && !isAttacking)
//                    {
//                        isAttacking = true; // ���� ����
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
//                // ���� ���� ���� �ְ�, ��Ÿ���� �����ٸ�
//                if (distance <= 4f)
//                {
//                    isMove = false;
//                    Debug.Log("attack");
//                    if (Time.time >= lastAttackTime + attackCooldown && !isMove && !isAttacking)
//                    {
//                        isAttacking = true; // ���� ����
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
//        animator.SetFloat("posY", direction.y);//�ϴ� �ִϸ��̼� �� �Ĵٺ���
//        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
//        ClickCtrl clickCtrl = GameObject.Find("GameManager").GetComponent<ClickCtrl>();
//        clickCtrl.isHandle = false;

//        // ���� ��ũ��Ʈ ��������
//        EnemyDamage enemyCtrl = enemy.GetComponent<EnemyDamage>();
//        if (enemyCtrl != null && enemyCtrl.health > 0)
//        {
//            float damage = 20f;
//            enemyCtrl.TakeDamage(damage);
//            ShowAttackLine(enemy.position);

//            GameObject attackEffect = Instantiate(attackMotion, enemy.position, Quaternion.identity);
//            Destroy(attackEffect, 0.1f); // ����Ʈ 0.1�� �� �ı�
//        }

//        // ������ ���� �ð� ������Ʈ
//        lastAttackTime = Time.time;
//        StartCoroutine(ResetAttackState());
//    }
//    void ShowAttackLine(Vector3 targetPosition)
//    {
//        lineRenderer.SetPosition(0, transform.position); // ���� ��ġ
//        lineRenderer.SetPosition(1, targetPosition); // �� ��ġ
//        lineRenderer.enabled = true; // ���� ������ Ȱ��ȭ

//        // ���� �������� ���� �� �β� ����
//        lineRenderer.startColor = Color.red; // ���� ����
//        lineRenderer.endColor = Color.red; // �� ����
//        lineRenderer.startWidth = 0.1f; // ���� �β�
//        lineRenderer.endWidth = 0.1f; // �� �β�

//        StartCoroutine(HideAttackLine());
//    }

//    // ���� �������� ����� ���� �ڷ�ƾ
//    private IEnumerator HideAttackLine()
//    {
//        yield return new WaitForSeconds(0.1f); // ��� ���
//        lineRenderer.enabled = false; // ���� ������ ��Ȱ��ȭ
//    }
//    private IEnumerator ResetAttackState()
//    {
//        yield return new WaitForSeconds(attackCooldown); // ��Ÿ�� ���
//        isAttacking = false; // ���� ���� �ʱ�ȭ
//    }
//    Transform FindClosestEnemy()
//    {

//        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyUnit");
//        Transform closestEnemy = null;
//        float closestDistance = 5f;

//        //�Ÿ�����ؼ� ��������� ����
//        foreach (GameObject enemy in enemies)
//        {
//            float distance = Vector3.Distance(transform.position, enemy.transform.position);
//            if (distance < closestDistance && distance <= 5)//���� 5�ȿ������� ��
//            {
//                closestDistance = distance;
//                closestEnemy = enemy.transform;
//                //Debug.Log(enemy.name);
//            }

//        }
//        if (closestEnemy == null)
//        {
//            GameObject[] enemies1 = GameObject.FindGameObjectsWithTag("EnemyBuilding");



//            //�Ÿ�����ؼ� ��������� ����
//            foreach (GameObject enemy in enemies1)
//            {
//                float distance = Vector3.Distance(transform.position, enemy.transform.position);
//                if (distance < 10 && distance <= 10)//���� 5�ȿ������� ��
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

//        //�迭 �ʱ�ȭ ������
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


//        // ���۰� �� ��� �ʱ�ȭ
//        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];

//        //���� ���(ĭ) = ���迭[��������.x - ���ϴ�.x, ��������.y - ���ϴ�.y]
//        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];
//        //Debug.Log(TargetNode.x);

//        //Ž���ĺ�����Ʈ�� ��ŸƮ ��� ����
//        OpenList = new List<Node>() { StartNode };
//        //Ž����������Ʈ�� �̸� ����
//        ClosedList = new List<Node>();
//        //��������Ʈ �̸� ����
//        FinalNodeList = new List<Node>();

//        foreach (Node node in NodeArray)
//        {
//            node.G = int.MaxValue; // ���Ѵ�
//            node.H = 0; // �ʱ�ȭ
//            node.ParentNode = null; // �θ� ��� �ʱ�ȭ
//        }

//        StartNode.G = 0;
//        StartNode.H = (Mathf.Abs(StartNode.x - TargetNode.x) + Mathf.Abs(StartNode.y - TargetNode.y)) * 10;

//        // A* �˰����� ���� ��� ã��
//        // OpenList�� ������� ���� ���� �ݺ�
//        while (OpenList.Count > 0)
//        {
//            // OpenList���� ù ��° ��带 CurNode�� ����
//            CurNode = OpenList[0];//������ġ ��带 �������

//            // OpenList���� ���� ���� F ���� ���� ��带 ã�� ���� �ݺ�
//            for (int i = 1; i < OpenList.Count; i++)
//            {
//                // ���� ����� F ������ �۰ų� ���� H ���� �� ���� ��� CurNode�� ������Ʈ
//                if (OpenList[i].F < CurNode.F || (OpenList[i].F == CurNode.F && OpenList[i].H < CurNode.H))
//                {
//                    CurNode = OpenList[i];
//                }
//            }

//            // ���� ���õ� ��带 OpenList���� ����
//            OpenList.Remove(CurNode);
//            // ���� ��带 ClosedList�� �߰� (Ž���� ���� ���)
//            ClosedList.Add(CurNode);

//            // ���� ���� ��尡 ��ǥ ���� ���ٸ�
//            if (CurNode == TargetNode)
//            {
//                Node TargetCurNode = TargetNode; // ��ǥ ��带 ����
//                                                 // ���� ������ �θ� ��带 �Ž��� �ö󰡸鼭 ���� ��� ����Ʈ�� ����
//                while (TargetCurNode != StartNode)
//                {
//                    FinalNodeList.Add(TargetCurNode); // ���� ��� ����Ʈ�� ���� ��� �߰�
//                    TargetCurNode = TargetCurNode.ParentNode; // �θ� ���� �̵�
//                }
//                FinalNodeList.Add(StartNode); // ���� ��� �߰�
//                FinalNodeList.Reverse(); // ����Ʈ�� �������� ������ (���ۿ��� ��ǥ������ ���)

//                // ���� ��� ����Ʈ�� �� ��� ��ǥ ���
//                //for (int i = 0; i < FinalNodeList.Count; i++)
//                //    print(i + "��°�� " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);

//                // ��� �ð�ȭ�� ���� �޼��� ȣ��

//                // �̵� ���� �ڷ�ƾ�� ������ �ߴ�
//                if (moveCoroutine != null)
//                {
//                    StopCoroutine(moveCoroutine);
//                }
//                // ���ο� ��θ� ���� �̵��ϴ� �ڷ�ƾ ����
//                moveCoroutine = StartCoroutine(MoveAlongPath());
//                return; // ��� ã�� ����
//            }


//            // �밢�� �̵��� ���Ǹ� �밢�� �������� �̿� ��� �߰�
//            if (allowDiagonal)
//            {
//                OpenListAdd(CurNode.x + 1, CurNode.y + 1); // ������ ��
//                OpenListAdd(CurNode.x - 1, CurNode.y + 1); // ���� ��
//                OpenListAdd(CurNode.x - 1, CurNode.y - 1); // ���� �Ʒ�
//                OpenListAdd(CurNode.x + 1, CurNode.y - 1); // ������ �Ʒ�
//            }

//            // �����¿� �̿� ��� �߰�
//            OpenListAdd(CurNode.x, CurNode.y + 1); // ����
//            OpenListAdd(CurNode.x + 1, CurNode.y); // ������
//            OpenListAdd(CurNode.x, CurNode.y - 1); // �Ʒ���
//            OpenListAdd(CurNode.x - 1, CurNode.y); // ����
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
