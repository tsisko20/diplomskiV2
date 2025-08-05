using UnityEngine;

namespace RTS.UI
{
    public class CursorManager : MonoBehaviour
    {
        public static CursorManager instance { get; private set; }
        [SerializeField] CursorObject[] cursorObjects;
        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            SetActiveCursorType(CursorType.Basic);
        }

        public void SetActiveCursorType(CursorType cursorType)
        {
            foreach (var cursorObject in cursorObjects)
            {
                if (cursorObject.GetCursorType() == cursorType)
                {
                    SetActiveCursorTexture(cursorObject.GetTexture());
                }
            }
        }

        private void SetActiveCursorTexture(Texture2D _cursorTexture)
        {
            Cursor.SetCursor(_cursorTexture, new Vector2(19, 17), CursorMode.Auto);
        }
    }
}
