using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KartGame.KartSystems
{
    public class PowerUpFollow : MonoBehaviour // Fishing Rod Script
    {   
        private GameObject player;
        private ArcadeKart kart;
        //public bool activated = false;

        void OnEnable()
        {
            //if (activated) {
                player = GameObject.FindWithTag("Player");
                kart = player.GetComponent<ArcadeKart>();
                //item = GetComponent<KartItem>();
                Debug.Log("PowerUpFollow: Awake");
                // Check if the ArcadeKart component is not null
                if (kart != null)
                {
                    Debug.Log("PowerUpFollow: Modified TopSpeed");
                    // Modify the TopSpeed property of the baseStats struct
                    kart.baseStats.TopSpeed = 25f;
                kart.baseStats.Acceleration = 15f;
                StartCoroutine(WaitToChangeSpeed(9f));
                    //activated = true;
                }
                else
                {
                    Debug.LogError("ArcadeKart component not found!");
                }
            //}
        }
        private IEnumerator WaitToChangeSpeed(float duration)
        {
            Debug.Log("Waiting to change speed...");
            yield return new WaitForSeconds(duration);
            Debug.Log("Speed changed back to 18");
            kart.baseStats.TopSpeed = 15f;
            kart.baseStats.Acceleration = 10f;
        }
    }
}
