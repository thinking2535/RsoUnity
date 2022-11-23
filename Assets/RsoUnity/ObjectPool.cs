using rso.Base;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace rso.unity
{
    public class CObjectPool
    {
        public struct SIterator
        {
            public string PrefabName;
            public CListB<GameObject>.SIterator Iterator;

            public SIterator(string PrefabName_, CListB<GameObject>.SIterator Iterator_)
            {
                PrefabName = PrefabName_;
                Iterator = Iterator_;
            }
            public GameObject gameObject
            {
                get
                {
                    return Iterator.Data;
                }
            }
        }
        Dictionary<string, CListB<GameObject>> _Pool = new Dictionary<string, CListB<GameObject>>();

        public SIterator New(string PrefabName_, Vector3 LocalPosition_, Transform Parent_)
        {
            var Container = _GetContainer(PrefabName_);
            var Iterator = Container.NewBuf(PrefabName_, LocalPosition_, Parent_);
            Iterator.Data.SetActive(true);

            return new SIterator(PrefabName_, Iterator);
        }
        public void Delete(SIterator Iterator_)
        {
            Iterator_.gameObject.SetActive(false);
            _Pool[Iterator_.PrefabName].Remove(Iterator_.Iterator);
        }
        public void Reserve(string PrefabName_, Transform Parent_, Int32 Size_)
        {
            var Container = _GetContainer(PrefabName_);

            for (Int32 i = 0; i < Size_; ++i)
            {
                var Obj = Container.ReserveBuf(PrefabName_, new Vector3(), Parent_);
                Obj.SetActive(false);
            }
        }
        CListB<GameObject> _GetContainer(string PrefabName_)
        {
            CListB<GameObject> Container;

            if (!_Pool.TryGetValue(PrefabName_, out Container))
            {
                Container = new CListB<GameObject>(
                        (object[] Params_) =>
                        {
                            return (GameObject)UnityEngine.Object.Instantiate(Resources.Load((string)Params_[0]), (Vector3)Params_[1], Quaternion.identity, (Transform)Params_[2]);
                        },
                        (GameObject GameObject_, object[] Params_) =>
                        {
                            GameObject_.transform.localPosition = (Vector3)Params_[1];
                            GameObject_.transform.SetParent((Transform)Params_[2]);
                        });

                _Pool.Add(PrefabName_, Container);
            }

            return Container;
        }
    }
}
