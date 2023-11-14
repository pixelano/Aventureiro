using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ItensA; 

namespace JogadorA
{
    public class Inventa : MonoBehaviour
    {
        public List<slotInventario> inventarioLista = new List<slotInventario>();
        public SalvarInventario sv;
        public float dinheiro;
        public static Inventa instance;
        private void Start()
        {
            if(Inventa.instance == null)
            {
                Inventa.instance = this;
                inventarioLista = sv.inventarioLista;
                dinheiro = sv.dinheiro;
            }
        }

        public void obterItem(GeralIten a, bool b)
        {
            if (b)
            {
                foreach (GeralIten c in a.receita) {
                    diminuir(c,1);
                }
            }
            else
            {
                dinheiro -= a.valor;
            }
            adicionarItem(a);
        }



        public void adicionarItem(GeralIten a)
        {
            if (inventarioLista.Exists(x => x.iten == a))
            {

                slotInventario aux = inventarioLista.Find(x => x.iten == a);//.adicionar(1);

                aux.quantidade++;
            }
            else
            {
                slotInventario aux = new slotInventario(a);

                inventarioLista.Add(aux);
            }
        }
        public void diminuir(GeralIten a, int b)
        {
            if (inventarioLista.Exists(x => x.iten == a))
            {

                slotInventario aux = inventarioLista.Find(x => x.iten == a);//.adicionar(1);

                aux.quantidade -= b;
                if(aux.quantidade <= 0)
                {
                    inventarioLista.Remove(aux);
                }
            }
          
        }
        public void ColetarItem(GameObject rh)
        {
            adicionarItem(rh.GetComponent<ItemT>().dataItem);
            Destroy(rh);

        }
        public bool temecItem(GeralIten a, int quantidade)
        {
            if (inventarioLista.Exists(x => x.iten == a))
            {
                slotInventario aa = inventarioLista.Find(x => x.iten == a);
                return aa.quantidade >= quantidade;
            }
            else
            {
                return false;
            }
        }
    }
    [System.Serializable]
    public class slotInventario
    {
        public GeralIten iten;
        public int quantidade;

        public slotInventario(GeralIten a)
        {
            iten = a;
            quantidade = 1;
        }

        public void adicionar(int q)
        {
            quantidade = quantidade + q;
        }

    }
}