using UnityEngine;

public class Magnet : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        UltaParticle ultaParticle = other.GetComponent<UltaParticle>();
        if(ultaParticle != null)
        {
            ultaParticle.Magnet(transform.position);
        }
    }
    private void OnDisable()
    {
        Debug.Log($"Magnet disabled! Stack: {StackTraceUtility.ExtractStackTrace()}", this);
    }

}
