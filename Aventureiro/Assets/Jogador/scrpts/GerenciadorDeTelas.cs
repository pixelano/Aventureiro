using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATransmut;
using Npcs;

namespace JogadorA
{
    public class GerenciadorDeTelas : MonoBehaviour
    {
        public GerenciadorTransmutador trans;
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            cam = Camera.main;
        }

        private void transmutar()
        {
            trans.flipflopabrir();

            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        }
        private void abrirconversa(RaycastHit a)
        {

        }
        Camera cam;
        public LayerMask npc;
        private void Update()
        {

            Ray raio = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit rh;
            Physics.Raycast(raio, out rh);
        
            if(rh.collider != null)
            {
                if(rh.collider.gameObject.layer == npc)
                {
                    abrirconversa(rh);
                }
            }
        }
    }
}
