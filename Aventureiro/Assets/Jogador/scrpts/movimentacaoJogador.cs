using PlasticGui.WebApi.Responses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ageral;
namespace JogadorA
{
    public class movimentacaoJogador : MonoBehaviour
    {
      
        private CharacterController controller; // Referência ao CharacterController

        void Start()
        {
        //    Cursor.visible = true;
          //  Cursor.lockState = CursorLockMode.Locked;
            controller = GetComponent<CharacterController>(); // Obtém a referência do CharacterController no objeto
        }
        public float PuloMaximo,MultiplicadorDePulo;
        float aux_pulo;
        bool flag_pulo;
        void Update()
        {
            // Obtém entrada do jogador para movimento
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calcula o vetor de movimento com base na entrada
            Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput));
            moveDirection *= ValoresUniversais.VelocidadeDeCaminhadaPortePequeno;

            // Aplica a gravidade
            moveDirection.y -= ValoresUniversais.gravidade;

            // Move o CharacterController

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (flag_pulo == false)
                {
                    aux_pulo += ValoresUniversais.gravidade;
                }
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (flag_pulo == false)
                {
                    aux_pulo = aux_pulo > PuloMaximo + ValoresUniversais.gravidade ? PuloMaximo + ValoresUniversais.gravidade : aux_pulo + (Time.deltaTime * MultiplicadorDePulo);
                }
            }
            else
            {
                flag_pulo = true;
                if (aux_pulo <= 0)
                {

                    flag_pulo = false;

                }

            }

            if (flag_pulo)
            {
                moveDirection.y += aux_pulo;
                aux_pulo -= Time.deltaTime * ValoresUniversais.gravidade;
            }

            controller.Move(moveDirection * Time.deltaTime);
            
        }
    }
}