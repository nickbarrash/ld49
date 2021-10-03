using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerMesh : MonoBehaviour
{
    Mesh mesh;

    public Vector3[] triangles;
    public Vector2[] points;

    void Awake() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.Clear();
        mesh.vertices = points.Select(p => (Vector3)p).ToArray();

        var flatTriangles = new int[triangles.Length * 3];
        for(int i = 0; i < triangles.Length; i++)
        {
            flatTriangles[i * 3 + 0] = (int)triangles[i].x;
            flatTriangles[i * 3 + 1] = (int)triangles[i].y;
            flatTriangles[i * 3 + 2] = (int)triangles[i].z;
        }
        mesh.triangles = flatTriangles;
    }
}
