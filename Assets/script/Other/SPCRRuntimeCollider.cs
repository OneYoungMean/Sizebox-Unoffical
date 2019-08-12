/*
 * MIT License
 *  Copyright (c) 2018 SPARKCREATIVE
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *  
 *  @author Noriyuki Hiromoto <hrmtnryk@sparkfx.jp>
*/

using UnityEngine;

public abstract class SPCRRuntimeCollider
{
    public enum ColliderType
    {
        Plane,
        Circle,
        Triangle,
        Sphere,
        Capsule,
        Column,  
        AABB,
    }
    /// <summary>
    /// 碰撞方式
    /// </summary>
    public enum CollideFunc
    {
        /// <summary>
        /// 体积会往外排斥,面会往法线方向排斥
        /// </summary>
        Outside,
        /// <summary>
        /// 体积会向内约束,面会往法线相反的方向排斥
        /// </summary>
        Inside,
        /// <summary>
        /// 所有的点都会被限制在面上或者体的外壳上
        /// </summary>
        Freeze,
        /// <summary>
        /// 嘿,它会给物体的两面都会碰撞,但是再碰撞之前它都是可以任意在里面或者外面
        /// </summary>
        Free
    }

    public ColliderType colliderType { get; set; }

    public CollideFunc collideFunc { get; set; }

    public Transform appendTransform { get;set; }
    public Vector3 positionOffset{ get;set; }
    public Vector3 normal { get; set; }//OYM：所有的碰撞面都是通过法线进行计算

    public static Vector3 defaultNormal = Vector3.up;//OYM：平面默认法线为(0,0,1)

    public float friction = 0.5f;//OYM：暂时还不清楚要不要改

    public virtual void OnDrawGizmos()
    {}

}
/// <summary>
/// 平面
/// </summary>
public class PlaneCollider : SPCRRuntimeCollider
{
    public static PlaneCollider GetPlaneCollider( Vector3 positionOffset, Vector3 normal, Transform appendTransform =null,CollideFunc collideFunc = CollideFunc.Outside)
    {
        return new PlaneCollider
        {
            colliderType = ColliderType.Plane,
            appendTransform = appendTransform,
            positionOffset = positionOffset,
            collideFunc =collideFunc,
            normal = normal.normalized
        };
    }
    public static PlaneCollider GetPlaneCollider( Vector3 positionOffset, Quaternion rotationOffset, Transform appendTransform=null, CollideFunc collideFunc = CollideFunc.Outside)
    {
        return GetPlaneCollider( positionOffset, rotationOffset*defaultNormal, appendTransform ,collideFunc);
    }

    public static PlaneCollider GetPlaneCollider(Vector3 positionOffset, Transform appendTransform = null,CollideFunc collideFunc = CollideFunc.Outside)
    {
        return  GetPlaneCollider(positionOffset, defaultNormal, appendTransform, collideFunc);
    }
}
/// <summary>
/// 圆形
/// </summary>
public class CircleCollider : SPCRRuntimeCollider
{
    public float radius { get; set; }

    public static CircleCollider GetCircleCollider(Vector3 positionOffset, Vector3 normal,float radius, Transform appendTransform = null, CollideFunc collideFunc = CollideFunc.Outside)
    {
        return new CircleCollider
        {
            colliderType = ColliderType.Circle,
            collideFunc= collideFunc,
            appendTransform = appendTransform,
            positionOffset = positionOffset,
            normal = normal.normalized,
            radius= radius
        };
    }
    public static CircleCollider GetCircleCollider(Vector3 positionOffset, Quaternion rotationOffset,float radius, Transform appendTransform = null, CollideFunc collideFunc = CollideFunc.Outside)
    {
        return GetCircleCollider(positionOffset, rotationOffset * defaultNormal, radius, appendTransform, collideFunc);
    }

