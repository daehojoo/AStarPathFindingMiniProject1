using NUnit.Framework.Constraints;
using System.Data;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngineInternal;

public class SpriteSplit : EditorWindow
{
    private int rows = 4; //��
    private int columns = 4; //��
    private int paddingRows = 0;
    private int paddingColumns = 0;
    private int rowsOffset = 0;
    private int columnsOffset = 0;
    private Texture2D selectedTexture; //���� �ؽ���
    private float sliderValue = 1;
    private int rowsCellSize = 0;
    private int columnsCellSize = 0;
    private Color targetColor = new Color(0f, 0f, 0f); // ������
    private Color wantColor = new Color(0f, 0f, 0f, 0f);

    private float tolerance = 0.3f; //��������





    [MenuItem("My Menu/Split Sprites %&s")] // Ctrl + Alt + S
    public static void ShowWindow()
    {
        GetWindow<SpriteSplit>("SpriteSplit");
    }

    private void OnEnable()
    {
        EditorApplication.update += UpdateSelectedTexture;
    }

    private void OnDisable()
    {
        EditorApplication.update -= UpdateSelectedTexture;
    }

    private void UpdateSelectedTexture()
    {
        //�ǽð� ������ �ؽ��� ����
        selectedTexture = Selection.activeObject as Texture2D;

        if (selectedTexture != null)
        {


            Repaint(); // â�� �ٽ� �׷��� ������Ʈ
        }
    }
    private void OnGUI()
    {
       
        GUIStyle guiStyleTitle = new GUIStyle//����Ÿ��
        {
            fontSize = 50,
            normal = { textColor = Color.magenta }
        };

        GUIStyle guiStyleDescription = new GUIStyle
        {
            fontSize = 10,
            normal = { textColor = Color.white }
        };

        GUILayout.Label("��������Ʈ �����", guiStyleTitle);
        GUILayout.Label("(sprite����Ǹ� ���̴� �̹����� ��¦ Ŀ�� ����)", guiStyleDescription);
        rows = EditorGUILayout.IntField("��(X)(0~100 50���� ����):", rows);
        columns = EditorGUILayout.IntField("��(Y)(0~100 50���� ����):", columns);
        paddingRows = EditorGUILayout.IntField("X�е���: (0 ~ 50)", paddingRows);
        paddingColumns = EditorGUILayout.IntField("Y�е���: (0 ~ 50)", paddingColumns);
        rowsOffset = EditorGUILayout.IntField("x������: (-500 ~ 500)", rowsOffset);
        columnsOffset = EditorGUILayout.IntField("y������: (-10 ~ 500)", columnsOffset);
        rows = Mathf.Clamp(rows, 0, 50);
        columns = Mathf.Clamp(columns, 0, 50);
        paddingRows = Mathf.Clamp(paddingRows, 0, 50);
        paddingColumns = Mathf.Clamp(paddingColumns, 0, 50);
        rowsOffset = Mathf.Clamp(rowsOffset, -500, 500);
        columnsOffset = Mathf.Clamp(columnsOffset, -10, 500);
        sliderValue = Mathf.Clamp(sliderValue, 0.1f, 3);

        GUILayout.Label($"{sliderValue}");
        Event e = Event.current;
        if (e.type == EventType.ScrollWheel)
        {
            float scrollAmount = e.delta.y / 100; // ��ũ�� �� ����
            sliderValue += scrollAmount; // ���� ���� �߰�
            sliderValue = Mathf.Clamp(sliderValue, 0.1f, 3); // Ŭ���� ����
            e.Use(); // �̺�Ʈ ��� ó��
            //e.Use();
        }


        GUILayout.Space(10);
        sliderValue = GUILayout.HorizontalSlider(sliderValue, 0.1f, 3);
        GUILayout.Space(20); ;


        if (GUILayout.Button("��������Ʈ �и�"))
        {
            if (selectedTexture != null)
            {
                // �ؽ�ó ������ �������� drxprt���׽�ó�� �ش��ϴ°� �����;���
                string path = AssetDatabase.GetAssetPath(selectedTexture);
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

                // �ؽ�ó Ÿ���� Sprite�� ����
                if (textureImporter != null)
                {
                    textureImporter.textureType = TextureImporterType.Sprite;

                    textureImporter.isReadable = true;
                    textureImporter.spriteImportMode = SpriteImportMode.Multiple;



                    AssetDatabase.ImportAsset(path); // �����Ϳ� ��������
                    SplitSprites(rows, columns);
                }
            }

        }
        targetColor = EditorGUILayout.ColorField("�ٲٰ������", targetColor);
        wantColor = EditorGUILayout.ColorField("��ǥ��", wantColor);
        GUILayout.Label("��ǥ���� �������� �ϰ������ A�� 0�ϱ�", guiStyleDescription);
        tolerance = EditorGUILayout.FloatField($"�� ���� ����ġ(0 ~ 1) : ", tolerance);
        GUILayout.Label("(���ٲٱ� ������ ����ġ 0 �ְ� ���������� 0.3������) ", guiStyleDescription);

        //Rect rect4 = new Rect(position.width / 2, 100, 50, 50);
        //Handles.DrawSolidRectangleWithOutline(rect4, targetColor, Color.white);

        tolerance = Mathf.Clamp(tolerance, 0, 1);
        if (GUILayout.Button("�ؽ��� ��ü �� ���� ����"))
        {
            if (selectedTexture != null)
            {
                string path = AssetDatabase.GetAssetPath(selectedTexture);
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

                // �ؽ�ó Ÿ���� Sprite�� ����
                if (textureImporter != null)
                {
                    textureImporter.textureType = TextureImporterType.Sprite;

                    textureImporter.isReadable = true;
                    textureImporter.spriteImportMode = SpriteImportMode.Multiple;
                    AssetDatabase.ImportAsset(path); // �����Ϳ� ��������
                    ColorChange();



                }
            }
        }

        if (selectedTexture != null)
        {//�»�ܺ���
            Rect rect = new Rect(50, 450, selectedTexture.width / sliderValue, selectedTexture.height / sliderValue);
            //(position.width - selectedTexture.width)/2 
            GUI.DrawTexture(rect, Texture2D.whiteTexture);//���
            GUI.DrawTexture(rect, selectedTexture);
            GUILayout.Label($"������ ���� : {selectedTexture.name}");

            GUILayout.Label($"���� ������ {selectedTexture.width}, {selectedTexture.height}");

            DrawGrid(rect, rows, columns);

            
        }
        
    }
    private void DrawGrid(Rect rect, int rows, int columns)
    {
        float cellWidth = (rect.width / columns);
        float cellHeight = (rect.height / rows);
        Handles.color = Color.red;

        // ���� �׸� �׸���
        for (int i = 0; i <= columns; i++)
        {
            float x = rect.x + (cellWidth * i);
            Rect lineRect = new Rect(rect.x + i * cellWidth - paddingColumns* 5/ 4 + rowsOffset*5/2, rect.y + columnsOffset  *5/2, paddingColumns * 5f / 2f + 1, rect.height);
            Handles.DrawSolidRectangleWithOutline(lineRect, Color.red, Color.clear);
        }

        // ���� �׸� �׸���
        for (int i = 0; i <= rows; i++)
        {
            float y = rect.y + (cellHeight * i);
            Rect lineRect = new Rect(rect.x + rowsOffset * 5 / 2, rect.y + i * cellHeight - paddingRows * 5 / 4 + columnsOffset * 5 / 2, rect.width, paddingRows * 5f / 2f + 1);
            Handles.DrawSolidRectangleWithOutline(lineRect, Color.red, Color.clear);
        }
    }
    private void ColorChange(/*int rows, int columns*/)
    {
        if (selectedTexture == null)
        {
            Debug.LogError("���õ� �ؽ�ó�� ����.");
            return;
        }
        //int spriteWidth = (selectedTexture.width / columns) - paddingColumns; // ��������Ʈ�� ����
        //int spriteHeight = (selectedTexture.height / rows) - paddingRows; // ��������Ʈ�� ����

        // ���� ���� �����
        string path = AssetDatabase.GetAssetPath(selectedTexture);
        string folderPath = path.Substring(0, path.LastIndexOf("/")) + $"/{selectedTexture.name}_Sprites";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder(path.Substring(0, path.LastIndexOf("/")), $"{selectedTexture.name}_Sprites");
        }

