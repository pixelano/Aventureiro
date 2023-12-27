using ItensA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JogadorA
{
    public class ColetarItem : MonoBehaviour
    {
        public Inventa inventario;
        
        public float maximoDistancia;
        private void Update()
        {

            Ray raio = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit rh;
            if (Physics.Raycast(raio, out rh, maximoDistancia))
            {
                if (rh.collider.tag == "Item")
                {
                    if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.interagir))
                    {
                        inventario.ColetarItem(rh.collider.gameObject);

                    }
                }
            }
        }
    }
}