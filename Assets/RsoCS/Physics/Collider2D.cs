using rso.physics;
using System;
using System.Collections.Generic;

namespace rso.physics
{
    public abstract class CCollider2D : CObject2D
    {
        public Int32 Number { get; protected set; }
        public bool LocalEnabled = true;
        public bool Enabled
        {
            get
            {
                return LocalEnabled && (_ParentCollider == null || _ParentCollider.Enabled);
            }
        }
        CCollider2D _ParentCollider = null;
        public void SetParentCollider(CCollider2D ParentCollider_)
        {
            _ParentCollider = ParentCollider_;
            SetParent(ParentCollider_);
        }
        public CCollider2D(STransform Transform_, Int32 Number_) :
            base(Transform_)
        {
            Number = Number_;
        }

        // �ᱹ CEngineObjectRect �� CEngineObjectRect �� ���� ��길 �����ϰ�, �Ķ���Ͱ� CEngineObject�� ���� ��ü�� �Ǿ�� CEngineObjectRect���� CEngineObjectContainer���� �ľ��� �����ϹǷ�
        // ���� ��ü�� CEngineObjectRect�϶����� �� �� ���� �Ķ���͸� �ٽ� ��ü�� �ٲپ� CEngineObjectRect �϶� ���� CollisionCheck �� ȣ���Ͽ�
        // ��ü�� �Ķ���͸� ��� CEngineObjectRect �� ���� �� ó��
        public abstract void OverlappedCheck(Int64 Tick_, CMovingObject2D MovingObject_, CCollider2D OtherCollider_, CMovingObject2D OtherMovingObject_);
        public abstract void OverlappedCheck(Int64 Tick_, CMovingObject2D MovingObject_, CRectCollider2D OtherRectCollider_, CMovingObject2D OtherMovingObject_);
        public abstract bool IsOverlapped(Int64 Tick_, CCollider2D OtherCollider_);
        public abstract bool IsOverlapped(Int64 Tick_, CRectCollider2D OtherRectCollider_);
    }
}
