using UnityEditor;
using UnityEngine;

namespace QuickVRPreview
{
    [CustomEditor(typeof(QuickVRPreview))]
    public class QuickVRPreviewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var preview = (QuickVRPreview)target;

            EditorGUILayout.Space(10);

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox(
                    "Enter Play Mode, then press \"Start VR\" to begin VR preview.",
                    MessageType.Info);
                GUI.enabled = false;
                GUILayout.Button("Start VR", GUILayout.Height(32));
                GUI.enabled = true;
                return;
            }

            if (preview.IsRunning)
            {
                EditorGUILayout.HelpBox("VR is running. Put on your headset!", MessageType.None);

                if (GUILayout.Button("Stop VR", GUILayout.Height(32)))
                {
                    preview.StopVR();
                }
            }
            else
            {
                if (GUILayout.Button("Start VR", GUILayout.Height(32)))
                {
                    preview.StartVR();
                }
            }

            Repaint();
        }

        [MenuItem("GameObject/QuickVRPreview", false, 10)]
        private static void CreateInScene()
        {
            var go = new GameObject("QuickVRPreview");
            go.AddComponent<QuickVRPreview>();
            Selection.activeGameObject = go;
            Undo.RegisterCreatedObjectUndo(go, "Create QuickVRPreview");
        }
    }
}
