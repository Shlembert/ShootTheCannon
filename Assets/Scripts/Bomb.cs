using UnityEngine;

public class Bomb : MonoBehaviour, IBonus
{
    [SerializeField] private ParticleSystem particleBomb;
    [SerializeField] private AudioSource audioSource;

    public void Hit()
    {
        GameManager.Instance.KillAllEnemy(true);
        particleBomb.transform.position = transform.position;
        particleBomb.Play();
        audioSource.Play();
        gameObject.SetActive(false);
    }
}

public interface IBonus { public void Hit(); }
