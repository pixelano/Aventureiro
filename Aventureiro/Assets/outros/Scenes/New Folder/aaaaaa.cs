using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace teste
{
    public class aaaaaa : MonoBehaviour
    {
        [SerializeField]
        public static aaaaaa oi;
  
        private void Awake()
        {
            if(oi == null)
            {
                Debug.Log("definio  " +transform.name);
                oi = this;
            }
            else
            {
                Debug.Log("destruiu  " + transform.name);
                Destroy(this);
            }
        }
    }
}
