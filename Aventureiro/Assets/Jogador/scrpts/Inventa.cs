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



        void adicionarItem(GeralIten a)
        {
            if (inventarioLista.Exists(x => x.iten == a))
            {

                slotInventario aux = inventarioLista.Find(x => x.iten == a);//.adicionar(1);

                aux.quantidade++;
            }
            else
            {
                slotInventario aux = new slotInventario(a);

                aux.adicionar(1);
                inventarioLista.Add(aux);
            }
        }
        public void ColetarItem(GameObject rh)
        {
            adicionarItem(rh.GetComponent<DropadorItem>().iten);
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