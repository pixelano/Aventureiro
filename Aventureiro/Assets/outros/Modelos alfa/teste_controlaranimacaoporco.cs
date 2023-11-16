using ATransmut;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teste_controlaranimacaoporco : MonoBehaviour
{
    public Animator animador;
    public float velocidade;

    public string nomeParaCaminhar;
    public CharacterController controller;
    public bool empe,caminhando,correr,atacar;
    Vector3 offset_pontoFocal;
    public Transform pontoFocal,pontocabeca;
    public float distancia,velocidadeRotacao,outravelocidadeRotacao,rotacaoCabeca,velocidadeCorrer;
    private float correrVel =1;
    private void Start()
    {
        offset_pontoFocal =  controller.transform.TransformDirection(pontoFocal.position);
    }
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // "correndo"     "atacar"
        Vector3 bb = pontoFocal.position;
        bb.y = 4;
        pontocabeca.position = bb;
        // Calcula o vetor de movimento com base na entrada
        Vector3 moveDirection = controller.transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput)) * velocidade * correrVel;
        moveDirection.y = 0;
        // modificar todo esse bloco a variavel em 1 esta livre

        /*
         * 
         * esta 
         * 
         * 
         */
        if(moveDirection != Vector3.zero)
        {
            animador.SetBool(0, true);
            if (VerificarAnimacaoConcluida(nomeParaCaminhar))
            {
                empe = true;

                animador.SetBool(1, true);
                
            }
            caminhando = true;
        }
        else
        {
            caminhando = false;
        }
        if (empe)
        {
            if (caminhando) { 
                controller.Move(moveDirection * Time.deltaTime);
            Vector3 aux = pontoFocal.position;
            aux.y = 0;
            pontoFocal.position = Vector3.Lerp(controller.transform.position + (controller.transform.forward * distancia) + (controller.transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput) * outravelocidadeRotacao)), pontoFocal.position, Time.deltaTime);
            // pontoFocal.RotateAround(controller.transform.position, Vector3.up, horizontalInput);
            Quaternion aux_ = Quaternion.LookRotation(pontoFocal.position - controller.transform.position);

            controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, aux_, Time.deltaTime / velocidadeRotacao);
        }
                }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            correr = true;
            correrVel = velocidadeCorrer;


        }
        else
        {
            correrVel = 1;
            correr = false;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            atacar = true;
        }
        else
        {
            atacar = false;
        }

        animador.SetBool(2, caminhando);
        animador.SetBool(3, correr);
        animador.SetBool(4, atacar);
       
    }

    private bool VerificarAnimacaoConcluida(string nomeAnimacao)
    {
        // Obtém informações sobre o estado atual da animação
        AnimatorStateInfo stateInfo = animador.GetCurrentAnimatorStateInfo(0);

        // Verifica se a animação específica está ativa e se chegou ao final
        return stateInfo.IsName(nomeAnimacao) && stateInfo.normalizedTime >= 1.0f;
    }
}
