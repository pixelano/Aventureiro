using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using static Ageral.triangulador;

namespace Ageral
{
    [CustomEditor(typeof(CriadorDeAreas))]
    public class EditorCriadorDeAreas : Editor
    {
        public int verificarPonto, verificarTriangulo_;
        // public override void OnInspectorGUI()
        Vector3 orig;
        List<Vector3> pp = new List<Vector3>();
        List<Vector3> tt = new List<Vector3>();
        private void OnSceneGUI()
        {
            
            CriadorDeAreas meuObjeto = (CriadorDeAreas)target;
            
            if (meuObjeto.guizmos) {
                if (meuObjeto.pontos_.Count <= 0)
                {
                    if (orig != meuObjeto.transform.position)
                    {
                        orig = meuObjeto.transform.position;
                        pp.Clear();


                        Vector3 pivo_ = Vector3.zero;//new Vector3(Random.Range(0, terreno.terrainData.size.x), 0, Random.Range(0,
                                                     //   terreno.terrainData.size.z));
                        float max_x = 0, min_x = 0, max_y = 0, min_y = 0;
                        for (int x = 0; x < meuObjeto.config.tamanhoMaximoDeArea; x++)
                        {
                            for (int z = 0; z < meuObjeto.config.tamanhoMaximoDeArea; z++)
                            {

                                float pl = Mathf.PerlinNoise(((pivo_.x + (x * meuObjeto.config.escala)) * meuObjeto.config.amplitude) / meuObjeto.config.frequencia,
                                    ((pivo_.z + (z * meuObjeto.config.escala)) * meuObjeto.config.amplitude) / meuObjeto.config.frequencia) * meuObjeto.config.escala;
                                if (pl > meuObjeto.config.tolerancia)
                                {
                                    Vector3 vertice_ = new Vector3(pivo_.x + (x * meuObjeto.config.escala), 0, pivo_.z + (z * meuObjeto.config.escala)) ;
                                    vertice_.y = 0;


                                    pp.Add(vertice_);

                                    max_x = vertice_.x > max_x ? vertice_.x : max_x;
                                    max_y = vertice_.x > max_y ? vertice_.x : max_y;


                                }
                            }
                        }
                        if (!meuObjeto.somentePontos) {
                            Vector3 aux_ = new Vector3(-(meuObjeto.config.escala + meuObjeto.config.tamanhoMaximoDeArea / 2), 0, -(meuObjeto.config.escala + meuObjeto.config.tamanhoMaximoDeArea) / 2);
                            for (int x = 0; x < pp.Count; x++)
                            {

                                pp[x] -= aux_;
                            }
                            tt = meuObjeto.trl.triangular(pp);

                            for (int x = 0; x <= tt.Count - 3; x += 3)
                            {

                                Handles.color = Color.green;
                                Vector3[] vv = new Vector3[3];
                                vv[0] = tt[x] + meuObjeto.transform.position;
                                vv[1] = tt[x + 1] + meuObjeto.transform.position;
                                vv[2] = tt[x + 2] + meuObjeto.transform.position;
                                Handles.DrawAAConvexPolygon(vv);

                                Handles.color = Color.red;
                                Handles.DrawLine(vv[0], vv[1]);
                                Handles.color = Color.red;
                                Handles.DrawLine(vv[1], vv[2]);
                                Handles.color = Color.red;
                                Handles.DrawLine(vv[0], vv[2]);
                            } }
                        foreach (var a in pp)
                        {
                            Handles.color = Color.red;

                            Vector3 aux = a;
                            aux.y = 0;
                            aux += meuObjeto.transform.position;
                            Handles.DrawSolidDisc(aux, Vector3.up, 0.4f);
                        }
                    }
                    else
                    {
                        if (!meuObjeto.somentePontos)
                        {
                            for (int x = 0; x <= tt.Count - 3; x += 3)
                            {

                                Handles.color = Color.green;
                                Vector3[] vv = new Vector3[3];
                                vv[0] = tt[x] + meuObjeto.transform.position;
                                vv[1] = tt[x + 1] + meuObjeto.transform.position;
                                vv[2] = tt[x + 2] + meuObjeto.transform.position;
                                Handles.DrawAAConvexPolygon(vv);

                                Handles.color = Color.red;
                                Handles.DrawLine(vv[0], vv[1]);
                                Handles.color = Color.red;
                                Handles.DrawLine(vv[1], vv[2]);
                                Handles.color = Color.red;
                                Handles.DrawLine(vv[0], vv[2]);
                            }
                        }
                        foreach (var a in pp)
                        {
                            Handles.color = Color.red;

                            Vector3 aux = a;
                            aux.y = 0;
                            aux += meuObjeto.transform.position;
                            Handles.DrawSolidDisc(aux, Vector3.up, 0.4f);
                        }
                    }
                }
                else
                {

                    for (int x = 0; x <= meuObjeto.triangulos_.Count - 3; x += 3)
                    {

                        Handles.color = Color.green;
                        Vector3[] vv = new Vector3[3];
                        vv[0] = meuObjeto.triangulos_[x] + meuObjeto.transform.position;
                        vv[1] = meuObjeto.triangulos_[x + 1] + meuObjeto.transform.position;
                        vv[2] = meuObjeto.triangulos_[x + 2] + meuObjeto.transform.position;
                        Handles.DrawAAConvexPolygon(vv);

                        Handles.color = Color.red;
                        Handles.DrawLine(vv[0], vv[1]);
                        Handles.color = Color.red;
                        Handles.DrawLine(vv[1], vv[2]);
                        Handles.color = Color.red;
                        Handles.DrawLine(vv[0], vv[2]);
                    }
                    foreach (var a in meuObjeto.pontos_)
                    {
                        Handles.color = Color.red;

                        Vector3 aux = a;
                        aux.y = 0;
                        aux += meuObjeto.transform.position;
                        Handles.DrawSolidDisc(aux, Vector3.up, 0.4f);
                    }

                }
            }

        }
    }
}
