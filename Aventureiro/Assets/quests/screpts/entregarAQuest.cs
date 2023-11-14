using JogadorA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestsA
{
    public class entregarAQuest : MonoBehaviour
    {
        public GerenciadorQuestJogador gqj;
        void Update()
        {
        
            if(Input.GetKeyDown(KeyCode.J)) {

                foreach (var a in gqj.QuestsCompletas)
                {
                    Inventa.instance.dinheiro += a.quest_.ouroRecompensa;
                    for (int x = 0; x < a.quest_.problema.itens.Count; x++)
                    {
                        Inventa.instance.diminuir(a.quest_.problema.itens[x].item, a.quest_.problema.itens[x].quantidade);
                    }
                    foreach (var b in a.quest_.recompensa)
                    {
                        Inventa.instance.adicionarItem(b);
                    }
                }
                gqj.QuestsCompletas.Clear(); 

            }
        }
    }
}
