using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JogadorXQuestsA
{
    public class JogadorXQuests : MonoBehaviour
    {
        public static JogadorXQuests instance;
        public List<int> listaClicks = new List<int>();

        private void Awake()
        {
            if (transform.name != "JogadorXQuests" && instance == null)
            {
                GameObject aux = GameObject.CreatePrimitive(PrimitiveType.Cube);
                aux.transform.name = "JogadorXQuests";
                Destroy(aux.GetComponent<MeshRenderer>());
                Destroy(aux.GetComponent<MeshFilter>());
                Destroy(aux.GetComponent<BoxCollider>());
                JogadorXQuests aux_ = aux.AddComponent<JogadorXQuests>();
                instance = aux_;

                Destroy(this);
            }
            else
            {
                if (transform.name != "JogadorXQuests")
                {
                    Destroy(this);
                }

            }
           
        }
        public void clikou(int a)
        {
            listaClicks.Add(a);
        }
    }
}
