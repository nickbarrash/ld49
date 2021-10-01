using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointTracker : MonoBehaviour
{
    public GameObject pointPrefab;
    [HideInInspector]
    public List<GameObject> points;
    public MeshManipulation mesh;

    private GameObject tmpPoint;

    public void Update() {
        if (Input.GetMouseButtonDown(0)) {
            AddPoint(InputUtility.instance.MouseToWorldZeroed());

        }
    }

    public void AddPoint(Vector3 coords) {
        //Debug.Log($"AddPoint: {coords}");
        var point = CreatePoint();
        point.transform.position = coords;
        mesh.UpdateMesh(ConvexHull.ClockwisePointSort(ConvexHull.ComputeConvexHull(points.Select(g => g.transform.position).ToList())));
    }

    public GameObject CreatePoint() {
        tmpPoint = Instantiate(pointPrefab, transform);
        tmpPoint.name = $"Point_{points.Count}";
        points.Add(tmpPoint);
        return tmpPoint;
    }
}
