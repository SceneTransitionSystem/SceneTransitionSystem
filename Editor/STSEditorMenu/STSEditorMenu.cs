﻿//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace SceneTransitionSystem
{
	/// <summary>
	/// STS editor menu.
	/// </summary>
	public class STSEditorMenu 
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SceneTransitionSystem.STSEditorMenu"/> class.
		/// </summary>
		public STSEditorMenu ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Idemobis the editor info show.
		/// </summary>
#if UNITY_MENU_IDEMOBI
		[MenuItem (STSConstants.K_MENU_IDEMOBI,false, 10000)]
#else
		[MenuItem (STSConstants.K_MENU_IDEMOBI,false, 100)]
#endif
		public static void IdemobiInfoShow()
		{
			if (EditorUtility.DisplayDialog (STSConstants.K_ALERT_IDEMOBI_TITLE,
				STSConstants.K_ALERT_IDEMOBI_MESSAGE,
				STSConstants.K_ALERT_IDEMOBI_OK,
				STSConstants.K_ALERT_IDEMOBI_SEE_DOC)) {
			} else {
				Application.OpenURL (STSConstants.K_ALERT_IDEMOBI_DOC_HTTP);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif