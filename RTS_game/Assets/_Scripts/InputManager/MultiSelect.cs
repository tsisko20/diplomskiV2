using UnityEngine;

namespace RTS.InputManager
{
    public static class MultiSelect
    {
        private static Texture2D _whiteTexture;

        public static Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }
                return _whiteTexture;
            }
        }
        public static void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, WhiteTexture);
        }
        public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
        {
            //top border
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            //bottom border
            DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
            //left border
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            //right border
            DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        }
        public static Rect GetScreenRect(Vector3 screenPos1, Vector3 screenPos2)
        {
            //from bottom right to top left
            screenPos1.y = Screen.height - screenPos1.y;
            screenPos2.y = Screen.height - screenPos2.y;
            Vector3 bR = Vector3.Max(screenPos1, screenPos2);
            Vector3 tL = Vector3.Min(screenPos1, screenPos2);

            return Rect.MinMaxRect(tL.x, tL.y, bR.x, bR.y);
        }
        public static Bounds GetVPBounds(Camera camera, Vector3 screenPos1, Vector3 screenPos2)
        {
            Vector3 pos1 = camera.ScreenToViewportPoint(screenPos1);
            Vector3 pos2 = camera.ScreenToViewportPoint(screenPos2);
            Vector3 min = Vector3.Min(pos1, pos2);
            Vector3 max = Vector3.Max(pos1, pos2);
            min.z = camera.nearClipPlane;
            max.z = camera.farClipPlane;
            Bounds bounds = new();
            bounds.SetMinMax(min, max);
            return bounds;
        }
    }
}

