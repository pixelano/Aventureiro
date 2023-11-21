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
        public List<GameObject> pontosdaEstrada = new List<GameObject>();
        public List<Vector3> pontosAuxiliares = new List<Vector3>();
        public List<Vector3> visualizar = new List<Vector3>();
        public void AdicionarVertice()
        {
            visualizar.Add(transform.position);
        }
        #region gerarcoisas
        public void gerarEstrada()
        {
            pontosAuxiliares.Clear();
            // criar pontos auxiliares entre A e B
            // angulo formado de A para B ?
            /*
            for (int y = 1; y < pontosdaEstrada.Count; y++)
            {
                if (y == pontosdaEstrada.Count - 1)
                    break;

                Vector3 MinhaOri = pontosdaEstrada[y ] - pontosdaEstrada[y -1];
                Vector3 queroIr = pontosdaEstrada[y + 1] - pontosdaEstrada[y];
               
                float angulo = Vector3.Angle(queroIr, MinhaOri) * Mathf.Deg2Rad;

                Vector3 dire = MinhaOri;
                float distancia = Vector3.Distance(pontosdaEstrada[y], pontosdaEstrada[y + 1]);
                float direc_X = 1, direc_Z = 1;
                float ori = 0;
                if (y <  pontosdaEstrada.Count - 2)
                {
                    Vector3 eleQuerIr = pontosdaEstrada[y + 2] - pontosdaEstrada[y + 1];

                    ori =  ValoresUniversais.Orientacao(Vector3.zero, MinhaOri, eleQuerIr);

                }
                Vector3 direcaoAB = pontosdaEstrada[y+1] - pontosdaEstrada[y];


                // Calcula um vetor perpendicular (para a esquerda) usando o produto vetorial
                Vector3 direcaoEsquerda = Vector3.Cross(direcaoAB.normalized, Vector3.up).normalized;
            

            
                for (float x = 0; x < distancia; x += escala + x/distancia)
                {
                           float sen = Mathf.Sin(angulo) * x *  x/distancia;
                           float cos = Mathf.Cos(angulo) * x*  x/distancia;
                           Vector3 irp = new Vector3(sen * direc_X,0,cos * queroIr.z) ;
                  
                    if (ori == 1) {
                        irp = irp + direcaoEsquerda/2 ;
                    } else
                    {
                        irp = irp - direcaoEsquerda/2;
                    }
                    dire = Vector3.Lerp(pontosdaEstrada[y] + irp, queroIr + pontosdaEstrada[y], x / distancia);

                    pontosAuxiliares.Add(dire) ;
                }
            }
            pontosAuxiliares = ValoresUniversais.OptimizePath(pontosAuxiliares);
            // resumir elas
            // adicionar a espessura da estrada seguindo a orientação
            // triangular de forma simples
            /*
             * 
             * para saber para que lado a curva tem que fazer é baseado para qual lado o vertice V+1 para V+2 aponta em relação a             
             * V para V+1 com isso descobri sé para a esquerda ou para a direita
             */
            if (visualizar.Count != pontosdaEstrada.Count)
            {
                for (int y = 0; y < pontosdaEstrada.Count; y++)
                {
                    DestroyImmediate(pontosdaEstrada[y]);
                }
                pontosdaEstrada.Clear();
                for (int y = 0; y < visualizar.Count; y++)
                {
                    GameObject aux = new GameObject("");
                    pontosdaEstrada.Add(aux);
                }
            }

                    for (int y = 0; y < visualizar.Count; y++)
                {
                      GameObject aux = pontosdaEstrada[y];
                    aux.transform.parent = transform;
                    aux.transform.position = visualizar[y];
                  

                }
            
            for (int y = 0; y < pontosdaEstrada.Count; y++)
            {
                if (y == pontosdaEstrada.Count - 1) {
                   // pontosdaEstrada[y].transform.LookAt(pontosdaEstrada[y ].transform);
                }
                else
                {
                    pontosdaEstrada[y].transform.LookAt(pontosdaEstrada[y + 1].transform);
                }
            }


            Vector3 ultimoPonto = pontosdaEstrada[0].transform.position; ;
            for (int y = 0; y < pontosdaEstrada.Count; y++)
            {
                if (y == pontosdaEstrada.Count - 1)
                    break;
                if (y < pontosdaEstrada.Count - 2)
                {
                    Vector3 pontoA = pontosdaEstrada[y].transform.position;
                    Vector3 pontoB = pontosdaEstrada[y + 1].transform.position;
                    Vector3 pontoC = pontosdaEstrada[y + 2].transform.position;
                


                    float Distancia_ = Vector3.Distance(pontoA, pontoB);
                    float orientacao = 0; // reto
                    try
                    {
                        Vector3 eleQuerIr = pontoC - pontoA;

                        orientacao = ValoresUniversais.Orientacao(pontoA,pontoB,pontoC);
                        Debug.Log("foi em " + y);
                    }
                    catch { }
                    
                    Vector3 Uponto = (pontoA+ pontoA + pontoB) / 3;
                    Vector3 aux_ = Vector3.Cross(pontoA - pontoB, Vector3.up);
                    Uponto += (aux_ * 0.2f) * (orientacao == 1 ? -1 : 1);
                    float distancia_ = Vector3.Distance(pontoA, pontoB);
                    // define se o proximo ponto esta na direita ou esquerda

                   
                    for(int r = 1;r < distancia_ / escala; r++) {
                      
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

                  
                    pontosAuxiliares = ValoresUniversais.OptimizePath(pontosAuxiliares, tolerancia);
                    Vector3 pontoAB = Vector3.Lerp(pontosdaEstrada[pontosdaEstrada.Count - 1].transform.position, pontosdaEstrada[pontosdaEstrada.Count-2].transform.position ,0.1f);

                    ultimoPonto = Vector3.Lerp(ultimoPonto, pontoAB, 0.5f);

                    pontosAuxiliares.Add(ultimoPonto);



                    

                  //  pontosAuxiliares.Add(pontosdaEstrada[pontosdaEstrada.Count - 1].transform.position);
                }   
                ultimoPonto = pontosAuxiliares[pontosAuxiliares.Count - 1];


            }

            pontosAuxiliares.Add(pontosdaEstrada[pontosdaEstrada.Count - 1].transform.position );



            }
        public Color cor;
        public bool dstvgz,MostrarRastros;
        public float tolerancia;

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
                    Vector3 pontoMeioA = pontosAuxiliares[x] ;
                    Vector3 pontoMeioB = pontosAuxiliares[x+1] ;
                    
                    Vector3 pontoA = pontoMeioA - (orientacao ) ;
                    Vector3 pontoB = pontoMeioB - (orientacao ) ;

                    ordemTrianguo.Add(pontoA);
                    ordemTrianguo.Add(pontoB);
                    ordemTrianguo.Add(pontoMeioA);

                    ordemTrianguo.Add(pontoMeioB);
                    ordemTrianguo.Add(pontoMeioA);
                    ordemTrianguo.Add(pontoB);

                 pontoA = pontoMeioA + (orientacao)  ;
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

                meuScript.dstvgz =!meuScript.dstvgz;
            }
            if (GUILayout.Button("Gerar Malha"))
            { 
            
                meuScript.renderizarMesh();
                meuScript.criarMalha();

            }
            }

        private void OnSceneGUI()
        {
            gerenciadorDeEstradas meuScript = (gerenciadorDeEstradas)target;
            if (meuScript.dstvgz)
            {
                for (int x = 0; x < meuScript.visualizar.Count; x++)
                {
                    meuScript.visualizar[x] = Handles.PositionHandle(meuScript.visualizar[x], Quaternion.identity);
                    Handles.color = Color.red;
                    Handles.DrawSolidDisc(meuScript.visualizar[x], Vector3.up, 1);

                    if (x < meuScript.visualizar.Count - 1)
                    {
                        float dist = Vector3.Distance(meuScript.visualizar[x], meuScript.visualizar[x + 1]);
                        //    Handles.color = meuScript.cor;
                        Handles.ArrowHandleCap(0, meuScript.visualizar[x],
                           Quaternion.LookRotation(meuScript.visualizar[x + 1] - meuScript.visualizar[x]), dist, EventType.Repaint);
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