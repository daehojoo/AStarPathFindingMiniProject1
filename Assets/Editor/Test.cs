
using UnityEngine;
using UnityEditor;

public class Test : EditorWindow
{
    int height = 0;
    int width = 0;


    [MenuItem("My Menu/Test Manager %&m")] // Ctrl + Alt + M
    public static void ShowWindow()
    {
        GetWindow<Test>("Test Manager");


        
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
       


            Repaint(); // 창을 다시 그려서 업데이트
        
    }
    private void OnGUI()
    {
        width = EditorGUILayout.IntField("넓이",width);
        height = EditorGUILayout.IntField("높이", height);

        Rect rect = new Rect(position.width/2, position.height/2, width, height);

        Handles.DrawSolidRectangleWithOutline(rect, Color.red, Color.clear);//좌하단 에서 피벗기준
    }




}
