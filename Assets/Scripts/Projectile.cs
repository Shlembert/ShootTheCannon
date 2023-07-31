using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int _damage;
    private ProjectilePool _pool;
    private float _speed;
    private Transform _transform;
    private bool _hit;
    private float _lifeTime;

    public void Init(Vector3 position, Vector3 direction, float speed, int damage, ProjectilePool pool)
    {
        _lifeTime = 2f;
        _hit = false;
        _transform = transform;
        _transform.position = position;
        _transform.forward = direction;
        _speed = speed;
        _pool = pool;
        _damage = damage;
    }

    private void Update()
    {
        if (_lifeTime >= 0) 
        {
            _transform.position += _transform.forward * _speed * Time.deltaTime;
            _lifeTime -= Time.deltaTime;
        } 
        else
        {
            Release();
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hit) return;

        Enemy enemy = collision.transform.GetComponent<Enemy>();
        IBonus bonus = collision.transform.GetComponent<IBonus>();

        if (enemy != null)
        {
            enemy.ApplayDamage(_damage);
            _hit = true;
        }

        if (bonus != null)
        {
            bonus.Hit();
            _hit = true;
        }

        Release();
    }

    private void Release()
    {
        _pool.Release(this);
    }
}
