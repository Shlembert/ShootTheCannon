using UnityEngine;
using UnityEngine.Pool;

public class PortalPool : MonoBehaviour
{
    [SerializeField] private Portal portal;
    private ObjectPool<Portal> _pool;

    void Start()
    {
        _pool = new ObjectPool<Portal>(
            () => Instantiate(portal, transform),
            (obj) => obj.gameObject.SetActive(true),
            (obj) => obj.gameObject.SetActive(false),
            (obj) => Destroy(obj.gameObject),
            false, 10, 100);
    }

    public Portal Create(Vector3 position)
    {
        Portal portal = _pool.Get();
        portal.Init(position, this);
        return portal;
    }

    public void Release(Portal portal)
    {
        _pool.Release(portal);
    }
}