    public static CircleCollider GetPlaneCollider(Vector3 positionOffset, float radius, Transform appendTransform = null, CollideFunc collideFunc = CollideFunc.Outside)
    {
        return GetCircleCollider(positionOffset, defaultNormal, radius, appendTransform, collideFunc);
    }
}
/// <summary>
/// 三角形
/// </summary>
public class TriangleCollider : SPCRRuntimeCollider
{
    public Vector3 pointA { get; set; }
    public Vector3 pointB { get; set; }
    public Vector3 pointC { get; set; }
    
    public Quaternion initiaQuarnion { get;set; }
 
    //normal=(V1-V0)x(V2-V0)
    //Vector3.Cross(pointB - pointA, pointC - pointA);
    //OYM：三角形需要计算法线,这个任务交给jobs完成好了
    //OYM：等下,仔细想一下的话,似乎完全不需要计算很多次,只需要算好一次,然后根据初始的rotation计算坐标就好了

    public static TriangleCollider GetTriangleCollider(Vector3[] point3, Transform appendTransform = null, CollideFunc collideFunc = CollideFunc.Outside)
    {
        TriangleCollider t = new TriangleCollider
        {
            colliderType = ColliderType.Triangle,
            collideFunc = collideFunc,
            pointA = point3[0],
            pointB = point3[1],
            pointC = point3[2],

        };
        t.GetNormal();
        return t;
    }
    public void GetNormal()
    {
        Vector3 m_normal = Vector3.Cross(pointB - pointA, pointC - pointA).normalized;
        normal = Quaternion.Inverse(appendTransform.rotation) * m_normal;
    }
}
/// <summary>
/// 球体
/// </summary>
public class SphereCollider : SPCRRuntimeCollider
{
    public float radius { get; set; }
    public static SphereCollider GetSphereCollider( float radius,Vector3 positionOffset, Transform appendTransform=null,CollideFunc collideFunc=CollideFunc.Outside)
    {
        return new SphereCollider
        {
            colliderType = ColliderType.Sphere,
            collideFunc = collideFunc,
            appendTransform = appendTransform,
            positionOffset = positionOffset,
            radius = radius,
        };
    }
   public override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(positionOffset+ (appendTransform? appendTransform.position:Vector3.zero), radius);
    }

}

public class CapsuleCollider : SPCRRuntimeCollider
{
    public float radiusHead { get; set; }
    public float radiusTail { get; set; }
    public float height { get; set; }
    Quaternion rotaionoffset { get; set; }
    public static CapsuleCollider GetCapsuleCollider(float radiusHead,float radiusTail,float height, Vector3 positionOffset,Quaternion rotaionoffset, Transform appendTransform = null, CollideFunc collideFunc = CollideFunc.Outside)
    {
        return new CapsuleCollider
        {
            colliderType = ColliderType.Capsule,
            appendTransform = appendTransform,
            positionOffset = positionOffset,
            rotaionoffset =rotaionoffset,
            radiusHead= radiusHead,
            radiusTail= radiusTail,
            height=height
        };
    }

