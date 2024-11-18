using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y)
    {
        isWall = _isWall; x = _x; y = _y;
        
    }
    public bool isWall;
    public Node ParentNode;
    public int x, y, G, H;//G=시작노드부터지금까지 H=지금노드부터타겟노드까지

    public int F
    {
        get { return G + H; }
    }
}

public class UnitMoveToTarget : MonoBehaviour
{
    public Transform tr;
    public Animator animator;

    private Vector2Int bottomLeft, startPos, targetPos;
    private Vector2Int topRight = new Vector2Int(10,10);//10x10의 크기
    private List<Node> FinalNodeList; //최종 노드 리스트
    private List<Node> OpenList, ClosedList; //탐색 후보 리스트, 이미 탐색 검사 끝난 리스트
    private Node[,] NodeArray; 
    private Node StartNode; //시작 노드
    private Node TargetNode; //목표 노드
    private Node CurNode; //현재 노드
    public bool isMoving = false;
    private bool allowDiagonal, dontCrossCorner;
    private int sizeX, sizeY; 
    private int currentNodeIndex = 0;
    [Header("UI")]
    public Text startNode;
    public Text curNode;
    public Text targetPointNode;
    public Text cost;
    public void Start()
    {
        animator = GetComponent<Animator>();
        tr = transform;
        Image panelImage = GameObject.FindWithTag("MainPanel").GetComponent<Image>();
        startNode = panelImage.transform.GetChild(2).GetComponent<Text>();
        curNode = panelImage.transform.GetChild(4).GetComponent<Text>();
        targetPointNode = panelImage.transform.GetChild(6).GetComponent<Text>();
        cost = panelImage.transform.GetChild(8).GetComponent<Text>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 rightClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int rightClick = new Vector2Int(Mathf.RoundToInt(rightClickPos.x), Mathf.RoundToInt(rightClickPos.y));
            startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            targetPos = new Vector2Int(Mathf.RoundToInt(rightClick.x), Mathf.RoundToInt(rightClick.y)); ;
            PathFinding();

        }
        if (FinalNodeList != null)
        {
            if (currentNodeIndex < FinalNodeList.Count)
                MoveAlongPath();

        }
    }
    public void PathFinding()
    {
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];
        List<Vector3> wallPositions = new List<Vector3>();
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                Vector3 wallPosition = new Vector3(i + bottomLeft.x, j + bottomLeft.y, 0);
                wallPositions.Add(wallPosition);
                bool isWall = false;
                
                Vector2Int currentPos = new Vector2Int(i + bottomLeft.x, j + bottomLeft.y);
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.3f))
                {
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall") )
                    {
                        isWall = true;
                        
                    }
                }
                NodeArray[i, j] = new Node(isWall,i + bottomLeft.x, j + bottomLeft.y);
                
            }
        }
        startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        startNode.text = $"{startPos.x - bottomLeft.x},{startPos.y - bottomLeft.y}";

        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];
        targetPointNode.text = $"{TargetNode.x},{TargetNode.y}";

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        foreach (Node node in NodeArray)
        {
            node.G = int.MaxValue;
            node.H = 0;
            node.ParentNode = null;
        }
        StartNode.G = 0;
        StartNode.H = (Mathf.Abs(StartNode.x - TargetNode.x) + Mathf.Abs(StartNode.y - TargetNode.y)) * 10;

        while (OpenList.Count > 0)
        {
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F < CurNode.F || (OpenList[i].F == CurNode.F && OpenList[i].H < CurNode.H))
                {
                    CurNode = OpenList[i];
                }
            }
            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                currentNodeIndex = 0;
                isMoving = true;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();
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
    private void MoveAlongPath()//이동로직
    {
        if (isMoving && currentNodeIndex < FinalNodeList.Count)
        {
            Node currentNode = FinalNodeList[currentNodeIndex];
            curNode.text = $"{currentNode.x}, {currentNode.y}";

            Vector3 targetPosition = new Vector3(currentNode.x, currentNode.y, 0);
            Vector3 roundedTargetPosition = new Vector3(Mathf.Round(targetPosition.x), Mathf.Round(targetPosition.y));
            transform.position = Vector3.MoveTowards(transform.position, roundedTargetPosition, Time.deltaTime * 4f);
            animator.SetFloat("posX", targetPosition.x - transform.position.x);
            animator.SetFloat("posY", targetPosition.y - transform.position.y);
            if (Vector3.Distance(transform.position, roundedTargetPosition) == 0f)
            {
                currentNodeIndex++;
            }
        }
        else if (currentNodeIndex >= FinalNodeList.Count)
        {
            isMoving = false;
        }
    }
    void OpenListAdd(int checkX, int checkY)
    {
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            if (allowDiagonal)//대각이동
                if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall)
                    return;
            if (dontCrossCorner)//코너이동
                if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall)
                    return;
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);//1:루트2:1 개념
            cost.text = $"({startPos.x - bottomLeft.x},{startPos.y - bottomLeft.y}) => ({TargetNode.x},{TargetNode.y})= {MoveCost.ToString()}";
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }
    //void OnDrawGizmos()//시각적효과
    //{
    //    if (FinalNodeList != null && FinalNodeList.Count != 0)
    //    {
    //        for (int i = 0; i < FinalNodeList.Count - 1; i++)
    //        {
    //            Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
    //        }
    //    }
    //}
}
