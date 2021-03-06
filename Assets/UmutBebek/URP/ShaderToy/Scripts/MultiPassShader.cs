﻿
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

//using UnityEngine.UI;

// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace UmutBebek.URP.ShaderToy
{
    public class MultiPassShader : MonoBehaviour
    {
        //public RawImage a1;
        //public RawImage a2;
        public Texture Texture1;
        public Texture Texture2;
        public Texture Texture3;
        public Texture Texture4;
        public Shader BufferA;
        /// <summary>
        /// They must follow the channel order, So if first buffer channel usage attached like "BufferB and BufferA"
        /// this must be B,A, if usage is BufferA and BufferB it must be "B,A", if no usage it must be empty
        /// If buffer uses a texture put its number as same in order,
        /// So if BufferA uses itself and BufferB and a texture which is attached to first slot, it will be
        /// "A,B,1" etc.
        /// </summary>
        public string UsePassForBufferA;
        public Shader BufferB;
        public string UsePassForBufferB;
        public Shader BufferC;
        public string UsePassForBufferC;
        public Shader BufferD;
        public string UsePassForBufferD;
        public Shader MainImage; //it is the last drawing shader, it maybe always attached!!
        public string UsePassForMainImage;
        //A is for buffer A :) Last one is M for MainImage :)
        RenderTexture _rtA1, _rtA2, _rtB1, _rtB2, _rtC1, _rtC2, _rtD1, _rtD2, _rtM1, _rtM2;
        private List<Renderer> _renderers;
        private Material _m1, _m2, _m3, _m4, _m5;
        private bool _change = true;

        // Start is called before the first frame update
        // ReSharper disable once ArrangeTypeMemberModifiers
        // ReSharper disable once UnusedMember.Local
        void Start()
        {
            int width = Screen.width + Screen.width % 2;
            int height = Screen.height + Screen.height % 2;
            int depth = 32;
            _rtA1 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
            _rtA2 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
            _rtB1 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
            _rtB2 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
            _rtC1 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
            _rtC2 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
            _rtD1 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
            _rtD2 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
            _rtM1 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
            _rtM2 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
            _rtA1.useMipMap = true;
            _rtA2.useMipMap = true;
            _rtB1.useMipMap = true;
            _rtB2.useMipMap = true;
            _rtC1.useMipMap = true;
            _rtC2.useMipMap = true;
            _rtD1.useMipMap = true;
            _rtD2.useMipMap = true;
            _rtM1.useMipMap = true;
            _rtM2.useMipMap = true;
            _rtA1.autoGenerateMips = true;
            _rtA2.autoGenerateMips = true;
            _rtB1.autoGenerateMips = true;
            _rtB2.autoGenerateMips = true;
            _rtC1.autoGenerateMips = true;
            _rtC2.autoGenerateMips = true;
            _rtD1.autoGenerateMips = true;
            _rtD2.autoGenerateMips = true;
            _rtM1.autoGenerateMips = true;
            _rtM2.autoGenerateMips = true;

            _renderers = this.GetComponentsInChildren<Renderer>().ToList();

            RenderPipelineManager.beginCameraRendering += RenderPipelineManager_beginCameraRendering;
        }

        private void RenderPipelineManager_beginCameraRendering(ScriptableRenderContext arg1, Camera arg2)
        {
            if (BufferA != null)
            {
                if (_m1 == null) _m1 = new Material(BufferA);
                if (!string.IsNullOrWhiteSpace(UsePassForBufferA))
                {
                    var buffers = UsePassForBufferA.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < buffers.Length; i++)
                    {
                        if (buffers[i] == "A")
                            _m1.SetTexture("_Channel" + i, _change ? _rtA1 : _rtA2);
                        if (buffers[i] == "B")
                            _m1.SetTexture("_Channel" + i, _change ? _rtB1 : _rtB2);
                        if (buffers[i] == "C")
                            _m1.SetTexture("_Channel" + i, _change ? _rtC1 : _rtC2);
                        if (buffers[i] == "D")
                            _m1.SetTexture("_Channel" + i, _change ? _rtD1 : _rtD2);
                        if (buffers[i] == "1")
                            _m1.SetTexture("_Channel" + i, Texture1);
                        if (buffers[i] == "2")
                            _m1.SetTexture("_Channel" + i, Texture2);
                        if (buffers[i] == "3")
                            _m1.SetTexture("_Channel" + i, Texture3);
                        if (buffers[i] == "4")
                            _m1.SetTexture("_Channel" + i, Texture4);
                    }
                }

                if (_change)
                {
                    Graphics.Blit(_rtA1, _rtA2, _m1);
                    //Texture2D tex = new Texture2D(_rtA2.width, _rtA2.height, TextureFormat.RGBAFloat, false);
                    //Rect rectReadPicture = new Rect(0, 0, _rtA2.width, _rtA2.height);
                    //// Read pixels
                    //RenderTexture.active = _rtA2;
                    //tex.ReadPixels(rectReadPicture, 0, 0);
                    //tex.Apply();
                    //for (int i = 0; i < _rtA2.width; i++)
                    //{
                    //    for (int j = 0; j < _rtA2.height; j++)
                    //    {
                    //        if( j%100 == 0)
                    //        Debug.Log("i: " + i + " j: " + j + " " + tex.GetPixel(i, j));
                    //    }
                    //}
                    //RenderTexture.active = null;
                }
                else
                    Graphics.Blit(_rtA2, _rtA1, _m1);
            }

            if (BufferB != null)
            {
                if (_m2 == null) _m2 = new Material(BufferB);
                if (!string.IsNullOrWhiteSpace(UsePassForBufferB))
                {
                    var buffers = UsePassForBufferB.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < buffers.Length; i++)
                    {
                        if (buffers[i] == "A")
                            _m1.SetTexture("_Channel" + i, _change ? _rtA2 : _rtA1); //reverse because it is rendered before
                        if (buffers[i] == "B")
                            _m1.SetTexture("_Channel" + i, _change ? _rtB1 : _rtB2); //through
                        if (buffers[i] == "C")
                            _m1.SetTexture("_Channel" + i, _change ? _rtC1 : _rtC2); //through
                        if (buffers[i] == "D")
                            _m1.SetTexture("_Channel" + i, _change ? _rtD1 : _rtD2); //through
                        if (buffers[i] == "1")
                            _m1.SetTexture("_Channel" + i, Texture1);
                        if (buffers[i] == "2")
                            _m1.SetTexture("_Channel" + i, Texture2);
                        if (buffers[i] == "3")
                            _m1.SetTexture("_Channel" + i, Texture3);
                        if (buffers[i] == "4")
                            _m1.SetTexture("_Channel" + i, Texture4);
                    }
                }

                if (_change)
                {
                    Graphics.Blit(_rtB1, _rtB2, _m2);
                }
                else
                    Graphics.Blit(_rtB2, _rtB1, _m2);
            }

            //if(a1) a1.texture = _rt1a;
            //if(a2) a2.texture = _rt1b;

            if (MainImage != null)
            {
                if (_m5 == null) _m5 = new Material(MainImage);
                if (!string.IsNullOrWhiteSpace(UsePassForMainImage))
                {
                    var buffers = UsePassForMainImage.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < buffers.Length; i++)
                    {
                        if (buffers[i] == "A")
                            _m5.SetTexture("_Channel" + i, _change ? _rtA2 : _rtA1);
                        if (buffers[i] == "B")
                            _m5.SetTexture("_Channel" + i, _change ? _rtB2 : _rtB1);
                        if (buffers[i] == "C")
                            _m5.SetTexture("_Channel" + i, _change ? _rtC2 : _rtC1);
                        if (buffers[i] == "D")
                            _m5.SetTexture("_Channel" + i, _change ? _rtD2 : _rtD1);
                        if (buffers[i] == "1")
                            _m1.SetTexture("_Channel" + i, Texture1);
                        if (buffers[i] == "2")
                            _m1.SetTexture("_Channel" + i, Texture2);
                        if (buffers[i] == "3")
                            _m1.SetTexture("_Channel" + i, Texture3);
                        if (buffers[i] == "4")
                            _m1.SetTexture("_Channel" + i, Texture4);
                    }

                    if (_change)
                        Graphics.Blit(_rtM1, _rtM2, _m5);
                    else
                        Graphics.Blit(_rtM2, _rtM1, _m5);
                }

                if (_renderers != null && _renderers.Count > 0)
                {
                    foreach (var ren in _renderers)
                    {
                        //this is just for to draw, nothing special
                        if (_change)
                            ren.sharedMaterial.SetTexture("_Channel0", _rtM2);
                        else
                            ren.sharedMaterial.SetTexture("_Channel0", _rtM1);
                    }
                }
            }

            _change = !_change; //!!
        }

        // ReSharper disable once ArrangeTypeMemberModifiers
        // ReSharper disable once UnusedMember.Local
        void OnDestroy()
        {
            _rtA1.Release();
            _rtA2.Release();
            _rtB1.Release();
            _rtB2.Release();
            _rtC1.Release();
            _rtC2.Release();
            _rtD1.Release();
            _rtD2.Release();
            _rtM1.Release();
            _rtM2.Release();

            RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_beginCameraRendering;
        }
    }
}
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace UmutBebek.URP.ShaderToy
//{
//    public class MultiPassShader : MonoBehaviour
//    {
//        public Shader[] Shaders;
//        RenderTexture _rt1, _rt2, _rt3, _rt4, _rt5;
//        private List<Renderer> _renderers;
//        private Material _m1, _m2, _m3, _m4, _m5;

