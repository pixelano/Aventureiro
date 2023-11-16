using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItensA;
using JogadorA;
using JogadorXQuestsA;
using Unity.Plastic.Newtonsoft.Json.Schema;

namespace QuestsA
{
    public class GerenciadorQuestJogador : MonoBehaviour
    {
        public List<DerrotarMonstro> monstr = new List<DerrotarMonstro>();
        public SalvarListaDerrotarMonstros salvarListaMonstros ;
        public List<TodosOsMonstros> monstros = new List<TodosOsMonstros>();

        public List<gQuest> ListaQuests = new List<gQuest>();
        public List<gQuest> QuestsCompletas = new List<gQuest>();
        public Inventa inventario_;
        public JogadorXQuests jq;
        RepositorioQuests rps;
        [System.Serializable]
        public struct gQuest
        {
            public Quest quest_;
            public List<TodosOsMonstros> quantidadeInicial ;

        }
        [System.Serializable]
        public struct TodosOsMonstros
        {
            public DerrotarMonstro monstro;
            public int quantidade,quantidadeAtual;
        }
      
        private void Start()
        {
        monstr = salvarListaMonstros.monstr;
            gameObject.AddComponent< JogadorXQuests>();
            try
            {
                rps = RepositorioQuests.instance;
            }
            catch
            { Debug.LogError("faltou definir o repositorio de quests"); }
                jq = JogadorXQuests.instance;

                foreach (DerrotarMonstro a in monstr)
            {
                TodosOsMonstros aa = new TodosOsMonstros();
                aa.monstro = a;
                monstros.Add(aa);
            }
        }

        public void clikou(int a)
        {
            Debug.Log("clikou");

            if (ListaQuests.Exists(x => x.quest_.ID_ == a) )
            {
                ListaQuests.Remove(ListaQuests.Find(x => x.quest_.ID_ == a));
            }else if (QuestsCompletas.Exists(x => x.quest_.ID_ == a))
            {

            }
            else
            {

                gQuest aux = new gQuest();
                aux.quest_ = rps.quests.Find(x => x.ID_ == a);
                aux.quantidadeInicial = new List<TodosOsMonstros>();
              
                if (aux.quest_.problema.monstros.Count > 0)
                {

                    foreach (var v in aux.quest_.problema.monstros)
                    {
                        TodosOsMonstros aux_x = new TodosOsMonstros();
                        aux_x.monstro = v.mosntro;
                        aux_x.quantidade = v.quantidade;
                        aux_x.quantidadeAtual = monstr.Find(x => x == aux_x.monstro).quantidade;

                        aux.quantidadeInicial.Add(aux_x);
                    }
                }
              
              
                ListaQuests.Add(aux);

            }
           
            jq.listaClicks.RemoveAt(0);
        }
        public bool completouQuestItens(GeralIten a, int quantidade)
        {
            return inventario_.temecItem(a, quantidade);

        }
        public bool completouQuestMonstros(gQuest c)
        {

            bool aux_ = true;
         for(int x= 0; x < c.quantidadeInicial.Count; x++)
            {
                float valorA = monstr.Find(y => y == c.quantidadeInicial[x].monstro).quantidade; // quantidade total de monstros mortos
                float valorB = c.quest_.problema.monstros.Find(j => j.mosntro == c.quantidadeInicial[x].monstro).quantidade;// quantidade necessaria
                float valorC = c.quantidadeInicial[x].quantidadeAtual;// quantos tinha quando iniciou a quest
                
                if (valorA>=
                  valorB+valorC)
                {
                 
                }
                else
                {
                    aux_ = false;
                    break;

                }
            }
          
            return aux_;
        }
        public bool completouQuest(gQuest a)
        {
            bool Qitem = false;
            foreach (var aux in a.quest_.problema.itens)
            {
                Qitem = completouQuestItens(aux.item, aux.quantidade);
                if (Qitem == false)
                    break;
            }
            Qitem = a.quest_.problema.itens.Count == 0 ? true : Qitem;
          
              bool  Qmonstro = completouQuestMonstros(a);
           
            return Qitem ? Qmonstro ? true : false : false;
        }
        float aux;
        private void Update()
        {
            if (aux > 1)
            {
                aux = 0;
                List<gQuest> aux__ = new List<gQuest>();
                foreach (gQuest a in ListaQuests)
                {
                    if (completouQuest(a))
                    {
                        Debug.Log("completou A quest");
                        aux__.Add(a);
                    }
                }
                QuestsCompletas.AddRange(aux__);
                ListaQuests.RemoveAll(x => aux__.Contains(x));
            }
            if (jq.listaClicks != null) 
            {
                if (jq.listaClicks.Count > 0)
                {
                    clikou(jq.listaClicks[0]);
                }
            }
            aux += Time.deltaTime;
        }
    }
}