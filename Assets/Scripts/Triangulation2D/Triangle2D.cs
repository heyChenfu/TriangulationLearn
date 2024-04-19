
using UnityEngine;

namespace Triangulation2D
{

    /// <summary>
    /// 2D三角面
    /// </summary>
    public class Triangle2D
    {
        public Vertex2D[] VertexArr = new Vertex2D[3];
        public Segment2D[] SegmentArr = new Segment2D[3];

        /// <summary>
        /// 外接圆
        /// </summary>
        private Circle2D _circumcircle;

        public Triangle2D(Segment2D s1, Segment2D s2, Segment2D s3)
        {
            SegmentArr[0] = s1;
            SegmentArr[1] = s2;
            SegmentArr[2] = s3;
            VertexArr[0] = s1.P0;
            VertexArr[1] = s1.P1;
            VertexArr[2] = (s3.P1 == s1.P0 || s3.P1 == s1.P1) ? s3.P0 : s3.P1;
        }

        public Triangle2D(Vertex2D a, Vertex2D b, Vertex2D c)
        {
            VertexArr[0] = a;
            VertexArr[1] = b;
            VertexArr[2] = c;
            SegmentArr[0] = new Segment2D(a, b);
            SegmentArr[1] = new Segment2D(b, c);
            SegmentArr[2] = new Segment2D(c, a);

        }

        /// <summary>
        /// 是否有目标点
        /// </summary>
        public bool HasPoint(Vector2 p)
        {
            for (int i = 0; i < VertexArr.Length; ++i)
            {
                if (VertexArr[i].Point == p)
                    return true;
            }
            return false;
        }

        public bool HasSegment(Segment2D s)
        {
            for (int i = 0; i < SegmentArr.Length; ++i)
            {
                if (SegmentArr[i] == s)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取半周长(三条边长之和的一半)
        /// </summary>
        /// <returns></returns>
        public float HalfCircumference()
        {
            return (SegmentArr[0].Magnitude + SegmentArr[1].Magnitude + SegmentArr[2].Magnitude) / 2;
        }

        /// <summary>
        /// 获取三角形面积(海伦公式
        /// S=√[p(p-a)(p-b)(p-c)]
        /// </summary>
        /// <returns></returns>
        public float GetTriangleArea()
        {
            float p = HalfCircumference();
            return Mathf.Sqrt(p * (p - SegmentArr[0].Magnitude) * (p - SegmentArr[1].Magnitude) * (p - SegmentArr[2].Magnitude));
        }

        /// <summary>
        /// 获取三角形外接圆
        /// </summary>
        /// <returns></returns>
        public Circle2D GetCircumcircle()
        {
            if(_circumcircle == null)
                _circumcircle = CalculationTool.GetCircumcircleByTriangle(this);
            return _circumcircle;
        }

        public void DrawGizmos()
        {
            for (int i = 0; i < SegmentArr.Length; ++i)
            {
                SegmentArr[i].DrawGizmos();
            }
            _circumcircle?.DrawGizmos();
        }


    }
}
