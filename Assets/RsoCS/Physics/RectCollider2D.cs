using System;

namespace rso.physics
{
    public class CRectCollider2D : CCollider2D
    {
        SRectCollider2D _Collider = null;
        public SRect Rect
        {
            get
            {
                return _Collider.ToRect().Multi(LocalScale).Add(Position);
            }
        }

        public CRectCollider2D(STransform Transform_, Int32 Number_, SRectCollider2D Collider_) :
            base(Transform_, Number_)
        {
            _Collider = Collider_;
        }
        public void SetSize(SPoint Size_)
        {
            _Collider.Size.Set(Size_);
        }
        public void SetSizeX(float X_)
        {
            _Collider.Size.X = X_;
        }
        public void SetSizeY(float Y_)
        {
            _Collider.Size.Y = Y_;
        }
        SPoint _OverlappedCheck(CPlayerObject2D PlayerObject_, SRect Rect_, SRect RectOther_)
        {
            if (!CPhysics.IsOverlappedRectRect(Rect_, RectOther_))
                return null;

            var rl = RectOther_.Right - Rect_.Left;
            var lr = Rect_.Right - RectOther_.Left;
            var tb = RectOther_.Top - Rect_.Bottom;
            var bt = Rect_.Top - RectOther_.Bottom;

            var Normal = new SPoint();

            if (rl < lr) // Normal.X : +
            {
                if (tb < bt) // Normal.Y : +
                {
                    if (rl < tb) // select Normal.X
                    {
                        if (rl > CEngine.ContactOffset)
                            PlayerObject_.LocalPosition.X += (rl - CEngine.ContactOffset);

                        Normal.X = 1.0f;
                    }
                    else // select Normal.Y
                    {
                        if (tb > CEngine.ContactOffset)
                            PlayerObject_.LocalPosition.Y += (tb - CEngine.ContactOffset);

                        Normal.Y = 1.0f;
                    }
                }
                else // Normal.Y : -
                {
                    if (rl < bt) // select Normal.X
                    {
                        if (rl > CEngine.ContactOffset)
                            PlayerObject_.LocalPosition.X += (rl - CEngine.ContactOffset);

                        Normal.X = 1.0f;
                    }
                    else // select Normal.Y
                    {
                        if (bt > CEngine.ContactOffset)
                            PlayerObject_.LocalPosition.Y += (CEngine.ContactOffset - bt);

                        Normal.Y = -1.0f;
                    }
                }
            }
            else // Normal.X : -
            {
                if (tb < bt) // Normal.Y : +
                {
                    if (lr < tb) // select Normal.X
                    {
                        if (lr > CEngine.ContactOffset)
                            PlayerObject_.LocalPosition.X += (CEngine.ContactOffset - lr);

                        Normal.X = -1.0f;
                    }
                    else // select Normal.Y
                    {
                        if (tb > CEngine.ContactOffset)
                            PlayerObject_.LocalPosition.Y += (tb - CEngine.ContactOffset);

                        Normal.Y = 1.0f;
                    }
                }
                else // Normal.Y : -
                {
                    if (lr < bt) // select Normal.X
                    {
                        if (lr > CEngine.ContactOffset)
                            PlayerObject_.LocalPosition.X += (CEngine.ContactOffset - lr);

                        Normal.X = -1.0f;
                    }
                    else // select Normal.Y
                    {
                        if (bt > CEngine.ContactOffset)
                            PlayerObject_.LocalPosition.Y += (CEngine.ContactOffset - bt);

                        Normal.Y = -1.0f;
                    }
                }
            }

            return Normal;
        }
        public override void OverlappedCheck(Int64 Tick_, CMovingObject2D MovingObject_, CCollider2D OtherCollider_, CMovingObject2D OtherMovingObject_)
        {
            if (!LocalEnabled)
                return;

            OtherCollider_.OverlappedCheck(Tick_, OtherMovingObject_, this, MovingObject_);
        }
        SPoint _OverlappedCheck(CRectCollider2D OtherRectCollider_, CPlayerObject2D PlayerObject_, CPlayerObject2D OtherPlayerObject_)
        {
            if (!LocalEnabled)
                return null;

            SPoint Normal;

            if (PlayerObject_ != null)
            {
                Normal = _OverlappedCheck(PlayerObject_, Rect, OtherRectCollider_.Rect);
                if (Normal == null)
                    return null;
            }
            else if (OtherPlayerObject_ != null)
            {
                Normal = _OverlappedCheck(OtherPlayerObject_, OtherRectCollider_.Rect, Rect);
                if (Normal == null)
                    return null;

                Normal.Multi(-1.0f);
            }
            else
            {
                return null;
            }

            return Normal;
        }
        public override void OverlappedCheck(Int64 Tick_, CMovingObject2D MovingObject_, CRectCollider2D OtherRectCollider_, CMovingObject2D OtherMovingObject_)
        {
            CPlayerObject2D PlayerObject = MovingObject_?.GetPlayerObject2D();
            CPlayerObject2D OtherPlayerObject = OtherMovingObject_?.GetPlayerObject2D();

            var Normal = _OverlappedCheck(OtherRectCollider_, PlayerObject, OtherPlayerObject);
            if (Normal != null)
            {
                PlayerObject?.Overlapped(Tick_, Normal, this, OtherRectCollider_, OtherMovingObject_);
                OtherPlayerObject?.Overlapped(Tick_, Normal.Multi(-1.0f), OtherRectCollider_, this, MovingObject_);
            }
            else
            {
                PlayerObject?.NotOverlapped(Tick_, this, OtherRectCollider_);
                OtherPlayerObject?.NotOverlapped(Tick_, OtherRectCollider_, this);
            }
        }
        public override bool IsOverlapped(Int64 Tick_, CCollider2D OtherCollider_)
        {
            return OtherCollider_.IsOverlapped(Tick_, this);
        }
        public override bool IsOverlapped(Int64 Tick_, CRectCollider2D OtherRectCollider_)
        {
            return (Enabled && OtherRectCollider_.Enabled && CPhysics.IsOverlappedRectRect(Rect, OtherRectCollider_.Rect));
        }
    }
}
