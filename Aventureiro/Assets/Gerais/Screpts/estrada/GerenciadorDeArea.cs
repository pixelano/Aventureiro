using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ageral
{
    public class GerenciadorDeArea : MonoBehaviour
    {
        
        public List<gerenciadorDeEstradas> Ruas = new List<gerenciadorDeEstradas>();
        public List<Vector3> pontosAxiliares = new List<Vector3>();
        gerenciadorDeEstradas RuaAlvo;
        public bool GuizmosAoRedor,GuizmosInterno;
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

        public void calcularAoReddor()
        {
           
         foreach(var a in aaaaaaaaa)
            {
                Debug.Log(a.zero.name + "  A " + a.indiceIntercessaoA + "   B  " + a.indiceIntercessaoB );
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
                serializedObject.ApplyModifiedProperties();
        }
        private void OnSceneGUI()
        {
            GerenciadorDeArea meuScript = (GerenciadorDeArea)target;
            if (meuScript.GuizmosAoRedor) { 
            if (meuScript.aaaaaaaaa.Count > 0)
            {
                    foreach (var a in meuScript.aaaaaaaaa)
                    {
                        for (int x = a.indiceIntercessaoA; x < a.indiceIntercessaoB; x++)
                        {
                            Handles.color = Color.red;
                            Handles.DrawSolidDisc(a.zero.pontosAuxiliares[x], Vector3.up, 10);
                        }
                    }
                }
            }
       
            }

        }
    }
