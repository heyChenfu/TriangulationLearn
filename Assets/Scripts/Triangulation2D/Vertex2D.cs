using UnityEngine;

namespace Triangulation2D
{
    /// <summary>
    /// 带引用计数的二维坐标
    /// </summary>
    public struct Vertex2D
    {
        private Vector2 _point;
        private int _refCount;

        public Vector2 Point { get { return _point; } }
        public int RefCount { get { return _refCount;} }

        public Vertex2D(Vector2 point)
        {
            _point = point;
            _refCount = 0;
        }

        public Vertex2D(float x, float y)
        {
            _point = new Vector2(x, y);
            _refCount = 0;
        }

        public void AddReference()
        {
            _refCount++;
        }

        public void SubtractReference()
        {
            _refCount--;
        }

        public static Vector2 operator +(Vertex2D a, Vertex2D b)
        {
            return a.Point + b.Point;
        }

        public static Vector2 operator -(Vertex2D a, Vertex2D b)
        {
            return a.Point - b.Point;
        }

        public static bool operator ==(Vertex2D a, Vertex2D b)
        {
            return a.Point == b.Point;
        }

        public static bool operator !=(Vertex2D a, Vertex2D b)
        {
            return !(a.Point == b.Point);
        }

        public override bool Equals(object other)
        {
            if (!(other is Vertex2D))
                return false;
            return Equals((Vertex2D)other);
        }

        public bool Equals(Vertex2D other)
        {
            return Point.Equals(other.Point);
        }

        public override int GetHashCode()
        {
            return Point.GetHashCode();
        }

    }

}

