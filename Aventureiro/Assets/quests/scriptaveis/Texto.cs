using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestsA
{
    [CreateAssetMenu(fileName = "NovoTexto", menuName = "Quests/Texto", order = 1)]
    public class Texto : ScriptableObject
    {
        public string text;
    }

}