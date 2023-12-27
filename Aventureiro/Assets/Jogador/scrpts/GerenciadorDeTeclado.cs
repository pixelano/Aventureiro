using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace JogadorA
{
    public class GerenciadorDeTeclado : MonoBehaviour
    {
     
        public KeyCode paraFrente,paraTras,paraEsquerda,paraDireita,paraPular,paraCorrer;

        public KeyCode inventario , interagir, abrirMapa , diario;

        public KeyCode itemA, itemB, itemC, itemD;

        public KeyCode AtivarMagia, magiaA, magiaB, magicaC, magiaD;

        public KeyCode acao, auxiliar;

        public static GerenciadorDeTeclado instanc;
        private void Start()
        {
            if(instanc == null)
            {
                instanc = this;
            }
        }

    }
}
