using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private PowerUpType _type;
    private float _speed;
    private PowerUpManager _manager;

    public void Init(PowerUpType type, float speed, PowerUpManager manager)
    {
        _type    = type;
        _speed   = speed;
        _manager = manager;
        GetComponent<SpriteRenderer>().color = type == PowerUpType.ExpandPaddle
            ? new Color(0.4f, 1f, 0.4f)
            : new Color(1f, 0.8f, 0.2f);
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
