using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 8f;
    public float viewAngle = 90f;
    public int rayCount = 200;
    public LayerMask obstacleMask;

    public Material fovMaterial;   // ⭐ Inspector에서 넣기

    private Mesh mesh;
    private Vector3 origin;

    void Awake()
    {
        mesh = new Mesh();
    }

    void LateUpdate()
    {
        origin = transform.position;
        GenerateMesh();

        // ⭐ 여기서 fovMaterial이 사용됨
        Graphics.DrawMesh(
            mesh,
            transform.localToWorldMatrix,
            fovMaterial,
            0
        );
    }

    void GenerateMesh()
    {
        float angle = -viewAngle / 2f;
        float angleIncrease = viewAngle / rayCount;

        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 dir = DirFromAngle(angle + transform.eulerAngles.z);
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, viewRadius, obstacleMask);

            Vector3 point;

            if (hit)
            {
                // 장애물에 막히면 장애물까지 Mesh 생성
                point = hit.point;
            }
            else
            {
                // 안 막히면 끝까지 그림
                point = origin + dir * viewRadius;
            }

            viewPoints.Add(point);
            angle += angleIncrease;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        // 중심점
        vertices[0] = transform.InverseTransformPoint(origin);

        for (int i = 0; i < viewPoints.Count; i++)
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

        int triIndex = 0;
        for (int i = 1; i < vertexCount - 1; i++)
        {
            triangles[triIndex] = 0;
            triangles[triIndex + 1] = i;
            triangles[triIndex + 2] = i + 1;
            triIndex += 3;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }


    Vector3 DirFromAngle(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}
