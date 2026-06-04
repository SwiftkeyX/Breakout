using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private BasePowerUp _effect;
    private float _speed;
    private PowerUpManager _manager;

    public void Init(BasePowerUp effect, float speed, PowerUpManager manager)
    {
        _effect  = effect;
        _speed   = speed;
        _manager = manager;
        GetComponent<SpriteRenderer>().color = effect.Color;
    }

    void Update() => transform.Translate(Vector2.down * _speed * Time.deltaTime);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Paddle")) return;
        AudioManager.Instance?.Play(AudioManager.Instance.SfxPowerUp);
        _effect.PickUp(_manager);
        Destroy(gameObject);
    }

    void OnBecameInvisible() => Destroy(gameObject);
}
