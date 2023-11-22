using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Ageral
{
    public class CriadorDeAreas : MonoBehaviour
    {
        [System.Serializable]
        public class configuracao {
        

            public float tamanhoMaximoDeArea,amplitude,frequencia,tolerancia,escala;
       

        }
        public Terrain terreno;
        public configuracao config;
      
    
        public List<float> cc = new List<float> ();
        public bool executar,guizmos,somentePontos,renderMalha;
        public triangulador trl;
     
      
        private void Update()
        {
            if(executar == true)
            {
                executar = false;
                pontos_.Clear();





                Vector3 pivo_ = Vector3.zero;
                List<Vector3> pp = new List<Vector3>();
                float max_x = 0, max_y = 0;
                for (int x = 0; x < config.tamanhoMaximoDeArea; x++)
                {
                    for (int z = 0; z < config.tamanhoMaximoDeArea; z++)
                    {

                        float pl = Mathf.PerlinNoise(((pivo_.x + (x * config.escala)) * config.amplitude) / config.frequencia,
                            ((pivo_.z + (z * config.escala)) * config.amplitude) / config.frequencia) * config.escala;
                        if (pl > config.tolerancia)
                        {
                            Vector3 vertice_ = new Vector3(pivo_.x + (x * config.escala), 0, pivo_.z + (z * config.escala)) + transform.position;
                            vertice_.y = 0;


                            pp.Add(vertice_);

                            max_x = vertice_.x > max_x ? vertice_.x : max_x;
                            max_y = vertice_.x > max_y ? vertice_.x : max_y;


                        }
                    }
                }
                Vector3 media = new Vector3(config.tamanhoMaximoDeArea / 2, 0, config.tamanhoMaximoDeArea / 2);
               
                for (int x = 0; x < pp.Count; x++)
                {
                    pp[x] -= media;
                  
                }
                pontos_ = pp;
                triangulos_ = trl.triangular(pp);


                CriarMalhaAPartirDeVertices(triangulos_);

           
            }



       
            
            }
        
        
        public Mesh mesh;

        public List<Vector3> pontos_ = new List<Vector3>();
        public List<Vector3> triangulos_ = new List<Vector3>();

        void CriarMalhaAPartirDeVertices(List<Vector3> vertices)
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

    }

    [CustomEditor(typeof(CriadorDeAreas))]
    public class EditorCriadorDeAreas : Editor
    {
        public int verificarPonto, verificarTriangulo_;
        // public override void OnInspectorGUI()
        Vector3 orig;
        List<Vector3> pp = new List<Vector3>();
        List<Vector3> tt = new List<Vector3>();
        private void OnSceneGUI()
        {

            CriadorDeAreas meuObjeto = (CriadorDeAreas)target;

            if (meuObjeto.guizmos)
            {
                if (meuObjeto.pontos_.Count <= 0)
                {
                    if (orig != meuObjeto.transform.position)
                    {
                        orig = meuObjeto.transform.position;
                        pp.Clear();


                        Vector3 pivo_ = Vector3.zero;//new Vector3(Random.Range(0, terreno.terrainData.size.x), 0, Random.Range(0,
                                                     //   terreno.terrainData.size.z));
                        float max_x = 0, max_y = 0;
                        for (int x = 0; x < meuObjeto.config.tamanhoMaximoDeArea; x++)
                        {
                            for (int z = 0; z < meuObjeto.config.tamanhoMaximoDeArea; z++)
                            {

                                float pl = Mathf.PerlinNoise(((pivo_.x + (x * meuObjeto.config.escala)) * meuObjeto.config.amplitude) / meuObjeto.config.frequencia,
                                    ((pivo_.z + (z * meuObjeto.config.escala)) * meuObjeto.config.amplitude) / meuObjeto.config.frequencia) * meuObjeto.config.escala;
                                if (pl > meuObjeto.config.tolerancia)
                                {
                                    Vector3 vertice_ = new Vector3(pivo_.x + (x * meuObjeto.config.escala) - meuObjeto.config.tamanhoMaximoDeArea / 2, 0, pivo_.z + (z * meuObjeto.config.escala) - meuObjeto.config.tamanhoMaximoDeArea / 2);
                                    vertice_.y = 0;


                                    pp.Add(vertice_);

                                    max_x = vertice_.x > max_x ? vertice_.x : max_x;
                                    max_y = vertice_.x > max_y ? vertice_.x : max_y;


                                }
                            }
                        }
                        if (!meuObjeto.somentePontos)
                        {
                            Vector3 aux_ = new Vector3(-(meuObjeto.config.escala + meuObjeto.config.tamanhoMaximoDeArea / 2), 0, -(meuObjeto.config.escala + meuObjeto.config.tamanhoMaximoDeArea) / 2);
                            for (int x = 0; x < pp.Count; x++)
                            {

                                pp[x] -= aux_;
                            }
                            tt = meuObjeto.trl.triangular(pp);

                            for (int x = 0; x <= tt.Count - 3; x += 3)
                            {

                                Handles.color = Color.green;
                                Vector3[] vv = new Vector3[3];
                                vv[0] = tt[x] + meuObjeto.transform.position;
                                vv[1] = tt[x + 1] + meuObjeto.transform.position;
                                vv[2] = tt[x + 2] + meuObjeto.transform.position;
                                Handles.DrawAAConvexPolygon(vv);

                                Handles.color = Color.red;
                                Handles.DrawLine(vv[0], vv[1]);
                                Handles.color = Color.red;
                                Handles.DrawLine(vv[1], vv[2]);
                                Handles.color = Color.red;
                                Handles.DrawLine(vv[0], vv[2]);
                            }
                        }
                        foreach (var a in pp)
                        {
                            Handles.color = Color.red;

                            Vector3 aux = a;
                            aux.y = 0;
                            aux += meuObjeto.transform.position;
                            Handles.DrawSolidDisc(aux, Vector3.up, 0.4f);
                        }
                    }
                    else
                    {
                        if (!meuObjeto.somentePontos)
                        {
                            for (int x = 0; x <= tt.Count - 3; x += 3)
                            {

                                Handles.color = Color.green;
                                Vector3[] vv = new Vector3[3];
                                vv[0] = tt[x] + meuObjeto.transform.position;
                                vv[1] = tt[x + 1] + meuObjeto.transform.position;
                                vv[2] = tt[x + 2] + meuObjeto.transform.position;
                                Handles.DrawAAConvexPolygon(vv);

                                Handles.color = Color.red;
                                Handles.DrawLine(vv[0], vv[1]);
                                Handles.color = Color.red;
                                Handles.DrawLine(vv[1], vv[2]);
                                Handles.color = Color.red;
                                Handles.DrawLine(vv[0], vv[2]);
                            }
                        }
                        foreach (var a in pp)
                        {
                            Handles.color = Color.red;

                            Vector3 aux = a;
                            aux.y = 0;
                            aux += meuObjeto.transform.position;
                            Handles.DrawSolidDisc(aux, Vector3.up, 0.4f);
                        }
                    }
                }
                else
                {

                    for (int x = 0; x <= meuObjeto.triangulos_.Count - 3; x += 3)
                    {

                        Handles.color = Color.green;
                        Vector3[] vv = new Vector3[3];
                        vv[0] = meuObjeto.triangulos_[x] + meuObjeto.transform.position;
                        vv[1] = meuObjeto.triangulos_[x + 1] + meuObjeto.transform.position;
                        vv[2] = meuObjeto.triangulos_[x + 2] + meuObjeto.transform.position;
                        Handles.DrawAAConvexPolygon(vv);

                        Handles.color = Color.red;
                        Handles.DrawLine(vv[0], vv[1]);
                        Handles.color = Color.red;
                        Handles.DrawLine(vv[1], vv[2]);
                        Handles.color = Color.red;
                        Handles.DrawLine(vv[0], vv[2]);
                    }
                    foreach (var a in meuObjeto.pontos_)
                    {
                        Handles.color = Color.red;

                        Vector3 aux = a;
                        aux.y = 0;
                        aux += meuObjeto.transform.position;
                        Handles.DrawSolidDisc(aux, Vector3.up, 0.4f);
                    }

                }
            }

        }
    }
}
