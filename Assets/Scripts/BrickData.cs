using UnityEngine;

[CreateAssetMenu(menuName = "Breakout2/BrickData")]
public class BrickData : ScriptableObject
{
    public int HitPoints;
    public int PointValue;
    public Color FullHealthColor;
    public Color DamagedColor;
}
