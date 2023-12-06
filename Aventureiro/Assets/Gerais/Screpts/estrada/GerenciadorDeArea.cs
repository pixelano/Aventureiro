using Codice.Client.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ageral
{
    public class GerenciadorDeArea : MonoBehaviour
    {
        
        public List<gerenciadorDeEstradas> Ruas = new List<gerenciadorDeEstradas>();
        public List<Vector3> PontosEmVolta, todosPontosEmVolta,pontosInternos;
        gerenciadorDeEstradas RuaAlvo;
        public bool GuizmosAoRedor,GuizmosInterno,guizmosLinhasCaminho;
        public void verificarSeEstaFechado()
        {    // crie uma lista temporaria com os mesmos valores de ruas
            // escolha uma rua
            // veja quais ruas estão conectada com esta rua
            // escolha uma, uma sera o alvo e a outra sera o incio

            // remova a rua escolida da lista temporaria
            // a rua de ponto de inicio deve receber a lista temporaria e a rua alvo,
            // quando ela receber este valor deve fazer as mesmas perguntas até achar a rua alvo

            RuaAlvo = Ruas[0].entrada.ConecatacoComEste;

            if (Ruas[0].saida.ConecatacoComEste == RuaAlvo)
            {

            }
            else
            {
                List<unidadeAlit> tempBL = new List<unidadeAlit>();
                tempBL.Add(new unidadeAlit(Ruas[0]));
                aaaaaaaaa = aliterar( new unidadeAlit(Ruas[0].saida.ConecatacoComEste), tempBL);
            }


            aaaaaaaaa[0].intercessaoA = RuaAlvo;
            aaaaaaaaa[0].indiceIntercessaoB =
                RuaAlvo.entrada.ConecatacoComEste == Ruas[0] ? 0 :
                RuaAlvo.saida.ConecatacoComEste == Ruas[0] ? RuaAlvo.pontosAuxiliares.Count - 1 :
                RuaAlvo.conecxoes.Exists(x => x.ConecatacoComEste == Ruas[0]) ?
                RuaAlvo.conecxoes.Find(x => x.ConecatacoComEste == Ruas[0]).meuIndiceEmQueEstaCOnectado :
                0;
            aaaaaaaaa.Add(new unidadeAlit(Ruas[0], Ruas[0].entrada.ConecatacoComEste, 0, Ruas[0].saida.ConecatacoComEste, Ruas[0].pontosAuxiliares.Count - 1
                ));

            Debug.Log(aaaaaaaaa.Count);

        }
        public List<unidadeAlit> aliterar(unidadeAlit atua , List<unidadeAlit> blkL)
        {
            blkL.Add(atua);
          if(atua.zero.entrada.ConecatacoComEste== RuaAlvo)
            {

                List<unidadeAlit> tempf = new List<unidadeAlit>();

                tempf.Add(new unidadeAlit(RuaAlvo, atua.zero.entrada.INdiceDeleEmQueEstaConectadoEmMim, atua.zero));
                tempf.Add(new unidadeAlit(atua.zero, atua.zero.entrada.ConecatacoComEste, atua.zero.entrada.meuIndiceEmQueEstaCOnectado));

                
              
                return tempf;
            }else
            if (atua.zero.saida.ConecatacoComEste == RuaAlvo)
            {
                List<unidadeAlit> tempf = new List<unidadeAlit>();

                tempf.Add(new unidadeAlit(RuaAlvo, atua.zero.saida.INdiceDeleEmQueEstaConectadoEmMim, atua.zero));
                tempf.Add(new unidadeAlit(atua.zero, atua.zero.saida.ConecatacoComEste, atua.zero.saida.meuIndiceEmQueEstaCOnectado));


                return tempf;
            }
            else
            if (atua.zero.conecxoes.Exists(x=>x.ConecatacoComEste == RuaAlvo)) 
            {
                List<unidadeAlit> tempf = new List<unidadeAlit>();
                var aa = atua.zero.conecxoes.Find(x=>x.ConecatacoComEste ==RuaAlvo);

                tempf.Add(new unidadeAlit(RuaAlvo, aa.INdiceDeleEmQueEstaConectadoEmMim, aa.ConecatacoComEste));
                tempf.Add(new unidadeAlit(atua.zero, aa.ConecatacoComEste, aa.meuIndiceEmQueEstaCOnectado));


                return tempf;
            }
            foreach(var a in atua.zero.conecxoes)
            {
                if(blkL.Exists(x=>x.zero == a.ConecatacoComEste))
                {
                    continue;
                }
                unidadeAlit b = new unidadeAlit(a.ConecatacoComEste);
                List<unidadeAlit> tempL = aliterar(b,blkL);

                if(tempL.Count > 0)
                {
                    tempL[tempL.Count - 1].intercessaoA = atua.zero;
                    tempL[tempL.Count - 1].indiceIntercessaoA = a.INdiceDeleEmQueEstaConectadoEmMim;
                    tempL.Add(new unidadeAlit(atua.zero,a.ConecatacoComEste,a.meuIndiceEmQueEstaCOnectado));

                    return tempL;
                }
            }

            return null;
        }
        /*
        public void verificarSeEstaFechado()
        {
        

            

            gerenciadorDeEstradas  Ruainicio = null,ruaZero;
            RuaAlvo = null;
            ruaZero = Ruas[0];
            //rua alvo

            int indiceInicial = -1, indiceFinal = -1;
            foreach (var item in Ruas)
            {
                if (item == ruaZero.entrada.ConecatacoComEste)
                {
                    RuaAlvo = ruaZero.entrada.ConecatacoComEste;
                    indiceInicial = 0;
                    break;
                }
                if (item == ruaZero.saida.ConecatacoComEste)
                {
                    RuaAlvo = ruaZero.entrada.ConecatacoComEste;
                    indiceInicial = ruaZero.pontosAuxiliares.Count - 1;
                    break;
                }

                if (ruaZero.conecxoes.Exists(x => x.ConecatacoComEste == item))
                {
                    RuaAlvo = item;
                    indiceInicial = ruaZero.conecxoes.Find(x => x.ConecatacoComEste == RuaAlvo).indiceDeleEmQueEstouConectado;
                    break;
                }
            }
          
            foreach (var item in Ruas)
            {
                if (item == RuaAlvo)
                    continue;
                if (item == ruaZero.entrada.ConecatacoComEste)
                {
                    Ruainicio = ruaZero.entrada.ConecatacoComEste;
                    indiceFinal = 0;
                    break;
                }
                if (item == ruaZero.saida.ConecatacoComEste)
                {
                    Ruainicio = ruaZero.entrada.ConecatacoComEste;
                    indiceFinal = ruaZero.pontosAuxiliares.Count - 1;
                    break;
                }

                if (ruaZero.conecxoes.Exists(x => x.ConecatacoComEste == item))
                {
                    Ruainicio = item;
                    indiceFinal = ruaZero.conecxoes.Find(x => x.ConecatacoComEste == RuaAlvo).indiceDeleEmQueEstouConectado;
                    break;
                }
            }

            Debug.Log(ruaZero.entrada.ConecatacoComEste);

            if (Ruainicio == null || RuaAlvo == null)
            {
             
            }
            else
            {



                List<gerenciadorDeEstradas> blackList = new List<gerenciadorDeEstradas>();
                blackList.Add(ruaZero);
                //    List<unidadeAlit> resultado = aliterarRuas(RuaAlvo, Ruas[1], new unidadeAlit(Ruas[1], ruaZero,0, Ruas[2], 0),blackList,0);
                // new unidadeAlit(ruaZero, RuaAlvo, 0, Ruas[1], 0)
                aaaaaaaaa = aliterarRuas(RuaAlvo, ruaZero, null, blackList, 0);

                aaaaaaaaa[aaaaaaaaa.Count - 1].intercessaoA = aaaaaaaaa[0].zero;
                aaaaaaaaa[aaaaaaaaa.Count - 1].indiceIntercessaoA = aaaaaaaaa[aaaaaaaaa.Count - 1].
                    zero.conecxoes.Find(x => x.ConecatacoComEste == RuaAlvo).indiceDeleEmQueEstouConectado;
             /*
                Debug.Log("procurando  " +ruaal.name);
                foreach(var a in RuaAlvo.conecxoes)
                {
                    Debug.Log(a.ConecatacoComEste.name);
                }
          
                //   gerenciadorDeEstradas entradaA = RuaAlvo.conecxoes.Find(x => x.ConecatacoComEste == aaaaaaaaa[aaaaaaaaa.Count - 2].zero).ConecatacoComEste;
              //  gerenciadorDeEstradas entradaB = RuaAlvo.conecxoes.Find(x => x.ConecatacoComEste == aaaaaaaaa[aaaaaaaaa.Count - 1].zero).ConecatacoComEste;
             
                 unidadeAlit bb = new unidadeAlit(RuaAlvo, aaaaaaaaa[aaaaaaaaa.Count - 1].zero,
                    RuaAlvo.conecxoes.Find(x=>x.ConecatacoComEste == aaaaaaaaa[aaaaaaaaa.Count - 1].zero).indiceDeleEmQueEstouConectado, 
                    aaaaaaaaa[0].zero, RuaAlvo.conecxoes.Find(x => x.ConecatacoComEste == aaaaaaaaa[0].zero).indiceDeleEmQueEstouConectado);
                aaaaaaaaa.Add(bb);
            
            }
           

        }  
        public List<unidadeAlit> aliterarRuas(gerenciadorDeEstradas alvo, gerenciadorDeEstradas  atual, unidadeAlit anterior , List<gerenciadorDeEstradas> blackList,int cont)
        {
            Debug.Log("testeeeee");
            blackList.Add(atual);

            // verificar se voce esta conectado nele
            if(atual.entrada.ConecatacoComEste == alvo && blackList.Exists(x=>x == atual.entrada.ConecatacoComEste))
            {
                unidadeAlit temp = new unidadeAlit(atual,anterior.zero,0
                    ,atual.entrada.ConecatacoComEste,atual.pontosAuxiliares.Count-1);

                List<unidadeAlit> primeiro = new List<unidadeAlit>();
                primeiro.Add(temp);
                Debug.Log("inicio");
                return primeiro;
            }
            if (atual.saida.ConecatacoComEste == alvo && blackList.Exists(x => x == atual.saida.ConecatacoComEste))
            {
                unidadeAlit temp = new unidadeAlit(atual, anterior.zero, atual.pontosAuxiliares.Count - 1
                    , atual.saida.ConecatacoComEste,0);

                List<unidadeAlit> primeiro = new List<unidadeAlit>();
                primeiro.Add(temp);
                Debug.Log("final");
                Debug.Log(cont);
                return primeiro;
            }

            // caso não esteja conectado verificar se ele que esta conectado em voce
            Debug.Log(atual.conecxoes);
            if(anterior != null) {
                foreach (var a in atual.conecxoes)
                {
                    if (blackList.Exists(x => x == a.ConecatacoComEste))
                        continue;

                    if (a.ConecatacoComEste == alvo)
                    {

                        int anteri = 0, proximo = 0;
                        if(anterior.zero == atual.entrada.ConecatacoComEste)
                        {
                            anteri = 0;
                        }
                        else if (anterior.zero == atual.saida.ConecatacoComEste)
                        {
                            anteri = atual.pontosAuxiliares.Count - 1;
                        }
                        else
                        {
                            anteri = -1;
                        }

                        if (a.ConecatacoComEste == atual.entrada.ConecatacoComEste)
                        {
                            proximo = 0;
                        }
                        else if (a.ConecatacoComEste == atual.saida.ConecatacoComEste)
                        {
                            proximo = atual.pontosAuxiliares.Count - 1;
                        }
                        else
                        {
                            proximo = -1;
                        }


                        unidadeAlit temp = new unidadeAlit(atual, anterior.zero,anteri == -1 ? anterior.indiceIntercessaoB:anteri
                       , a.ConecatacoComEste, proximo == -1 ? a.INdiceDeleEmQueEstaConectadoEmMim:proximo);

                        List<unidadeAlit> primeiro = new List<unidadeAlit>();
                        primeiro.Add(temp);

                        return primeiro;
                    }
                }
            }
            // caso não tenha conexao direita verificar se alguem que voce esta conectado tem conexao com ele

            foreach(var a in atual.conecxoes)
            {
                if (blackList.Exists(x => x == a.ConecatacoComEste))
                    continue;
                unidadeAlit temporario = new unidadeAlit(atual,anterior == null ? null: anterior.zero, anterior == null ? 0:anterior.indiceIntercessaoB,                    a.ConecatacoComEste, a.INdiceDeleEmQueEstaConectadoEmMim);
                List<unidadeAlit> listTemp = new List<unidadeAlit>();
             
                Debug.Log(cont);
                listTemp.AddRange(aliterarRuas(alvo, a.ConecatacoComEste, temporario, blackList,cont++));
                if(listTemp.Count > 0)
                {
                    listTemp.Add(temporario);
                    return listTemp;
                }
            }
            Debug.Log("teste");
            return null;

        }
      */

        public class unidadeAlit
        {
            public gerenciadorDeEstradas zero,intercessaoA,intercessaoB;
            // a onde zero esta conectando com as intercessoes
            public int indiceIntercessaoA,indiceIntercessaoB;
            public unidadeAlit (gerenciadorDeEstradas z )
            {
                zero = z;
            }
            public unidadeAlit(gerenciadorDeEstradas z,gerenciadorDeEstradas b, int b_i)
            {
                zero = z;
              
                intercessaoB = b;
                indiceIntercessaoB = b_i;
            }
            public unidadeAlit(gerenciadorDeEstradas z, int a_i, gerenciadorDeEstradas a)
            {
                zero = z;

                intercessaoA = a;
                indiceIntercessaoA = a_i;
            }
            public unidadeAlit(gerenciadorDeEstradas z , gerenciadorDeEstradas a , int a_i , gerenciadorDeEstradas b ,  int b_i)
            {
                zero = z;
                intercessaoA = a;
                indiceIntercessaoA = a_i;
                intercessaoB = b;
                indiceIntercessaoB = b_i;
            }


        }
        [SerializeField]
        public List<unidadeAlit> aaaaaaaaa  ;

        public int quantidadeEmVolta;
        public void calcularAoReddor()
        {
            todosPontosEmVolta = new List<Vector3>();
         
            foreach(var a in aaaaaaaaa)
            {
              for(int x = a.indiceIntercessaoA; x != a.indiceIntercessaoB; x  = a.indiceIntercessaoA < a.indiceIntercessaoB ?
                    x+1:x-1)
                {
                    todosPontosEmVolta.Add(a.zero.pontosAuxiliares[x]);
                }
            }
            PontosEmVolta = new List<Vector3>();
            for(int x =0; x < quantidadeEmVolta; x++)
            {
                Vector3 tempv = todosPontosEmVolta[Random.Range(0, todosPontosEmVolta.Count - 1)];
                PontosEmVolta.Add(tempv);
                todosPontosEmVolta.Remove(tempv);
            }
            pontosExternos.Clear();
            foreach(var a in PontosEmVolta)
            {
                pontosExternos.Add(new pontos_(a));
            }
            

        }

        public int quantidadeDePontos;
        public float distanciaDeCorte;
        public void adicionarPontosInternos()
        {

            //criar os pontos
            Vector3 meio = Vector3.zero;

            foreach(var a in todosPontosEmVolta)
            {
                meio += a;
            }
            meio /= todosPontosEmVolta.Count;
            pontosInternos.Clear();
            while (pontosInternos.Count < quantidadeDePontos) {
                int pontoI = Random.Range(0, todosPontosEmVolta.Count);
                float forca = Random.Range(0.1f, 0.75f);

                pontosInternos.Add(Vector3.Lerp(todosPontosEmVolta[pontoI], meio, forca));
            }

            List<Vector3> tempL = new List<Vector3>();
            foreach(var a in pontosInternos)
            {
                foreach(var b in pontosInternos)
                {
                    if (a == b)
                        continue;
                    float dist = Vector3.Distance(a, b);

                    if(dist < distanciaDeCorte)
                    {
                        tempL.Add(a);
                        break;
                    }
                }
            }
            pontosINternos.Clear();
          
            Debug.Log("vai remover : " +  tempL.Count + "   de  " + pontosInternos.Count);
            pontosInternos.RemoveAll(x=> tempL.Contains(x));
            Debug.Log("sobraram  " + pontosInternos.Count);

            foreach (var a in pontosInternos)
            {
                pontosINternos.Add(new pontos_(a));
            }

            /*
            int auxint = 0;
            pontosInternos = new List<Vector3>();
           for (int j = 0; j < quantidadeDePontos;j++)
            {
                int indc = Random.Range(0, todosPontosEmVolta.Count);
                Vector3 pontoA = todosPontosEmVolta[indc];
                Vector3 pontoB = todosPontosEmVolta[Random.Range(0, todosPontosEmVolta.Count)];

                Vector3 pontoF = Vector3.Lerp(pontoA, pontoB, 0.5f);

                bool aux = true;
                float dentro=0, fora=0;
                for(int x = 0; x < todosPontosEmVolta.Count-1; x++)
                {
                    if (ValoresUniversais.Orientacao3(todosPontosEmVolta[x], todosPontosEmVolta[x + 1], pontoF) == 2)
                    {
                        dentro += 1;

                    }
                    else
                    {
                        fora++;
                    }
                }
                Debug.Log("dentro   " +dentro + "   fora  " + fora);
                if (dentro > fora)
                {
                    pontosInternos.Add(pontoF);
                }
                //  pontoF /= 2;
               
                                float dist = Vector3.Distance(pontoA, pontoA);

                              for(int x =0; x < todosPontosEmVolta.Count; x++)
                                {
                                    float aaa = Vector3.Distance(pontoF, todosPontosEmVolta[x]);
                                    if (aaa < dist)
                                    {
                                        indc = x;
                                        dist = aaa;
                                    }
                                }

                                float ori = ValoresUniversais.Orientacao3(todosPontosEmVolta[indc], todosPontosEmVolta[indc + 1 > todosPontosEmVolta.Count ? 0 : indc + 1],pontoF);

                                if(ori == 2)
                                {
                                    pontosInternos.Add(pontoF);
                                }*/


        }

        [System.Serializable]
        public class pontos_
        {
            public Vector3 origem;
            public List<Vector3> linhas = new List<Vector3>();
        public pontos_(Vector3 a)
            {
                origem = a;
            }
        
        }

       
        public List<pontos_> pontosINternos = new List<pontos_>(), pontosExternos = new List<pontos_>();
        public float rangeDistPulo;
        public void criarCaminhos()
        {


            foreach (var a in pontosINternos)
            { a.linhas.Clear(); }

            // adiciona as linhas internas
            foreach (var a in pontosINternos)
            {
                foreach (var b in pontosINternos)
                {
                    if (a == b)
                        continue;

                    // vai de A para B
                    pontos_ pontoAtual_ = a;
                    pontos_ proximoPonto = a;
                    float euriBase = Mathf.Infinity;
                    int contador = 0;
                    while (proximoPonto != b)
                    {
                        if (contador > 100)
                            break;
                        List<pontos_> tempListProximo = new List<pontos_>();
                        foreach (var c in pontosINternos)
                        {
                            if (c == pontoAtual_)
                                continue;

                            float euristica = Vector3.Distance(pontoAtual_.origem, c.origem);
                            if (euristica < euriBase)
                            {
                                euriBase = euristica;
                                tempListProximo.Add(c);
                            }
                        }
                        if (tempListProximo.Count > 0)
                        {
                            float tempdist = Mathf.Infinity;
                            foreach (var c in tempListProximo)
                            {
                                float eurist = Vector3.Distance(c.origem, b.origem);

                                if (eurist < tempdist)
                                {
                                    tempdist = eurist;
                                    proximoPonto = c;
                                }

                            }
                        }
                        else
                        {
                            Debug.Log("erro 587");
                            break;
                        }
                        pontoAtual_.linhas.Add(proximoPonto.origem);
                        pontoAtual_ = proximoPonto;

                    }

                }
            }

            // remove as linhas cruzadas
            for (int x = 0; x < pontosINternos.Count; x++)
            {
                pontos_ A = pontosINternos[x];
                for (int y = 0; y < pontosINternos.Count; y++)
                {
                    pontos_ B = pontosINternos[y];

                    if (A == B) { continue; }

                    for (int x_ = 0; x_ < A.linhas.Count; x_++)
                    {
                        for (int y_ = 0; y_ < B.linhas.Count; y_++)
                        {
                            Vector3 A_ = A.linhas[x_];
                            Vector3 B_ = B.linhas[y_];

                            if (A_ == B_ || A_ == B.origem || B_ == A.origem)
                                continue;

                            if (ValoresUniversais.LinhasCruzadas(A.origem, A_, B.origem, B_))
                            {
                                y_ = 0;

                                B.linhas.Remove(B_);

                            }
                        }
                    }
                }
            }


          
            
            // adiciona as entradas de dentro pra fora    
            List<pontos_> tempPE = new List<pontos_>();
            int indcz = pontosINternos.Count - 1;
       for(int x = 0; x < pontosExternos.Count; x++)
            {

                pontos_ tempP = new pontos_(pontosExternos[x].origem);
                float PI = Mathf.Infinity;
                Vector3 tempV = Vector3.zero;
                foreach(var a in pontosInternos)
                {
                    float tempDist = Vector3.Distance(tempP.origem,a);
                    if ( tempDist< PI)
                    {
                        tempV = a;
                        PI = tempDist;
                    }
                }

                tempP.linhas.Add(tempV);

             
                    tempPE.Add(tempP);
            }
            // remove as linhas cruzadas

            for (int x = 0; x < tempPE.Count; x++)
            {
                for (int z = 0; z < tempPE[x].linhas.Count; z++)
                {
                    for (int a = 0; a < pontosINternos.Count; a++)
                    {
                        for (int b = 0; b < pontosINternos[a].linhas.Count; b++)
                        {

                            if (tempPE[x].linhas[z] == pontosINternos[a].origem ||
                                tempPE[x].linhas[z] == pontosINternos[a].linhas[b])
                                continue;

                            if (ValoresUniversais.LinhasCruzadas(
                                tempPE[x].origem, tempPE[x].linhas[z], pontosINternos[a].origem,
                                pontosINternos[a].linhas[b]
                                ))
                            {
                                pontosINternos[a].linhas.RemoveAt(b);
                                b = 0;
                            }
                        }
                    }
                }
            }
            for(int x = 0; x < aaaaaaaaa.Count; x++)
            {
                for(int z = 0; z < aaaaaaaaa[x].zero.pontosAuxiliares.Count-1; z++)
                {
                    foreach(var a in tempPE)
                    {
                        for(int c = 0; c < a.linhas.Count; c++)
                        {
                            if (ValoresUniversais.LinhasCruzadas(aaaaaaaaa[x].zero.pontosAuxiliares[z],
                                aaaaaaaaa[x].zero.pontosAuxiliares[z+1],
                                a.origem, a.linhas[c]))
                            {
                                a.linhas.RemoveAt (c);
                                c = 0;
                            }   
                        }
                    }

                }
            }

            
           pontosINternos.AddRange(tempPE);




        }



        public class removerCruz {
           public Vector3 A, A_, B, B_;
            public removerCruz(Vector3 a, Vector3 a_, Vector3 b, Vector3 b_)
            {
                A = a;
                A_ = a_;
                B = b;
                B_ = b_;
            }
        }
        public Mesh mesh;
        public bool gerarMalha;
        public void criarMalha()
        {
            ordemTrianguo.Clear();
            foreach(var a in pontosINternos)
            {
                foreach( var b in a.linhas)
                {
                    Vector3 orientacao = Vector3.Cross(b - a.origem, Vector3.up).normalized;

                    Vector3 AE = a.origem + orientacao - transform.position;
                    Vector3 AD = a.origem - orientacao - transform.position;

                    Vector3 BE = b + orientacao - transform.position;
                    Vector3 BD = b - orientacao- transform.position;

                    ordemTrianguo.Add(AE);
                    ordemTrianguo.Add(BE);
                    ordemTrianguo.Add(AD);

                    ordemTrianguo.Add(BE);
                    ordemTrianguo.Add(BD);
                    ordemTrianguo.Add(AD);


                }
            }
            gerarmalha();
        }
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        public List<Vector3> ordemTrianguo = new List<Vector3>();
        public void gerarmalha()
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
            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
                if (meshFilter == null)
                {
                    meshFilter = gameObject.AddComponent<MeshFilter>();
                }
            }
            meshFilter.mesh = mesh;

            // Atribuir um material (pode ajustar conforme necessário)
            if (gerarMalha)
            {
                 meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    meshRenderer = gameObject.AddComponent<MeshRenderer>();
                }
                meshRenderer.material = new Material(Shader.Find("Standard"));
            }


        }

    }

    [CustomEditor(typeof(GerenciadorDeArea))]
    public class EditorGerenciadorDeArea  : Editor
    {
       // public SerializedProperty aaaaaaaaa;
        void OnEnable()
        {
          //  aaaaaaaaa = serializedObject.FindProperty("aaaaaaaaa");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
          //  EditorGUILayout.PropertyField(aaaaaaaaa, new GUIContent("aaaaaaaaaaaaaaaaaa"));
            GerenciadorDeArea meuScript = (GerenciadorDeArea)target;
            if (GUILayout.Button("Testar"))
            {
              meuScript.verificarSeEstaFechado();

            }
            if (GUILayout.Button("Criar arredor"))
            {

                meuScript.calcularAoReddor();
            }
            if (GUILayout.Button("Criar pontos internos"))
            {
                meuScript. adicionarPontosInternos();
                    }

            if (GUILayout.Button("Criar caminhos"))
            {
                meuScript.criarCaminhos();
            }
            if (GUILayout.Button( " renderizar malha"))
            {
               
                    meuScript.criarMalha();
                
               
            }


                serializedObject.ApplyModifiedProperties();
        }
        private void OnSceneGUI()
        {
            GerenciadorDeArea meuScript = (GerenciadorDeArea)target;
            if (meuScript.GuizmosAoRedor) { 
            if (meuScript.PontosEmVolta != null)
            {
                    foreach (var a in meuScript.PontosEmVolta)
                    {
                       
                            Handles.color = Color.red;
                            Handles.DrawSolidDisc(a, Vector3.up, 10);
                        
                    }
                }
            }
            if (meuScript.GuizmosInterno)
            {
                if(meuScript.pontosInternos.Count > 0)
                {
                    foreach (var a in meuScript.pontosInternos)
                    {

                        Handles.color = Color.blue;
                        Handles.DrawSolidDisc(a, Vector3.up, 10);

                    }
                }
            }

            if (meuScript.guizmosLinhasCaminho)
            {
                if (meuScript.pontosINternos.Count > 0)
                {
                    foreach(var a in meuScript.pontosINternos)
                    {
                        foreach(var b in a.linhas)
                        {
                            Handles.color = Color.red;
                            Handles.DrawLine(a.origem, b,5);
                        }
                    }
                }
                {

                }
            }
            }

        }
    }
