using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace montros
{
    public class GerenciadorDeAtaques : MonoBehaviour
    {
        public bool liberado;
        public float distancia;
       public List<SAtaques> lista;
        SAtaques ataqueAtual;
      public bool podeAtacar()
        {
            bool resposta = false ;


            liberado = resposta;
            return resposta ;
        }
        public void atacar()
        {
            if (podeAtacar())
            {


                SortearAtaque();
            }
        }
        public void SortearAtaque()
        {
            if(lista.Count > 0)
            {
                ataqueAtual = lista[Random.Range(0,lista.Count)];
            }
        }

    }
}
