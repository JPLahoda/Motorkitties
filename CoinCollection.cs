using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public int coinCount = 0;
    public TextMeshProUGUI coinText;
    private bool canCollect = true;

    private void OnTriggerEnter(Collider other)
    {   
        if (canCollect && other.CompareTag("Coin"))
        {
            StartCoroutine(CollectCoinAndDelay(other.gameObject));
        }
    }

    private IEnumerator CollectCoinAndDelay(GameObject coin)
    {
        canCollect = false;

        coinCount++;
        coinText.text = "Cheese: " + coinCount.ToString();
        Debug.Log("Collected " + coinCount + " coins.");
        Destroy(coin);

        yield return new WaitForSeconds(0.15f);

        canCollect = true;
    }

}
