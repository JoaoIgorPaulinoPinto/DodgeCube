using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovimentation : MonoBehaviour
{
    public static PlayerMovimentation instance;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
    }

    Rigidbody rb;

    public float smooth = 5f;
    public float speed = 5f;

    public Transform[] points;
    public int currentPoint;
    public bool inMoviment;

    private void Update()
    {
        Moviment();
    }

    void Moviment()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentPoint == 0)
            {
                currentPoint = points.Length - 1;
            }
            else
            {
                currentPoint--;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentPoint != points.Length - 1)
            {
                currentPoint++;
            }
            else
            {
                currentPoint = 0;
            }
        }

        UpdatePlayerPosition();
    }

    void UpdatePlayerPosition()
    {

        Vector3 targetPosition = points[currentPoint].position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
    }
}
