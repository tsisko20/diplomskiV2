using RTS.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cursor", menuName = "New Cursor Texture")]
public class CursorObject : ScriptableObject
{
    [SerializeField] private Texture2D texture;
    [SerializeField] private CursorType type;

    public Texture2D GetTexture() => texture;
    public CursorType GetCursorType() => type;
}
