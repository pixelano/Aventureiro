using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using QuestsA;
using JogadorA;
namespace Estruturas
{
    public class gerenciadorCursor : MonoBehaviour
    {
        public GameObject cursor;
        public GameObject camera_, teste;
        ClickarEmCoisas clk;
        public GerenciadorDeQuestsMural mural;
        public float AjustarEscala;
        float localScale_X, localScale_Y, cursor_X, cursor_Y;

        public int sairDaTela, sairDaTelaContador;
        private void Awake()
        {
            localScale_X = transform.localScale.x / 2;
            localScale_Y = transform.localScale.y / 2;
            camera_ = Camera.main.gameObject;
            clk = camera_.GetComponent<ClickarEmCoisas>();
        }
        void Update()
        {
            if (flag)
            {
                moverCursor();

                cliques();
            }
            else
            {
                Ray ray = new Ray(camera_.transform.position, camera_.transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "mural")
                    {
                        cursor.transform.position = hit.point;
                        flag = true;
                        camera_.GetComponent<LookAtConstraint>().enabled = true;

                    }

                }

            }
        }
        void cliques()
        {
            RaycastHit[] hits;
            Ray raio = new Ray(camera_.transform.position, camera_.transform.forward);
            hits = Physics.RaycastAll(raio);
            List<RaycastHit> r = new List<RaycastHit>();
            r.AddRange(hits);

            if (r.Exists(x => x.collider.GetComponent<ImouseInteracao>() != null))
            {

                ImouseInteracao aux = r.Find(x => x.collider.GetComponent<ImouseInteracao>() != null).collider.GetComponent<ImouseInteracao>();
                if (aux != null)
                {
                    Quest au = mural.essaQuest(aux.gameobject());
                    aux.Selecionando();
                    clk.mostrarResumo(au.narrativa.resumo.text);


                    if (Input.GetMouseButtonDown(0))
                    {
                        
                        aux.Clickando();
                        clk.clikou(au.ID_);
                    }
                }
                else
                {
                    clk.mostrarResumo(null);

                }
            }
            else
            {
                clk.mostrarResumo(null);

            }
        }
        float sensibilidade = 0.15f;
        void moverCursor()
        {
            float mouseX = Input.GetAxis("Mouse X") + (Input.GetAxis("Horizontal") * sensibilidade);
            float mouseY = Input.GetAxis("Mouse Y") + (Input.GetAxis("Vertical") * sensibilidade / 3);


            clk.flipflop(true);

            cursor.GetComponent<MeshRenderer>().enabled = true;
            Vector3 posicaoAgora = cursor.transform.position - transform.position;


            float distancia = Vector3.Distance(camera_.transform.position, cursor.transform.position);

            float veloc = Mathf.Abs(Mathf.Clamp(((distancia / AjustarEscala)), 0.09f, 1)) * 0.3f;
            float posicaoFinalX = posicaoAgora.x + mouseX * veloc;
            float posicaoFinalY = posicaoAgora.y + mouseY * veloc;


            if (posicaoFinalX >= localScale_X - cursor_X ||
                posicaoFinalY >= localScale_Y - cursor_Y ||
                posicaoFinalX <= -localScale_X + cursor_X ||
                posicaoFinalY <= -localScale_Y + cursor_Y)
            {
                if (mouseX != 0 || mouseY != 0)
                    sairDaTelaContador++;
            }
            else
            {
                sairDaTelaContador = sairDaTelaContador < 1 ? 0 : sairDaTelaContador - 1;
            }
            if (sairDaTelaContador >= sairDaTela)
            {
                sairDaTelaContador = 0;
                flag = false;
                camera_.GetComponent<LookAtConstraint>().enabled = false;
                clk.flipflop(false);
                cursor.GetComponent<MeshRenderer>().enabled = false;
                //this.enabled = false;
            }
            cursor_X = cursor.transform.localScale.x / 2;
            cursor_Y = cursor.transform.localScale.y / 2;

            cursor.transform.localScale = Vector3.one * (distancia / AjustarEscala);
            Vector3 posicaoFinal = new Vector3(Mathf.Clamp(posicaoFinalX, -localScale_X + cursor_X, localScale_X - cursor_X),
                Mathf.Clamp(posicaoFinalY, -localScale_Y + cursor_Y, localScale_Y - cursor_Y), -0.11f);// + Vector3.one * Mathf.Abs( Mathf.Clamp(((distancia / AjustarEscala) ),0.3f,1));


            cursor.transform.position = transform.position + posicaoFinal;
            Vector3 aux = cursor.transform.localPosition;
            aux.z = -0.02f;
            cursor.transform.localPosition = aux;



        }
        public bool flag = false;
        private void OnEnable()
        {
            flag = false;
       //   camera_.GetComponent<LookAtConstraint>().enabled = true;
            clk.flipflop(true);
            cursor.GetComponent<MeshRenderer>().enabled = true;

        }
        private void OnDisable()
        {
            flag = false;
            try
            {
                camera_.GetComponent<LookAtConstraint>().enabled = false;
                clk.flipflop(false);
                cursor.GetComponent<MeshRenderer>().enabled = false;
            }
            catch { }
        }
    }
}