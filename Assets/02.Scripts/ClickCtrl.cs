    //using System;
    //using System.Collections;
    //using System.Collections.Generic;
    //using Unity.VisualScripting;
    //using UnityEngine;
    //using UnityEngine.EventSystems;
    //using UnityEngine.UI;

    //public class ClickCtrl : MonoBehaviour
    //{
    //    public static ClickCtrl instance;
    //    public GameObject selectedUnit;
    //    public Texture2D normalCursorSprite;
    //    Vector3 m_vecMouseDownPos;
    //    public UnitMoveToTarget UnitMoveToTarget;
    //    private bool isDragging = false; // 드래그 여부
    //    private Vector2 dragStartPos; // 드래그 시작 위치
    //    private Rect selectionRect; // 선택 영역

    //    public bool isAttackClick = false;
    //    public bool isMoveClick = false;
    //    public bool isStop = false;
    //    public bool isHandle = false;

    //    void Awake()
    //    {
    //        instance = this;
    //        Cursor.SetCursor(normalCursorSprite, Vector2.zero, CursorMode.Auto);
    //    }

    //    void Update()
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            if (!EventSystem.current.IsPointerOverGameObject())
    //            {
    //                selectedUnit = null; // 선택된 유닛 초기화
    //                m_vecMouseDownPos = Input.mousePosition;
    //                dragStartPos = m_vecMouseDownPos;
    //                isDragging = true;
    //            }
    //            else
    //            {
    //                return;
    //            }
    //        }

    //        if (isDragging)
    //        {
    //            if (Input.GetMouseButton(0))
    //            {
    //                Vector3 currentMousePos = Input.mousePosition;
    //                selectionRect = new Rect(dragStartPos.x, dragStartPos.y, currentMousePos.x - dragStartPos.x, currentMousePos.y - dragStartPos.y);
    //            }

    //            // 드래그가 끝나면
    //            if (Input.GetMouseButtonUp(0))
    //            {
    //                UnitSelect();
    //            }
    //        }

    //        if (Input.GetMouseButtonDown(1))
    //        {
    //            m_vecMouseDownPos = Input.mousePosition;
    //            Vector2 rightClickPos = Camera.main.ScreenToWorldPoint(m_vecMouseDownPos);
    //            Vector2Int rightClick = new Vector2Int(Mathf.RoundToInt(rightClickPos.x), Mathf.RoundToInt(rightClickPos.y));
    //            UnitAi unitAi = selectedUnit.GetComponent<UnitAi>();
    //            unitAi.Move(rightClick);
    //        }
    //    }

    //    private void UnitSelect()
    //    {
    //        isDragging = false;
    //        Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(selectionRect.xMin, selectionRect.yMax));
    //        Vector2 topRight = Camera.main.ScreenToWorldPoint(new Vector3(selectionRect.xMax, selectionRect.yMin));
    //        Collider2D[] colliders = Physics2D.OverlapAreaAll(bottomLeft, topRight);

    //        // 한 마리만 선택하도록 변경
    //        foreach (Collider2D collider in colliders)
    //        {
    //            if (collider.gameObject.CompareTag("PlayerUnit"))
    //            {
    //                selectedUnit = collider.gameObject; // 선택된 유닛으로 대체
    //                break; // 첫 번째 유닛을 선택한 후 반복 종료
    //            }
    //        }
    //    }

       
    //}
