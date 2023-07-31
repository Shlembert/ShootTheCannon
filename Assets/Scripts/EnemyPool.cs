using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    private ObjectPool<Enemy> _pool;

    void Start()
    {
        _pool = new ObjectPool<Enemy>(
            () => Instantiate(enemy, transform),
            (obj) => obj.gameObject.SetActive(true),
            (obj) => obj.gameObject.SetActive(false),
            (obj) => Destroy(obj.gameObject),
            false, 10, 100);
    }

    public void Create(Vector3 position, Transform target, float hpMultiplier, float speedMultiplier)
    {
        Enemy enemy = _pool.Get();
        enemy.Init(position, target, hpMultiplier, speedMultiplier, this);
    }

    public void Release(Enemy enemy)
    {
        _pool.Release(enemy);
    }
}
