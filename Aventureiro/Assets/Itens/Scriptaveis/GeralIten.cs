using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItensA
{
    [CreateAssetMenu(fileName = "NovoItem", menuName = "Itens/NovoItem", order = 1)]
    public class GeralIten : ScriptableObject
    {
        public GameObject modeloItem;
    }
}