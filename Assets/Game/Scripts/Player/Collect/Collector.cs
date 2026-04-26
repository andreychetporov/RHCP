using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Coin coin = other.GetComponent<Coin>();
        coin = other.GetComponent<Coin>();
        if (coin != null)
        {

        }
    }
}
