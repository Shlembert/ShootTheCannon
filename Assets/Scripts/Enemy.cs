using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HealsBar healsBar;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float baseHp;
    [SerializeField] private float stoppingDistance = 4f;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider coll;
    [SerializeField] private Vector3 offsetPortal;
    [SerializeField] private AudioSource audioSourceStep, audioSourceSFX;
    [SerializeField] private AudioClip hitSound, deadSound;
    [SerializeField] private int score;

    private float _currentSpeed;
    private float _speed;
    private int _maxHp;
    public int _hp;
    private EnemyPool _pool;
    private Transform _transform;
    private Transform _target;
    private bool _dead;
    private bool _hit;
    private bool _portal;

    public bool Dead { get => _dead; set => _dead = value; }

    public void Init(Vector3 position, Transform target, float hpMultiplier, float speedMultiplier, EnemyPool pool)
    {
        GameManager.Instance.StopTime += Freezing;
        GameManager.Instance.PlayTime += UnFreezing;
        healsBar.ShowHPBar(false);
        audioSourceStep.Play();
        coll.enabled = true;
        _transform = transform;
        _transform.position = position;
        _maxHp = Mathf.RoundToInt(baseHp * hpMultiplier);
        _hp = _maxHp;
        healsBar.ChangeHP(_maxHp, _hp);
       _currentSpeed = _speed = baseSpeed * speedMultiplier;
        animator.SetFloat("RunSpeed", _speed * 0.4f);
        _transform.forward = target.position - _transform.position;
        _target = target;
        _pool = pool;
        Dead = false;
        _hit = false;
        _portal = true;
    }

    private void Freezing()
    {
        _speed = _currentSpeed * 0.2f;
        animator.SetFloat("RunSpeed", _speed * 0.4f);
    }

    private void UnFreezing()
    {
        _speed = _currentSpeed;
        animator.SetFloat("RunSpeed", _speed * 0.4f);
    }

    private void Update()
    {
        if (!_portal) return;

        if (Vector3.Distance(_transform.position, _target.position) > stoppingDistance && !Dead)
        {
            if (!_hit) _transform.position += transform.forward * _speed * Time.deltaTime;
        }
        else
        {
            audioSourceStep.Stop();
            animator.SetTrigger("Attack");
        }
    }

    public async void ApplayDamage(int damage)
    {
        _hp -= damage;
        healsBar.ChangeHP(_maxHp, _hp);
        healsBar.ShowHPBar(true);

        if (_hp <= 0)
        {
            _hp = 0;
            coll.enabled = false;
            Dead = true;
            _portal = false;
            audioSourceStep.Stop();
            audioSourceSFX.PlayOneShot(deadSound);
            animator.SetTrigger("Dead");
            GameManager.Instance.MonsterDead(score);
            healsBar.ShowHPBar(false);
            BonusDrop.instance.RandomDrop(_transform.position);
        }
        else
        {
            audioSourceStep.Stop();
            animator.SetTrigger("Hit");
            audioSourceSFX.PlayOneShot(hitSound);
            _hit = true;
            await UniTask.Delay(700);
            audioSourceStep.Play();
            _hit = false;
        }
    }

    public void Release()
    {
        GameManager.Instance.StopTime -= Freezing;
        GameManager.Instance.PlayTime -= UnFreezing;
        _pool.Release(this);
    }
}
