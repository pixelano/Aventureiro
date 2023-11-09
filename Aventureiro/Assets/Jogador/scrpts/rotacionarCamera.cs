using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotacionarCamera : MonoBehaviour
{
    public float rotationSpeed = 2.0f; // Velocidade de rotação da câmera

    void Update()
    {
        // Obtém as entradas do mouse para controlar a rotação
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Aplica a rotação horizontal à câmera (em torno do eixo vertical)
        Vector3 aux_X = Vector3.up;
        aux_X.z = 0;
        transform.Rotate(aux_X * mouseX * rotationSpeed);

        // Aplica a rotação vertical à câmera (em torno do eixo horizontal)
        Vector3 aux_Y = Vector3.left;
        aux_X.z = 0;
        transform.Rotate(aux_Y * mouseY * rotationSpeed);

        // Limita a rotação vertical para evitar que a câmera gire completamente
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z = 0;
      //  currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);
       transform.eulerAngles = currentRotation;
    }
}
