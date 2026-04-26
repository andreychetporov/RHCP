using System.Runtime.InteropServices;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private Coin coin;
    private void OnTriggerEnter(Collider other)
    {
        coin = other.GetComponent<Coin>();
        if (coin != null)
        {

        }
    }

    private void Coin_OnCollected(object sender, System.EventArgs e)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (coin != null)
        {

        }
    }
}
