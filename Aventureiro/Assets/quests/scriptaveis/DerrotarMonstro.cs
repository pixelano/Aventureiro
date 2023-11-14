using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using montros;
namespace QuestsA
{
    [CreateAssetMenu(fileName = "NovoProblemaDerrotarMonstros", menuName = "Monstros/DerrotarMonstro_", order = 1)]


    public class DerrotarMonstro : ScriptableObject
    {
        public int quantidade;
        public Sinimigo data;
    }
}