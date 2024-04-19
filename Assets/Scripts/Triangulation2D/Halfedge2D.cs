
namespace Triangulation2D
{
    /// <summary>
    /// 快速查找三角形的拓扑关系使用顶点进行三角形表示欠佳, 所以出现半边数据结构
    /// 半边数据结构是一种用于表示和处理三角剖分和凸多边形的数据结构
    /// 它将三角形的每条边拆分为两个半边，每个半边记录了其 起始顶点、终止顶点、相邻面 等信息
    /// 这种数据结构在图形学和几何算法中被广泛应用，能够高效地进行各种几何计算和查询操作
    /// </summary>
    public class Halfedge2D
    {
        /// <summary>
        /// 起始顶点和终止顶点
        /// </summary>
        public Vertex2D StartPoint;
        public Vertex2D EndPoint;
        /// <summary>
        /// 上一个半边, 下一个半边, 顶点相同的另一个半边
        /// </summary>
        public Halfedge2D Previous, Next, Twins;
        //半边方向所相邻的三角面
        public Triangle2D Face;

    }

}
