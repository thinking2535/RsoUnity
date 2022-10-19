using rso.Base;
using rso.gameutil;
using System;
using System.Collections.Generic;

namespace rso.physics
{
    public class CEngine
    {
        protected Int64 _NetworkTickSync;
        protected CTick _CurTick = null;
        public Int64 Tick { get; private set; } = 0; // 물리 처리가 끝난 시점의 Tick

        public static Single ContactOffset { get; private set; }
        public static Int32 FPS { get; private set; }
        public static Int64 UnitTick { get; private set; }
        public static Single DeltaTime { get; private set; }

        public CList<CCollider2D> Objects { get; private set; } = new CList<CCollider2D>();
        public CList<CMovingObject2D> MovingObjects { get; private set; } = new CList<CMovingObject2D>();
        public List<CPlayerObject2D> Players { get; private set; } = new List<CPlayerObject2D>();

        public CEngine(Int64 NetworkTickSync_, Int64 CurTick_, Single ContactOffset_, Int32 FPS_)
        {
            _NetworkTickSync = NetworkTickSync_;
            _CurTick = new CTick(CurTick_);
            Tick = CurTick_;
            ContactOffset = ContactOffset_;
            FPS = FPS_;
            UnitTick = 10000000 / FPS;
            DeltaTime = 1.0f / FPS;
        }
        protected void _Update(Int64 ToTick_)
        {
            for (; Tick < ToTick_; Tick += UnitTick)
            {
                foreach (var i in MovingObjects)
                    i.fFixedUpdate?.Invoke(Tick);

                foreach (var p in Players)
                    p.fFixedUpdate?.Invoke(Tick);

                for (Int32 pi = 0; pi < Players.Count - 1; ++pi)
                {
                    for (Int32 ti = pi + 1; ti < Players.Count; ++ti)
                        Players[pi].OverlappedCheck(Tick, Players[ti]);
                }

                foreach (var p in Players)
                {
                    for (var it = MovingObjects.Begin(); it;)
                    {
                        var itCheck = it;
                        it.MoveNext();

                        // 여기에서 itCheck을 삭제할 수 있음
                        p.OverlappedCheck(Tick, itCheck.Data);
                    }

                    for (var it = Objects.Begin(); it;)
                    {
                        var itCheck = it;
                        it.MoveNext();

                        // 여기에서 itCheck을 삭제할 수 있음
                        p.OverlappedCheck(Tick, itCheck.Data);
                    }
                }

                fFixedUpdate?.Invoke(Tick);
            }
        }
        public CList<CCollider2D>.SIterator AddObject(CCollider2D Object_)
        {
            return Objects.Add(Object_);
        }
        public void RemoveObject(CList<CCollider2D>.SIterator Iterator_)
        {
            Iterator_.Data.LocalEnabled = false;
            Objects.Remove(Iterator_);
        }
        public CList<CMovingObject2D>.SIterator AddMovingObject(CMovingObject2D Object_)
        {
            return MovingObjects.Add(Object_);
        }
        public void RemoveMovingObject(CList<CMovingObject2D>.SIterator Iterator_)
        {
            Iterator_.Data.Collider.LocalEnabled = false;
            MovingObjects.Remove(Iterator_);
        }
        // RemovePlayer 는 추가하지 말것
        // 외부에서 등록한 Collision 콜백을 호출하는데 거기에서 RemovePlayer 할 경우 OtherPlayer에 대한 CollisionCallback을 호출 할 수 없음
        public void AddPlayer(CPlayerObject2D Player_)
        {
            Players.Add(Player_);
        }
        public void Start()
        {
            _CurTick.Start();
        }
        public void Stop()
        {
            _CurTick.Stop();
        }
        public bool IsStarted()
        {
            return _CurTick.IsStarted();
        }
        public Action<Int64> fFixedUpdate;
    }
}