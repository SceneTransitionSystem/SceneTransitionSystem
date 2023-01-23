﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	SceneTransitionSystem for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;

//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSAddressableAssetButton : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public STSScene ActiveScene;
        public STSScene IntermissionScene;
        public STSScene[] AdditionnalScenes;
        //-------------------------------------------------------------------------------------------------------------
        public void RunTransition()
        {
            Debug.Log("STSSceneButton RunTransition()");
            STSAddressableAssets.ReplaceAllByScenes(ActiveScene, AdditionnalScenes, IntermissionScene);
        }
        //-------------------------------------------------------------------------------------------------------------

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================