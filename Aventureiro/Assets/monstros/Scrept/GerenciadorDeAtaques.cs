using PlasticGui.WorkspaceWindow.Home.Repositories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ageral;
using PlasticGui.WorkspaceWindow.Replication;

namespace montros
{
    public class GerenciadorDeAtaques : MonoBehaviour
    {
        public float distancia;
        float distancia_;
        public List<SAtaques> lista;
        SAtaques ataqueAtual;
        float ultimoAtaque;
        public bool liberado;
       
        public void Awake()
        {
            distancia_ = distancia;
            distancia = lista[0].distanciaAtaque;
         
            ataqueAtual = lista[0];
          
         
        }

      
        public bool podeAtacar()
        {
            bool resposta = false;


            float aux_f = Vector3.Distance(transform.position, alvo.transform.position);
            Debug.Log(aux_f);
                if (aux_f <= ataqueAtual.distanciaAtaque + 0.6f)
                {
                    resposta = true;
                   
                }
         

         

            return resposta ;
        }
       
        public bool atacando;
public void executarAtaque()
        {

           
           
                ultimoAtaque = Time.time;
                ataqueAtual = null;
                SortearAtaque();
                atacando = false;
                alvo.diminuirVida(ataqueAtual.dano);
                distancia = distancia_;
          
        }
        GerenciadoDeVida alvo;
        public bool aux_caminhando;
        bool flag_;
        public bool atacar(GerenciadoDeVida alvo_)
        {
            alvo = alvo_;
          

            if ( Time.time > ultimoAtaque + ataqueAtual.tempoDeRecarga)
            {
                if (podeAtacar())
                {
                    atacando = true;

                    // executarAtaque();
                    return true;

                }

                aux_caminhando = false;
                atacando = false;
                distancia = 1;
              

            }
            else
            {
                atacando = false;
                distancia = distancia_;
            }
            return false;
        }
        public void SortearAtaque()
        {if (ataqueAtual == null)
            {
                if (lista.Count > 0)
                {
                    ataqueAtual = lista[Random.Range(0, lista.Count)];
                }
            }
        }

    }
}
