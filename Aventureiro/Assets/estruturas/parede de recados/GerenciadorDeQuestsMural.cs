using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestsA;
namespace Estruturas
{
    public class GerenciadorDeQuestsMural : MonoBehaviour
    {
        public List<Quest> listaDeQuests;
        public List<PlacaDeQuest> placas = new List<PlacaDeQuest>();
        public GameObject PrefabPlaca, PrefabResum, fundo;
        private float tamanhoX, tamanhoY;
        public int quantidadeH, quantidadeY;
        public int indiceH = -1, indiceY = -1;
        Vector3 nml;
        private void Start()
        {
            quantidadeH = (int)((fundo.transform.localScale.x - PrefabPlaca.transform.localScale.x) / PrefabPlaca.transform.localScale.x) - 2;
            quantidadeY = (int)((fundo.transform.localScale.y - PrefabPlaca.transform.localScale.y) / PrefabPlaca.transform.localScale.y) - 1;

            tamanhoX = fundo.transform.localScale.x / 2;
            tamanhoY = fundo.transform.localScale.y / 2;
            nml = fundo.transform.localScale / 2;
            nml *= -1;
            nml += PrefabPlaca.transform.localScale * 0.7f;
            nml.z = 0;
            foreach (Quest a in listaDeQuests)
            {
                PlacaDeQuest aux = Instantiate(PrefabPlaca, lugarParaPlaca(), Quaternion.identity, transform).GetComponent<PlacaDeQuest>();
                aux.iniciarPlaca(a.narrativa.placa.text);
                aux.q = a;
                placas.Add(aux);
            }
        }
        bool cheio = false;


        Vector3 lugarParaPlaca()
        {
            Vector3 f;
            if (!cheio)
            {
                if (indiceH < quantidadeH)
                {
                    if (indiceY < quantidadeY)
                    {

                        f = localIndice();
                        indiceH++;
                    }
                    else
                    {
                        f = localRandom();
                    }
                }
                else
                {


                    if (indiceY >= quantidadeY) { f = localRandom(); }
                    else
                    {
                        f = localIndice();
                    }
                    indiceH = -1;
                    indiceY++;

                }
            }
            else
            {
                f = localRandom();
            }
            return f;

        }
        public float espasamentoH, espasamentoV, ruido;
        Vector3 localIndice()
        {
            float RX = (PrefabPlaca.transform.localScale.x * (indiceH)) + Random.Range(-ruido, ruido);
            float RY = (PrefabPlaca.transform.localScale.y * (indiceY)) + Random.Range(-ruido, ruido);
            float z = Mathf.Clamp((placas.Count + 1 * 0.00001f * 13), 0.00001f, 0.0199f);
            return fundo.transform.position + new Vector3(PrefabPlaca.transform.localScale.x + RX + espasamentoH * indiceH,
                                                          PrefabPlaca.transform.localScale.y + RY + espasamentoV * indiceY, -z) + nml;
        }
        Vector3 localRandom()
        {
            cheio = true;
            float RX = Random.Range(-tamanhoX + PrefabPlaca.transform.localScale.x, tamanhoX - PrefabPlaca.transform.localScale.x);
            float RY = Random.Range(-tamanhoY + PrefabPlaca.transform.localScale.y, tamanhoY - PrefabPlaca.transform.localScale.y);
            float z = Mathf.Clamp((placas.Count * 0.00001f) + 0.0002f, 0, 0.11f);
            return fundo.transform.position + new Vector3(RX, RY, -z);
        }

        public Quest essaQuest(GameObject a)
        {
            return placas.Find(x => x.gameObject == a).q;
        }
    }
}