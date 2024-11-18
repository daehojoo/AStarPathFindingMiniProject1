using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Camera mainCam;
    [SerializeField]
    private float moveSpeed = 5f; // 카메라 이속
    public float borderThickness = 10f; //범위임
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

            #region 방향키 이동설정
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
                // 마우스 휠을 움직일 때 카메라 줌 인/줌 아웃
                float scrollData = Input.GetAxis("Mouse ScrollWheel");
                if (scrollData != 0)
                {
                    // 줌 속도 조정
                    float zoomSpeed = 5f;
                    mainCam.orthographicSize -= scrollData * zoomSpeed; // Orthographic 카메라의 경우
                                                                        // 혹은
                                                                        // mainCam.fieldOfView -= scrollData * zoomSpeed; // Perspective 카메라의 경우

                    // 최소 및 최대 줌 제한
                    mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize, 2f, 20f); // Orthographic 카메라의 경우
                                                                                               // mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView, 30f, 90f); // Perspective 카메라의 경우
                }
            }






            //카메라 맵 리밋제한
            camPosition.x = Mathf.Clamp(camPosition.x, -screenLimits.x, screenLimits.x);
            camPosition.y = Mathf.Clamp(camPosition.y, -screenLimits.y, screenLimits.y);
            transform.position = camPosition;
        }
    }
}
