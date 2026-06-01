using UnityEngine;

public class Brick : MonoBehaviour
{
    private int _hp;
    private int _maxHp;
    private bool _destructible;
    private BrickData _data;
    private BrickManager _manager;
    private SpriteRenderer _sr;

    void Awake() => _sr = GetComponent<SpriteRenderer>();

    public void Init(BrickData data, BrickManager manager, bool destructible)
    {
        _data         = data;
        _manager      = manager;
        _destructible = destructible;
        _hp = _maxHp  = data.HitPoints;
        _sr.color     = data.FullHealthColor;
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
        {
            _manager.OnBrickDestroyed(_data, transform.position);
            Destroy(gameObject);
        }
        else
        {
            _manager.OnBrickDamaged();
        }
    }

    private void UpdateVisual()
    {
        if (_maxHp <= 1) return;
        float t = (float)_hp / _maxHp;
        _sr.color = Color.Lerp(_data.DamagedColor, _data.FullHealthColor, t);
    }
}

