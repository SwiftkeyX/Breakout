using UnityEngine;

public class TripleBallPowerUp : BasePowerUp
{
    public override Color Color => new Color(1f, 0.15f, 0.85f);
    public override void PickUp(PowerUpManager manager) => manager.ApplyTripleBall();
}
