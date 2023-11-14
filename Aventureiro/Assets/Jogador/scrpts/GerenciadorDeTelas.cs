using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATransmut;
namespace JogadorA
{
    public class GerenciadorDeTelas : MonoBehaviour
    {
        public GerenciadorTransmutador trans;
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                trans.flipflopabrir();

                Cursor.lockState = Cursor.lockState ==CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }
    }
}
