using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropadorItem : MonoBehaviour
{
    public GeralIten iten;

    private void Start()
    {
        Instantiate(iten.modeloItem, transform.position, Quaternion.identity, transform);
    }
}
