using System;
using System.Collections;
using System.Collections.Generic;
using Triangulation2D;
using UnityEngine;

/// <summary>
/// 随机产生一定数目点的三角剖分
/// </summary>
public class TriangulationBowyerWatsonDemo : MonoBehaviour
{
    public int RandomPointCount = 10;
    public List<Vector2> PointList = new List<Vector2>();

    private TriangulationBowyerWatson _algorithm;

    private void Awake()
    {
        UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);
        int range = RandomPointCount;
        for (int i = 0; i < RandomPointCount; ++i)
        {
            float randomX = UnityEngine.Random.Range(-range, range);
            float randomZ = UnityEngine.Random.Range(-range, range);
            PointList.Add(new Vector2(randomX, randomZ));
        }
        Build();

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < PointList.Count; ++i)
        {
            Gizmos.DrawSphere(new Vector3(PointList[i].x, PointList[i].y, 0),
                0.1f);
        }
        Gizmos.color = Color.red;
        _algorithm?.DrawGizmos();
    }

    public void Build()
    {
        _algorithm = new TriangulationBowyerWatson();
        _algorithm.Build(PointList, 0.1f);

    }

}
