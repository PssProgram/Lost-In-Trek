using System;
using System.Collections.Generic;
using Sce.Pss.Core;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;

using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base
{
    /// <summary>
    /// 检测两个OBB是否相交
    /// 
    ///   分离轴碰撞检测：
    ///   rect1，rect2 为OBB ，存储的数据是4个顶点坐标（Vector2）。
    ///   
    ///   算法说明 ： 
    ///   
    ///         使用rect1 的四条边 作为候选分离轴，检测rect2的四个点在这4条边上的投影，
    ///   是否与 该边的两个点之间的线段有交叉， 再用rect2作候选分离轴一次，这样就对8个候选分离轴
    ///   进行了判断，如果8个轴当中没有找到真正的分离轴，就说两个矩形 不是分离的，即发生了碰撞。
    /// </summary>
    public class OBB
    {
        Vector2 center = Vector2.Zero;
        //当前方向
        Vector2 direction = Vector2.Zero;

        float width;
        float height;

        public OBB(Vector2 pos, Vector2 dir, float _width, float _height)
        {
            this.width = _width * 0.5f;
            this.height = _height * 0.5f;
            this.center = pos;
            this.direction = dir;
        }

        public List<Vector2> GetPoints()
        {
            //构造OBB
            //body的方向
            Vector2 bodyDir = direction;
            if (bodyDir != Vector2.Zero)
                bodyDir.Normalize();
            //body的方向 的垂直方向
            Vector2 bodyDirC = new Vector2(bodyDir.Y, -bodyDir.X);
            if (bodyDirC != Vector2.Zero)
                bodyDirC.Normalize();

            Vector2 point1 = bodyDir * height + bodyDirC * width + center;
            Vector2 point2 = bodyDir * height + -bodyDirC * width + center;
            Vector2 point3 = -bodyDir * height + -bodyDirC * width + center;
            Vector2 point4 = -bodyDir * height + bodyDirC * width + center;

            List<Vector2> points = new List<Vector2>();
            points.Add(point1);
            points.Add(point2);
            points.Add(point3);
            points.Add(point4);

            return points;
        }

        public Vector2 GetCenter()
        {
            return center;
        }

        /// <summary>
        /// OBB之间是否接触 
        /// </summary>
        public static bool IsContacted(OBB obb1, OBB obb2)
        {
            bool rect1Torect2IsContact = false;
            bool rect2Torect1IsContact = false;

            rect1Torect2IsContact = IsContact(obb1, obb2);
            rect2Torect1IsContact = IsContact(obb2, obb1);

            if (rect1Torect2IsContact == true &&
                rect2Torect1IsContact == true)
            {
                return true;
            }
            return false;
        }

        // 使用一个矩形的4条边作候选分离轴
        static bool IsContact(OBB obb1, OBB obb2)
        {
            bool isInLine1 = false;
            bool isInLine2 = false;
            bool isInLine3 = false;
            bool isInLine4 = false;

            Vector2 line1p1 = obb1.GetPoints()[0];
            Vector2 line1p2 = obb1.GetPoints()[1];
            isInLine1 = RectAllPointIsInLine(line1p1, line1p2, obb2);

            Vector2 line2p1 = obb1.GetPoints()[1];
            Vector2 line2p2 = obb1.GetPoints()[2];
            isInLine2 = RectAllPointIsInLine(line2p1, line2p2, obb2);


            Vector2 line3p1 = obb1.GetPoints()[2];
            Vector2 line3p2 = obb1.GetPoints()[3];
            isInLine3 = RectAllPointIsInLine(line3p1, line3p2, obb2);

            Vector2 line4p1 = obb1.GetPoints()[3];
            Vector2 line4p2 = obb1.GetPoints()[0];
            isInLine4 = RectAllPointIsInLine(line4p1, line4p2, obb2);

            if (isInLine1 == true && isInLine2 == true &&
                isInLine3 == true && isInLine4 == true)
            {//全部边都满足 投影相交
                return true;
            }
            return false;
        }

        //判断4个投影点有没有与指定线段相交
        static bool RectAllPointIsInLine(Vector2 linep1, Vector2 linep2, OBB obb2)
        {
            //获取rect4的4个投影点
            Vector2 pointPro1 = GetProjectionPoint(linep1, linep2, obb2.GetPoints()[0]);
            Vector2 pointPro2 = GetProjectionPoint(linep1, linep2, obb2.GetPoints()[1]);
            Vector2 pointPro3 = GetProjectionPoint(linep1, linep2, obb2.GetPoints()[2]);
            Vector2 pointPro4 = GetProjectionPoint(linep1, linep2, obb2.GetPoints()[3]);

            //根据line 的斜率选择映射到X/Y轴来计算
            float k = (linep2.Y - linep1.Y) / (linep2.X - linep1.X);

            #region ……
            if (k > -0.25f && k <= 0.25)
            {
                //映射到X轴
                List<float> rectPX = new List<float>();
                rectPX.Add(pointPro1.X);
                rectPX.Add(pointPro2.X);
                rectPX.Add(pointPro3.X);
                rectPX.Add(pointPro4.X);

                return PointIsInLineSegment(linep1.X, linep2.X, rectPX);
            }
            else
            {
                //映射到Y轴
                List<float> rectPY = new List<float>();
                rectPY.Add(pointPro1.Y);
                rectPY.Add(pointPro2.Y);
                rectPY.Add(pointPro3.Y);
                rectPY.Add(pointPro4.Y);

                return PointIsInLineSegment(linep1.Y, linep2.Y, rectPY);
            }
            #endregion

        }

        //获取指定边上，指定点的投影点
        static Vector2 GetProjectionPoint(Vector2 linep1, Vector2 linep2, Vector2 point)
        {
            Vector2 pointPro = Vector2.Zero;

            float k = Vector2.Dot(point - linep1, linep2 - linep1) /
               Vector2.Dot(linep2 - linep1, linep2 - linep1);

            pointPro = k * (linep2 - linep1) + linep1;

            return pointPro;
        }

        //判定4个投影点 与 投影所在直线上的线段是否有交叉
        static bool PointIsInLineSegment(float lp1, float lp2, List<float> rectP)
        {
            // 先检查4个投影点是否有包含在线段点上的
            float min = lp1 < lp2 ? lp1 : lp2;
            float max = lp1 < lp2 ? lp2 : lp1;

            for (int i = 0; i < rectP.Count; i++)
            {
                if (rectP[i] > min && rectP[i] <= max)
                    return true;
            }
            //再检测 2个线段点是否包含在4个投影点之间
			
            float minPro = MinFloat(rectP);
            float maxPro = MaxFloat(rectP);
			int[] a = new int[2];
            if (lp1 > minPro && lp1 <= maxPro)
                return true;
            if (lp2 > minPro && lp2 <= maxPro)
                return true;

            return false;
        }
		
		private static float MinFloat(List<float> list)
		{
			float min = list[0];
			foreach(float f in list)
			{
				if(f < min)
					min = f;
			}
			return min;
		}
		
		private static float MaxFloat(List<float> list)
		{
			float max = list[0];
			foreach(float f in list)
			{
				if(f > max)
					max = f;
			}
			return max;
		}
	}
}
