using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KartItem : MonoBehaviour
{
    public GameObject[] itemGameobjects; // array of power ups dispensed
    public GameObject pickupEffectPrefab; // particle system
    public float respawnDelay = 5.0f; // Delay before respawning the item box
    public float effectDuration = 10.0f; // Duration of the power-up effect (dont mind the huge numbers)
    public GameObject objectToActivate;
    public bool reactivate = true; //item box reactivates
    // test: [SerializeField] float delay = 2f;
    // private bool used = false; // flag to check if the item box has been used
    private bool delayActive = false;
    public Animator itemboxanimator;
    public bool hasItem = false;

    public Sprite[] itemSprites;
    public Image yourSprite;

    public Animator ItemUIAnim;
    public Animator ItemUIScroll;
    public AnimationClip animation1;

    int index;


    public GameObject player;
    //public PowerUpFollow powerUpFollow;


    public Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        initialPosition = transform.position;
    }




    private void OnTriggerEnter(Collider other)
    {   
        //if (other.CompareTag("ItemBox") && !delayActive)
        //{
        //    objectToActivate = other.gameObject;
        //    Debug.Log("ItemBox!!!!!!!!!!!");
        //    StartCoroutine(ActivationRoutine(objectToActivate, respawnDelay));
        //}
        if (other.CompareTag("Player") && !hasItem && !delayActive)
        {
            Debug.Log("Player!!!!!!!!!!");
            Instantiate(pickupEffectPrefab);
            /*, player.transform.position, Quaternion.identity*/
            ApplyPowerUpEffect(other.gameObject);

            hasItem = true; //JASON ADDED LINE
            //used = true;
            //itemboxanimator = other.GetComponent<Animator>();
            itemboxanimator.SetBool("idle", true);



            // objectToActivate.SetActive(false);
            StartCoroutine(ActivationRoutine(objectToActivate, respawnDelay));
            StartCoroutine(ActivationRoutine2(objectToActivate, .4f));
            StartCoroutine(DelayCoroutine(8f));
            
        }
    }

    private IEnumerator ActivationRoutine(GameObject objectToActivate, float delayTime)
    {
        //delayActive = true;
        yield return new WaitForSeconds(delayTime);
        objectToActivate.SetActive(true);
    }

    private IEnumerator ActivationRoutine2(GameObject objectToActivate, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        objectToActivate.SetActive(false);
        itemboxanimator.SetBool("idle", false);

    }

    private IEnumerator UIStuff(Collider other)
    {
        if (other.gameObject.tag == "ItemBox")
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;

            other.gameObject.GetComponent<Animator>().SetBool("Enlarge", false);

            StartCoroutine(getItem());
            ItemUIAnim.SetBool("ItemIn", true);
            ItemUIScroll.SetBool("Scroll", true);

            //renable

            yield return new WaitForSeconds(1);
            other.gameObject.GetComponent<Animator>().SetBool("Enlarge", true);
            other.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }


    public IEnumerator getItem()
    {
        if (!hasItem)
        {
            index = Random.Range(0, itemGameobjects.Length);
            yourSprite.sprite = itemSprites[index];
            yield return new WaitForSeconds(4f);

            itemGameobjects[index].SetActive(true);
            hasItem = true;
        }
    }

    private IEnumerator DelayCoroutine(float delayTime)
    {
        delayActive = true;
        yield return new WaitForSeconds(delayTime);
        delayActive = false;
        hasItem = false;
    }

    public void ApplyPowerUpEffect(GameObject player)
    {

        // Check if the player already has a power-up
        PowerUp existingPowerUp = player.GetComponent<PowerUp>();

        if (existingPowerUp != null)
        {
            // Destroy the existing power-up
            Destroy(existingPowerUp.gameObject);
        }

        if (itemGameobjects.Length > 0)
        {

            int randomIndex = Random.Range(0, itemGameobjects.Length); // Choose a random item from the array
            GameObject itemToDispense = itemGameobjects[randomIndex];

            if (randomIndex == 0) // Logic for fishing rod (Power Up Follow script)
            {
                //powerUpFollow.activated = true;
                Vector3 spawnPosition = player.transform.position + player.transform.forward * 2.0f + Vector3.up * 2.5f;

                // Calculate rotation to face the same direction of the player
                Quaternion rotation = Quaternion.LookRotation(player.transform.forward) * Quaternion.Euler(30, 0, 0);

                // Instantiate the chosen item at the calculated position and rotation
                GameObject powerUpInstance = Instantiate(itemToDispense, spawnPosition, rotation);

                // Set the power-up as a child of the player
                powerUpInstance.transform.SetParent(player.transform);

                // Remove the power-up effect after the specified duration
                Destroy(powerUpInstance, effectDuration);
            }
            else if (randomIndex == 1) // Logic for Ray Gun
            {
                Vector3 spawnPosition = player.transform.position + player.transform.forward;

                // Instantiate the chosen item at the player's position
                GameObject powerUpInstance = Instantiate(itemToDispense, spawnPosition, player.transform.rotation); //player.transform.position, Quaternion.identity

                // Set the power-up as a child of the player
                powerUpInstance.transform.SetParent(player.transform);

                // Remove the power-up effect after the specified duration
                Destroy(powerUpInstance, effectDuration);
                //     hasItem = false;
                //     ItemUIAnim.SetBool("ItemIn", false);
                //     ItemUIScroll.SetBool("Scroll", false);
                //     itemGameobjects[index].SetActive(false);
            }
        }
        else
        {
            Debug.LogError("No items assigned to the KartItem!");
        }
    }
}

