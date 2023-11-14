using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestsA
{
    [CreateAssetMenu(fileName = "MOsntrosDerrotados", menuName = "Save/ListaMonstrosDerrotados", order = 1)]

    public class SalvarListaDerrotarMonstros : ScriptableObject
    {
        public List<DerrotarMonstro> monstr;
    }
}
