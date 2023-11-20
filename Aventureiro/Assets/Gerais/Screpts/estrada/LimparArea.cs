using System.Collections;
using System.Collections.Generic;
using teste;
using UnityEditor;
using UnityEngine;

namespace Ageral
{
    public class LimparArea : MonoBehaviour
    {
        public List<Vector3> ListaVertices = new List<Vector3>();
        public GerenciadorFloresta grf;
        public triangulador trl;
        public void adicionarVertice()
        {
            ListaVertices.Add(transform.position);
        }
        public Mesh mesh;
        public void GerarMalha()
        {
            criarMalha(trl.triangular(ListaVertices));
        }
        public bool renderMalha;
        public void criarMalha(List<Vector3> vertices)
        {
            mesh = new Mesh();

            // Atribuir os vértices à malha
            mesh.vertices = vertices.ToArray();

            // Definir os triângulos (assumindo que os vértices estão em grupos de três para formar triângulos)
            int[] triangles = new int[vertices.Count];
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = i;
            }

            // Atribuir os triângulos à malha
            mesh.triangles = triangles;

            // Recalcular normais e bounds (opcional, mas geralmente desejável)
            //   mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            // Atribuir a malha ao componente MeshFilter do GameObject
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = mesh;

            // Atribuir um material (pode ajustar conforme necessário)
            if (renderMalha)
            {
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    meshRenderer = gameObject.AddComponent<MeshRenderer>();
                }
                meshRenderer.material = new Material(Shader.Find("Standard"));
            }


        }
        public void removerArvores()
        {
            // interar todas as arvores
            // pegar a distancia mais procima da malha na arvore
            //se for pequena remover a arvore
            meshFilter = GetComponent<MeshFilter>();
            tempCollider = gameObject.AddComponent<MeshCollider>();
            tempCollider.sharedMesh = meshFilter.sharedMesh;

            grf.arvores_g.RemoveAll(x => x == null);
            foreach (GameObject a in grf.arvores_g)
            {
                if (ArovreNaMalha(a))
                {
                    DestroyImmediate(a);
                }

            }
            DestroyImmediate(tempCollider);


        }
       public  MeshFilter meshFilter;
        public MeshCollider tempCollider;
        bool ArovreNaMalha(GameObject arvore)
        {

        

            Vector3 pontoMaisProximo = tempCollider.ClosestPointOnBounds(arvore.transform.position);
            pontoMaisProximo.y = 0;
            Vector3 auxJ = arvore.transform.position;
            auxJ.y = 0;
            


            // Agora você pode verificar se o jogador está dentro da área demarcada pela mesh
            if (Vector3.Distance(auxJ, pontoMaisProximo) < 0.1f)
            {
                return true;
            }
            else
            {
                Debug.Log(Vector3.Distance(auxJ, pontoMaisProximo));
                return false;

            }

        }
        [HideInInspector]
        public bool AtivadoDesativadoGuizmos;

    }
    [CustomEditor(typeof(LimparArea))]
    public class EditorLimparArea : Editor {

        public override void OnInspectorGUI()
        {
            LimparArea meuScript = (LimparArea)target;
            base.OnInspectorGUI();


            if (GUILayout.Button("Adicionar Vertice"))
            {

                meuScript.adicionarVertice();
            }
            if (GUILayout.Button("Gerar malha"))
            {

                meuScript.GerarMalha();
            }
            if (GUILayout.Button("Remover arvores"))
            {

                meuScript.removerArvores();
            }

            if (GUILayout.Button("Ativar  /  desativar guizomos"))
            {

                meuScript.AtivadoDesativadoGuizmos = !meuScript.AtivadoDesativadoGuizmos;
            }

            }

        private void OnSceneGUI()
        {
            LimparArea meuScript = (LimparArea)target;
            if (meuScript.AtivadoDesativadoGuizmos) { 
            for (int x = 0; x < meuScript.ListaVertices.Count; x++)
            {
                meuScript.ListaVertices[x] = Handles.PositionHandle(meuScript.ListaVertices[x], Quaternion.identity);
                Handles.color = Color.red;
                Handles.DrawSolidDisc(meuScript.ListaVertices[x], Vector3.up, 1);

                    if (x < meuScript.ListaVertices.Count - 1)
                    {
                        float dist = Vector3.Distance(meuScript.ListaVertices[x], meuScript.ListaVertices[x + 1]);

                        Handles.ArrowHandleCap(0, meuScript.ListaVertices[x],
                           Quaternion.LookRotation(meuScript.ListaVertices[x + 1] - meuScript.ListaVertices[x]), dist, EventType.Repaint);
                    }
                }
            }
        }


        }

}
