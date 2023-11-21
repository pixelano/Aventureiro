using System;
using System.Collections;
using System.Collections.Generic;
using teste;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Ageral.triangulador;
using static UnityEngine.GraphicsBuffer;

namespace Ageral
{
    public class gerenciadorDeEstradas : MonoBehaviour
    {
        public float espessura;
        public float escala;
        public List<Vector3> pontosdaEstrada = new List<Vector3>();
        public List<Vector3> pontosAuxiliares = new List<Vector3>();
       
        public void AdicionarVertice()
        {
            pontosdaEstrada.Add(transform.position);
        }
        #region gerarcoisas
        public void gerarEstrada()
        {
            pontosAuxiliares.Clear();
            // criar pontos auxiliares entre A e B
            // angulo formado de A para B ?
         
            Vector3 ultimoPonto = pontosdaEstrada[0];
            for (int y = 0; y < pontosdaEstrada.Count; y++)
            {
                if (y == pontosdaEstrada.Count - 1)
                    break;
                if (y < pontosdaEstrada.Count - 2)
                {
                    Vector3 pontoA = pontosdaEstrada[y];
                    Vector3 pontoB = pontosdaEstrada[y + 1];
                    Vector3 pontoC = pontosdaEstrada[y + 2];
                


                   
                    float orientacao = 0; // reto
                    try
                    {
                        Vector3 eleQuerIr = pontoC - pontoA;

                        orientacao = ValoresUniversais.Orientacao3(pontoA,pontoB,pontoC);
                        Debug.Log("foi "+ orientacao + " em " + y);
                    }
                    catch { }
                    
                    Vector3 Uponto = (pontoA+ pontoA + pontoB) / 3;
                    Vector3 aux_ = Vector3.Cross(pontoA - pontoB, Vector3.up);
                    Uponto += (aux_ * 0.2f) * (orientacao == 1 ? -1 : 1);
                    float distancia_ = Vector3.Distance(pontoA, pontoB);
                    // define se o proximo ponto esta na direita ou esquerda

                   
                    for(int r = 2;r < (distancia_ / escala); r++) {
                      
                        for (int x= 0;x < escala;x++) {
                            pontosAuxiliares.Add(Vector3.Lerp(ultimoPonto,Uponto,x/escala));
                            ultimoPonto = pontosAuxiliares[pontosAuxiliares.Count - 1];
                        }
                        Vector3 aux = Vector3.Cross(pontoA - pontoB, Vector3.up);
                       

                         
                      
                        // reset
                        pontoA = Uponto;
                     
                        Uponto = (pontoA + pontoB) / 2;
                        Uponto += (aux * 0.2f) * (orientacao == 1 ? -1 : 1);
                    }

                }
                else
                {

                  
                    pontosAuxiliares = ValoresUniversais.OptimizePath(pontosAuxiliares, DiminuirEmRetas);
                    Vector3 pontoAB = Vector3.Lerp(pontosdaEstrada[pontosdaEstrada.Count - 1], pontosdaEstrada[pontosdaEstrada.Count-2] ,0.1f);

                    ultimoPonto = Vector3.Lerp(ultimoPonto, pontoAB, 0.5f);

                    pontosAuxiliares.Add(ultimoPonto);



                    

                  //  pontosAuxiliares.Add(pontosdaEstrada[pontosdaEstrada.Count - 1].transform.position);
                }   
                ultimoPonto = pontosAuxiliares[pontosAuxiliares.Count - 1];


            }

            pontosAuxiliares.Add(pontosdaEstrada[pontosdaEstrada.Count - 1] );


            }
        [Range(0,1)]
        public float Suavização;
      
        public Color cor;
        public bool dstvgz,MostrarRastros;
        public float DiminuirEmRetas,AdicionarPoligonosACadaXDistancia;

