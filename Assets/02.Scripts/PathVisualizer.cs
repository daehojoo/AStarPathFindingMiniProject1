using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer; // LineRenderer�� �巡�� �� ������� ����

    public void DrawPath(List<Node> path)
    {
        if (path == null || path.Count == 0)
        {
            lineRenderer.enabled = false; // ��ΰ� ������ ��Ȱ��ȭ
            return;
        }

        lineRenderer.positionCount = path.Count; // ��� ����ŭ ������ ����
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 nodePosition = new Vector3(path[i].x, path[i].y, 0);
            lineRenderer.SetPosition(i, nodePosition); // �� ����� ��ġ ����
        }

        lineRenderer.enabled = true; // LineRenderer Ȱ��ȭ
    }
}
