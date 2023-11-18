using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                            Vector3 vertice_ = new Vector3(pivo_.x + (x * config.escala), 0, pivo_.z + (z * config.escala));
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
    }
