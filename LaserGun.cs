using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    //public LineRenderer beam;
    public Transform muzzlePoint;
    public float maxLength = 20f;

    public ParticleSystem Muzzle;
    public ParticleSystem HitPoint;

    public float damageAmount = 5f;
    public GameObject bulletPrefab; // Prefab of the bullet to fire
    private GameObject bullet; // Current bullet
    public float bulletSpeed = 50f; // Speed of the bullet
    public float fireInterval = 0.5f; // Interval between firing bullets

    // Reference to the empty GameObject that will hold hitParticles
    public GameObject hitParticlesHolder;

    private void Awake()
    {
        HitPoint.Stop();
        Debug.Log("LaserGun: Awake");
    }

    private void Start()
    {
        StartCoroutine(FireBullets());
        Debug.Log("LaserGun: Start");
    }

    private IEnumerator FireBullets()
    {   
        while (true)
        {
            Debug.Log("LaserGun: Shooting");
            Shoot();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void Shoot()
    {
        Debug.Log("LaserGun: Start of Shoot");
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = (muzzlePoint.position - worldMousePosition).normalized;

        RaycastHit hit;
        if (Physics.Raycast(muzzlePoint.position, direction, out hit, maxLength))
        {
            Debug.Log("LaserGun: if");
            bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.identity); //muzzlePoint.rotation?
            bullet.transform.LookAt(hit.point); // ADDED

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = direction * bulletSpeed; //bulletRb.velocity = muzzlePoint.forward * bulletSpeed;

            //bullet.transform.rotation = Quaternion.LookRotation(bulletRb.velocity);

            Destroy(bullet, maxLength / bulletSpeed);

            Muzzle.transform.position = muzzlePoint.position;

            hitParticlesHolder.transform.position = hit.point;
            StartCoroutine(PlayHitParticles(1.5f));
        }
        else
        {
            // If raycast doesn't hit anything, just fire the bullet in the direction of the mouse
            Debug.Log("LaserGun: else");
            bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.identity);
            bullet.transform.LookAt(muzzlePoint.position + direction); // Rotate the bullet to look in the direction of fire
        
            // Calculate bullet velocity
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = direction * bulletSpeed;
        
            // Destroy bullet after reaching maxLength
            Destroy(bullet, maxLength / bulletSpeed);
        
            // Set muzzleParticles position to muzzlePoint
            Muzzle.transform.position = muzzlePoint.position;
        
        
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            return; // Exit the method if the collider is null
        }
        if (other.CompareTag("enemy"))
        {
            Debug.Log("LaserGun: isEnemy");
            other.GetComponent<damage>().ApplyDamage(3.5f);
            Vector3 closestPoint = other.ClosestPoint(bullet.transform.position);
            //hitParticles.transform.position = other.ClosestPoint(bullet.transform.position);
            hitParticlesHolder.transform.position = closestPoint;
            StartCoroutine(PlayHitParticles(1.5f));
            Destroy(bullet); // Destroy the bullet upon hitting an enemy (NOT WORKING)
                             //StartCoroutine(PlayHitParticles(1.5f));
        }
        else //if (bullet != null)
        {
            Debug.Log("LaserGun: else if");
            Vector3 closestPoint = other.ClosestPoint(bullet.transform.position);
            //hitParticles.transform.position = other.ClosestPoint(bullet.transform.position);
            hitParticlesHolder.transform.position = closestPoint;
            StartCoroutine(PlayHitParticles(1.5f));
            Destroy(bullet);
        }
        //else
        //{
        //    hitParticles.transform.position = other.ClosestPoint(bullet.transform.position);
        //    Destroy(bullet);
        //    StartCoroutine(PlayHitParticles(1.5f));
        //}
    }

    private IEnumerator PlayHitParticles(float duration)
    {
        HitPoint.Play();
        Debug.Log("LaserGun: Play Hit Particles");

        yield return new WaitForSeconds(duration);

        HitPoint.Stop();
    }
}