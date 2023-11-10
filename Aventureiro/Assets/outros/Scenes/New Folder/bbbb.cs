using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace teste
{
    public class bbbb : MonoBehaviour
    {
        public aaaaaa teste_;
        void Start()
        {
           gameObject.AddComponent<aaaaaa>();
            teste_ = aaaaaa.oi;

            GameObject.CreatePrimitive(PrimitiveType.Cube) ;
        }

    }
}
