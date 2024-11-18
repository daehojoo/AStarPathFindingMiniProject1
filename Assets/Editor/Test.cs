
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
        //�ǽð� ������ �ؽ��� ����
       


            Repaint(); // â�� �ٽ� �׷��� ������Ʈ
        
    }
    private void OnGUI()
    {
        width = EditorGUILayout.IntField("����",width);
        height = EditorGUILayout.IntField("����", height);

        Rect rect = new Rect(position.width/2, position.height/2, width, height);

        Handles.DrawSolidRectangleWithOutline(rect, Color.red, Color.clear);//���ϴ� ���� �ǹ�����
    }




}
