using UnityEngine;

public class Brick : MonoBehaviour
{
    private int _hp;
    private int _maxHp;
    private bool _destructible;
    private BrickData _data;
    private BrickManager _manager;
    private PowerUpManager _powerUpManager;
    private SpriteRenderer _sr;

    void Awake() => _sr = GetComponent<SpriteRenderer>();

    public void Init(BrickData data, BrickManager manager, bool destructible,
                     PowerUpManager powerUpManager = null)
    {
        _data           = data;
        _manager        = manager;
        _destructible   = destructible;
        _powerUpManager = powerUpManager;
        _hp = _maxHp   = data.HitPoints;
        _sr.color       = data.FullHealthColor;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!_destructible) return;
        TakeDamage(1);
    }

    private void TakeDamage(int amount)
    {
        _hp -= amount;
        UpdateVisual();
        if (_hp <= 0)
            Die();
        else
        {
            // Reinforced brick survived — brief hitstop + hit SFX
            CameraEffects.Instance?.HitStop(0.03f);
            AudioManager.Instance?.Play(AudioManager.Instance.SfxHitBrick);
        }
    }

    private void UpdateVisual()
    {
        if (_maxHp <= 1) return;
        float t = (float)_hp / _maxHp;
        _sr.color = Color.Lerp(_data.DamagedColor, _data.FullHealthColor, t);
    }

    private void Die()
    {
        CameraEffects.Instance?.HitStop(0.06f);
        CameraEffects.Instance?.Shake(0.08f, 0.15f);
        ParticlePool.Instance?.Burst(transform.position, _data.FullHealthColor);
        AudioManager.Instance?.Play(AudioManager.Instance.SfxBrickBreak);

        _manager.OnBrickDestroyed();
        ScoreManager.Instance?.AddScore(_data.PointValue);
        _powerUpManager?.TrySpawnDrop(transform.position);
        Destroy(gameObject);
    }
}
