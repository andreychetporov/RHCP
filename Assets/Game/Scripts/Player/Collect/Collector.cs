using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        UltaParticle particle = other.GetComponent<UltaParticle>();
        if (particle != null)
        {
            particle.AddParticleEvent.Raise();
            Destroy(particle.gameObject);
        }
    }
}
