using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool usingTemps = true;
    public float movementInput;

    private float rotation;
    public float rotationInput
    {
        get { return rotation; }
        set
        {
            rotation = value;
            RotatePlayer();
        }
    }

    [Range(-1, 1)] public float tempMove = 0;
    [Range(-1, 1)] public float tempRotation = 0;

    public float force;
    public float rotSpeed = 2;
    public float currentRot;

    private Rigidbody _rb;

    public bool isYUp = true;
    ArduinoSerialReadWrite _arduino;

    public static Action<Quaternion> OnStopMoving;
    bool justStopped = false;
    bool checkCalibrate = false;
    IEnumerator doCheckCalibrate;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _arduino = FindAnyObjectByType<ArduinoSerialReadWrite>();
    }

    private void FixedUpdate()
    {
        if (movementInput != 0)
            _rb.AddForce(transform.up * movementInput * force, ForceMode.Impulse);
        else
        {
            _rb.linearVelocity = Vector3.zero;
            if (justStopped)
            {
                OnStopMoving?.Invoke(transform.rotation);
                justStopped = false;
            }
        }
    }

    void RotatePlayer()
    {
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
    }

    void Update()
    {
        if (usingTemps)
        {
            movementInput = tempMove;
            rotationInput = tempRotation;
        }
        else
        {
            float rotDir;
            if (isYUp)
                rotDir = _arduino.angularVelocity.x;
            else
                rotDir = _arduino.angularVelocity.y;

            if (currentRot != rotDir)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, rotDir), rotSpeed * Time.deltaTime);
            
            if (WithinRotation(rotDir))
                currentRot = rotDir;

            movementInput = 0;
            if (_arduino.forwardPress == 1)
            {
                movementInput = 1;
                justStopped = true;
            }
            if (_arduino.backPress == 1)
            {
                movementInput = -1;
                justStopped = true;
            }

            if (_arduino.backPress == 1 && _arduino.forwardPress == 1)
            {
                if (doCheckCalibrate == null)
                {
                    doCheckCalibrate = RecalibrateController();
                    StartCoroutine(doCheckCalibrate);
                }
            }
        }
    }

    bool WithinRotation(float comp)
    {
        float difference = transform.eulerAngles.z - comp;
        if (difference < 0) difference *= -1;
        if (difference <= 2)
            return true;

        return false;
    }

    IEnumerator RecalibrateController()
    {
        float countdown = 2;
        while (countdown > 0)
        {
            if (_arduino.backPress == 1 && _arduino.forwardPress == 1)
            {
                countdown -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                IEnumerator currentRoutine = doCheckCalibrate;
                doCheckCalibrate = null;
                StopCoroutine(currentRoutine);
            }    
        }

        _arduino.WriteToControler("0");
        IEnumerator curRoutine = doCheckCalibrate;
        doCheckCalibrate = null;
        StopCoroutine(curRoutine);
    }
}
