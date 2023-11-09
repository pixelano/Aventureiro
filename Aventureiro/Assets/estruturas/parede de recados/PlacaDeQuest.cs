using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlacaDeQuest : MonoBehaviour
{
    public TextMeshPro txt;
    public Quest q;
   public void iniciarPlaca(Texto a)
    {
        txt.text = a.text;
    }
}
