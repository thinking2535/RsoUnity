using System;
using System.Collections.Generic;
using System.Linq;

namespace rso.physics
{
    using TContactPoint2Ds = Dictionary<SContactPoint2D, CMovingObject2D>;
    public struct SContactPoint2D
    {
        public CCollider2D Collider;
        public CCollider2D OtherCollider;

        public SContactPoint2D(CCollider2D Collider_, CCollider2D OtherCollider_)
        {
            Collider = Collider_;
            OtherCollider = OtherCollider_;
        }
    }
    public class CPlayerObject2D : CMovingObject2D
    {
        public delegate void FCollision(Int64 Tick_, SPoint Normal_, CCollider2D Collider_, CCollider2D OtherCollider_, CMovingObject2D OtherMovingObject_);
        public delegate void FCollisionExit(Int64 Tick_, CCollider2D Collider_, CCollider2D OtherCollider_, CMovingObject2D OtherMovingObject_);

        public FCollision fCollisionEnter;
        public FCollision fCollisionStay;
        public FCollisionExit fCollisionExit;

        // key�� SContactPoint2D���� ���� ������ �������� Collider�� ������ ��ü�� OnCollisionStay ����
        // ���� ó������ ��� _ContactPoint2Ds �� ���Ͽ� ���Ǿ�� �ϰ�,
        // FixedUpdate�� �ѹ� ���Ǿ�� �ϱ� ������ _ContactPoint2Ds �� CMovingObject2D �� �����ؾ��ϰ�
        // �ټ��� Collider�� ������ CMovingObject2D �� �ٴ�ٷ� Contact�� �Ͼ�� ������
        TContactPoint2Ds _ContactPoint2Ds = new TContactPoint2Ds();
        public CPlayerObject2D(STransform Transform_, SPoint Velocity_) :
            base(Transform_, Velocity_)
        {
        }
        public override CPlayerObject2D GetPlayerObject2D()
        {
            return this;
        }
        public void Overlapped(Int64 Tick_, SPoint Normal_, CCollider2D Collider_, CCollider2D OtherCollider_, CMovingObject2D OtherMovingObject_)
        {
            var Key = new SContactPoint2D(Collider_, OtherCollider_);
            if (_ContactPoint2Ds.ContainsKey(Key))
            {
                fCollisionStay?.Invoke(Tick_, Normal_, Collider_, OtherCollider_, OtherMovingObject_);
            }
            else
            {
                _ContactPoint2Ds.Add(Key, OtherMovingObject_);
                fCollisionEnter?.Invoke(Tick_, Normal_, Collider_, OtherCollider_, OtherMovingObject_);
            }
        }
        public void NotOverlapped(Int64 Tick_, CCollider2D Collider_, CCollider2D OtherCollider_)
        {
            var Key = new SContactPoint2D(Collider_, OtherCollider_);
            if (_ContactPoint2Ds.ContainsKey(Key))
            {
                fCollisionExit?.Invoke(Tick_, Collider_, OtherCollider_, _ContactPoint2Ds[Key]);
                _ContactPoint2Ds.Remove(Key);
            }
        }
        public void OverlappedCheck(Int64 Tick_, CCollider2D OtherCollider_)
        {
            Collider.OverlappedCheck(Tick_, this, OtherCollider_, null);
        }
        public void OverlappedCheck(Int64 Tick_, CMovingObject2D OtherMovingObject_)
        {
            Collider.OverlappedCheck(Tick_, this, OtherMovingObject_.Collider, OtherMovingObject_);
        }
    }
}