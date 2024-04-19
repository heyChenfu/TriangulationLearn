
using UnityEngine;

namespace Triangulation2D
{
    public class Circle2D
    {

        public Vector2 Center;
        public float Radius;

        public Circle2D(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// 判断点是否在圆内
        /// </summary>
        /// <returns></returns>
        public bool Contains(Vector2 point)
        {
            Vector2 vec = point - Center;
            return Radius * Radius > vec.sqrMagnitude;
        }

        public void DrawGizmos()
        {
            Gizmos.DrawWireSphere(new Vector3(Center.x, Center.y, 0), Radius);
        }

    }

}
