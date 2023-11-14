using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JogadorA
{
    [CreateAssetMenu(fileName = "SaveInventario", menuName = "Save/Save inventario", order = 1)]
    public class SalvarInventario : ScriptableObject
    {
        public List<slotInventario> inventarioLista ;
        public float dinheiro;
    }
}
