using System.Collections;
using System.Collections.Generic;
using Ageral;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace montros
{
    public class MovimentacaoAEstrela : MonoBehaviour
    {
        public Terrain terreno; 
        public GameObject terreno_;
        public Vector3 target, target_;

        public List<Vector3> caminho = new List<Vector3>();
        [HideInInspector]
        public List<Vector3> aaa = new List<Vector3>();
        [HideInInspector]
        public List<Vector3> abbb = new List<Vector3>();
        public Vector3 rota;


        public float distanciaMaximaASePercorrer;


        private bool calcularRota_b; // remover isso

        public float alturaDoRaycast, distanciaVisao,escala;

        public LayerMask layersColisores;
        public int MaximoDeNodes;
        public bool correndo,caminhando_;
        public float bonus_corrida= 2,DistanciaParaCorrer = 8;
        public float distanciaParaDiminuirVelocidade,distandiaDoAlvo;

        private CharacterController controller;

       
        void Start()
        {
            controller = GetComponent<CharacterController>();
            RaycastHit hit;
            Physics.Raycast(transform.position,-transform.up,out hit);
            if (hit.collider)
            {
                if (hit.collider.GetComponent<Terrain>())
                {
                    terreno = terreno == null ? FindAnyObjectByType<Terrain>() : terreno;
                }
                else
                {
                    terreno_ = hit.collider.gameObject;
                }
            }
          
            else
            {
                Debug.LogError(transform.name + "    " + transform.position + "   não encontrou um chão");
            }
           
            distanciaMaximaASePercorrer = distanciaMaximaASePercorrer <=0 ? 100 : distanciaMaximaASePercorrer;
            alturaDoRaycast = alturaDoRaycast <=0 ? 1 : alturaDoRaycast;
            distanciaVisao = distanciaVisao <= 1 ? 3 : distanciaVisao;
            escala = escala < 0.5f ? 2 : escala;
            if(layersColisores == 0){
                Debug.LogError(" não foi atribuido o layer dos colisores");
            }
            MaximoDeNodes = MaximoDeNodes < 10 ?90 :MaximoDeNodes;
         
            distanciaParaDiminuirVelocidade = distanciaParaDiminuirVelocidade < 1 ? 4 : distanciaParaDiminuirVelocidade;


        }
      List<Vector3 >converterParaVetor(List<node> a)
        {
            List<Vector3> bb = new List<Vector3>(); 
            if (a != null)
            {
               
                foreach (node c in a)
                {
                    bb.Add(c.local);
                }
                return bb;
            }
            else
            {
                Debug.Log("deu nulo essa merda");
                bb.Add(transform.position);
                return bb;
            }
        }
        public void movimentarParaS(Vector3 a){
           
            if (a != target_)
            {
                if (Vector3.Distance(a,target_) > escala) 
                {
                    target = a;
                    target.y = transform.position.y;
                    target_ = a;
                    calcularRota_b = true;
                }
            }
      
        }
        public void movimentarPara(Vector3 a)
        {
            
                target = a;
                target_ = a;
                calcularRota_b = true;
           
        }
        public void Parar()
        {
           // caminho.Clear();
            target = transform.position;
            target.y = transform.position.y;
            target_ = transform.position;
            calcularRota_b = true;
        }
        public void pare(){
         caminho.Clear();   
        }
        private void Update()
        {
            

           if(calcularRota_b)
            {

                try
                {
                    List<node> aux_c = calcularRota(target, layersColisores);
                    if (aux_c.Count == 1) {
                        caminho = (converterParaVetor(aux_c));
                    }
                    else
                    {


                   
                    caminho = OptimizePath(converterParaVetor(aux_c));
                    }
                    rota = caminho[0];
                    calcularRota_b = false;
                }
                catch {
                   
                        caminho = (converterParaVetor(calcularRota(target, layersColisores)));

                        rota = caminho[0];
                        calcularRota_b = false;
                 
                }
                aaa.Clear();
                abbb.Clear();
            }
            distandiaDoAlvo = Vector3.Distance(transform.position, target);
            correndo = distandiaDoAlvo > DistanciaParaCorrer;
auxVelocidade =Mathf.Lerp(auxVelocidade,(distandiaDoAlvo <= distanciaParaDiminuirVelocidade ? ValoresUniversais.VelocidadeDeCaminhadaPortePequeno + 3 : ValoresUniversais.VelocidadeDeCaminhadaPortePequeno),
    0.5f);
            caminhando_ = caminho.Count == 0 ? false : distandiaDoAlvo <= 1 ? false : true;
            if (correndo)
                auxVelocidade *= bonus_corrida;

            if (caminho.Count >0)
            {
                rota = attNode();
                if (rota != null)
                {
                      //  transform.position = AjustarAlturaChaoS(                            Vector3.Lerp(                                transform.position, (                                Vector3.Normalize(rota - transform.position) * auxVelocidade) + transform.position, Time.deltaTime));
                    Vector3 moveDirection = Vector3.Normalize(rota - transform.position) * auxVelocidade;
                    moveDirection.y -= ValoresUniversais.gravidade;
                    controller.Move(moveDirection * Time.deltaTime);
                }
            }
            else
            {
                // transform.position = AjustarAlturaChaoS(transform.position);

                Vector3 moveDirection = Vector3.zero;
                moveDirection.y -= ValoresUniversais.gravidade;
                controller.Move(moveDirection * Time.deltaTime);
            }

            if(caminho.Count <= 1)
            {
                chegou = true;
            }
            else
            {
                chegou = false;
            }
        }
        float auxVelocidade;
        public Vector3 AjustarAlturaChao(Vector3 a)
        {
            Vector3 local = a;
          local.x = (int)(((int)(local.x / escala)) * escala);
           local.z = (int)(((int)(local.z / escala)) * escala);
           if(terreno){
            local.y = (int)(((int)(terreno.SampleHeight(a) / escala)) * escala);
            }else{
                   RaycastHit hit;
            Physics.Raycast(transform.position,-transform.up,out hit);

  local.y = (int)(((int)(hit.point.y / escala)) * escala);
            }
            return local;
        }
        public Vector3 AjustarAlturaChaoS(Vector3 a)
        {
            Vector3 local = a;

            if(terreno){
            local.y = terreno.SampleHeight(a);
            }else{
                   RaycastHit hit;
            Physics.Raycast(transform.position,-transform.up,out hit);

                local.y = (hit.point.y / escala);
            }

          
            return local;
        }
    
       
        public Vector3 attNode()
        {
            Vector3 aux = AjustarAlturaChaoS(rota);

             if (Vector3.Distance(aux,   (transform.position)) < 1 )
            {
                caminho.Remove(rota);
                if (caminho.Count == 0)
                    return transform.position;
           
                return caminho[0];
            }
            return rota;
        }
        public bool chegou;
        public List<node > calcularRota(Vector3 destino,LayerMask lms)
        {
            List<node> caminhosPossiveis = new List<node>();
            caminhosPossiveis.Add(new node(AjustarAlturaChao(transform.position),null));
            int contNode = 0;
          while(caminhosPossiveis.Count > 0)
            {

                
                if(Vector3.Distance(transform.position , target_) < distanciaVisao || Physics.Raycast(transform.position,target_ - transform.position,distanciaVisao,layersColisores))
                {
                    caminhosPossiveis.Clear();
                    
                    caminhosPossiveis.Add(definirUltimo(null));
                    return caminhosPossiveis;
                }
              
                node inicial = caminhosPossiveis[0];

                for(int x= 0; x < caminhosPossiveis.Count; x++)
                {
                    if(caminhosPossiveis[x].pontuacao < inicial.pontuacao)
                    {
                        inicial = caminhosPossiveis[x];
                    }
                }
                caminhosPossiveis.Remove(inicial);
               
                    if (Vector3.Distance(AjustarAlturaChao( inicial.local),AjustarAlturaChao(destino)) < 1 * escala)
                {
                    inicial = definirUltimo(inicial);
                    List<node> reverso = new List<node>();
                    while(inicial != null)
                    {
                        reverso.Add(inicial);
                        inicial = inicial.origem;
                    }
                    reverso.Reverse();
                    return reverso;
                }
                if (MaximoDeNodes != 0)
                {
                    if (contNode >= MaximoDeNodes)
                    {
                        List<node> reverso = new List<node>();
                        while (inicial != null)
                        {
                            reverso.Add(inicial);
                            inicial = inicial.origem;
                        }
                        reverso.Reverse();
                        Debug.Log("bbbbb");
                        return reverso;
                    }

                    contNode++;
                }
                List<node> aoredor = testarLocais(inicial,lms);
                List<node> novos = new List<node>();
                List<node> ajustar = new List<node>();

                foreach (node aa in aoredor)
                {
                    int pontosQ = (int)(inicial.Qpontos +1);
                  

                    if (!caminhosPossiveis.Exists(x=> x.local == aa.local))
                    {
                        aa.calcular(pontosQ,AjustarAlturaChao(destino));
                        novos.Add(aa);
                    }
                   
                 
                }
               caminhosPossiveis.AddRange(novos);
                foreach(node aa in ajustar)
                {
                    node bb = caminhosPossiveis.Find(x=>x.local == aa.local);
                    bb.atualizar(aa);
                }



            }
            Debug.Log("aaa");
            return null;
        }
  
        public node definirUltimo(node a)
        {
            if (a != null)
            {
                RaycastHit hit;
                Physics.Raycast(a.local, target_ - a.local, out hit, distanciaVisao);
                if (hit.collider != null)
                {
                    Vector3 maisperto = hit.point;

                    node novo = new node(maisperto, a);
                    return novo;
                }
                else
                {
                    Vector3 maisperto = target_;

                    node novo = new node(maisperto, a);
                    return novo;
                }

            }
            else
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, target_ - transform.position, out hit, distanciaVisao,layersColisores);
                if (hit.collider != null)
                {
                    Vector3 maisperto = hit.point;

                    node novo = new node(maisperto, null);
                    return novo;
                }
                else
                {
                    Vector3 maisperto = target_;

                    node novo = new node(maisperto, null);
                    return novo;
                }
            }
        }

        public List<node> testarLocais(node origem,LayerMask mlks)
        {
            List<node> testados = new List<node>();
            Vector3 localOrigem =  origem.local;
            localOrigem.y += alturaDoRaycast;

            for(int x= -1; x <2; x++)
            {
                for(int z = -1; z < 2; z++)
                {
                    if (x == 0 && z == 0)
                        continue;
                   
                    Vector3 olharpara = (new Vector3(x, 0, z ));
                    olharpara.y = localOrigem.y;
                    float aux_ = Vector3.Distance(olharpara + transform.position, transform.position);
                    if (aux_ >= distanciaMaximaASePercorrer)
                    {
                      
                        continue;
                    }
                    Ray raio = new Ray(localOrigem, olharpara);
                    RaycastHit hit;
                    Physics.Raycast(raio, out hit, distanciaVisao * 1.5f, mlks);
                    if(hit.collider == null)
                    {
                        aaa.Add(AjustarAlturaChao((new Vector3(x, 0, z) * escala) + localOrigem));
                        testados.Add(new node(AjustarAlturaChao((new Vector3(x, 0, z) * escala) + localOrigem), origem));
                    }
                    else
                    {
                        abbb.Add(AjustarAlturaChao((new Vector3(x, 0, z) * escala) + localOrigem));
                    }
                }
            }

            return testados;
        }

    [System.Serializable]
        public class node   
        {

            public Vector3 local;
            public node origem;

            public int Qpontos;
            public float DistanciaParaChegada;
            public float  pontuacao;
            public node(Vector3 a,node orig)
            {
                local = a;
                origem = orig;
            }
            public void calcular(int Qpontos_, Vector3 destino)
            {
                Qpontos = Qpontos_;
                DistanciaParaChegada = Vector3.Distance(local, destino);
                pontuacao = Qpontos + DistanciaParaChegada;

            }
            public void atualizar(node aa)
            {
                   Qpontos = aa.Qpontos;
           DistanciaParaChegada = aa.DistanciaParaChegada;
            pontuacao = aa.pontuacao;
        }
        }
        public static List<Vector3> OptimizePath(List<Vector3> originalPath)
        {
            if (originalPath.Count < 3)
            {
                return originalPath;
            }

            List<Vector3> optimizedPath = new List<Vector3>();
            optimizedPath.Add(originalPath[0]);

            for (int i = 1; i < originalPath.Count - 1; i++)
            {
                Vector3 previousPoint = optimizedPath[optimizedPath.Count - 1];
                Vector3 currentPoint = originalPath[i];
                Vector3 nextPoint = originalPath[i + 1];

                if (!ArePointsCollinear(previousPoint, currentPoint, nextPoint))
                {
                    optimizedPath.Add(currentPoint);
                }
            }

            optimizedPath.Add(originalPath[originalPath.Count - 1]);

            return optimizedPath;
        }

        public static bool ArePointsCollinear(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector2 ab = new Vector2(b.x - a.x, b.z - a.z);
            Vector2 ac = new Vector2(c.x - a.x, c.z - a.z);

            float crossProduct = ab.x * ac.y - ab.y * ac.x;

            // Se o produto cruzado for quase zero, os pontos est�o em linha reta.
            return Mathf.Approximately(crossProduct, 0);
        }
    

}
}
