using UnityEditor;
using UnityEngine;

namespace Ab.ScreenShot.Editor
{
    public class ScreenShotWindow : EditorWindow
    {
        [MenuItem("Window/スクリーンショット")]
        private static void Open()
        {
            GetWindow<ScreenShotWindow>("スクリーンショット");
        }

        private void OnGUI()
        {
            var scene = EditorPrefs.GetBool("Ab.ScreenShot|Scene", true);
            var next = GUILayout.Toggle(scene, "シーンビューに表示する");
            if (scene != next)
            {
                EditorPrefs.SetBool("Ab.ScreenShot|Scene", next);
                SceneView.RepaintAll();
            }

            var sceneView = SceneView.lastActiveSceneView;
            GUILayout.Space(10);
            
            if (GUILayout.Button($"撮影\n({sceneView.position.width}x{sceneView.position.height})", GUILayout.Height(40)))
            {
                SceneViewScreenShot.Capture(sceneView, (int) sceneView.position.width, (int) sceneView.position.height, 1, 4);
            }
            GUILayout.Space(10);
            if (GUILayout.Button($"撮影\n({sceneView.position.width*2}x{sceneView.position.height*2})", GUILayout.Height(40)))
            {
                SceneViewScreenShot.Capture(sceneView, (int) sceneView.position.width, (int) sceneView.position.height, 2, 4);
            }
            GUILayout.Space(10);
            if (GUILayout.Button($"撮影\n({sceneView.position.width*3}x{sceneView.position.height*3})", GUILayout.Height(40)))
            {
                SceneViewScreenShot.Capture(sceneView, (int) sceneView.position.width, (int) sceneView.position.height, 3, 4);
            }
            GUILayout.Space(10);
            if (GUILayout.Button($"撮影\n({sceneView.position.width*4}x{sceneView.position.height*4})", GUILayout.Height(40)))
            {
                SceneViewScreenShot.Capture(sceneView, (int) sceneView.position.width, (int) sceneView.position.height, 4, 4);
            }
            GUILayout.Space(10);
        }
    }
}