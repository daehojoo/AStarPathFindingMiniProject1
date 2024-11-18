using UnityEngine;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{
    public int width = 11;
    public int height = 11;
    public GameObject wallPrefab;
    public GameObject pathPrefab;
    public GameObject player;
    public Button regenerateButton;
    public Button quitButton;

    private int[,] maze;
    private UnitMoveToTarget unitMoveToTarget;
    void Start()
    {
        
        GenerateMaze();
        CreateMazeVisuals();
        player = Instantiate(player, new Vector3(width - 10, height - 1, 0), Quaternion.identity);
        unitMoveToTarget = player.GetComponent<UnitMoveToTarget>();

        regenerateButton.onClick.AddListener(GenerateNewMaze);
        quitButton.onClick.AddListener(ExitGame);
    }

    private void GenerateMaze()
    {
        maze = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1; //1 = ��
            }
        }
        //�ݵ�� Ȧ�� ��ġ���� ����
        int startX = Random.Range(0, (width / 2)) * 2 + 1; // Ȧ�� ��ġ
        int startY = Random.Range(0, (height / 2)) * 2 + 1; // Ȧ�� ��ġ
        maze[startX, startY] = 0;//0 = ��
        DFS(startX, startY);
        //�Ա� ����
        maze[width - 10, height - 1] = 0;
        //�ⱸ ������
        maze[width - 1, height - 10] = 0;
    }

    private void DFS(int x, int y)
    {
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, -2),
            new Vector2Int(0, 2),
            new Vector2Int(-2, 0),
            new Vector2Int(2, 0)
        };

        Shuffle(directions);

        foreach (var direction in directions)
        {
            int newX = x + direction.x;
            int newY = y + direction.y;

            if (IsInBounds(newX, newY) && maze[newX, newY] == 1)
            {
                maze[newX, newY] = 0;//�� ����
                maze[x + direction.x / 2, y + direction.y / 2] = 0;//�� ����
                DFS(newX, newY);//��� ȣ��
            }
        }
    }

    private bool IsInBounds(int x, int y)
    {
        return x > 0 && x < width && y > 0 && y < height;

    }

    private void Shuffle(Vector2Int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(i, array.Length);
            var temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private void CreateMazeVisuals()
    {
        foreach (Transform child in transform)//�ʱ�ȭ
        {
            Destroy(child.gameObject);
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1) //1= ��
                {
                    Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                }
                else //0= ��
                {
                    Instantiate(pathPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                }
            }
        }
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GenerateNewMaze()
    {
        player.transform.position = new Vector3(width - 10, height - 1, 0);
        unitMoveToTarget.isMoving = false;
        GenerateMaze();
        CreateMazeVisuals();
    }
}
