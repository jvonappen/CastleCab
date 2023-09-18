using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HorsePhysics : MonoBehaviour
{
    //Suspension
    //Offset is the range from Rest Distance
    //Force=(Offset * SpringStrength) - (Velocity * Damper)
    //-------------------------------------
    private bool rayDidHit;
    private Transform tireTransform;
    private Rigidbody carRigidBody;
    private RaycastResult tireRay; //dunno
    private float suspensionRestDist;
    private float springDamper;
    private float springStrength;
    //-------------------------------------
    private float tireGripFactor; //0to1 //gripFactor = curve.Evaluate( steeringVel / tireWorldVel.magnitude );
    private float tireMass; //Mass
    private float accelerationInput;
    //--------------------------------
    private Transform carTransform;
    private float carTopSpeed;
    private AnimationCurve powerCurve;

    private void Suspension()
    {
        if(rayDidHit)
        {
            //World-space direction of the spring force
            Vector3 springDirection = tireTransform.up;

            //World-space velocity of this tire
            Vector3 tireWorldVelocity = carRigidBody.GetPointVelocity(tireTransform.position);

            //Calculate offset from the Raycast
            float offset = suspensionRestDist - tireRay.distance;

            //Calculate velocity along the spring direction.
            //SpringDirection is a unit vector, which returns the magnitude of tireWorldVelocity, projected onto springDirection
            float velocity = Vector3.Dot(springDirection, tireWorldVelocity);

            // calculate magnitude of the dampened spring force
            float force = (offset * springStrength) - (velocity * springDamper);

            //Apply force to this tire, in direction of suspension
            carRigidBody.AddForceAtPosition(springDirection * force, tireTransform.position);

        }
    }

    private void SteeringForce()
    {
        if(rayDidHit)
        {
            //World-space
            Vector3 steeringDirection = tireTransform.right;

            Vector3 tireWorldVelocity = carRigidBody.GetPointVelocity(tireTransform.position);

            //Velocity of steering direction
            //SteeringDirection is a unit vector, which returns the magnitude of tireWorldVelocity, projected onto steeringDirection
            float steeringVelocity = Vector3.Dot(steeringDirection, tireWorldVelocity);

            //Change in velocity need is -steeringVelocity * gripFactor.
            //gripFactor range 0 to 1, 0 = no grip, 1 = max grip.
            float desiredVelocityChange = -steeringVelocity * tireGripFactor;

            //acceleration = (change in velocity / time)
            float desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;

            //Force = Mass * Acceleration, (multiply mass of the tire and apply as a force)
            carRigidBody.AddForceAtPosition(steeringDirection * tireMass * desiredAcceleration, tireTransform.position);

        }
    }

    private void Movement()
    {
        if(rayDidHit)
        {
            Vector3 accelerationDirection = tireTransform.forward;

            //acceleration torque
            if(accelerationInput > 0.0f)
            {
                //Forward speed
                float carSpeed = Vector3.Dot(carTransform.forward, carRigidBody.velocity);

                //normalized
                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / carTopSpeed);

                //available torque
                float availableTorque = powerCurve.Evaluate(normalizedSpeed) * accelerationInput;

                carRigidBody.AddForceAtPosition(accelerationDirection * availableTorque, tireTransform.position);
            }
        }
    }

}