        public List<Vector3> ordemTrianguo = new List<Vector3>();
        public void renderizarMesh()
        {
            // pegar um ponto
            // adicionar pont de um lado e depous do mesmo lado só que do ponto da frente
            //triangular eles
            //fazer com o outro laddo
            ordemTrianguo.Clear();
         
            for (int x = 0; x < pontosAuxiliares.Count-1; x++)
            {
               
                    Vector3 orientacao = Vector3.Cross(pontosAuxiliares[x + 1] - pontosAuxiliares[x] , Vector3.up) ;
                orientacao.Normalize();
               // Vector3 pontoMeioA = pontosAuxiliares[x];
                Vector3 pontoMeioA = pontosAuxiliares[x];
                Vector3 pontoMeioB = pontosAuxiliares[x+1] ;
                    
                  
                
                // Vector3 pontoA = pontoMeioA - (orientacao ) ;
                Vector3 pontoA = x== 0 ? pontoMeioA - (orientacao) : ordemTrianguo[ordemTrianguo.Count -7 ];
                Vector3 aux = Vector3.zero;
                if (x != 0)
                {
                 aux = ordemTrianguo[ordemTrianguo.Count - 3];
                }
                Vector3 pontoB = pontoMeioB - (orientacao ) ;

                    ordemTrianguo.Add(pontoA);
                    ordemTrianguo.Add(pontoB);
                    ordemTrianguo.Add(pontoMeioA);

                    ordemTrianguo.Add(pontoMeioB);
                    ordemTrianguo.Add(pontoMeioA);
                    ordemTrianguo.Add(pontoB);

                //pontoA = pontoMeioA + (orientacao)  ;

                pontoA =  x == 0 ? pontoMeioA + (orientacao) : aux;
                pontoB = pontoMeioB + (orientacao)  ;

                ordemTrianguo.Add(pontoMeioA);
                ordemTrianguo.Add(pontoMeioB);
                ordemTrianguo.Add(pontoA);

                ordemTrianguo.Add(pontoB);
                ordemTrianguo.Add(pontoA);
                ordemTrianguo.Add(pontoMeioB);

            }
            ordemTrianguo.Reverse();

            for(int x = 0; x < ordemTrianguo.Count; x++)
            {
                ordemTrianguo[x] -= transform.position;
            }
        }

        public Mesh mesh;
        public bool gerarMalha;
        public void criarMalha()
        {
            mesh = new Mesh();

            // Atribuir os vértices à malha
            mesh.vertices = ordemTrianguo.ToArray();

            // Definir os triângulos (assumindo que os vértices estão em grupos de três para formar triângulos)
            int[] triangles = new int[ordemTrianguo.Count];
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = i;
            }

            // Atribuir os triângulos à malha
            mesh.triangles = triangles;

            // Recalcular normais e bounds (opcional, mas geralmente desejável)
              mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            // Atribuir a malha ao componente MeshFilter do GameObject
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = mesh;

