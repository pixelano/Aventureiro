using Codice.CM.Common.Replication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ageral;
namespace montros
{
    public class Alcateia : MonoBehaviour
    {
       public List<unidade> frontLine = new List<unidade>();
        //  public List<unidade> backLine = new List<unidade>();
        public List<monstros_> lista;
        [System.Serializable]

        public struct monstros_
        {
            public GameObject modelo;
            public int quantidade;
        }
        [System.Serializable]
        public class unidade {
            // tem que multiplicar pela distancia e dps adicionar a V3 do alvo
            public Vector3 posicao;
            public MovimentacaoAEstrela movimento;
            public GerenciadorDeAtaques atk;
            public GerenciadoDeVida alvo;

            public unidade()
            {
                try
                {
                    atk = movimento.GetComponent<GerenciadorDeAtaques>();
                }
                catch { }
            }
            public unidade(GameObject aux)
            {
                try
                {
                    movimento = aux.GetComponent<MovimentacaoAEstrela>();
                    atk = aux.GetComponent<GerenciadorDeAtaques>();
                }
                catch { }
            }

            public void mover()
            {
                if (alvo == null) {
                    movimento.movimentarParaS((posicao));
                }
                else
                {
                  
                        movimento.movimentarParaS((posicao * atk.distancia) + alvo.transform.position);
                        movimento.transform.localRotation = Quaternion.LookRotation(alvo.transform.position - movimento.transform.position);

                        if (movimento.chegou)
                        {
                            atk.atacar(alvo);
                        }
                    
                }
            }
        }
        public GerenciadoDeVida alvo;
         float distancia;
        bool flag_ = false;

        private void Start()
        {
           
            Vector3 aux = transform.position;
            aux.y = 0;
          //  transform.position = aux;

            for(int x = 0; x < lista.Count; x++) { 
            for(int y=0; y < lista[x].quantidade; y++)
                {
                    GameObject aux_ = Instantiate(lista[x].modelo, transform.position, Quaternion.identity,transform);
                    frontLine.Add(new unidade(aux_));
                }
            }
         
        }
        public float distanciaDaMatilha;
        private void Update()
        {
            frontLine.RemoveAll(x => x.movimento == null);
            if (Vector3.Distance(transform.position, alvo.transform.position) < distanciaDaMatilha *10 || flag_ )
            {
                if (frontLine.Count > 0)
                {
                    flag_ = true;
                    distancia = frontLine.Count;
                    int inicialFront = 1;
                    int quantidadePorLado = (int)(frontLine.Count / 2);



                    frontLine[0].posicao = (alvo.transform.forward);
                    frontLine[0].alvo = alvo;


                    float anguloCada = 90f / quantidadePorLado + inicialFront; // Corrigindo o c�lculo do �ngulo em graus

                    for (int x = -1; x < 2; x += 2)
                    {
                        for (int y = inicialFront; y < quantidadePorLado + inicialFront; y++)
                        {
                            float angulo = y * anguloCada * Mathf.Deg2Rad; // Convertendo o �ngulo para radianos
                            float sen = Mathf.Cos(angulo - 90);
                            float cos = Mathf.Sin(angulo - 90);

                            float offsetX = cos * x;
                            float offsetY = sen;

                            Vector3 posi = alvo.transform.TransformDirection(new Vector3(offsetX, 0, offsetY));

                            posi.y = 0;
                            try
                            {
                                frontLine[((x < 0 ? 0 : 1) * quantidadePorLado) + y].posicao = posi;
                                frontLine[((x < 0 ? 0 : 1) * quantidadePorLado) + y].alvo = alvo;
                            }
                            catch
                            {
                                frontLine[frontLine.Count - inicialFront].posicao = posi;
                                frontLine[frontLine.Count - inicialFront].alvo = alvo;
                                continue;
                            }
                        }
                    }



                    frontLine.ForEach(x => x.mover());



                }
            }
            else
            {
               

                    frontLine.ForEach(x => x.posicao = new Vector3(Random.Range(-distanciaDaMatilha, distanciaDaMatilha),
                         0, Random.Range(-distanciaDaMatilha, distanciaDaMatilha)));

                frontLine.ForEach(x => x.alvo = null);
                frontLine.ForEach(x => x.mover());

            }
          
        }

    }
}
