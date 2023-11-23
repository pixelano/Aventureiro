using Codice.CM.Common.Replication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ageral;
using UnityEngine.Animations;
using System.Security.Cryptography;

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
            public GerenciadoDeAnimação anima;
         
            public bool EmAlerta;

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
                    anima = aux.GetComponent<GerenciadoDeAnimação>();
                 
                

                    
                }
                catch { }
            }

            public void mover()
            {
                
                anima.executar(posicao, movimento, atk, alvo,EmAlerta);

               
            }
        }
        public GerenciadoDeVida alvo;
        public CriadorDeAreas cra;

        bool flag_alerta;
    
        MeshFilter meshFilter;
        bool aux_iniciador = false;
  bool alvoNaArea()
        {
            
        if(meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
            }

                MeshCollider tempCollider = gameObject.AddComponent<MeshCollider>();
                tempCollider.sharedMesh = meshFilter.sharedMesh;

                Vector3 pontoMaisProximo = tempCollider.ClosestPointOnBounds(alvo.transform.position);
            pontoMaisProximo.y = 0;
            Vector3 auxJ = alvo.transform.position;
            auxJ.y = 0;
                Destroy(tempCollider);

          
                // Agora você pode verificar se o jogador está dentro da área demarcada pela mesh
                if (Vector3.Distance(auxJ, pontoMaisProximo) < 0.1f)
                {
                return true;
                }
                else
                {
                return false;

            }
           
        }
        private void Update()
        {
            if (!aux_iniciador)
            {
                if (cra.mesh != null)
                {
                    aux_iniciador = true;
                    if (frontLine.Count <= 0)
                    {
                        Vector3 aux = transform.position;
                        aux.y = 0;
                        //  transform.position = aux;

                        for (int x = 0; x < lista.Count; x++)
                        {
                            for (int y = 0; y < lista[x].quantidade; y++)
                            {
                                GameObject aux_ = Instantiate(lista[x].modelo,Vector3.Lerp( ManipulacaoDeMalha.GetRandomPositionInMesh(cra.mesh) + transform.position,transform.position,0.5f), Quaternion.Euler(Vector3.up * Random.Range(-90,90)), transform);
                                frontLine.Add(new unidade(aux_));
                            }
                        }
                    }
                    if (!alvo)
                    {
                        Debug.LogError("faltou definir o lavo");
                    }
                }
            }
            else
            {


                frontLine.RemoveAll(x => x.movimento == null);
                if (alvoNaArea())
                {
                  
                    if (frontLine.Count > 0)
                    {

                    
                        int quantidadePorLado = (int)(frontLine.Count / 2);



                        frontLine[0].posicao = (alvo.transform.forward);
                        frontLine[0].alvo = alvo;


                        float anguloCada = 90f / quantidadePorLado+2 ; // Corrigindo o cálculo do ângulo em graus

                        for (int x = -1; x < 2; x += 2)
                        {
                            for (int y = 0; y < quantidadePorLado ; y++)
                            {
                                if (frontLine[((x < 0 ? 0 : 1) * quantidadePorLado) + y].anima == null)
                                {
                                    continue;
                                }

                                if (Vector3.Distance(frontLine[((x < 0 ? 0 : 1) * quantidadePorLado) + y].movimento.transform.position, alvo.transform.position) < 20)
                                {


                                    float angulo = y * anguloCada * Mathf.Deg2Rad; // Convertendo o ângulo para radianos
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
                                        frontLine[frontLine.Count - 1].posicao = posi;
                                        frontLine[frontLine.Count - 1].alvo = alvo;
                                        continue;
                                    }
                                }
                            }
                        }



                        frontLine.ForEach(x => x.mover());

                        if (!flag_alerta)
                        {
                            frontLine.ForEach(x => x.EmAlerta = true);
                        }
                        flag_alerta = true;
                    }
                }
                else
                {


                    frontLine.ForEach(x => x.posicao = ManipulacaoDeMalha.GetRandomPositionInMesh(cra.mesh));
                    if (flag_alerta)
                    {
                        frontLine.ForEach(x => x.EmAlerta = false);
                        flag_alerta = false;
                    }
                    frontLine.ForEach(x => x.alvo = null);
                    frontLine.ForEach(x => x.mover());

                }

            }
        }

    }
}
