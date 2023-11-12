using Ageral;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JogadorA
{
    public class CombateJogador : MonoBehaviour
    {

        public LayerMask layer;
        public float disntacia;
        public float tempodeRecarga;
        float aux_tempoDeRecarga;
 
        void Update()
        {
            if (aux_tempoDeRecarga <= 0)
            {
                aux_tempoDeRecarga = tempodeRecarga;
                if (Input.GetMouseButton(0))
                {
                    RaycastHit hit;
                    Physics.BoxCast(transform.position, Vector3.one / 2, transform.forward, out hit, Quaternion.identity, disntacia, layer);
                    if (hit.collider != null)
                    {
                        GerenciadoDeVida aux = hit.collider.GetComponent<GerenciadoDeVida>();
                        if (aux != null)
                        {
                            aux.diminuirVida(1);
                        }

                    }
                }
            }
            else
            {
                aux_tempoDeRecarga -= Time.deltaTime;
            }
        }
    }
}
