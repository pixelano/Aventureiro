using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ItensA;
namespace ATransmut
{
    public class auxiliarBotao : MonoBehaviour
    {
      
        public GeralIten a;
        public bool craft;
        public auxResumo back;

        public void mandarBack()
        {
            back.receba(a,craft);
        }
      
    }
}
