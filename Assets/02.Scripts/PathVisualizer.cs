using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer; // LineRenderer를 드래그 앤 드롭으로 연결

    public void DrawPath(List<Node> path)
    {
        if (path == null || path.Count == 0)
        {
            lineRenderer.enabled = false; // 경로가 없으면 비활성화
            return;
        }

        lineRenderer.positionCount = path.Count; // 노드 수만큼 포지션 설정
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 nodePosition = new Vector3(path[i].x, path[i].y, 0);
            lineRenderer.SetPosition(i, nodePosition); // 각 노드의 위치 설정
        }

        lineRenderer.enabled = true; // LineRenderer 활성화
    }
}
