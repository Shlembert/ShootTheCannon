using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    private ObjectPool<Projectile> _pool;
    void Start()
    {
        _pool = new ObjectPool<Projectile>(
            () => Instantiate(projectile,transform),
            (obj) => obj.gameObject.SetActive(true),
            (obj) => obj.gameObject.SetActive(false),
            (obj) => Destroy(obj.gameObject),
            false, 10, 100);
    }

    public void Create(Vector3 position, Vector3 direction, float speed, int damage)
    {
        Projectile projectile = _pool.Get();
        projectile.Init(position, direction, speed, damage, this);
    }

    public void Release(Projectile projectile)
    {
        _pool.Release(projectile);
    }
}
