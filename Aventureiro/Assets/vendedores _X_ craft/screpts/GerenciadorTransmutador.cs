using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace ATransmut
{
    public class GerenciadorTransmutador : MonoBehaviour
    {
        public transmutar atual;
        public GameObject tela,lista;
        public GameObject prefab_ListaNome;
        List<GameObject> cadaNome = new List<GameObject>();
        public auxResumo ars;
        public bool abrir;
     
        void executarLista()
        {
            if(cadaNome.Count > 0)
            {
                while(cadaNome.Count > 0)
                {
                    Destroy(cadaNome[0]);
                    cadaNome.RemoveAt(0);
                }
            }
            for(int x= 0; x < atual.para.Count; x++)
            {

                GameObject a = (Instantiate(prefab_ListaNome, lista.transform));
                TextMeshProUGUI b = a.GetComponentInChildren<TextMeshProUGUI>();
                b.text = atual.para[x].name;

                auxiliarBotao c = a.GetComponent<auxiliarBotao>();
                c.a = atual.para[x];
                c.craft = atual.craft;
                c.back = ars;
                
                cadaNome.Add(a);
            }
        }
        public void flipflopabrir()
        {
            abrir = !abrir;
            if (abrir)
                executarLista();
            tela.SetActive(abrir);
        }
    }
}
