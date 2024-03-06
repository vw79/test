using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TurretControl : MonoBehaviour
{
    public GameObject target;
    public Transform turretTransform;
    public GameObject bulletPrefab;
    private Vector3 targetPosition;
    private Vector3 selfPosition;
    private Vector3 selfVelocity;
    private Vector3 targetVelocity;
    private Vector3 interceptPosition;
    private Quaternion originalRotation;

    [SerializeField] private float bulletSpeed;
    private Health targetHealth;

    void Start()
    {
        originalRotation = turretTransform.rotation;
        targetHealth = target.GetComponent<Health>();
    }

    void Update()
    {

        selfPosition = transform.position;
        selfVelocity = gameObject.GetComponent<Drive>().CurrentSpeed;
        targetPosition = target.transform.position;
        targetVelocity = target.GetComponent<Drive>().CurrentSpeed;

        if (targetHealth.isDead)
        {
            turretTransform.rotation = Quaternion.Slerp(turretTransform.rotation, originalRotation, Time.deltaTime * 100);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) 
        {
            bool canIntercept = CalculateInterceptPosition(selfPosition, selfVelocity, targetPosition, targetVelocity, bulletSpeed, out Vector3 interceptPosition);
            if (canIntercept)
            {
                Shoot(selfPosition, interceptPosition);
                Debug.Log("Intercept position: " + interceptPosition);
            }
            else
            {
                turretTransform.rotation = originalRotation;
            }
        }
    }

    bool CalculateInterceptPosition(Vector3 selfPosition, Vector3 selfVelocity, Vector3 targetPosition, Vector3 targetVelocity, float bulletSpeed, out Vector3 interceptPosition)
    {
        Vector3 targetRelativeVelocity = targetVelocity - selfVelocity;
        Vector3 displacement = targetPosition - selfPosition;

        float a = Vector3.Dot(targetRelativeVelocity, targetRelativeVelocity) - bulletSpeed * bulletSpeed;
        float b = 2 * Vector3.Dot(displacement, targetRelativeVelocity);
        float c = Vector3.Dot(displacement, displacement);

        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            interceptPosition = Vector3.zero;
            return false;
        }

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float t1 = (-b + sqrtDiscriminant) / (2 * a);
        float t2 = (-b - sqrtDiscriminant) / (2 * a);

        float interceptionTime = Mathf.Min(t1, t2);
        if (interceptionTime < 0)
        {
            interceptionTime = Mathf.Max(t1, t2);
        }
        if (interceptionTime < 0)
        {
            interceptPosition = Vector3.zero;
            return false;
        }

        // Future position of the target
        interceptPosition = targetPosition + targetVelocity * interceptionTime;

        return true;

        // Im sorry, I have tried my best to implement but this is still buggy
    }

    public void Shoot(Vector3 selfPosition, Vector3 interceptPosition)
    {
        Vector3 rotateDirection = targetPosition - turretTransform.position;
        Quaternion lookRotation = Quaternion.LookRotation(rotateDirection);
        turretTransform.rotation = Quaternion.Slerp(turretTransform.rotation, lookRotation, Time.deltaTime * 200);

        Vector3 shootDirection = (interceptPosition - selfPosition).normalized;
        GameObject bullet = Instantiate(bulletPrefab, selfPosition, Quaternion.identity);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootDirection * bulletSpeed;
        }
    }
}