using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Main : MonoBehaviour
{
    Test t;
    void Awake()
    {
        Test tt = UnityEngine.Object.Instantiate(t);
    }
}
