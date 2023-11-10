using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JogadorA
{
    public class movimentacaoJogador : MonoBehaviour
    {
        public float moveSpeed = 5.0f; // Velocidade de movimento
        private CharacterController controller; // Referência ao CharacterController

        void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
            controller = GetComponent<CharacterController>(); // Obtém a referência do CharacterController no objeto
        }

        void Update()
        {
            // Obtém entrada do jogador para movimento
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calcula o vetor de movimento com base na entrada
            Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput));
            moveDirection *= moveSpeed;

            // Aplica a gravidade
            moveDirection.y -= 9.8f;

            // Move o CharacterController
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}