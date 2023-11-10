using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JogadorXQuestsA;


namespace JogadorA
{
    public class ClickarEmCoisas : MonoBehaviour
    {
        public TextMeshProUGUI txt;
        public GameObject rm;
        public JogadorXQuests gq;
        private void Start()
        {
            gameObject.AddComponent<JogadorXQuests>();
            gq = JogadorXQuests.instance;
        }
        public void mostrarResumo(string a)
        {
            if (a != null)
            {
                txt.text = a;
            }
            else
            {
                txt.text = "";
            }
        }
        public void flipflop(bool a)
        {
            rm.SetActive(a);

        }
        public void clikou(int a)
        {
            gq.clikou(a);
        }

    }
}