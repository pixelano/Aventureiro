using ATransmut;
using ItensA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JogadorA
{
    public class Comprar_CrafitarItem : MonoBehaviour
    {
     
        public auxResumo back;

        public void comprar()
        {
            if (back.data)
            {
                bool aux = true;
                if (back.craft)
                {
                    foreach (var a in back.listareceita)
                    {
                        if (!Inventa.instance.temecItem(a.data, a.quantidade))
                        {
                            aux = false;
                            break;
                        }
                    }

                }
                else
                {
                    if (Inventa.instance.dinheiro < back.data.valor)
                        aux = false;
                }

                if (aux)
                {
                    Inventa.instance.obterItem(back.data, back.craft);
                }
            }
        }
    }
}
