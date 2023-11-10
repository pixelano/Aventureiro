using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestsA
{
    [CreateAssetMenu(fileName = "NovaQuest", menuName = "Quests/Quest", order = 1)]

    public class Quest : ScriptableObject
    {
        public Narrativa narrativa;
        public Problema problema;
        public int ID_;
    }
}