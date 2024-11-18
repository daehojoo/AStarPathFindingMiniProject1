using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Camera mainCam;
    [SerializeField]
    private float moveSpeed = 5f; // ī�޶� �̼�
    public float borderThickness = 10f; //������
    private Vector2 screenLimits = new Vector2(40, 30);

    void Start()
    {
        mainCam = GetComponent<Camera>();
    }

    void Update()
    {
        if (mainCam != null)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 camPosition = transform.position;
            if (mousePos.x <= borderThickness)
            {
                camPosition.x -= moveSpeed * Time.deltaTime; 
            }
            if (mousePos.x >= Screen.width - borderThickness)
            {
                camPosition.x += moveSpeed * Time.deltaTime;
            }
            if (mousePos.y <= borderThickness)
            {
                camPosition.y -= moveSpeed * Time.deltaTime;
            }
            if (mousePos.y >= Screen.height - borderThickness)
            {
                camPosition.y += moveSpeed * Time.deltaTime;
            }

            #region ����Ű �̵�����
            if (Input.GetKey(KeyCode.UpArrow))
            {
                camPosition.y += moveSpeed * Time.deltaTime; 
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                camPosition.y -= moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                camPosition.x -= moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                camPosition.x += moveSpeed * Time.deltaTime;
            }
            #endregion
            if (mainCam != null)
            {
                // ���콺 ���� ������ �� ī�޶� �� ��/�� �ƿ�
                float scrollData = Input.GetAxis("Mouse ScrollWheel");
                if (scrollData != 0)
                {
                    // �� �ӵ� ����
                    float zoomSpeed = 5f;
                    mainCam.orthographicSize -= scrollData * zoomSpeed; // Orthographic ī�޶��� ���
                                                                        // Ȥ��
                                                                        // mainCam.fieldOfView -= scrollData * zoomSpeed; // Perspective ī�޶��� ���

                    // �ּ� �� �ִ� �� ����
                    mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize, 2f, 20f); // Orthographic ī�޶��� ���
                                                                                               // mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView, 30f, 90f); // Perspective ī�޶��� ���
                }
            }






            //ī�޶� �� ��������
            camPosition.x = Mathf.Clamp(camPosition.x, -screenLimits.x, screenLimits.x);
            camPosition.y = Mathf.Clamp(camPosition.y, -screenLimits.y, screenLimits.y);
            transform.position = camPosition;
        }
    }
}
