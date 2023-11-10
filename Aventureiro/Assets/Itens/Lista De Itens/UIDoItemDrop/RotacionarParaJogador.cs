using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
namespace ItensA
{
    public class RotacionarParaJogador : MonoBehaviour
    {
        Transform jogador;
        public LookAtConstraint lk;
        void Start()
        {
            jogador = Camera.main.transform; 

            ConstraintSource a = new ConstraintSource();
            a.sourceTransform = jogador;
            a.weight = 1;
            lk.AddSource(a);



        }
    }
}
