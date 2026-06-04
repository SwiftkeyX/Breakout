using UnityEngine;

public class SpeedBoostPowerUp : BasePowerUp
{
    public override Color Color => new Color(1f, 0.5f, 0f);
    public override void PickUp(PowerUpManager manager) => manager.ApplySpeedBoost();
}
