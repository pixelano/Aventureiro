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
        public List<DerrotarMonstro> monstr;
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
            public int quantidadeInicial;

        }
        public struct TodosOsMonstros
        {
            public DerrotarMonstro monstro;
            public int quantidade;
        }
        private void Start()
        {

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
            if (ListaQuests.Count > 0)
            {
                if (ListaQuests.Exists(x => x.quest_.ID_ == a) || QuestsCompletas.Exists(x => x.quest_.ID_ == a))
                {
                    ListaQuests.Remove(ListaQuests.Find(x => x.quest_.ID_ == a));
                }
                else
                {
                    gQuest aux = new gQuest();
                    aux.quest_ = rps.quests.Find(x => x.ID_ == a);
                    ListaQuests.Add(aux);
                }
            }
            jq.listaClicks.RemoveAt(0);
        }
        public bool completouQuestItens(GeralIten a, int quantidade)
        {
            return inventario_.temecItem(a, quantidade);

        }
        public bool completouQuestMonstros(DerrotarMonstro a, int b, gQuest c)
        {
            if (c.quantidadeInicial == 0)
            {
                c.quantidadeInicial = monstros.Find(x => x.monstro == a).quantidade;
            }

            if (monstros.Find(x => x.monstro == a).quantidade >= c.quantidadeInicial + b)
            {
                return true;
            }
            return false;
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
            bool Qmonstro = false;
            foreach (var aux in a.quest_.problema.monstros)
            {
                Qmonstro = completouQuestMonstros(aux.mosntro, aux.quantidade, a);
                if (Qmonstro == false)
                    break;
            }
            Qmonstro = a.quest_.problema.monstros.Count == 0 ? true : Qmonstro;
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