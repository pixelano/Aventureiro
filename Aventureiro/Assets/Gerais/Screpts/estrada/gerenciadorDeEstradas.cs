using log4net.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using teste;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using static Ageral.triangulador;
using static UnityEngine.GraphicsBuffer;
 
namespace Ageral
{
    public class gerenciadorDeEstradas : MonoBehaviour
    {
        public float espessura = 5;
        public float escala = 5;
        public int AnguloAgudo = 45;
        public bool dstvgz, MostrarRastros;
        [Tooltip("valor aproximado de 1 que define se o ponto C de AB esta reto ou não")]
      
        public float DiminuirEmRetas = 2;
        public float AdicionarPoligonosACadaXDistancia =5;
        public int QuantidadeDeAmostragemParaSuavizacaoAutomatica = 30;

        [Tooltip("é o quanto o B esta de C apartir de A")]
        [Range(0, 1)]
        public float Suavizacao;
        public LayerMask layerDaFloresta;
        public LimparArea lmp;

        public List<Vector3> pontosdaEstrada = new List<Vector3>();
        public List<Vector3> pontosAuxiliares = new List<Vector3>();
    public void AdicionarVertice()
        {
            pontosdaEstrada.Add(transform.position);
        }
        public void DiminuirVertice()
        {
            pontosdaEstrada.RemoveAt(pontosdaEstrada.Count-1);
        }
        

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
     
        public Color cor;

        private void Start()
        {
            if (lmp == null)
            {
                if(GetComponentInChildren<LimparArea>() != null)
                {
                    lmp = GetComponentInChildren<LimparArea>();
                }
                else
                {
                    GameObject a = new GameObject("limpador De Area");
                    a.transform.position = transform.position;
                    a.transform.parent = transform;
                    lmp= a.AddComponent<LimparArea>();
                    a.GetComponent<LimparArea>().TamanhoParaRemover = 30;
                    a.GetComponent<LimparArea>().trl = a.AddComponent<triangulador>();


                }
            }
        }
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
               
                    Vector3 orientacao = Vector3.Cross(pontosAuxiliares[x +1] - pontosAuxiliares[x] , Vector3.up) ;
                orientacao.Normalize();
               // Vector3 pontoMeioA = pontosAuxiliares[x];
                Vector3 pontoMeioA = pontosAuxiliares[x];
                Vector3 pontoMeioB = pontosAuxiliares[x+1] ;
                    
                  
                
                // Vector3 pontoA = pontoMeioA - (orientacao ) ;
                Vector3 pontoA_E = x==0? pontoMeioA - (orientacao * espessura) : ordemTrianguo[ordemTrianguo.Count-1] ;
                Vector3 pontoB_E = pontoMeioB - (orientacao * espessura);
                Vector3 pontoA_D = x == 0 ?pontoMeioA + (orientacao * espessura) : ordemTrianguo[ordemTrianguo.Count - 3]; 
                Vector3 pontoB_D = pontoMeioB + (orientacao * espessura);

                ordemTrianguo.Add(pontoA_E);
                    ordemTrianguo.Add(pontoB_E);
                    ordemTrianguo.Add(pontoA_D);

                    ordemTrianguo.Add(pontoB_D);
                    ordemTrianguo.Add(pontoA_D);
                    ordemTrianguo.Add(pontoB_E);

         
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
                Vector3 AB = Vector3.Lerp(pontosAuxiliares[x - 1], pontosAuxiliares[x], Suavizacao);
                AB = Vector3.Lerp(AB, pontosAuxiliares[x + 1], Suavizacao);

