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
            public Vector3 posicao;
            public MovimentacaoAEstrela movimento;
        }
        public GameObject alvo;
         float distancia;
        public void mover(unidade a , Vector3 b)
        {
            a.movimento.movimentarPara( b);
            a.movimento.transform.localRotation = Quaternion.LookRotation(alvo.transform.position - a.movimento.transform.position);
                }
        private void Update()
        {
            if(frontLine.Count > 0)
            {
                distancia = frontLine.Count;
                int inicialFront = 0;
                int quantidadePorLado = (int)(frontLine.Count / 2);
                if (frontLine.Count % 2 == 1)
                {
                    inicialFront = 1;
                    quantidadePorLado = (int)((frontLine.Count -1) / 2);
                    mover(frontLine[0], alvo.transform.position + (alvo.transform.forward * distancia));
                }
                if (inicialFront != 0)
                {
                    float anguloCada = 90f / quantidadePorLado +1; // Corrigindo o cálculo do ângulo em graus
                  
                    for (int x = -1; x < 2; x += 2)
                    {
                        for (int y = 1; y < quantidadePorLado + 1; y++)
                        {
                            float angulo = y * anguloCada * Mathf.Deg2Rad; // Convertendo o ângulo para radianos
                            float sen = Mathf.Cos(angulo - 90);
                            float cos = Mathf.Sin(angulo - 90);

                            float offsetX = cos * distancia * x;
                            float offsetY = sen * distancia ;

                            Vector3 posi = alvo.transform.TransformDirection(new Vector3(offsetX, 0, offsetY));
                            posi.y = 0;
                            mover(frontLine[((x < 0 ? 0 : 1) * quantidadePorLado) + y], posi + alvo.transform.position);
                        }
                    }
                }
               
              
                

                  
                
            }
        }

    }
}
