using System;
using System.Collections.Generic;

namespace rso.physics
{
    public class CCollectionCollider2D : CCollider2D
    {
        public List<CCollider2D> Colliders = new List<CCollider2D>();

        public CCollectionCollider2D(STransform Transform_, Int32 Number_) :
            base(Transform_, Number_)
        {
        }
        public override void OverlappedCheck(Int64 Tick_, CMovingObject2D MovingObject_, CCollider2D OtherCollider_, CMovingObject2D OtherMovingObject_)
        {
            if (!LocalEnabled)
                return;

            foreach (var o in Colliders)
                o.OverlappedCheck(Tick_, MovingObject_, OtherCollider_, OtherMovingObject_);
        }
        public override void OverlappedCheck(Int64 Tick_, CMovingObject2D MovingObject_, CRectCollider2D OtherRectCollider_, CMovingObject2D OtherMovingObject_)
        {
            if (!LocalEnabled)
                return;

            foreach (var o in Colliders)
                o.OverlappedCheck(Tick_, MovingObject_, OtherRectCollider_, OtherMovingObject_);
        }
        public override bool IsOverlapped(Int64 Tick_, CCollider2D OtherCollider_)
        {
            throw new Exception();
        }
        public override bool IsOverlapped(Int64 Tick_, CRectCollider2D OtherRectCollider_)
        {
            throw new Exception();
        }

        public static void SetCollectionToCollectionCollider2D(List<CCollider2D> Collection_, CCollectionCollider2D Collider_) // C++ 와 맞추기 위함
        {
            Collider_.Colliders = Collection_;

            foreach (var i in Collider_.Colliders)
                i.SetParentCollider(Collider_);
        }
    }
}