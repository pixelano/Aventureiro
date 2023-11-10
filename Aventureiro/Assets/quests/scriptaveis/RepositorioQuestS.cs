using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestsA
{
    [CreateAssetMenu(fileName = "NovoRepositorio", menuName = "Quests/Quest repositorio", order = 1)]

    public class RepositorioQuestS : ScriptableObject
    {
        public List<Quest> quests;
    }
}