            // Atribuir um material (pode ajustar conforme necessário)
            if (gerarMalha)
            {
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    meshRenderer = gameObject.AddComponent<MeshRenderer>();
                }
                meshRenderer.material = new Material(Shader.Find("Standard"));
            }


        }
        public void adicionarPoligonos()
        {
            List<Vector3> auxListaPolig = new List<Vector3>();

            for(int x= 0; x < pontosAuxiliares.Count-1; x++)
            {
                float distAB = Vector3.Distance(pontosAuxiliares[x], pontosAuxiliares[x + 1]);
                auxListaPolig.Add(pontosAuxiliares[x]);
                int repts = (int)(distAB / AdicionarPoligonosACadaXDistancia);
             
                for (int y= 1; y < repts; y++)
                {
                    auxListaPolig.Add(Vector3.Lerp(pontosAuxiliares[x], pontosAuxiliares[x + 1], (float)((float)y/(float) repts)));
                }
            }
            pontosAuxiliares = auxListaPolig;
        }
        public void suavisarMalha()
        {

            for (int x = 1; x < pontosAuxiliares.Count - 1; x++)
            {
                Vector3 AB = Vector3.Lerp(pontosAuxiliares[x - 1], pontosAuxiliares[x], Suavização);
                AB = Vector3.Lerp(AB, pontosAuxiliares[x + 1], Suavização);

                pontosAuxiliares[x] = AB;
            }
        }
        public List<Vector3> suavisarMalha(List<Vector3> lista)
        {

            for (int x = 1; x < lista.Count - 1; x++)
            {
                Vector3 AB = Vector3.Lerp(lista[x - 1], lista[x], Suavização);
                AB = Vector3.Lerp(AB, lista[x + 1], Suavização);

                lista[x] = AB;
            }
            return lista;
        }
        public int QuantidadeDeAmostragem, AnguloAgudo;
        public void suavizacaoAutomatica()
        {
            for (int x = 0; x < pontosAuxiliares.Count - 1; x++)
            {
                List<Vector3> TempVertices = new List<Vector3>();
                for (int z = 0; z < QuantidadeDeAmostragem; z++)
                {
                    if (x + z > pontosAuxiliares.Count - 1)
                        break;
                    TempVertices.Add(pontosAuxiliares[x + z]);
                }
                if (ValoresUniversais.VerificarCurvaAguda(TempVertices, AnguloAgudo))
                {
                    TempVertices = suavisarMalha(TempVertices);

                    for(int z = 0;z < TempVertices.Count - 1; z++)
                    {
                        pontosAuxiliares[x + z] = TempVertices[z];
                    }
                }
            }
        }
    }
    #endregion

    [CustomEditor(typeof(gerenciadorDeEstradas))]
    public class EditorgerenciadorDeEstradas : Editor
    {

        public override void OnInspectorGUI()
        {
            gerenciadorDeEstradas meuScript = (gerenciadorDeEstradas)target;
            base.OnInspectorGUI();


            if (GUILayout.Button("Adicionar Vertice"))
            {

                meuScript.AdicionarVertice();
            }
            if (GUILayout.Button("Gerar estrada"))
            {

                meuScript.gerarEstrada();
            }

            if (GUILayout.Button("desativar guizmo"))
            {

                meuScript.dstvgz = !meuScript.dstvgz;
            }
            if (GUILayout.Button("Gerar Malha"))
            {

                meuScript.renderizarMesh();
                meuScript.criarMalha();

            }
            if (GUILayout.Button("suavizar Malha"))
            {
                meuScript.suavisarMalha();
            }

            if (GUILayout.Button("Adicionar poligonos na Malha"))
            {
                meuScript.adicionarPoligonos();
            }
            if (GUILayout.Button("Suavização automatica"))
            {

                meuScript.suavizacaoAutomatica();
                    }
        }

        private void OnSceneGUI()
        {
            gerenciadorDeEstradas meuScript = (gerenciadorDeEstradas)target;
            if (meuScript.dstvgz)
            {
                for (int x = 0; x < meuScript.pontosdaEstrada.Count; x++)
                {
                    meuScript.pontosdaEstrada[x] = Handles.PositionHandle(meuScript.pontosdaEstrada[x], Quaternion.identity);
                    Handles.color = Color.red;
                    Handles.DrawSolidDisc(meuScript.pontosdaEstrada[x], Vector3.up, 1);

                    if (x < meuScript.pontosdaEstrada.Count - 1)
                    {
                        float dist = Vector3.Distance(meuScript.pontosdaEstrada[x], meuScript.pontosdaEstrada[x + 1]);
                        //    Handles.color = meuScript.cor;
                        Handles.ArrowHandleCap(0, meuScript.pontosdaEstrada[x],
                           Quaternion.LookRotation(meuScript.pontosdaEstrada[x + 1] - meuScript.pontosdaEstrada[x]), dist, EventType.Repaint);
                        // Handles.DrawLine(meuScript.pontosdaEstrada[x], meuScript.pontosdaEstrada[x+1]);
                    }
                }
                for (int x = 0; x < meuScript.pontosAuxiliares.Count; x++)
                {
                    Handles.color = Color.magenta;
                    Handles.DrawSolidDisc(meuScript.pontosAuxiliares[x], Vector3.up, 1);
                }
            }
            if (meuScript.MostrarRastros)
            {
                for(int x =0; x < meuScript.pontosAuxiliares.Count - 1;x++) {
                    Handles.color = Color.red;
                    Handles.DrawLine(meuScript.pontosAuxiliares[x], meuScript.pontosAuxiliares[x + 1]);
                    Handles.color = Color.black;
                    Handles.DrawSolidDisc(meuScript.pontosAuxiliares[x], Vector3.up, 1);


                }

            }



        }
    }
}