                pontosAuxiliares[x] = AB;
            }
        }
        public List<Vector3> suavisarMalha(List<Vector3> lista)
        {

            for (int x = 1; x < lista.Count - 1; x++)
            {
                Vector3 AB = Vector3.Lerp(lista[x - 1], lista[x], Suavizacao);
                AB = Vector3.Lerp(AB, lista[x + 1], Suavizacao);

                lista[x] = AB;
            }
            return lista;
        }
      
        public void suavizacaoAutomatica()
        {
            for (int x = 0; x < pontosAuxiliares.Count - 1; x++)
            {
                List<Vector3> TempVertices = new List<Vector3>();
                for (int z = 0; z < QuantidadeDeAmostragemParaSuavizacaoAutomatica; z++)
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
        public void simplificar()
        {
            pontosAuxiliares = ValoresUniversais.OptimizePath(pontosAuxiliares, DiminuirEmRetas);
        }
        public void removerArvores()
        {
            //  acessar as classes de floresta
            // adicionar um box colider do tamanho da estrada
            // cirar uma mesh do tamanho que vai ser retirado as arores
            // verificar cada ponto das arvores tem distancia do menor caminho da mesh <0.1f
            // remover o ponto da lista de arvores
            // re-renderizar as arvores

            float pontoMaximoX = pontosAuxiliares[0].x, pontominimoX = pontosAuxiliares[0].x, pontomaximoY = pontosAuxiliares[0].x,
                pontominimoY = pontosAuxiliares[0].x;
            for(int x = 0;x  < pontosAuxiliares.Count;x++)
            {
                if (pontoMaximoX < pontosAuxiliares[x].x)
                    pontoMaximoX = pontosAuxiliares[x].x;

                if (pontominimoX > pontosAuxiliares[x].x)
                    pontominimoX = pontosAuxiliares[x].x;

                if (pontomaximoY < pontosAuxiliares[x].z)
                    pontomaximoY = pontosAuxiliares[x].z;

                if (pontominimoY > pontosAuxiliares[x].z)
                    pontominimoY = pontosAuxiliares[x].z;
            }
         
            Vector3 TamanhoCaixa =  new Vector3(pontoMaximoX-pontominimoX,100,pontomaximoY-pontominimoY);
           
            List<Collider> FlorestarCOlididas = Physics.OverlapBox(transform.position, TamanhoCaixa, Quaternion.identity, layerDaFloresta).ToList();
            //   lmp.criarMalhaParaRemover(pontosAuxiliares);
            FlorestarCOlididas.RemoveAll(x => x == null);
            if (FlorestarCOlididas.Count != 0) {
              
                foreach(Collider a in FlorestarCOlididas)
                {
                    GerenciadorFloresta aux = a.GetComponent<GerenciadorFloresta>();
                          
                    for (int x= 0; x < aux.arvores_g.Count; x++)
                    {
                      
                        lmp.TestarERemover(aux.arvores_g[x]);
                    }
                    aux.arvores_g.RemoveAll(x => x == null);
                }
            }
          
        }
    }

    [CustomEditor(typeof(gerenciadorDeEstradas))]
    public class EditorgerenciadorDeEstradas : Editor
    {
        public SerializedProperty espessura, layerDaFloresta, escala,suavizacao,DiminuirEmRetas,AdicionarPoliACadaXDistancia,AnguloAgudo, QuantidadeDeAmostragemParaSuavizacaoAutomatica;

        void OnEnable()
        {
            espessura = serializedObject.FindProperty("espessura");
            escala = serializedObject.FindProperty("escala");

            suavizacao = serializedObject.FindProperty("Suavizacao");

            DiminuirEmRetas = serializedObject.FindProperty("DiminuirEmRetas");

            AdicionarPoliACadaXDistancia = serializedObject.FindProperty("AdicionarPoligonosACadaXDistancia");

            AnguloAgudo = serializedObject.FindProperty("AnguloAgudo");
            QuantidadeDeAmostragemParaSuavizacaoAutomatica = serializedObject.FindProperty("QuantidadeDeAmostragemParaSuavizacaoAutomatica");

            layerDaFloresta = serializedObject.FindProperty("layerDaFloresta");
            gerenciadorDeEstradas meuScript = (gerenciadorDeEstradas)target;
            if (meuScript.lmp == null)
            {
                if (meuScript.GetComponentInChildren<LimparArea>() != null)
                {
                    meuScript.lmp = meuScript.GetComponentInChildren<LimparArea>();
                }
                else
                {
                    GameObject a = new GameObject("limpador De Area");
                    a.transform.parent = meuScript.transform;
                    a.transform.position = meuScript.transform.position;
                    meuScript.lmp = a.AddComponent<LimparArea>();
                    a.GetComponent<LimparArea>().TamanhoParaRemover = 30;
                    a.GetComponent<LimparArea>().trl = a.AddComponent<triangulador>();

                }
            }
        }
        public override void OnInspectorGUI()
        {
            gerenciadorDeEstradas meuScript = (gerenciadorDeEstradas)target;

            serializedObject.Update();

            if (GUILayout.Button("Adicionar Vertice"))
            {

                meuScript.AdicionarVertice();
            }
            if (GUILayout.Button("DiminuirVectice Vertice"))
            {

                meuScript.DiminuirVertice();
            }
            EditorGUILayout.PropertyField(layerDaFloresta, new GUIContent("layerDaFloresta da floresta"));
            EditorGUILayout.PropertyField(espessura, new GUIContent("Espessura da rua"));
            EditorGUILayout.PropertyField(escala, new GUIContent("Distancia relativa entre os vertices"));

            if (GUILayout.Button("Gerar estrada"))
            {

                meuScript.gerarEstrada();
            }

           
            if (GUILayout.Button("Gerar Malha"))
            {

                meuScript.renderizarMesh();
                meuScript.criarMalha();

            }
            
                 EditorGUILayout.PropertyField(AdicionarPoliACadaXDistancia, new GUIContent("Escala para adicionar poligonos"));
            if (GUILayout.Button("Adicionar poligonos na Malha"))
            {
                meuScript.adicionarPoligonos();

                meuScript.renderizarMesh();
                meuScript.criarMalha();
            }
            

                EditorGUILayout.PropertyField(AnguloAgudo, new GUIContent("Valor de angulo para ser agudo"));
            EditorGUILayout.PropertyField(suavizacao, new GUIContent("Forca de suavizacao AC em B"));
            
             EditorGUILayout.PropertyField(QuantidadeDeAmostragemParaSuavizacaoAutomatica, new GUIContent("Quantidade de vertices para suavizar a malha"));

            if (GUILayout.Button("Suavização automatica"))
            {

                meuScript.suavizacaoAutomatica();

                meuScript.renderizarMesh();
                meuScript.criarMalha();
            }

                 EditorGUILayout.PropertyField(DiminuirEmRetas, new GUIContent("Valor para considerar Se o ponto C em relação a AB é reto"));
            if (GUILayout.Button("simplificar malha"))
            {

                meuScript.simplificar();

                meuScript.renderizarMesh();
                meuScript.criarMalha();
            }

            if (GUILayout.Button("desativar guizmo"))
            {

                meuScript.dstvgz = !meuScript.dstvgz;
            }
            if (GUILayout.Button("Tirar Arvores"))
            {
          
                meuScript.removerArvores();


            }
            serializedObject.ApplyModifiedProperties();
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

/*
 * espessura
 * escala
 * suavização
 * diminuir em retas
 * adicionar poligonos a cada xdistancia
 * angulo agudo
 * 
 */