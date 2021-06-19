using UnityEngine;
using UnityEngine.Rendering;

namespace SencanUtils
{
    public class ScreenshotHandler : Singleton<ScreenshotHandler>
    {
        private int _screenshotWidth;
        private int _screenshotHeight;
        private string _screenshotName;
        private bool _takeScreenshotOnNextFrame;

        private void OnEnable()
        {
            RenderPipelineManager.endCameraRendering += OnRenderEnd;
        }

        private void OnDisable()
        {
            RenderPipelineManager.endCameraRendering -= OnRenderEnd;
        }

        public void TakeScreenshot(int width, int height, string name)
        {
            _screenshotWidth = width;
            _screenshotHeight = height;
            _takeScreenshotOnNextFrame = true;
            _screenshotName = name;
        }


        private void OnRenderEnd(ScriptableRenderContext context, Camera cam)
        {
            if (_takeScreenshotOnNextFrame)
            {
                _takeScreenshotOnNextFrame = false;
                
                cam.targetTexture = RenderTexture.GetTemporary(_screenshotWidth, _screenshotHeight, 16);
                
                RenderTexture renderTexture = cam.targetTexture;
                Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
                Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
                texture2D.ReadPixels(rect, 0, 0);

                byte[] bytes = texture2D.EncodeToPNG();
                var path = Application.dataPath + "/Screenshots/" + _screenshotName + ".png";
                System.IO.File.WriteAllBytes(path, bytes);
                Debug.Log("Saved " + path);

                RenderTexture.ReleaseTemporary(renderTexture);
                cam.targetTexture = null;
            }   
        }
    }
}
