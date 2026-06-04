using UnityEngine;

// Don't 
public class PowerUp : MonoBehaviour
{
    private PowerUpType _type;
    private float _speed;
    private PowerUpManager _manager;

    /// <summary>
    /// Don't hard code color
    /// Make them more flexible, imagine we need to add new powerup in the future.
    /// My approach: 
    /// Each powerup should have its own class, and each color should be state inside those class, not here.
    /// Each Powerup should inherit from abstract class BasePowerUp.cs instead
    /// PickUp() effect should be inside each Powerup class
    /// 
    /// </summary>
    public void Init(PowerUpType type, float speed, PowerUpManager manager)
    {
        _type    = type;
        _speed   = speed;
        _manager = manager;
        GetComponent<SpriteRenderer>().color = (type == PowerUpType.TripleBall)
            ? new Color(1f, 0.15f, 0.85f)
            : new Color(1f, 0.5f, 0f);
    }

    void Update() => transform.Translate(Vector2.down * _speed * Time.deltaTime);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Paddle")) return;
        AudioManager.Instance?.Play(AudioManager.Instance.SfxPowerUp);
        _manager.OnPickup(_type);
        Destroy(gameObject); 
    }

    void OnBecameInvisible() => Destroy(gameObject);
}
