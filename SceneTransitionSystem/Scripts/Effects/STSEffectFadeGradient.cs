﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using UnityEngine;

//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [STSEffectName("Fade Gradient")]
    // *** Remove some parameters in inspector
    [STSNoTexturePrimary]
    [STSNoTextureSecondary]
    [STSNoParameterOne]
    [STSNoParameterTwo]
    [STSNoParameterThree]
    [STSNoOffset]
    [STSNoFiveCross]
    // ***
    public class STSEffectFadeGradient : STSEffect
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void Draw(Rect sRect)
        {
            STSBenchmark.Start();
            if (Purcent > 0)
            {
                // Do drawing with purcent
                Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, Purcent);
                Color tFadeColorAlpha = new Color(tColorLerp.r, tColorLerp.g, tColorLerp.b, Purcent);
                STSTransitionDrawing.DrawQuad(sRect, tFadeColorAlpha);
            }
            STSBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================