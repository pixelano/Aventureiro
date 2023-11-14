using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

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
                LookAtConstraint a = other.gameObject.transform.parent.GetComponentInChildren<LookAtConstraint>();
               ConstraintSource b = a.GetSource(0);
                b.sourceTransform =gcursor.cursor.transform;
                a.SetSource(0,b);
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