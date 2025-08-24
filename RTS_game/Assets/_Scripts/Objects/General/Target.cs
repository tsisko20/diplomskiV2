using UnityEngine;

public enum TargetType
{
    Terrain,
    Unit,
    Resource,
    Building
}
public class Target : MonoBehaviour
{
    public TargetType targetType;
}
