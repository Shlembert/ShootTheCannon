using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
    [SerializeField] private ProjectilePool projectilePool;
    [SerializeField] private int damage;
    [SerializeField] private Target target;
    [SerializeField] private float rotationSpeed = 180;
    [SerializeField] private Transform barrel;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private Transform cannon;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cannonSound;
    [SerializeField] private Image imageReload;

    private float _fireTimer;
    private bool _isGame;

    public float FireRate { get => fireRate; set => fireRate = value; }
    public bool IsGame { get => _isGame; set => _isGame = value; }

    private void Update()
    {
        if (!_isGame) return;

        Vector3 cursorScreenPosition = Input.mousePosition;
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint
            (new Vector3(cursorScreenPosition.x, cursorScreenPosition.y, transform.position.z));

        Vector3 targetDirection = cursorWorldPosition - transform.position;
        targetDirection.y = 0f; 

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-targetDirection.normalized, Vector3.up); 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        _fireTimer -= Time.deltaTime;

        float fillAmount = 1.0f - (_fireTimer / FireRate); 
        imageReload.fillAmount = fillAmount;

        if (Input.GetMouseButton(0)&& _fireTimer <=0)
        {
            _fireTimer = FireRate;
            projectilePool.Create(barrel.position, barrel.forward, projectileSpeed, damage);
            Recoil();
        }
    }

    private void Recoil()
    {
        particle.Play();
        audioSource.PlayOneShot(cannonSound);
        cannon.DOLocalMoveZ(cannon.localPosition.z - 0.8f, FireRate * 0.2f, false).OnComplete(() => 
        {
            cannon.DOLocalMoveZ(cannon.localPosition.z + 0.8f, FireRate * 0.8f, false);
        });
    }
}