//        // Start is called before the first frame update
//        void Start()
//        {
//            int width = Screen.width + Screen.width % 2;
//            int height = Screen.height + Screen.height % 2;
//            int depth = 32;
//            _rt1 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
//            _rt2 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
//            _rt3 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
//            _rt4 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
//            _rt5 = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
//            _rt1.useMipMap = true;
//            _rt2.useMipMap = true;
//            _rt3.useMipMap = true;
//            _rt4.useMipMap = true;
//            _rt5.useMipMap = true;
//            _rt1.autoGenerateMips = true;
//            _rt2.autoGenerateMips = true;
//            _rt3.autoGenerateMips = true;
//            _rt4.autoGenerateMips = true;
//            _rt5.autoGenerateMips = true;

//            if(Shaders.Length>0)
//                _m1 = new Material(Shaders[0]);
//            if (Shaders.Length > 1)
//                _m2 = new Material(Shaders[1]);
//            if (Shaders.Length > 2)
//                _m3 = new Material(Shaders[2]);
//            if (Shaders.Length > 3)
//                _m4 = new Material(Shaders[3]);
//            if (Shaders.Length > 4)
//                _m5 = new Material(Shaders[4]);

//            _renderers = this.GetComponentsInChildren<Renderer>().ToList();
//        }

//        // Update is called once per frame
//        void Update()
//        {
//            Graphics.Blit(_rt1, _rt2, _m1);

//            _m2.SetTexture("_Channel0",_rt2);
//            Graphics.Blit(_rt2, _rt3, _m2);

//            if (_renderers != null && _renderers.Count > 0)
//            {
//                foreach (var ren in _renderers)
//                {
//                    ren.sharedMaterial.SetTexture("_Channel0", _rt3);
//                }
//            }
//        }

//        void OnDestroy()
//        {
//            _rt1.Release();
//            _rt2.Release();
//            _rt3.Release();
//            _rt4.Release();
//            _rt5.Release();
//        }
//    }
//}