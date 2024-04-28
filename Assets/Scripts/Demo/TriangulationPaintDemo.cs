
using System;
using System.Collections.Generic;
using System.Linq;
using Triangulation2D;
using UnityEngine;

/// <summary>
/// 根据在屏幕手动拖拽产生的点三角剖分
/// </summary>
public class TriangulationPaintDemo : MonoBehaviour
{

    List<Vector2> points = new List<Vector2>();
    [SerializeField]
    float threshold = 0.1f;
    bool dragging = false;
    private TriangulationBowyerWatson _algorithm;
    private MeshFilter _meshFilter;
    private Mesh _showMesh;
    [SerializeField]
    private Material _paintLineMat;

    private void Awake()
    {
        _meshFilter = gameObject.GetComponent<MeshFilter>();
        _algorithm = new TriangulationBowyerWatson();
        _showMesh = new Mesh();


    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            Clear();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            Build();
        }

        if (dragging)
        {
            Vector3 screen = Input.mousePosition;
            //Debug.Log($"当前拖拽点屏幕坐标位置{screen}");
            //透视投影时, 屏幕坐标的z值并不直接代表物体与相机之间的距离, 而是代表物体在相机视锥体中的深度
            screen.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
            Vector3 p = Camera.main.ScreenToWorldPoint(screen);
            //Debug.Log($"当前拖拽点世界坐标位置{p}");
            Vector2 p2D = new Vector2(p.x, p.y);
            if (points.Count <= 0 || Vector2.Distance(p2D, points.Last()) > threshold)
            {
                points.Add(p2D);
            }
        }
    }

    void OnRenderObject()
    {
        if (points != null)
        {
            //Material.SetPass mostly used in direct drawing code.
            _paintLineMat.SetPass(0);
            //GL immediate drawing functions use whatever is the "current material" set up right now
            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);
            GL.Begin(GL.LINES);
            for (int i = 0; i < points.Count - 1; i++)
            {
                GL.Vertex(points[i]);
                GL.Vertex(points[i + 1]);
            }
            GL.End();
            GL.PopMatrix();
        }

        if (_algorithm.Triangle2Ds.Count > 0)
        {
            _showMesh.Clear();
            Vector3[] vertexs = new Vector3[_algorithm.Triangle2Ds.Count * 3];
            var triangles = new List<int>();
            for (int i = 0, vertexIndex = 0; i < _algorithm.Triangle2Ds.Count; ++i, vertexIndex += 3)
            {
                int a = vertexIndex, b = vertexIndex + 1, c = vertexIndex + 2;
                vertexs[a] = 
                    new Vector3(_algorithm.Triangle2Ds[i].VertexArr[0].Point.x, _algorithm.Triangle2Ds[i].VertexArr[0].Point.y, 0);
                vertexs[b] =
                    new Vector3(_algorithm.Triangle2Ds[i].VertexArr[1].Point.x, _algorithm.Triangle2Ds[i].VertexArr[1].Point.y, 0);
                vertexs[c] =
                    new Vector3(_algorithm.Triangle2Ds[i].VertexArr[2].Point.x, _algorithm.Triangle2Ds[i].VertexArr[2].Point.y, 0);
                //顺时针添加顶点
                if (CalculationTool.CrossProduct(_algorithm.Triangle2Ds[i].VertexArr[2].Point,
                    _algorithm.Triangle2Ds[i].VertexArr[0].Point, _algorithm.Triangle2Ds[i].VertexArr[1].Point) > 0)
                {
                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(b);
                }
                else
                {
                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(c);
                }
            }
            _showMesh.vertices = vertexs;
            _showMesh.SetTriangles(triangles, 0);
            _showMesh.RecalculateNormals();
            _meshFilter.sharedMesh = _showMesh;
        }

    }

    private void OnDrawGizmos()
    {
        if (points != null)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Gizmos.DrawSphere(new Vector3(points[i].x, points[i].y, 0), 0.03f);
            }
        }
        _algorithm?.DrawGizmos();
    }

    private void Clear()
    {
        points.Clear();
    }

    private void Build()
    {
        _algorithm.Build(points, threshold);
        Clear();
    }

}
