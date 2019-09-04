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
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class STSSceneManager : MonoBehaviour, STSTransitionInterface, STSIntermissionInterface
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void AddSubScene(string sAdditionalSceneName, string sIntermissionSceneName = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, new List<string> { sAdditionalSceneName }, null, sIntermissionSceneName, sTransitionData, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddSubScenes(List<string> sAdditionalScenes, string sIntermissionScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, sAdditionalScenes, null, sIntermissionScene, sTransitionData, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddScene(string sNextActiveScene, string sIntermissionScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, null, sIntermissionScene, sTransitionData, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddScenes(string sNextActiveScene, List<string> sScenesToAdd, string sIntermissionScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, sScenesToAdd, null, sIntermissionScene, sTransitionData, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveSubScene(string sSceneToRemove, string sIntermissionScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, null, new List<string> { sSceneToRemove }, sIntermissionScene, sTransitionData, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveSubScenes(List<string> sScenesToRemove, string sIntermissionScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, null, sScenesToRemove, sIntermissionScene, sTransitionData, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveScene(string sNextActiveScene, string sSceneToRemove, string sIntermissionScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, new List<string> { sSceneToRemove }, sIntermissionScene, sTransitionData, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveScenes(string sNextActiveScene, List<string> sScenesToRemove, string sIntermissionScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, sScenesToRemove, sIntermissionScene, sTransitionData, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ReplaceAllByScene(string sNextActiveScene, string sIntermissionScene = null, STSTransitionData sTransitionData = null)
        {
            ReplaceAllByScenes(sNextActiveScene, null, sIntermissionScene, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ReplaceAllByScenes(string sNextActiveScene, List<string> sScenesToAdd, string sIntermissionScene = null, STSTransitionData sTransitionData = null)
        {
            List<string> tScenesToRemove = new List<string>();
            for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
            {
                Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                tScenesToRemove.Add(tScene.name);
            }
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, sScenesToAdd, tScenesToRemove, sIntermissionScene, sTransitionData, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void INTERNAL_ChangeScenes(
            string sActualActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            string sIntermissionScene,
            STSTransitionData sTransitionData,
            bool sHistorical)
        {
            if (TransitionInProgress == false)
            {
                // null protection
                if (sScenesToAdd == null)
                {
                    sScenesToAdd = new List<string>();
                }
                if (sScenesToRemove == null)
                {
                    sScenesToRemove = new List<string>();
                }
                // trim
                while (sScenesToAdd.Contains(string.Empty) == true)
                {
                    sScenesToAdd.Remove(string.Empty);
                }
                while (sScenesToRemove.Contains(string.Empty) == true)
                {
                    sScenesToRemove.Remove(string.Empty);
                }
                // active scene protection
                if (SceneManager.GetSceneByName(sNextActiveScene).isLoaded == false)
                {
                    if (sScenesToAdd.Contains(sNextActiveScene) == false)
                    {
                        sScenesToAdd.Add(sNextActiveScene);
                    }
                }
                if (sScenesToRemove.Contains(sNextActiveScene) == true)
                {
                    sScenesToRemove.Remove(sNextActiveScene);
                }
                // futur scenes protection
                foreach (string tSceneName in sScenesToAdd)
                {
                    if (sScenesToRemove.Contains(tSceneName) == true)
                    {
                        sScenesToRemove.Remove(tSceneName);
                    }
                }
                // test possibilities
                bool tPossible = false;
                List<Scene> tScenes = new List<Scene>();
                for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
                {
                    Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                    tScenes.Add(tScene);
                    sScenesToAdd.Remove(tScene.name);
                    if (tScene.name == sActualActiveScene)
                    {
                        tPossible = true;
                    }
                }

                if (sHistorical == true)
                {
                    INTERNAL_AddNavigation(sNextActiveScene, sScenesToAdd, sIntermissionScene, sTransitionData);
                }

                if (tPossible == true)
                {
                    if (string.IsNullOrEmpty(sIntermissionScene))
                    {
                        StartCoroutine(INTERNAL_ChangeScenesWithoutIntermission(sActualActiveScene, sNextActiveScene, sScenesToAdd, sScenesToRemove, sTransitionData));
                    }
                    else
                    {
                        StartCoroutine(INTERNAL_ChangeScenesWithIntermission(sIntermissionScene, sActualActiveScene, sNextActiveScene, sScenesToAdd, sScenesToRemove, sTransitionData));
                    }
                }
                else
                {
                    Debug.LogWarning(K_SCENE_MUST_BY_LOADED);
                }
            }
            else
            {
                Debug.LogWarning(K_TRANSITION_IN_PROGRESS);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private IEnumerator INTERNAL_ChangeScenesWithoutIntermission(
            string sActualActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            STSTransitionData sTransitionData)
        {
            bool tRemoveActual = false;
            if (sScenesToRemove.Contains(sActualActiveScene))
            {
                sScenesToRemove.Remove(sActualActiveScene);
                tRemoveActual = true;
            }
            //AsyncOperation tAsyncOperation;
            Dictionary<string, AsyncOperation> tAsyncOperationList = new Dictionary<string, AsyncOperation>();
            //-------------------------------
            // ACTUAL SCENE DISABLE
            //-------------------------------
            TransitionInProgress = true;
            //-------------------------------
            Scene tActualScene = SceneManager.GetSceneByName(sActualActiveScene);
            //-------------------------------
            STSTransition tActualSceneParams = GetTransitionsParams(tActualScene, true);
            // disable the user interactions
            EventSystemPrevent(false);
            // post scene is disable!
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionSceneDisable(sTransitionData);
            }
            // post scene start effect transition out!
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionExitStart(sTransitionData);
            }
            // scene start effect transition out!
            AnimationTransitionOut(tActualSceneParams, sTransitionData);
            // waiting effect will finish
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            // post scene finish effcet transition out
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            }
            //-------------------------------
            // COUNT SCENES TO REMOVE OR ADD
            //-------------------------------
            float tSceneCount = sScenesToAdd.Count + sScenesToRemove.Count;
            int tSceneCounter = 0;
            //-------------------------------
            // LOADED SCENES ADDED
            //-------------------------------
            foreach (string tSceneToAdd in sScenesToAdd)
            {
                //Debug.Log("tSceneToAdd :" + tSceneToAdd);
                if (SceneManager.GetSceneByName(tSceneToAdd).isLoaded)
                {
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " allready finish!");
                }
                else
                {
                    AsyncOperation tAsyncOperationAdd = SceneManager.LoadSceneAsync(tSceneToAdd, LoadSceneMode.Additive);
                    tAsyncOperationList.Add(tSceneToAdd, tAsyncOperationAdd);
                    tAsyncOperationAdd.allowSceneActivation = false;
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " 90%!");
                }
                tSceneCounter++;
            }
            //-------------------------------
            // UNLOADED SCENES REMOVED
            //-------------------------------
            foreach (string tSceneToRemove in sScenesToRemove)
            {
                //Debug.Log("tSceneToRemove :" + tSceneToRemove);
                // fadeout is finish
                // will unloaded the  scene
                Scene tSceneToDelete = SceneManager.GetSceneByName(tSceneToRemove);
                STSTransition tSceneToDeleteParams = GetTransitionsParams(tSceneToDelete, false);
                if (tSceneToDeleteParams.Interfaced != null)
                {
                    tSceneToDeleteParams.Interfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }
                if (SceneManager.GetSceneByName(tSceneToRemove).isLoaded)
                {
                    AsyncOperation tAsyncOperationRemove = SceneManager.UnloadSceneAsync(tSceneToRemove);
                    tAsyncOperationRemove.allowSceneActivation = true; //? needed?
                                                                       //while (tAsyncOperationRemove.progress < 0.9f)
                                                                       //{
                                                                       //    yield return null;
                                                                       //}
                                                                       //while (!tAsyncOperationRemove.isDone)
                                                                       //{
                                                                       //    yield return null;
                                                                       //}
                }
                tSceneCounter++;
                Debug.Log("tSceneToRemove :" + tSceneToRemove + " finish!");
            }
            //-------------------------------
            // ACTIVE ADDED SCENES
            //-------------------------------
            foreach (string tSceneToAdd in sScenesToAdd)
            {
                // scene is loaded!
                if (tAsyncOperationList.ContainsKey(tSceneToAdd))
                {
                    AsyncOperation tAsyncOperationAdd = tAsyncOperationList[tSceneToAdd];
                    tAsyncOperationAdd.allowSceneActivation = true;
                    while (!tAsyncOperationAdd.isDone)
                    {
                        yield return null;
                    }
                    Scene tNextScene = SceneManager.GetSceneByName(tSceneToAdd);
                    STSTransition tNextSceneParams = GetTransitionsParams(tNextScene, false);
                    if (tNextSceneParams.Interfaced != null)
                    {
                        tNextSceneParams.Interfaced.OnTransitionSceneLoaded(sTransitionData);
                    }
                    EventSystemPrevent(false);
                    AudioListenerPrevent();
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " finish!");
                }
            }
            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            Scene tNextActiveScene = SceneManager.GetSceneByName(sNextActiveScene);
            SceneManager.SetActiveScene(tNextActiveScene);
            AudioListenerPrevent();
            // get params
            STSTransition sNextSceneParams = GetTransitionsParams(tNextActiveScene, false);
            EventSystemEnable(tNextActiveScene, false);
            // Next scene appear by fade in
            //-------------------------------
            // Intermission UNLOAD
            //-------------------------------
            if (tRemoveActual == true)
            {
                if (tActualSceneParams.Interfaced != null)
                {
                    tActualSceneParams.Interfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }
                AsyncOperation tAsyncOperationIntermissionUnload = SceneManager.UnloadSceneAsync(sActualActiveScene);
                tAsyncOperationIntermissionUnload.allowSceneActivation = true; //? needed?
                                                                               //while (tAsyncOperationIntermissionUnload.progress < 0.9f)
                                                                               //{
                                                                               //    yield return null;
                                                                               //}
                                                                               //while (!tAsyncOperationIntermissionUnload.isDone)
                                                                               //{
                                                                               //    yield return null;
                                                                               //}
            }
            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            AnimationTransitionIn(sNextSceneParams, sTransitionData);
            if (sNextSceneParams.Interfaced != null)
            {
                sNextSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            if (sNextSceneParams.Interfaced != null)
            {
                sNextSceneParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            }
            // fadein is finish
            EventSystemPrevent(true);
            // next scene is enable
            if (sNextSceneParams.Interfaced != null)
            {
                sNextSceneParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            }
            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        private IEnumerator INTERNAL_ChangeScenesWithIntermission(
            string sIntermissionScene,
            string sActualActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            STSTransitionData sTransitionData)
        {

            //AsyncOperation tAsyncOperation;
            Dictionary<string, AsyncOperation> tAsyncOperationList = new Dictionary<string, AsyncOperation>();
            //-------------------------------
            // ACTUAL SCENE DISABLE
            //-------------------------------
            TransitionInProgress = true;
            //-------------------------------
            Scene tActualScene = SceneManager.GetSceneByName(sActualActiveScene);
            //-------------------------------
            STSTransition tActualSceneParams = GetTransitionsParams(tActualScene, true);
            // disable the user interactions
            EventSystemPrevent(false);
            // post scene is disable!
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionSceneDisable(sTransitionData);
            }
            // post scene start effect transition out!
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionExitStart(sTransitionData);
            }
            // scene start effect transition out!
            AnimationTransitionOut(tActualSceneParams, sTransitionData);
            // waiting effect will finish
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            // post scene finish effcet transition out
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            }
            //-------------------------------
            // Intermission SCENE LOAD AND ENABLE
            //-------------------------------
            // load transition scene async
            AsyncOperation tAsyncOperationIntermission = SceneManager.LoadSceneAsync(sIntermissionScene, LoadSceneMode.Additive);
            tAsyncOperationIntermission.allowSceneActivation = true;
            while (tAsyncOperationIntermission.progress < 0.9f)
            {
                yield return null;
            }
            while (!tAsyncOperationIntermission.isDone)
            {
                yield return null;
            }
            // get Transition Scene
            Scene tIntermissionScene = SceneManager.GetSceneByName(sIntermissionScene);
            // Active the next scene as root scene 
            SceneManager.SetActiveScene(tIntermissionScene);
            // disable audiolistener of preview scene
            AudioListenerPrevent();
            // get params
            STSTransition tIntermissionSceneParams = GetTransitionsParams(tIntermissionScene, false);
            // disable the user interactions until fadein 
            EventSystemEnable(tIntermissionScene, false);
            // Intermission scene is loaded
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionSceneLoaded(sTransitionData);
            }
            // animation in
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            // animation in Go!
            AnimationTransitionIn(tIntermissionSceneParams, sTransitionData);
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            // animation in Finish
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            }
            // enable the user interactions 
            EventSystemEnable(tIntermissionScene, true);
            // enable the user interactions 
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            }
            //-------------------------------
            // Intermission SCENE START STAND BY
            //-------------------------------
            // start stand by
            STSIntermission tIntermissionSceneStandBy = GetStandByParams(tIntermissionScene);
            if (tIntermissionSceneStandBy.Interfaced != null)
            {
                tIntermissionSceneStandBy.Interfaced.OnStandByStart(tIntermissionSceneStandBy);
            }
            StandBy();
            //-------------------------------
            // COUNT SCENES TO REMOVE OR ADD
            //-------------------------------
            float tSceneCount = sScenesToAdd.Count + sScenesToRemove.Count;
            int tSceneCounter = 0;
            //-------------------------------
            // LOADED SCENES ADDED
            //-------------------------------
            foreach (string tSceneToAdd in sScenesToAdd)
            {
                //Debug.Log("tSceneToAdd :" + tSceneToAdd);
                if (SceneManager.GetSceneByName(tSceneToAdd).isLoaded)
                {
                    if (tIntermissionSceneStandBy.Interfaced != null)
                    {
                        tIntermissionSceneStandBy.Interfaced.OnSceneAllReadyLoaded(sTransitionData, tSceneToAdd, tSceneCounter, (tSceneCounter + 1.0F) / tSceneCount);
                    }
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " allready finish!");
                }
                else
                {
                    if (tIntermissionSceneStandBy.Interfaced != null)
                    {
                        tIntermissionSceneStandBy.Interfaced.OnLoadingSceneStart(sTransitionData, tSceneToAdd, tSceneCounter, 0.0F, 0.0F);
                    }

                    AsyncOperation tAsyncOperationAdd = SceneManager.LoadSceneAsync(tSceneToAdd, LoadSceneMode.Additive);
                    tAsyncOperationList.Add(tSceneToAdd, tAsyncOperationAdd);
                    tAsyncOperationAdd.allowSceneActivation = false;
                    while (tAsyncOperationAdd.progress < 0.9f)
                    {
                        if (tIntermissionSceneStandBy.Interfaced != null)
                        {
                            tIntermissionSceneStandBy.Interfaced.OnLoadingScenePercent(sTransitionData, tSceneToAdd, tSceneCounter, tAsyncOperationAdd.progress, (tSceneCounter + tAsyncOperationAdd.progress) / tSceneCount);
                        }
                        yield return null;
                    }
                    if (tIntermissionSceneStandBy.Interfaced != null)
                    {
                        tIntermissionSceneStandBy.Interfaced.OnLoadingSceneFinish(sTransitionData, tSceneToAdd, tSceneCounter, 1.0F, (tSceneCounter + 1.0F) / tSceneCount);
                    }
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " 90%!");
                }
                tSceneCounter++;
            }
            //-------------------------------
            // UNLOADED SCENES REMOVED
            //-------------------------------
            foreach (string tSceneToRemove in sScenesToRemove)
            {
                //Debug.Log("tSceneToRemove :" + tSceneToRemove);
                // fadeout is finish
                // will unloaded the  scene
                Scene tSceneToDelete = SceneManager.GetSceneByName(tSceneToRemove);
                STSTransition tSceneToDeleteParams = GetTransitionsParams(tSceneToDelete, false);
                if (tSceneToDeleteParams.Interfaced != null)
                {
                    tSceneToDeleteParams.Interfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }
                if (SceneManager.GetSceneByName(tSceneToRemove).isLoaded)
                {
                    AsyncOperation tAsyncOperationRemove = SceneManager.UnloadSceneAsync(tSceneToRemove);

                    if (tAsyncOperationRemove != null)
                    {
                        tAsyncOperationRemove.allowSceneActivation = true; //? needed?
                                                                           //while (tAsyncOperationRemove.progress < 0.9f)
                                                                           //{
                                                                           //    if (tIntermissionSceneStandBy.Interfaced != null)
                                                                           //    {
                                                                           //        tIntermissionSceneStandBy.Interfaced.OnLoadingNextScenePercent(sTransitionData, tSceneToRemove, tSceneCounter, tAsyncOperationRemove.progress, (tSceneCounter + tAsyncOperationRemove.progress) / tSceneCount);
                                                                           //    }
                                                                           //    yield return null;
                                                                           //}
                                                                           //while (!tAsyncOperationRemove.isDone)
                                                                           //{
                                                                           //    yield return null;
                                                                           //}
                    }
                    else
                    {
                        Debug.LogWarning("UnloadSceneAsync is not possible for " + tSceneToRemove);
                    }
                }
                if (tIntermissionSceneStandBy.Interfaced != null)
                {
                    tIntermissionSceneStandBy.Interfaced.OnUnloadScene(sTransitionData, tSceneToRemove, tSceneCounter, (tSceneCounter + 1.0F) / tSceneCount);
                }
                tSceneCounter++;
                Debug.Log("tSceneToRemove :" + tSceneToRemove + " finish!");
            }
            //-------------------------------
            // Intermission STAND BY
            //-------------------------------
            while (StandByIsProgressing(tIntermissionSceneStandBy))
            {
                yield return null;
            }
            // As soon as possible 
            if (tIntermissionSceneStandBy.Interfaced != null)
            {
                tIntermissionSceneStandBy.Interfaced.OnStandByFinish(tIntermissionSceneStandBy);
            }
            // Waiting to load the next Scene
            while (WaitingToLauchNextScene(tIntermissionSceneStandBy))
            {
                //Debug.Log ("StandByIsNotFinished loop");
                yield return null;
            }
            //-------------------------------
            // Intermission GO TO NEXT SCENE PROCESS
            //-------------------------------
            // stanby is finished And the next scene can be lauch
            // disable user interactions on the Intermission scene
            EventSystemEnable(tIntermissionScene, false);
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionSceneDisable(sTransitionData);
            }
            // Intermission scene Transition Out start 
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            // Intermission scene Transition Out GO! 
            AnimationTransitionOut(tIntermissionSceneParams, sTransitionData);
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            // Intermission scene Transition Out finished! 
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            }
            // fadeout is finish
            // will unloaded the Intermission scene
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionSceneWillUnloaded(sTransitionData);
            }
            //-------------------------------
            // ACTIVE ADDED SCENES
            //-------------------------------
            foreach (string tSceneToAdd in sScenesToAdd)
            {
                // scene is loaded!
                if (tAsyncOperationList.ContainsKey(tSceneToAdd))
                {
                    AsyncOperation tAsyncOperationAdd = tAsyncOperationList[tSceneToAdd];
                    tAsyncOperationAdd.allowSceneActivation = true;
                    while (!tAsyncOperationAdd.isDone)
                    {
                        yield return null;
                    }
                    Scene tNextScene = SceneManager.GetSceneByName(tSceneToAdd);
                    STSTransition tNextSceneParams = GetTransitionsParams(tNextScene, false);
                    if (tNextSceneParams.Interfaced != null)
                    {
                        tNextSceneParams.Interfaced.OnTransitionSceneLoaded(sTransitionData);
                    }
                    EventSystemPrevent(false);
                    AudioListenerPrevent();
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " finish!");
                }
            }
            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            Scene tNextActiveScene = SceneManager.GetSceneByName(sNextActiveScene);
            SceneManager.SetActiveScene(tNextActiveScene);
            AudioListenerPrevent();
            // get params
            STSTransition sNextSceneParams = GetTransitionsParams(tNextActiveScene, false);
            EventSystemEnable(tNextActiveScene, false);
            // Next scene appear by fade in
            //-------------------------------
            // Intermission UNLOAD
            //-------------------------------
            AsyncOperation tAsyncOperationIntermissionUnload = SceneManager.UnloadSceneAsync(tIntermissionScene);
            if (tAsyncOperationIntermissionUnload != null)
            {
                tAsyncOperationIntermissionUnload.allowSceneActivation = true; //? needed?
                while (tAsyncOperationIntermissionUnload.progress < 0.9f)
                {
                    yield return null;
                }
                while (!tAsyncOperationIntermissionUnload.isDone)
                {
                    yield return null;
                }
            }
            else
            {
                Debug.LogWarning("UnloadSceneAsync is not possible for " + tIntermissionScene);
            }
            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            AnimationTransitionIn(sNextSceneParams, sTransitionData);
            if (sNextSceneParams.Interfaced != null)
            {
                sNextSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            if (sNextSceneParams.Interfaced != null)
            {
                sNextSceneParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            }
            // fadein is finish
            EventSystemPrevent(true);
            // next scene is enable
            if (sNextSceneParams.Interfaced != null)
            {
                sNextSceneParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            }
            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================