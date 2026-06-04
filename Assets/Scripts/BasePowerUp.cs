using UnityEngine;

public abstract class BasePowerUp
{
    public abstract Color Color { get; }
    public abstract void PickUp(PowerUpManager manager);
}
