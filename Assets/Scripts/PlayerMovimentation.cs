using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovimentation : NetworkBehaviour
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

    public List<Transform> points;
    public List<Transform> aditionalPoints;
    public int currentPoint;
    public bool inMoviment;

    public bool freeMovimentation;
    public int levelToAddPoints = 5;

    private void Update()
    {
        if (!IsSpawned || !IsOwner) return;
        else
        {
            Moviment();

            if (PlayerSttsManager.instance.level > levelToAddPoints)
            {
                foreach (var item in aditionalPoints)
                {
                    if (!points.Contains(item))
                    {
                        points.Add(item);
                    }
                }
            }
        }

    }

    void Moviment()
    {
        if (freeMovimentation)
        {
         
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

          
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            rb.linearVelocity = direction * speed;
                
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (currentPoint == 0)
                {
                    currentPoint = points.Count - 1;
                }
                else
                {
                    currentPoint--;
                }
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                if (currentPoint != points.Count - 1)
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
    }

    void UpdatePlayerPosition()
    {
        Vector3 targetPosition = points[currentPoint].position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
    }
}
