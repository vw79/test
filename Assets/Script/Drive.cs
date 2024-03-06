using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Drive : MonoBehaviour
{
    public enum EngineState
    {
        Accelerate,
        Idle,
        Brake,
    }

    public enum TankColor 
    { 
        Yellow,
        Red
    }

    [SerializeField] private Transform target;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float forwardAcceleration;
    [SerializeField] private float brakeDeceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private TankColor tankColor;
    [SerializeField] private TextMeshProUGUI speedDisplay;
    private bool reachedTarget;

    private void Update()
    {
        if (!reachedTarget)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            EngineState engineState = DetermineEngineState(distanceToTarget, currentSpeed, forwardAcceleration, brakeDeceleration, maxSpeed);

            // Engine State
            switch (engineState)
            {
                case EngineState.Accelerate:
                    currentSpeed += forwardAcceleration * Time.deltaTime;
                    break;
                case EngineState.Brake:
                    currentSpeed -= brakeDeceleration * Time.deltaTime;
                    break;
                case EngineState.Idle:
                    break;
            }

            // 0 < currentSpeed < maxSpeed
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

            // Move 
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

            // Vehicle is stopped if it's very close to the target
            if (distanceToTarget < 1f && Mathf.Abs(currentSpeed) < 1f)
            {
                currentSpeed = 0;
                reachedTarget = true;
            }

            switch (tankColor)
            {
                case TankColor.Red:
                    {
                        speedDisplay.text = $"Red: {currentSpeed:F2} m/s";
                    }
                    break;
                case TankColor.Yellow:
                    {
                        speedDisplay.text = $"Yellow: {currentSpeed:F2} m/s";
                    }
                    break;
            }
        }
    }

    private EngineState DetermineEngineState(float distanceToTarget, float currentSpeed, float forwardAcceleration, float brakeDeceleration, float maxSpeed)
    {
        // v2 = u2 + 2as
        float stoppingDistance = (currentSpeed * currentSpeed) / (2 * brakeDeceleration);

        if (distanceToTarget > stoppingDistance && currentSpeed < maxSpeed)
        {
            return EngineState.Accelerate;
        }
        else if (distanceToTarget <= stoppingDistance || distanceToTarget < 1f) // Added condition for when very close to the target
        {
            return EngineState.Brake;
        }
        else
        {
            return EngineState.Idle;
        }
    }

    public Vector3 CurrentSpeed
    {
        get { return transform.forward * currentSpeed; }
    }
}