    public static CapsuleCollider GetCapsuleCollider(float radiusHead, float radiusTail, Vector3 pointHead,Vector3 pointTail,Transform appendTransform = null, CollideFunc collideFunc = CollideFunc.Outside)
    {
        return GetCapsuleCollider(radiusHead, radiusTail, (pointHead - pointTail).sqrMagnitude, (pointHead + pointTail) * 0.5f, Quaternion.FromToRotation(Vector3.up, pointHead - pointTail), appendTransform, collideFunc);
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        var pos = appendTransform.position+positionOffset;
        var rot = appendTransform.rotation*rotaionoffset;

        var halfLength = height / 2.0f;
        var up = Vector3.up * halfLength;
        var down = Vector3.down * halfLength;
        var right_head = Vector3.right * radiusHead;
        var right_tail = Vector3.right * radiusTail;
        var forward_head = Vector3.forward * radiusHead;
        var forward_tail = Vector3.forward * radiusTail;
        var top = pos + rot * up;
        var bottom = pos + rot * down;

        var mOld = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(pos, rot, Vector3.one);
        Gizmos.DrawLine(right_head - up, right_tail + up);
        Gizmos.DrawLine(-right_head - up, -right_tail + up);
        Gizmos.DrawLine(forward_head - up, forward_tail + up);
        Gizmos.DrawLine(-forward_head - up, -forward_tail + up);

        Gizmos.matrix = Matrix4x4.Translate(top) * Matrix4x4.Rotate(rot);
        DrawWireArc(radiusTail, 360);
        Gizmos.matrix = Matrix4x4.Translate(bottom) * Matrix4x4.Rotate(rot);
        DrawWireArc(radiusHead, 360);

        Gizmos.matrix = Matrix4x4.Translate(top) * Matrix4x4.Rotate(rot * Quaternion.AngleAxis(90, Vector3.forward));
        DrawWireArc(radiusTail, 180);
        Gizmos.matrix = Matrix4x4.Translate(top) * Matrix4x4.Rotate(rot * Quaternion.AngleAxis(90, Vector3.up) * Quaternion.AngleAxis(90, Vector3.forward));
        DrawWireArc(radiusTail, 180);
        Gizmos.matrix = Matrix4x4.Translate(bottom) * Matrix4x4.Rotate(rot * Quaternion.AngleAxis(90, Vector3.up) * Quaternion.AngleAxis(-90, Vector3.forward));
        DrawWireArc(radiusHead, 180);
        Gizmos.matrix = Matrix4x4.Translate(bottom) * Matrix4x4.Rotate(rot * Quaternion.AngleAxis(-90, Vector3.forward));
        DrawWireArc(radiusHead, 180);

        Gizmos.matrix = mOld;

    }

    void DrawWireArc(float radius, float angle)
    {
        Vector3 from = Vector3.forward * radius;
        var step = Mathf.RoundToInt(angle / 15.0f);
        for (int i = 0; i <= angle; i += step)
        {
            var rad = (float)i * Mathf.Deg2Rad;
            var to = new Vector3(radius * Mathf.Sin(rad), 0, radius * Mathf.Cos(rad));
            Gizmos.DrawLine(from, to);
            from = to;
        }
    }
}

public class ColumnCollider : SPCRRuntimeCollider
{
    public float radiusHead { get; set; }
    public float radiusTail { get; set; }
    public float height { get; set; }
    Quaternion rotaionoffset { get; set; }
    public static ColumnCollider GetColumnCollider(float radiusHead, float radiusTail, float height, Vector3 positionOffset, Quaternion rotaionoffset, Transform appendTransform = null, CollideFunc collideFunc = CollideFunc.Outside)
    {
        return new ColumnCollider
        {
            colliderType = ColliderType.Column,
            appendTransform = appendTransform,
            positionOffset = positionOffset,
            rotaionoffset = rotaionoffset,
            radiusHead = radiusHead,
            radiusTail = radiusTail,
            height = height
        };
    }

    public static ColumnCollider GetCapsuleCollider(float radiusHead, float radiusTail, Vector3 pointHead, Vector3 pointTail, Transform appendTransform = null, CollideFunc collideFunc = CollideFunc.Outside)
    {
        return GetColumnCollider(radiusHead, radiusTail, (pointHead - pointTail).sqrMagnitude, (pointHead + pointTail) * 0.5f, Quaternion.FromToRotation(Vector3.up, pointHead - pointTail), appendTransform, collideFunc);
    }
}

    public class AABBcollider : SPCRRuntimeCollider
{
    public Vector3 PointA { get; set; }

    public Vector3 PointB { get; set; }

    public AABBcollider(ColliderType colliderType, Transform appendTransform, Vector3 positionOffset, Quaternion rotationOffset, Vector3 PointA, Vector3 PointB)
    {
        this.colliderType = colliderType;
        this.appendTransform = appendTransform;
        this.positionOffset = positionOffset;
        this.PointA = PointA;
        this.PointB = PointB;
    }
}