using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ageral
{
    public class ValoresUniversais : MonoBehaviour
    {
        public static float gravidade;

        public static float VelocidadeDeCaminhadaPortePequeno;


        public float gravidade_;
        public float VelocidadeDeCaminhadaPortePequeno_;

        private void Update()
        {
            gravidade = gravidade_;
            VelocidadeDeCaminhadaPortePequeno = VelocidadeDeCaminhadaPortePequeno_;
        }

    }
}
