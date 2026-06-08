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

    void Update()
    {
        var pos = transform.position;
        pos.y -= _speed * Time.deltaTime;
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Paddle")) return;
        AudioManager.Instance?.Play(AudioManager.Instance.SfxPowerUp);
        _effect.PickUp(_manager);
        Destroy(gameObject);
    }

    void OnBecameInvisible() => Destroy(gameObject);
}
