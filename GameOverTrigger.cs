using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverTrigger : MonoBehaviour
{
    //add respawn points
    //make player lose coins
    public CoinCollection otherScript;
    private bool canExecute = true;
    private void OnTriggerEnter(Collider other)
    {
        if (!canExecute)
        {
            return; // If not allowed to execute, exit the method
        }
        if (other.CompareTag("enemy") || other.CompareTag("Terrain"))
        {
            StartCoroutine(ExecuteWithCooldown());
        }
    }

    

    private IEnumerator ExecuteWithCooldown()
    {
        canExecute = false;
        Debug.LogWarning("OnTriggerEnter: GameOverTrigger: Coin Script Begin"); //debugging
        if (otherScript.coinCount >= 3)
        {
            otherScript.coinCount -= 3;
        }
        else
        {
            otherScript.coinCount = 0;
        }
        otherScript.coinText.text = "Cheese: " + otherScript.coinCount.ToString();

        GameObject[] respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        GameObject[] respawnReferences = GameObject.FindGameObjectsWithTag("directionReference");
        // Initialize variables to store the nearest respawn point and its distance
        GameObject nearestRespawnPoint = null;
        GameObject nearestRespawnReference = null;
        float nearestDistance = Mathf.Infinity;

        // Iterate through all respawn point to find the nearest one
        foreach (GameObject respawnPoint in respawnPoints)
        {
            float distance = Vector3.Distance(transform.position, respawnPoint.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestRespawnPoint = respawnPoint;
            }
        }

        // Check if a nearest respawn point is found
        if (nearestRespawnPoint != null)
        {
            // Respawn the player at the position of the nearest respawn point
            GameObject.FindWithTag("Player").transform.position = nearestRespawnPoint.transform.position;

            // Find out where nearest respawnReference is AFTER teleporting
            nearestDistance = Mathf.Infinity;
            foreach (GameObject respawnReference in respawnReferences)
            {
                float distance = Vector3.Distance(transform.position, respawnReference.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestRespawnReference = respawnReference;
                }
            }

            // Rotate the player to face the correct way (towards the object in front of the respawn point)
            Vector3 direction = nearestRespawnReference.transform.position - GameObject.FindWithTag("Player").transform.position;
            GameObject.FindWithTag("Player").transform.rotation = Quaternion.LookRotation(direction);

        }
        else
        {
            Debug.LogWarning("No respawn objects found.");
        }

        yield return new WaitForSeconds(0.25f); // Wait for 0.25 seconds
        canExecute = true;

        // Load the game over scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
