
using System.Drawing;
using UnityEngine;

namespace Triangulation2D
{
    /// <summary>
    /// 2D线段
    /// </summary>
    public class Segment2D
    {

        public Vertex2D P0;
        public Vertex2D P1;
        private int _refCount;
        private float _magnitude = 0f;
        private float _sqrMagnitude = 0f;

        public int RefCount { get { return _refCount; } }
        /// <summary>
        /// 返回线段长度
        /// </summary>
        public float Magnitude { 
            get 
            {
                if (_magnitude <= float.Epsilon)
                    _magnitude = (P0 - P1).magnitude;
                return _magnitude; 
            } 
        }
        /// <summary>
        /// 返回长度的平方
        /// </summary>
        public float SqrMagnitude { 
            get 
            {
                if (_sqrMagnitude <= float.Epsilon)
                    _sqrMagnitude = (P0 - P1).sqrMagnitude;
                return _sqrMagnitude; 
            } 
        }

        public Segment2D(Vertex2D p0, Vertex2D p1) 
        {
            P0 = p0;
            P1 = p1;

        }

        public static bool operator ==(Segment2D a, Segment2D b)
        {
            return (a.P0 == b.P0 && a.P1 == b.P1) || (a.P1 == b.P0 && a.P0 == b.P1);
        }

        public static bool operator !=(Segment2D a, Segment2D b)
        {
            return !((a.P0 == b.P0 && a.P1 == b.P1) || (a.P1 == b.P0 && a.P0 == b.P1));
        }

        public override bool Equals(object other)
        {
            if (!(other is Segment2D))
                return false;
            return Equals((Segment2D)other);
        }

        public bool Equals(Segment2D other)
        {
            return (P0.Equals(other.P0) && P1.Equals(other.P1)) || (P1.Equals(other.P0) && P0.Equals(other.P1));
        }

        public override int GetHashCode()
        {
            return P0.GetHashCode() ^ P1.GetHashCode();
        }

        /// <summary>
        /// 获取线段中间点
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMidPoint()
        {
            return (P0 + P1) / 2;
        }

        public void AddReference()
        {
            P0.AddReference();
            P1.AddReference();
            _refCount++;
        }

        public void SubtractReference()
        {
            P0.SubtractReference();
            P1.SubtractReference();
            _refCount--;
        }

        /// <summary>
        /// 目标点是否在当前线段上
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool Contains(Vector2 p)
        {
            if(HasPoint(p))
                return true;
            if(Distance(p) > float.Epsilon)
                return false;

            bool xInSegment = P0.Point.x < P1.Point.x ? (p.x <= P1.Point.x && p.x >= P0.Point.x) : (p.x <= P0.Point.x && p.x >= P1.Point.x);
            bool yInSegment = P0.Point.y < P1.Point.y ? (p.y <= P1.Point.y && p.y >= P0.Point.y) : (p.y <= P0.Point.y && p.y >= P1.Point.y);
            return xInSegment && yInSegment;
        }

        public bool ContainsPoint(Vertex2D p)
        {
            return Contains(p.Point);
        }

        /// <summary>
        /// 是否有目标点
        /// </summary>
        public bool HasPoint(Vector2 p)
        {
            return P0.Point == p || P1.Point == p;
        }

        /// <summary>
        /// 求点到当前线段所在直线的距离
        /// 直线到点的距离公式 d=|Ax+By+C|/sqrt(A^2+B^2)
        /// 由于斜率m=(p1.y-p0.y)/(p1.x-p0.x)
        /// 而由一般式方程Ax+By+C=0且C=0过原点可以得到y=-A*x/B, 所以斜率m可以表示为-A/B
        /// 进而求得A=p1.y-p0.y, B=p1.x-p0.x, C=p1.x*p0.y-p0.x*p1.y (负号给A或B都可以)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public float Distance(Vector2 p)
        {
            Vector2 p0 = P0.Point, p1 = P1.Point;
            float b = (p1.x - p0.x), a = (p1.y - p0.y);
            return Mathf.Abs((a * p.x) - (b * p.y) + (p1.x * p0.y) - (p1.y * p0.x)) / Mathf.Sqrt(a * a + b * b);
        }

        public void DrawGizmos()
        {
            Gizmos.DrawLine(P0.Point, P1.Point);
        }

    }
}
