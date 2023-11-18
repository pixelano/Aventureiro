using Ageral;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace montros
{
    public class GerenciadoDeAnimação : MonoBehaviour
    {

        /*
        * 0 = em alerta true = em pé parado != sentado
        * 1 = deve estar em postura para caminhar/correr ?
        * 2 = esta caminhando ou esta se movendo agora ?
        * 3 = correndo
        * 4 = atacando
        */
        public float tempoParaDescansar;
        float TempoAlerta;
        bool aux_alerta,aux_caminhar,aux_caminhando;

        bool flag_iniciar;
        public Animator animador;
        public float aux_x;
        public LookAtConstraint lkc;
        public void executar(    
         Vector3 posicao,
         MovimentacaoAEstrela movimento,
         GerenciadorDeAtaques atk,
         GerenciadoDeVida alvo,bool alerta)
        {
            if (alvo == null) { }
            else
            {

                if (aux_alerta && aux_caminhar && !atk.atacando)
                {

                    if (alvo == null)
                    {
                        //    movimento.movimentarParaS((posicao));
                        // da pra alterar o argumento recebido ?
                        alerta = false;
                    }
                    else
                    {
                        Vector3 aux_ = posicao;
                        aux_.y = 0;


                        movimento.movimentarParaS((aux_ * atk.distancia) + alvo.transform.position);

                        if (movimento.chegou)
                        {

                            aux_caminhando = atk.aux_caminhando;
                            if (atk.atacar(alvo))
                            {
                                animador.SetBool("TerminouAtaque", false);
                            }
                        }
                        else
                        {
                            // caminhando
                            aux_caminhando = true;
                        }

                    }
                }
                Quaternion aux_q;
                if (movimento.caminho.Count > 0)
                {
                    aux_q = Quaternion.LookRotation(movimento.rota - movimento.transform.position);

                }
                else
                {
                    aux_q = Quaternion.LookRotation(alvo.transform.position - movimento.transform.position);

                }
                aux_q.x = 0;
                aux_q.z = 0;
                movimento.transform.localRotation = Quaternion.Lerp(aux_q, movimento.transform.localRotation, aux_x);
                if (atk.atacando)
                {
                    if (VerificarAnimacaoConcluida("atacar"))
                    {
                        atk.executarAtaque();
                        animador.SetBool("TerminouAtaque", true);
                    }
                }
                if (!alerta && aux_alerta)
                {
                    if (TempoAlerta > tempoParaDescansar)
                    {
                        aux_alerta = false;
                        TempoAlerta = 0;
                    }
                    else
                    {

                        aux_alerta = true;
                        aux_caminhar = false;
                    }
                    TempoAlerta += Time.deltaTime;
                }
                else
                {
                    if (flag_iniciar)
                        aux_caminhar = true;
                    if (alerta)
                        aux_alerta = true;
                }
                if (!flag_iniciar)
                    flag_iniciar = VerificarAnimacaoConcluida("em alerta");



                animador.SetBool("Em alerta", aux_alerta);
                if (aux_caminhando)
                {
                    aux_caminhando = movimento.caminhando_;
                }
                if (!alerta)
                {
                    animador.SetBool("Caminhar", false);
                    animador.SetBool("caminhando", false);
                }
                else
                {
                    animador.SetBool("Caminhar", aux_caminhar);
                    animador.SetBool("caminhando", aux_caminhando);
                }
                animador.SetBool("Correr", movimento.correndo);
                animador.SetBool("atacar", atk.atacando);



                if (animador.GetBool("Em alerta") == true)
                {

                    if (Vector3.Distance(alvo.transform.position - transform.position, (posicao * atk.distancia)) < tolerancia)
                    {

                        lkc.enabled = true;
                        lkc.roll = 176f;
                        lkc.weight = 1;
                    }
                    else
                    {

                        lkc.weight = 0;

                        lkc.enabled = false;
                    }
                }
                else
                {
                    lkc.weight = 0;

                    lkc.enabled = false;
                }
            }
        }
        public float tolerancia;
        private bool VerificarAnimacaoConcluida(string nomeAnimacao)
        {
            // Obtém informações sobre o estado atual da animação
            AnimatorStateInfo stateInfo = animador.GetCurrentAnimatorStateInfo(0);

            // Verifica se a animação específica está ativa e se chegou ao final
            return stateInfo.IsName(nomeAnimacao) && stateInfo.normalizedTime >= 1.0f;
        }
    }
}
