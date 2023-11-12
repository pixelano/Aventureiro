using PlasticGui.WorkspaceWindow.Home.Repositories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ageral;

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

        private void Update()
        {
           
        }
        public bool podeAtacar()
        {
            bool resposta = false;

           
             
                if (distancia <= ataqueAtual.distanciaAtaque)
                {
                    resposta = true;
                   
                }
         

         

            return resposta ;
        }
        float tempoDeCast=0;
public void executarAtaque()
        {
            if (tempoDeCast >  ataqueAtual.tempoDeCast) {
            
           
                tempoDeCast = 0;
                ultimoAtaque = Time.time;
                ataqueAtual = null;
                SortearAtaque();

                alvo.diminuirVida(ataqueAtual.dano);
            }
            else
            {
                tempoDeCast += Time.deltaTime;
            }
        }
        GerenciadoDeVida alvo;
        public void atacar(GerenciadoDeVida alvo_)
        {
            alvo = alvo_;
          

            if (Time.time > ultimoAtaque + ataqueAtual.tempoDeRecarga)
            {
                distancia = ataqueAtual.distanciaAtaque;
                if (podeAtacar())
                {
                    executarAtaque();


                }
            }
            else
            {
                distancia = distancia_;
            }
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
