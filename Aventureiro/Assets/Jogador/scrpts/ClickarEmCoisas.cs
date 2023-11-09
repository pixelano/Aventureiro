using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClickarEmCoisas : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public GameObject rm;
    public GerenciadorQuestJogador gq;
    private void Start()
    {
        gq = FindObjectOfType<GerenciadorQuestJogador>();
    }
    public void mostrarResumo(Quest a)
    {
        if (a != null)
        {
            txt.text = a.narrativa.resumo.text;
        }
        else
        {
            txt.text = "";
        }
    }
    public void flipflop(bool a)
    {
        rm.SetActive ( a);
            
            }
    public void clikou(Quest a)
    {
        gq.clikou(a);
    }

}
