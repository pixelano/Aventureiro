using JogadorXQuestsA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestsA
{
    public class RepositorioQuests : MonoBehaviour
    {
   

        public  List<Quest> quests = new List<Quest>();
        public RepositorioQuestS rtqs;
        public static RepositorioQuests instance;
        public bool main_;
        private void Awake()
        {
            if(main_ ) { instance = this; }
            if (rtqs != null)
            {
                quests.AddRange(rtqs.quests);
            }
            quests.ForEach(x => x.ID_ = x.GetInstanceID());

        }
    }
}