        Texture2D newTexture = new Texture2D(selectedTexture.width, selectedTexture.height);
        Color[] pixels = selectedTexture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixelColor = pixels[i];

            // ���� �� ���ǹ�
            if (Mathf.Abs(pixelColor.r - targetColor.r) < tolerance &&
                Mathf.Abs(pixelColor.g - targetColor.g) < tolerance &&
                Mathf.Abs(pixelColor.b - targetColor.b) < tolerance &&
                Mathf.Abs(pixelColor.a - targetColor.a) < tolerance)
            {
                pixels[i] = wantColor; // Ÿ�� ���� �ش��ϸ� ���ϴ� ������ ����
            }
        }

        newTexture.SetPixels(pixels);
        newTexture.Apply();

        // PNG�� ���ڵ� �� ���� ����
        byte[] pngData = newTexture.EncodeToPNG();
        string newSpritePath = $"{folderPath}/{selectedTexture.name}_Colored.png";

        System.IO.File.WriteAllBytes(newSpritePath, pngData);
        AssetDatabase.ImportAsset(newSpritePath);

        TextureImporter textureImporter = AssetImporter.GetAtPath(newSpritePath) as TextureImporter;
        if (textureImporter != null)
        {
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.isReadable = true;
            textureImporter.spriteImportMode = SpriteImportMode.Single;
            AssetDatabase.ImportAsset(newSpritePath);
        }

        AssetDatabase.Refresh();
        Debug.Log("���� ���� �Ϸ�!");
    }




    private void SplitSprites(int rows, int columns)
    {
        if (selectedTexture == null)
        {
            Debug.LogError("���õ� �ؽ�ó�� ����.");
            return;
        }



        // ���� ���� �����
        string path = AssetDatabase.GetAssetPath(selectedTexture);
        string folderPath = path.Substring(0, path.LastIndexOf("/")) + $"/{selectedTexture.name}_Sprites";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder(path.Substring(0, path.LastIndexOf("/")), $"{selectedTexture.name}_Sprites");
        }

        int spriteWidth = (selectedTexture.width / columns) - paddingColumns; // ��������Ʈ�� ����
        int spriteHeight = (selectedTexture.height / rows) - paddingRows; // ��������Ʈ�� ����
        Debug.Log($"spriteHeight : {spriteHeight}");

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Texture2D newTexture = new Texture2D(spriteWidth, spriteHeight);
                Color[] pixels = new Color[spriteWidth * spriteHeight];//�ȼ�����ŭ�� �÷� �迭 �����
                int startX = (x * selectedTexture.width / columns) + (paddingColumns / 2) + (rowsOffset);//�������� x=����(���� 0,1,2,3) X*�� ĭ�� ���� + �е�/2 + ������
                //Debug.Log($"StartX = {startX}");
                int startY = (y * selectedTexture.height / rows) + (paddingRows / 2) - (columnsOffset);  //��������
                Debug.Log($"StartY = {startY}");
                // ���� üũ �� �ȼ� ����
                for (int j = 0; j < spriteHeight; j++)
                {
                    for (int i = 0; i < spriteWidth; i++)
                    {
                        int pixelX = startX + i;
                        int pixelY = startY + j;

                        Color pixelColor;
                        if (pixelX < 0 || pixelY < 0 || pixelX >= selectedTexture.width || pixelY >= selectedTexture.height)
                        {
                            pixelColor = Color.clear;
                        }
                        else
                        {
                            pixelColor = selectedTexture.GetPixel(pixelX, pixelY);
                        }

                        pixels[j * spriteWidth + i] = pixelColor; // ���� ���� �״�� ����
                    }
                }

                newTexture.SetPixels(pixels);
                newTexture.Apply();

                // PNG�� ���ڵ� �� ���� ����
                byte[] pngData = newTexture.EncodeToPNG();
                string newSpritePath = $"{folderPath}/{selectedTexture.name}_{rows - y}_{x}.png";

                System.IO.File.WriteAllBytes(newSpritePath, pngData);
                AssetDatabase.ImportAsset(newSpritePath);
                TextureImporter textureImporter = AssetImporter.GetAtPath(newSpritePath) as TextureImporter;
                if (textureImporter != null)
                {
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.isReadable = true;
                    textureImporter.spriteImportMode = SpriteImportMode.Single;
                    AssetDatabase.ImportAsset(newSpritePath);
                }
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("��������Ʈ ������ �Ϸ�!");
    }






}