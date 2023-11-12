using ATransmut;
using ItensA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JogadorA
{
    public class Comprar_CrafitarItem : MonoBehaviour
    {
        public GeralIten data;
    
     
        public void aux_inst(auxiliarBotao a)
        {
            data = a.a;




            Inventa.solicitado(a.a, a.craft);
        }
    }
}
