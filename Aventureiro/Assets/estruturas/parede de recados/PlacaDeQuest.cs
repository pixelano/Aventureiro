using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using QuestsA;
namespace Estruturas
{
    public class PlacaDeQuest : MonoBehaviour
    {
        public TextMeshPro txt;
        public Quest q;
        public void iniciarPlaca(string a)
        {
            txt.text = a;
        }
    }
}