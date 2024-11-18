using NUnit.Framework.Constraints;
using System.Data;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngineInternal;

public class SpriteSplit : EditorWindow
{
    private int rows = 4; //행
    private int columns = 4; //열
    private int paddingRows = 0;
    private int paddingColumns = 0;
    private int rowsOffset = 0;
    private int columnsOffset = 0;
    private Texture2D selectedTexture; //메인 텍스쳐
    private float sliderValue = 1;
    private int rowsCellSize = 0;
    private int columnsCellSize = 0;
    private Color targetColor = new Color(0f, 0f, 0f); // 검은색
    private Color wantColor = new Color(0f, 0f, 0f, 0f);

    private float tolerance = 0.3f; //오차범위





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
        //실시간 선택한 텍스쳐 지정
        selectedTexture = Selection.activeObject as Texture2D;

        if (selectedTexture != null)
        {


            Repaint(); // 창을 다시 그려서 업데이트
        }
    }
    private void OnGUI()
    {
       
        GUIStyle guiStyleTitle = new GUIStyle//제목스타일
        {
            fontSize = 50,
            normal = { textColor = Color.magenta }
        };

        GUIStyle guiStyleDescription = new GUIStyle
        {
            fontSize = 10,
            normal = { textColor = Color.white }
        };

        GUILayout.Label("스프라이트 도우미", guiStyleTitle);
        GUILayout.Label("(sprite적용되면 보이는 이미지만 살짝 커짐 ㅇㅇ)", guiStyleDescription);
        rows = EditorGUILayout.IntField("행(X)(0~100 50이하 권장):", rows);
        columns = EditorGUILayout.IntField("열(Y)(0~100 50이하 권장):", columns);
        paddingRows = EditorGUILayout.IntField("X패딩값: (0 ~ 50)", paddingRows);
        paddingColumns = EditorGUILayout.IntField("Y패딩값: (0 ~ 50)", paddingColumns);
        rowsOffset = EditorGUILayout.IntField("x오프셋: (-500 ~ 500)", rowsOffset);
        columnsOffset = EditorGUILayout.IntField("y오프셋: (-10 ~ 500)", columnsOffset);
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
            float scrollAmount = e.delta.y / 100; // 스크롤 양 조정
            sliderValue += scrollAmount; // 기존 값에 추가
            sliderValue = Mathf.Clamp(sliderValue, 0.1f, 3); // 클램프 적용
            e.Use(); // 이벤트 사용 처리
            //e.Use();
        }


        GUILayout.Space(10);
        sliderValue = GUILayout.HorizontalSlider(sliderValue, 0.1f, 3);
        GUILayout.Space(20); ;


        if (GUILayout.Button("스프라이트 분리"))
        {
            if (selectedTexture != null)
            {
                // 텍스처 임포터 가져오기 drxprt그테스처에 해당하는거 가져와야함
                string path = AssetDatabase.GetAssetPath(selectedTexture);
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

                // 텍스처 타입을 Sprite로 설정
                if (textureImporter != null)
                {
                    textureImporter.textureType = TextureImporterType.Sprite;

                    textureImporter.isReadable = true;
                    textureImporter.spriteImportMode = SpriteImportMode.Multiple;



                    AssetDatabase.ImportAsset(path); // 데이터에 사항적용
                    SplitSprites(rows, columns);
                }
            }

        }
        targetColor = EditorGUILayout.ColorField("바꾸고싶은색", targetColor);
        wantColor = EditorGUILayout.ColorField("목표색", wantColor);
        GUILayout.Label("목표색을 투명으로 하고싶으면 A값 0하기", guiStyleDescription);
        tolerance = EditorGUILayout.FloatField($"색 감지 보정치(0 ~ 1) : ", tolerance);
        GUILayout.Label("(색바꾸기 싫으면 보정치 0 넣고 같은값지정 0.3이적당) ", guiStyleDescription);

        //Rect rect4 = new Rect(position.width / 2, 100, 50, 50);
        //Handles.DrawSolidRectangleWithOutline(rect4, targetColor, Color.white);

        tolerance = Mathf.Clamp(tolerance, 0, 1);
        if (GUILayout.Button("텍스쳐 전체 색 수정 적용"))
        {
            if (selectedTexture != null)
            {
                string path = AssetDatabase.GetAssetPath(selectedTexture);
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

                // 텍스처 타입을 Sprite로 설정
                if (textureImporter != null)
                {
                    textureImporter.textureType = TextureImporterType.Sprite;

                    textureImporter.isReadable = true;
                    textureImporter.spriteImportMode = SpriteImportMode.Multiple;
                    AssetDatabase.ImportAsset(path); // 데이터에 사항적용
                    ColorChange();



                }
            }
        }

        if (selectedTexture != null)
        {//좌상단부터
            Rect rect = new Rect(50, 450, selectedTexture.width / sliderValue, selectedTexture.height / sliderValue);
            //(position.width - selectedTexture.width)/2 
            GUI.DrawTexture(rect, Texture2D.whiteTexture);//배경
            GUI.DrawTexture(rect, selectedTexture);
            GUILayout.Label($"선택한 에셋 : {selectedTexture.name}");

            GUILayout.Label($"에셋 사이즈 {selectedTexture.width}, {selectedTexture.height}");

            DrawGrid(rect, rows, columns);

            
        }
        
    }
    private void DrawGrid(Rect rect, int rows, int columns)
    {
        float cellWidth = (rect.width / columns);
        float cellHeight = (rect.height / rows);
        Handles.color = Color.red;

        // 세로 네모 그리기
        for (int i = 0; i <= columns; i++)
        {
            float x = rect.x + (cellWidth * i);
            Rect lineRect = new Rect(rect.x + i * cellWidth - paddingColumns* 5/ 4 + rowsOffset*5/2, rect.y + columnsOffset  *5/2, paddingColumns * 5f / 2f + 1, rect.height);
            Handles.DrawSolidRectangleWithOutline(lineRect, Color.red, Color.clear);
        }

        // 가로 네모 그리기
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
            Debug.LogError("선택된 텍스처가 없음.");
            return;
        }
        //int spriteWidth = (selectedTexture.width / columns) - paddingColumns; // 스프라이트의 넓이
        //int spriteHeight = (selectedTexture.height / rows) - paddingRows; // 스프라이트의 높이

        // 저장 폴더 만들기
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

            // 색상 비교 조건문
            if (Mathf.Abs(pixelColor.r - targetColor.r) < tolerance &&
                Mathf.Abs(pixelColor.g - targetColor.g) < tolerance &&
                Mathf.Abs(pixelColor.b - targetColor.b) < tolerance &&
                Mathf.Abs(pixelColor.a - targetColor.a) < tolerance)
            {
                pixels[i] = wantColor; // 타겟 색상에 해당하면 원하는 색으로 변경
            }
        }

        newTexture.SetPixels(pixels);
        newTexture.Apply();

        // PNG로 인코딩 및 파일 저장
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
        Debug.Log("색상 변경 완료!");
    }




    private void SplitSprites(int rows, int columns)
    {
        if (selectedTexture == null)
        {
            Debug.LogError("선택된 텍스처가 없음.");
            return;
        }



        // 저장 폴더 만들기
        string path = AssetDatabase.GetAssetPath(selectedTexture);
        string folderPath = path.Substring(0, path.LastIndexOf("/")) + $"/{selectedTexture.name}_Sprites";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder(path.Substring(0, path.LastIndexOf("/")), $"{selectedTexture.name}_Sprites");
        }

        int spriteWidth = (selectedTexture.width / columns) - paddingColumns; // 스프라이트의 넓이
        int spriteHeight = (selectedTexture.height / rows) - paddingRows; // 스프라이트의 높이
        Debug.Log($"spriteHeight : {spriteHeight}");

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Texture2D newTexture = new Texture2D(spriteWidth, spriteHeight);
                Color[] pixels = new Color[spriteWidth * spriteHeight];//픽셀수만큼의 컬러 배열 만들기
                int startX = (x * selectedTexture.width / columns) + (paddingColumns / 2) + (rowsOffset);//시작지점 x=순서(가로 0,1,2,3) X*한 칸의 넓이 + 패딩/2 + 오프셋
                //Debug.Log($"StartX = {startX}");
                int startY = (y * selectedTexture.height / rows) + (paddingRows / 2) - (columnsOffset);  //시작지점
                Debug.Log($"StartY = {startY}");
                // 범위 체크 및 픽셀 복사
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

                        pixels[j * spriteWidth + i] = pixelColor; // 원본 색상 그대로 유지
                    }
                }

                newTexture.SetPixels(pixels);
                newTexture.Apply();

                // PNG로 인코딩 및 파일 저장
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
        Debug.Log("스프라이트 나누기 완료!");
    }






}