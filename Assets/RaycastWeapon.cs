using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{

    class Bullet
    {
        public float time; // time the bullet should alive

        // use equation to  calcilate the position of bullet at given time
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }
    public ActiveWeapon.WeaponSlot weaponSlot;
    public bool isFiring = false;
    public int fireRate = 25; // 25 bullet /sec
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f; // how far the bullet is going to drop over time
    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public Transform raycastOrigin;
    public Transform raycastDestination;

    public string weaponName;

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>(); // store the bullet which is fired
    float maxLifetime = 3.0f;

    Vector3 GetPosition(Bullet bullet)
    {
        // position of bullet over time
        //p + v*t + 0.5 *g*t*t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position , Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position); // starting point
        return bullet;
    }

    // Start is called before the first frame update

    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0.0f; // when to fire the next bullet
        FireBullet();
    }

    public void UpdateFiring(float deltaTime)
    { // it will called each frame

        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate; // interval between the bullet
        while(accumulatedTime >= 0.0f) // if time left then fire it
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }


    public void UpdateWeapon(float deltaTime)
    {

        if (Input.GetButtonDown("Fire1"))
        {
            StartFiring();
        }

        if (isFiring)
        {
            UpdateFiring(deltaTime);
        }
        UpdateBullets(deltaTime);

        if (Input.GetButtonUp("Fire1"))
        {
            StopFiring();
        }
    }

    public void UpdateBullets(float deltaTime)
    {// update the time value eaqch frame
        SimulateBullets(deltaTime);
        DestoryBullets();
    }

    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            // get the current bullet position before simulation
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime; // how much the bullet is alive for
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    void DestoryBullets()
    {
        bullets.RemoveAll(bullet => bullet.time > maxLifetime);
    }

    void RaycastSegment(Vector3 start, Vector3 end,Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red,1.0f);
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifetime;
        }
        else {
            bullet.tracer.transform.position = end;
        }
    }

    private void FireBullet()
    {
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }
        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed; // direction of ray * bullet speed
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);

        //ray.origin = raycastOrigin.position;

        ////direction for raycast of gun shoot from muzzle to cross hair
        //ray.direction = raycastDestination.position - raycastOrigin.position;

        //var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        //tracer.AddPosition(ray.origin);
        //if (Physics.Raycast(ray, out hitInfo))
        //{
        //    //Debug.DrawLine(ray.origin, hitInfo.point, Color.red,1.0f);
        //    hitEffect.transform.position = hitInfo.point;
        //    hitEffect.transform.forward = hitInfo.normal;
        //    hitEffect.Emit(1);

        //    tracer.transform.position = hitInfo.point;
        //}
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
