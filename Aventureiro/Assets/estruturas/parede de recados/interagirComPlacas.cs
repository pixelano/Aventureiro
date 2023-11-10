using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestsA;
using UnityEngine.UI;
using TMPro;
namespace Estruturas
{
    public class interagirComPlacas : MonoBehaviour, ImouseInteracao
    {
        public Text texto;
        public void Selecionando()
        {
            //  Debug.Log("selecionando");
        }
        public void Clickando()
        {

            //  Debug.Log("clikou");
        }
        public GameObject gameobject()
        {
            return gameObject;
        }
        public void Selecionando(Quest a)
        {

        }
    }

}