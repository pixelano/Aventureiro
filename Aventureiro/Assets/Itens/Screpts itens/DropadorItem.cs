using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ItensA
{
    public class DropadorItem : MonoBehaviour
    {
        public List<GeralIten >iten;
   
        private void OnDestroy()
        {
            foreach(GeralIten aux in iten)
            {
                try
                {
                    GameObject aux_ = Instantiate(aux.modeloItem, transform.position, Quaternion.identity);
                    aux_.GetComponent<Rigidbody>().AddForce(transform.up * 5 + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * 3, ForceMode.Impulse);
                    aux_.GetComponent<ItemT>().dataItem = aux;
                }
                catch { }
                }
        }
    
    }
}