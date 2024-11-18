using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyMoveAndAttack : MonoBehaviour
{
    public Transform unitTransform; // �̵���Ű�� ���� ���õ� ����
    public Vector2Int bottomLeft,  startPos, targetPos;
    public Vector2Int topRight = new Vector2Int(60,60);
    public List<Node> FinalNodeList; // ���������� ������ ��� ����Ʈ
    public bool allowDiagonal, dontCrossCorner; // �밢�� �̵�, �ڳ� �̵� �Ұ�

    private int sizeX, sizeY; // x�� ������ ��� ����, y�� ������ ��� ����
    [SerializeField]
    public Node[,] NodeArray; // ����� �迭
    [SerializeField]
    public Node StartNode; // ���� ���
    [SerializeField]
    private Node TargetNode; // ��ǥ ���
    [SerializeField]
    private Node CurNode; // ���� Ž�� ���� ���
    [SerializeField]
    private List<Node> OpenList, ClosedList; // Ž�� �ĺ� ����Ʈ, �̹� Ž�� �˻� ���� ����Ʈ
    public Coroutine moveCoroutine; // �̵� �ڷ�ƾ

    //public UnitData unitData;
    public Animator animator;
    public GameObject attackMotion;
    public float attackCooldown = 2f; // ���� ��Ÿ��
    public Rigidbody2D rb;
    private float lastAttackTime; // ������ ���� �ð�
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
        

        // ��ó �� Ž��
        closestEnemy = FindClosestEnemy();

        if (closestEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, closestEnemy.position);

            // ���� ���� ���� �ְ�, ��Ÿ���� �����ٸ�
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
        animator.SetFloat("posY", direction.y);//�ϴ� �ִϸ��̼� �� �Ĵٺ���
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;


        // ���� ��ũ��Ʈ ��������
        PlayerUnitDamage enemyCtrl = enemy.GetComponent<PlayerUnitDamage>();
        if (enemyCtrl != null && enemyCtrl.health > 0)
        {
            float damage = 20f;
            enemyCtrl.TakeDamage(damage);
            GameObject attackEffect = Instantiate(attackMotion, enemy.position, Quaternion.identity);
            Destroy(attackEffect, 0.1f); // ����Ʈ 0.1�� �� �ı�
        }

        // ������ ���� �ð� ������Ʈ
        lastAttackTime = Time.time;
    }
    Transform FindClosestEnemy()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("PlayerUnit");
        Transform closestEnemy = null;
        float closestDistance = 30f;

        //�Ÿ�����ؼ� ��������� ����
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

        //�迭 �ʱ�ȭ ������
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
            node.H = 0; // �ʱ�ȭ
            node.ParentNode = null;
        }

        StartNode.G = 0;
        StartNode.H = (Mathf.Abs(StartNode.x - TargetNode.x) + Mathf.Abs(StartNode.y - TargetNode.y)) * 10;
        while (OpenList.Count > 0)
        {
            CurNode = OpenList[0];//������ġ ��带 �������
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F < CurNode.F || (OpenList[i].F == CurNode.F && OpenList[i].H < CurNode.H))
                {
                    CurNode = OpenList[i];
                }
            }
            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);
            //���� ���� ��尡 ��ǥ ���� ���ٸ�
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;              
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode); 
                    TargetCurNode = TargetCurNode.ParentNode; //�θ� ���� �̵�
                }
                FinalNodeList.Add(StartNode);//���� ��� �߰�
                FinalNodeList.Reverse(); //����Ʈ�� �������� ������ (���ۿ��� ��ǥ������ ���)
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
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);//��Ʈ2 )1:��Ʈ2:1)
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }

    private IEnumerator MoveAlongPath()//�̵�����
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
