using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teeeeeeeeeeeeste : MonoBehaviour
{
    public List<List<Vector3>> mapaGerado;
    public int tamanhoHorizontal, tamanhovertical;

    public float amplitude, frequencia;
    public float baiasX,baiasY,MultiplicadorDeAltura =1 ,multiplicadorEscala = 1;
    public Material mat;
    public void gerarMapa()
    {
        mapaGerado = new List<List<Vector3>>();
        for(int x = 0; x < tamanhoHorizontal;x++)
        {
            mapaGerado.Add(new List<Vector3>());
            for(int z = 0; z < tamanhovertical; z++)
            {
                mapaGerado[mapaGerado.Count-1].Add(new Vector3(x,0,z));  
            }
        }


    }
    public void movimentar()
    {
        for (int x = 0; x < tamanhoHorizontal; x++)
        {
            mapaGerado.Add(new List<Vector3>());
            for (int z = 0; z < tamanhovertical; z++)
            {
                float y = Mathf.PerlinNoise(((x+baiasX) *amplitude)/frequencia,((z + baiasY) * amplitude)/frequencia);
                mapaGerado[x][z] = new Vector3(x * multiplicadorEscala, y * MultiplicadorDeAltura, z * multiplicadorEscala);
            }
        }
    }
    public List<Vector3> triangulos;
    public void triangular()
    {
        triangulos = new List<Vector3>();

        for(int x = 0; x < tamanhoHorizontal - 1; x++)
        {
            for(int z = 0; z < tamanhovertical - 1; z++)
            {
                triangulos.Add(mapaGerado[x][z]);
                triangulos.Add(mapaGerado[x][z+1]);
                triangulos.Add(mapaGerado[x+1][z]);

                triangulos.Add(mapaGerado[x+1][z+1]);
                triangulos.Add(mapaGerado[x+1][z]);
                triangulos.Add(mapaGerado[x][z+1]);
            }
        }
    }
    public Mesh mesh;
    public void criarMalha()
    {
        mesh = new Mesh();

        // Atribuir os vértices à malha
        mesh.vertices = triangulos.ToArray();

        // Definir os triângulos (assumindo que os vértices estão em grupos de três para formar triângulos)
        int[] triangles = new int[triangulos.Count];
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

      
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
            meshRenderer.material =mat;
        


    }

    public bool gerarmapa_, movimentar_, triangular_, criarmalha_, updatar,movimentarHorizontal,movimentarVertical;
    public float timer;
    public void Update()
    {
        if (gerarmapa_)
        {
            gerarmapa_ = false;
            gerarMapa();
        }
        if (movimentar_)
        {
            movimentar_ = false;
            movimentar();
        }
        if (triangular_)
        {
            triangular_ = false;
            triangular();
        }
        if (criarmalha_)
        {
            criarmalha_ = false;
            criarMalha();
        }
        if (updatar)
        {
           if(tt_ > timer)
            {
                if (movimentarHorizontal)
                {
                    baiasX+= timer;
                }
                if (movimentarVertical)
                {
                    baiasY+= timer;
                }

                tt_ = 0;
                gerarMapa();
                movimentar();
                triangular();
                criarMalha();
            }
            tt_ += Time.deltaTime;
        }
    }
    private float tt_;
}
