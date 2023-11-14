using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestsA
{
    public class att_contadorMortes : MonoBehaviour
    {
        public DerrotarMonstro DM;
        public void OnDestroy()
        {
            DM.quantidade++;
        }
    }
}
