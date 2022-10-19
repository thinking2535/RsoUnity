using System;

namespace rso.physics
{
    public class CMovingObject2D : CObject2D
    {
        public delegate void FFixedUpdate(Int64 Tick_);

        public CCollider2D Collider { get; private set; } = null;
        public SPoint Velocity;

        public CMovingObject2D(STransform Transform_, SPoint Velocity_) :
            base(Transform_)
        {
            Velocity = Velocity_;
        }
        public virtual CPlayerObject2D GetPlayerObject2D()
        {
            return null;
        }
        public FFixedUpdate fFixedUpdate;

        public static void SetColliderToMovingObject2D(CCollider2D Collider_, CMovingObject2D MovingObject_) // C++°ú ÄÚµå ¸ÂÃã
        {
            MovingObject_.Collider = Collider_;
            Collider_.SetParent(MovingObject_);
        }
    }
}
