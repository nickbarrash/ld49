using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshManipulation : MonoBehaviour
{
    Mesh mesh;

    Vector3[] verticies;
    int[] triangles;

    // Start is called before the first frame update
    void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //CreateShape();
        //UpdateMesh();
    }

    public void UpdateMesh(IList<Vector3> points) {
        //Debug.Log("--------");
        //foreach(var p in points)
        //{
        //    Debug.Log(p);
        //}

        if (points.Count >= 3)
        {
            mesh.Clear();
            mesh.vertices = points.Select(p => p).ToArray();
        
            UpdateTriangles();

            mesh.RecalculateNormals();
        }
    }

    void UpdateTriangles() {
        var triangles = new int[(mesh.vertexCount - 2) * 3];
        for (int i = 0; i < mesh.vertexCount - 2; i++)
        {
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
        mesh.triangles = triangles;
    }

    //void CreateShape(List<Vector3> points)
    //{
    //    verticies = new Vector3[]{
    //        new Vector3(0,0,0),
    //        new Vector3(0,1,0),
    //        new Vector3(1,0,0)
    //    };

    //    triangles = new int[]{
    //        0,1,2
    //    };
    //}
}
