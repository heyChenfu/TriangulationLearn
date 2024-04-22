
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace Triangulation2D
{
    /// <summary>
    /// Bowyer-Watson算法Delaunay三角剖分
    /// 根据wiki: https://en.wikipedia.org/wiki/Bowyer%E2%80%93Watson_algorithm 伪代码编写
    /// </summary>
    public class TriangulationBowyerWatson
    {
        /// <summary>
        /// 创建包含所有点集的大三角形偏移量
        /// </summary>
        public const float CreateLargeTriangleOffset = 10;
        public List<Triangle2D> Triangle2Ds = new List<Triangle2D>();

        public TriangulationBowyerWatson()
        {

        }

        public void Build(List<Vector2> pointList)
        {
            Profiler.BeginSample("TriangulationBowyerWatson");

            List<Vertex2D> pointRefList = CalculationTool.RemoveTooClosePoint(pointList, 0.1f);
            float minX = 0, minY = 0, maxX = 0, maxY = 0;
            //获取最大最小点
            for (int i = 0; i < pointRefList.Count; ++i)
            {
                if (pointRefList[i].Point.x < minX)
                    minX = pointRefList[i].Point.x;
                if (pointRefList[i].Point.y < minY)
                    minY = pointRefList[i].Point.y;
                if (pointRefList[i].Point.x > maxX)
                    maxX = pointRefList[i].Point.x;
                if (pointRefList[i].Point.y > maxY)
                    maxY = pointRefList[i].Point.y;
            }
            //创建一个大的外接三角形以包含所有点
            float offset = Mathf.Max(maxX - minX, maxY - minY) * CreateLargeTriangleOffset;
            Vertex2D p1 = new Vertex2D(minX - offset, minY - offset);
            Vertex2D p2 = new Vertex2D(minX - offset, maxY + offset);
            Vertex2D p3 = new Vertex2D(maxX + offset, minY - offset);
            Vertex2D p4 = new Vertex2D(maxX + offset, minY + offset);
            Segment2D s1 = new Segment2D(p1, p2);
            Segment2D s2 = new Segment2D(p1, p3);
            Segment2D s3 = new Segment2D(p2, p3);
            Segment2D s4 = new Segment2D(p4, p2);
            Segment2D s5 = new Segment2D(p4, p3);
            Triangle2D largeTriangle1 = new Triangle2D(s1, s2, s3);
            Triangle2D largeTriangle2 = new Triangle2D(s3, s4, s5);
            Triangle2Ds.Add(largeTriangle1);
            Triangle2Ds.Add(largeTriangle2);

            //受新插入点影响的三角形
            List<Triangle2D> badTriangles = new List<Triangle2D>();
            List<Segment2D> polygonHole = new List<Segment2D>();
            //遍历并将所有点插入
            for (int i = 0; i < pointRefList.Count; ++i)
            {
                badTriangles.Clear();
                //判断需要插入的点是否在现有三角形外接圆内
                for (int j = 0; j < Triangle2Ds.Count; ++j)
                {
                    if ((Triangle2Ds[j].GetCircumcircle()?.Contains(pointRefList[i].Point)).GetValueOrDefault())
                        badTriangles.Add(Triangle2Ds[j]);
                }
                //将受影响三角形且不属于其他三角形的边取出
                polygonHole.Clear();
                for (int j = 0; j < badTriangles.Count; ++j)
                {
                    for (int z = 0; z < badTriangles[j].SegmentArr.Length; ++z)
                    {
                        bool bFind = false;
                        for (int k = 0; k < badTriangles.Count; ++k)
                        {
                            if (badTriangles[k] == badTriangles[j])
                                continue;
                            if (badTriangles[k].HasSegment(badTriangles[j].SegmentArr[z]))
                                bFind = true;
                        }
                        if (!bFind)
                            polygonHole.Add(badTriangles[j].SegmentArr[z]);
                    }
                }
                //删除受影响的三角形
                for (int j = 0; j < badTriangles.Count; ++j)
                {
                    Triangle2Ds.Remove(badTriangles[j]);
                }
                //根据找到的边构建新的三角形
                Vertex2D insertPoint = new Vertex2D(pointRefList[i].Point);
                for (int j = 0; j  < polygonHole.Count; ++j)
                {
                    Triangle2D newTriangle = new Triangle2D(polygonHole[j], 
                        new Segment2D(polygonHole[j].P0, insertPoint), 
                        new Segment2D(polygonHole[j].P1, insertPoint));
                    Triangle2Ds.Add(newTriangle);
                }
            }
            //删除外接三角形
            for (int i = Triangle2Ds.Count - 1; i >= 0; --i)
            {
                bool bDelete = false;
                for (int j = 0; j < Triangle2Ds[i].VertexArr.Length; ++j)
                {
                    if (Triangle2Ds[i].VertexArr[j] == p1 ||
                        Triangle2Ds[i].VertexArr[j] == p2 ||
                        Triangle2Ds[i].VertexArr[j] == p3 ||
                        Triangle2Ds[i].VertexArr[j] == p4)
                        bDelete = true;
                }
                if (bDelete)
                    Triangle2Ds.RemoveAt(i);
            }

            Profiler.EndSample();
        }

        public void DrawGizmos()
        {
            for (int i = 0; i < Triangle2Ds.Count; ++i)
            {
                Triangle2Ds[i].DrawGizmos();
            }
        }

    }

}
