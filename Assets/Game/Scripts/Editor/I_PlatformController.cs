using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Platinio
{
    [CustomEditor(typeof(PlatformController))]
    public class InspectorPlatformController: Editor
    {
        private static PlatformController platform;

        void Awake()
        {
            platform = (PlatformController)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Set Center"))
            {
               
                             
            }

            if (GUI.changed) EditorUtility.SetDirty(platform);
        }

    }
}

