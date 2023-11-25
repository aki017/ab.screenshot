using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ab.ScreenShot.Editor
{
    [InitializeOnLoad]
    public class SceneViewScreenShot
    {
        static SceneViewScreenShot()
        {
            SceneView.duringSceneGui += OnGUI;
        }

        private static void OnGUI(SceneView sceneView)
        {
            if (!EditorPrefs.GetBool("Ab.ScreenShot|Scene", true))
            {
                return;
            }
            Handles.BeginGUI();

            GUILayout.FlexibleSpace();

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button($"撮影\n({sceneView.position.width}x{sceneView.position.height})", GUILayout.Height(40), GUILayout.Width(100)))
                {
                    Capture(sceneView, (int)sceneView.position.width, (int)sceneView.position.height, 1, 4);
                }
                GUILayout.Space(10);
                if (GUILayout.Button($"撮影\n({sceneView.position.width * 2}x{sceneView.position.height * 2})", GUILayout.Height(40), GUILayout.Width(100)))
                {
                    Capture(sceneView, (int)sceneView.position.width, (int)sceneView.position.height, 2, 4);
                }
                GUILayout.Space(10);
            }

            GUILayout.Space(30);

            Handles.EndGUI();
        }

        internal static void Capture(SceneView sceneView, int width, int height, int size, int scale)
        {
            width *= size;
            height *= size;
            var path = EditorUtility.SaveFilePanel("Save Image", "", "image.png", "png");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var texture = new Texture2D(width * scale, height * scale, TextureFormat.RGBA32, false);
            var texture2 = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var rt = RenderTexture.GetTemporary(width * scale, height * scale, 32, RenderTextureFormat.Default);
            var camera = sceneView.camera;
            var flag = camera.clearFlags;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.clear;
            camera.targetTexture = rt;
            camera.Render();
            camera.targetTexture = null;
            camera.clearFlags = flag;

            RenderTexture.active = rt;
            texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            texture.Apply();

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    for (var ii = 0; ii < scale; ii++)
                    {
                        var r = 0f;
                        var g = 0f;
                        var b = 0f;
                        var a = 0f;
                        var tcount = 0;
                        for (var jj = 0; jj < scale; jj++)
                        {
                            var c = texture.GetPixel(i * scale + ii, j * scale + jj);
                            if (c.a == 0)
                            {
                                tcount += 1;
                                continue;
                            }
                            r += c.r;
                            g += c.g;
                            b += c.b;
                            a += c.a;
                        }
                        if (tcount == scale)
                        {
                            texture2.SetPixel(i, j, Color.clear);
                        }
                        texture2.SetPixel(i, j, new Color(r / (scale - tcount), g / (scale - tcount), b / (scale - tcount), a / scale));
                    }
                }
            }

            byte[] bytes = texture2.EncodeToPNG();
            Object.DestroyImmediate(texture);
            Object.DestroyImmediate(texture2);

            File.WriteAllBytes(path, bytes);
            RenderTexture.active = null;
        }
    }
}
