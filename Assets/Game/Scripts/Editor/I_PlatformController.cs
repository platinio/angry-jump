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
                Transform center = platform.transform.FindChild("Center");

                if (center == null)
                {
                    center = new GameObject("Center").transform;
                    center.transform.parent = platform.transform;
                }

                center.localPosition = platform.GetComponent<BoxCollider2D>().offset;
                //center.position = platform.GetComponent<BoxCollider2D>().offset;
                             
            }

            if (GUI.changed) EditorUtility.SetDirty(platform);
        }

    }
}

