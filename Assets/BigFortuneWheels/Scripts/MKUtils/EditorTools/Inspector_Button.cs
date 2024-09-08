using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
#endif
/*
  06.01.2020 - first
  07.07.2020 - only one button
 */
namespace Mkey
{
    [ExecuteInEditMode]
    public class Inspector_Button : MonoBehaviour
    {
        [SerializeField]
        public Button.ButtonClickedEvent clickFirstEvent;
        public void FirstButton_Click()
        {
            clickFirstEvent?.Invoke();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Inspector_Button))]
    public class Inspector_ButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Inspector_Button myScript = (Inspector_Button)target;
            string buttonName = (myScript.clickFirstEvent!=null && myScript.clickFirstEvent.GetPersistentEventCount()>0) ? myScript.clickFirstEvent.GetPersistentMethodName(0) : "not defined";
            if (string.IsNullOrEmpty(buttonName)) buttonName = "not defined";
            if (GUILayout.Button(buttonName))
            {
                myScript.FirstButton_Click();
            }
            DrawDefaultInspector();
        }
    }
#endif
}