using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColetarItem : MonoBehaviour
{
    public Inventa inventario;
    public float maximoDistancia;
    private void Update()
    {
        
        Ray raio = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit rh;
        if(Physics.Raycast(raio,out rh, maximoDistancia))
        {
            if (rh.collider.tag == "Item")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    inventario.ColetarItem(rh.collider.transform.parent.gameObject);
                    
                }
            }
        }
    }
}
