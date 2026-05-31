using UnityEngine;

[CreateAssetMenu(menuName = "Breakout2/LevelData")]
public class LevelData : ScriptableObject
{
    public int Rows;
    public int Columns;
    public BrickType[] Grid;
    public float BallSpeedMultiplier = 1f;
}
