using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestsA
{
    [CreateAssetMenu(fileName = "NovaNarrativa", menuName = "Quests/Narrativa", order = 1)]
    public class Narrativa : ScriptableObject
    {
        public Texto placa, resumo;
    }
}