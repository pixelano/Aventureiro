using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Estruturas
{
    public class AtivadorCursor : MonoBehaviour
    {
        private gerenciadorCursor gcursor;
        private void Start()
        {
            gcursor = GetComponent<gerenciadorCursor>();
        }
        private void OnTriggerEnter(Collider other)
        {

            if (other.tag == "Player")
            {
                gcursor.enabled = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                gcursor.enabled = false;
            }
        }
    }
}