using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItensA;
using TMPro;
namespace ATransmut
{
    public class auxResumo : MonoBehaviour
    {
        public TextMeshProUGUI nomeDoItem,botao;
        public GeralIten data;
        public GameObject ListaDeRecurso_, prefabNomeListaRecurso;
       List<GameObject> listaDeRecurso = new List<GameObject>();
        public bool craft;
        public List<receita> listareceita = new List<receita>();
        public class receita {
            public GeralIten data;
            public int quantidade;
            public TextMeshProUGUI gm;
            public receita(GeralIten a, TextMeshProUGUI gm)
            {
                data = a;
                quantidade = 1;
                this.gm = gm;
            }
        }

        public void receba(GeralIten a , bool b)
        {
            craft = b;
            if (a != data)
            {
                while(listaDeRecurso.Count > 0)
                {
                    Destroy(listaDeRecurso[0]);
                    listaDeRecurso.RemoveAt(0);

                }

                if (b)
                {
                    botao.text = "Fazer";

                    if (a.receita.Count > 0)
                    {
                        for (int x = 0; x < a.receita.Count; x++)
                        {
                            if (listareceita.Exists(c => c.data == a.receita[x]))
                            {
                                receita aux = listareceita.Find(c => c.data == a.receita[x]);//.quantidade++;
                                aux.quantidade++;
                                aux.gm.text = aux.data.name + " " + aux.quantidade;
                            }
                            else
                            {


                                GameObject aux_ = Instantiate(prefabNomeListaRecurso, ListaDeRecurso_.transform);

                                TextMeshProUGUI aux_b = aux_.GetComponentInChildren<TextMeshProUGUI>();
                                aux_b.text = a.receita[x].name + " " + 1;
                                listaDeRecurso.Add(aux_);

                                listareceita.Add(new receita(a.receita[x], aux_b));

                            }
                        }
                    }
                }

                else
                {
                    GameObject aux_ = Instantiate(prefabNomeListaRecurso, ListaDeRecurso_.transform);

                    aux_.GetComponentInChildren<TextMeshProUGUI>().text = "custo  " + a.valor;
                    listaDeRecurso.Add(aux_);
                    botao.text = "Comprar";
                }
            }


            nomeDoItem.text = a.name;
            data = a;

        }
    }
}
