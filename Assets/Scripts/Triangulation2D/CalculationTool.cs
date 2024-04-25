

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Triangulation2D
{
    public static class CalculationTool
    {

        /// <summary>
        /// 任意数目点是否共线
        /// </summary>
        /// <param name="pointArr"></param>
        /// <returns></returns>
        public static bool IsPointCollinear(params Vector2[] points)
        {
            if(points.Length < 2)
                return false;

            //计算第一个线段斜率
            float slope = (points[1].y - points[0].y) / (points[1].x - points[0].x);
            for (int i = 1;  i < points.Length; ++i)
            {
                float newSlope = (points[i].y - points[0].y) / (points[i].x - points[0].x);
                if(Mathf.Abs(slope - newSlope) > float.Epsilon)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 获取三角形外接圆
        /// 外接圆的圆心是三角形三条边的垂直平分线的交点，根据圆心到顶点的距离相等，可以列出以下方程：
        /// (x1,y1),(x2,y2), (x3,y3)求外接圆心坐标O(x, y)
        ///(x1-x)*(x1-x)+(y1-y)*(y1-y)=(x2-x)*(x2-x)+(y2-y)*(y2-y);
        ///(x2-x)*(x2-x)+(y2-y)*(y2-y)=(x3-x)*(x3-x)+(y3-y)*(y3-y);
        ///可以推导出外接圆圆心x和y
        ///且半径: r = √(x1-x)*(x1-x)+(y1-y)*(y1-y);
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        public static Circle2D GetCircumcircleByTriangle(Triangle2D triangle)
        {
            if (IsPointCollinear(triangle.VertexArr[0].Point, triangle.VertexArr[1].Point, triangle.VertexArr[2].Point))
            {
                Debug.Log("三点共线情况无法获取外接圆!");
                return null;
            }

            float x1 = triangle.VertexArr[0].Point.x;
            float y1 = triangle.VertexArr[0].Point.y;
            float x2 = triangle.VertexArr[1].Point.x;
            float y2 = triangle.VertexArr[1].Point.y;
            float x3 = triangle.VertexArr[2].Point.x;
            float y3 = triangle.VertexArr[2].Point.y;
            float x = ((y2 - y1) * (y3 * y3 - y1 * y1 + x3 * x3 - x1 * x1) - (y3 - y1) * (y2 * y2 - y1 * y1 + x2 * x2 - x1 * x1)) /
                (2 * (x3 - x1) * (y2 - y1) - 2 * (x2 - x1) * (y3 - y1));
            float y = ((x2 - x1) * (x3 * x3 - x1 * x1 + y3 * y3 - y1 * y1) - (x3 - x1) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1)) /
                (2 * (y3 - y1) * (x2 - x1) - 2 * (y2 - y1) * (x3 - x1));
            float r = Mathf.Sqrt((x1 - x) * (x1 - x) + (y1 - y) * (y1 - y));
            return new Circle2D(new Vector2(x, y), r);
        }

        /// <summary>
        /// 去除点集中相邻太近的点
        /// </summary>
        /// <param name="list"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static List<Vertex2D> RemoveTooClosePoint(List<Vector2> list, float threshold)
        {
            List<Vertex2D> removeList = new List<Vertex2D>(list.Count);
            for (int i = 0; i < list.Count; ++i)
            {
                bool bTooClose = false;
                for (int j = i + 1; j < list.Count; ++j)
                {
                    if(Vector2.Distance(list[i], list[j]) <= threshold)
                        bTooClose = true;
                }
                if (!bTooClose)
                    removeList.Add(new Vertex2D(list[i]));
            }
            return removeList;
        }

        public static double GetCurrentTimestamp()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;
        }

    }

}
