
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace Triangulation2D
{
    /// <summary>
    /// Bowyer-Watson算法Delaunay三角剖分(朴素无优化版本)
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

        public void Build(List<Vector2> pList)
        {
            //Profiler.BeginSample("TriangulationBowyerWatson");

            List<Vertex2D> pointList = CalculationTool.RemoveTooClosePoint(pList, 0.1f);
            Triangle2D largeTriangle = SuperTriangle(pointList, out Vertex2D p1, out Vertex2D p2, out Vertex2D p3);
            Triangle2Ds.Add(largeTriangle);

            //受新插入点影响的坏三角形集合
            List<Triangle2D> badTriangles = new List<Triangle2D>();
            List<Segment2D> polygonHole = new List<Segment2D>();
            //遍历并将所有点插入
            for (int i = 0; i < pointList.Count; ++i)
            {
                badTriangles.Clear();
                //判断需要插入的点是否在现有三角形外接圆内
                for (int j = 0; j < Triangle2Ds.Count; ++j)
                {
                    if ((Triangle2Ds[j].GetCircumcircle()?.Contains(pointList[i].Point)).GetValueOrDefault())
                        badTriangles.Add(Triangle2Ds[j]);
                }
                //取出坏三角形集合中, 不在两个坏三角形之间共享的边
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
                Vertex2D insertPoint = new Vertex2D(pointList[i].Point);
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
                        Triangle2Ds[i].VertexArr[j] == p3)
                        bDelete = true;
                }
                if (bDelete)
                    Triangle2Ds.RemoveAt(i);
            }

            //Profiler.EndSample();
        }

        /// <summary>
        /// 构建一个足够大包含了所有点的三角形
        /// </summary>
        /// <param name="pointList"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public Triangle2D SuperTriangle(List<Vertex2D> pointList, out Vertex2D p1, out Vertex2D p2, out Vertex2D p3)
        {
            float minX = 0, minY = 0, maxX = 0, maxY = 0;
            //获取最大最小点
            for (int i = 0; i < pointList.Count; ++i)
            {
                if (pointList[i].Point.x < minX)
                    minX = pointList[i].Point.x;
                if (pointList[i].Point.y < minY)
                    minY = pointList[i].Point.y;
                if (pointList[i].Point.x > maxX)
                    maxX = pointList[i].Point.x;
                if (pointList[i].Point.y > maxY)
                    maxY = pointList[i].Point.y;
            }
            var dx = (maxX - minX) * CreateLargeTriangleOffset;
            var dy = (maxY - minY) * CreateLargeTriangleOffset;
            p1 = new Vertex2D(minX - dx, minY - dy * 3);
            p2 = new Vertex2D(minX - dx, maxY + dy);
            p3 = new Vertex2D(maxX + dx * 3, maxY + dy);
            return new Triangle2D(p1, p2, p3);
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
