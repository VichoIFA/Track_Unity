using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanclaThrower : MonoBehaviour
{
    [SerializeField] private GameObject chanclaPrefab;
    [SerializeField] private Transform[] throwPoints;

    private void Update()
    {
        float randNum = Random.Range(0, 1f);

        if (randNum <= 0.01f)
        {
            int randThrowPoint = Random.Range(0, throwPoints.Length);

            GameObject chancla = Instantiate(chanclaPrefab, throwPoints[randThrowPoint].position, 
                throwPoints[randThrowPoint].rotation * Quaternion.Euler(Random.Range(2f, 15f), Random.Range(2f, 15f), Random.Range(2f, 15f)));

            chancla.transform.localScale = new Vector3(1f, 1f, 1f);

            Rigidbody rb = chancla.GetComponent<Rigidbody>();


            rb.AddForce(45000f * (throwPoints[randThrowPoint].forward) * Time.deltaTime);
            rb.AddTorque(new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)) * Time.deltaTime * 20000f);
        }
    }
}
