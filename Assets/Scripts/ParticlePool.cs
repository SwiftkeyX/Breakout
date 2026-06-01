using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Instance { get; private set; }

    [SerializeField] private int _poolSize = 8;

    private ParticleSystem[] _pool;
    private int _index;

    void Awake()
    {
        if (Instance != null) return;
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _pool = new ParticleSystem[_poolSize];
        for (int i = 0; i < _poolSize; i++)
        {
            var go = new GameObject($"BurstParticles_{i}");
            go.transform.SetParent(transform);
            _pool[i] = BuildParticleSystem(go);
        }
    }

    public void Burst(Vector3 position, Color color)
    {
        var ps = _pool[_index];
        if (ps == null)
        {
            var go = new GameObject($"BurstParticles_{_index}");
            go.transform.SetParent(transform);
            ps = BuildParticleSystem(go);
            _pool[_index] = ps;
        }
        _index = (_index + 1) % _poolSize;
        ps.transform.position = position;
        var main = ps.main;
        main.startColor = color;
        ps.Play();
    }

    private static ParticleSystem BuildParticleSystem(GameObject go)
    {
        var ps = go.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.playOnAwake    = false;
        main.startLifetime  = 0.5f;
        main.startSpeed     = 5f;
        main.startSize      = 0.1f;
        main.maxParticles   = 20;
        main.duration       = 0.2f;
        main.loop           = false;
        main.stopAction     = ParticleSystemStopAction.None;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 12) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Rectangle;
        shape.scale     = new Vector3(0.8f, 0.25f, 0.1f);

        var vel = ps.velocityOverLifetime;
        vel.enabled  = true;
        vel.speedModifier = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
            new Keyframe(0f, 1f), new Keyframe(1f, 0f)));

        ps.Stop();
        return ps;
    }
}
