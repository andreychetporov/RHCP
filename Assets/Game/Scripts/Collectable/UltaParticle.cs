using Game.Level;
using UnityEngine;

public class UltaParticle : MonoBehaviour
{
    [SerializeField] private GameEvent addParticleEvent;
    [SerializeField] private float speed = 10.0f;
    public GameEvent AddParticleEvent => addParticleEvent;

    private void Start()
    {
        if (speed <= 0.0f) speed = 10.0f;
    }

    public void Magnet(Vector3 playerPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, LevelBootstrap.Instance.PlayerController.transform.position, speed * Time.deltaTime);
    }
}
