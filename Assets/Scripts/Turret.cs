using UnityEngine;

public class Turret : MonoBehaviour {

    private Transform target;           //Target the turret... well targets

    [Header("Attributes")]
    [Range(0f, 20f)] public float range = 15f;           //Range of the turret
    [Range(0f, 20f)] public float fireRate = 1f;         //Rate of fire
    private float fireCountdown = 0f;

    [Header("References")]
    public Transform partToRotate;      //Part of the turret that rotates
    public string enemyTag = "Enemy";    
    [Range(0f, 20f)] public float rotationSpeed = 15f;   //Speed of the head rotation
    public GameObject bulletPrefab;
    public Transform firePoint;
    
    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 0.5f);
    }

    // Update is called once per frame
    void Update () {
        //If there is no target, don't do anything
        if (target == null)
        {
            return;
        }        
        
        //Target lock on
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;

        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);        
        
        if(fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
	}

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation) as GameObject;
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
