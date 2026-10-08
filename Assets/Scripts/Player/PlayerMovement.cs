using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool usingTemps = true;
    private float movement;
    public float movementInput
    {
        get { return movement; }
        set
        { 
            movement = value;
            MovePlayer();
        }
    }

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

    void MovePlayer()
    {
        transform.Translate(Vector3.up * movement * Time.deltaTime);
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
    }
}
