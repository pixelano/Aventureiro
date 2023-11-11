using Codice.CM.Common.Replication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace montros
{
    public class Alcateia : MonoBehaviour
    {
       public List<unidade> frontLine = new List<unidade>();
      //  public List<unidade> backLine = new List<unidade>();
        [System.Serializable]
          public class unidade {
            // tem que multiplicar pela distancia e dps adicionar a V3 do alvo
            public Vector3 posicao;
            public MovimentacaoAEstrela movimento;
            public GerenciadorDeAtaques atk;
            public GameObject alvo;
          
         
            public void mover()
            {
                movimento.movimentarPara((posicao * atk.distancia )+ alvo.transform.position);
                movimento.transform.localRotation = Quaternion.LookRotation(alvo.transform.position - movimento.transform.position);
                atk.liberado = movimento.chegou;
            }
        }
        public GameObject alvo;
         float distancia;

        private void Start()
        {
            frontLine.ForEach(x => x.atk = x.movimento.GetComponent<GerenciadorDeAtaques>());
         
        }
        private void Update()
        {
            if(frontLine.Count > 0)
            {
                distancia = frontLine.Count;
                int inicialFront = 1;
                int quantidadePorLado = (int)(frontLine.Count / 2);
                
                   
                
                    frontLine[0].posicao =  (alvo.transform.forward );
                frontLine[0].alvo = alvo;
              
               
                    float anguloCada = 90f / quantidadePorLado + inicialFront; // Corrigindo o cálculo do ângulo em graus
                  
                    for (int x = -1; x < 2; x += 2)
                    {
                    for (int y = inicialFront; y < quantidadePorLado + inicialFront; y++)
                    {
                        float angulo = y * anguloCada * Mathf.Deg2Rad; // Convertendo o ângulo para radianos
                        float sen = Mathf.Cos(angulo - 90);
                        float cos = Mathf.Sin(angulo - 90);

                        float offsetX = cos  * x;
                        float offsetY = sen ;

                        Vector3 posi = alvo.transform.TransformDirection(new Vector3(offsetX, 0, offsetY));
                        
                        posi.y = 0;
                        try
                        {
                            frontLine[((x < 0 ? 0 : 1) * quantidadePorLado) + y].posicao= posi ;
                            frontLine[((x < 0 ? 0 : 1) * quantidadePorLado) + y].alvo = alvo;
                        }
                        catch
                        {
                            frontLine[frontLine.Count-inicialFront].posicao= posi ;
                            frontLine[frontLine.Count - inicialFront].alvo = alvo;
                            continue;
                        }
                    }
                    }



                frontLine.ForEach(x => x.mover());

                  
                
            }
        }

    }
}
