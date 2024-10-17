using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanclaThrower : MonoBehaviour
{
    [SerializeField] private GameObject chanclaPrefab;
    [SerializeField] private Transform[] throwPoints;

    [SerializeField] private Transform[] shootingPoints;
    private bool[] isShootingPointUsed;

    [SerializeField] private GameObject shootingTargetPrefab;
    [SerializeField] private Transform shootingTargetSpawnPoint;

    private float shootingTargetSpawnDelay = .5f;

    private bool finishedCoroutineFindings = true;

    private void Start()
    {
        isShootingPointUsed = new bool[shootingPoints.Length];

        shootingTargetSpawnDelay = 1f;

        finishedCoroutineFindings = true;
    }

    private void Update()
    {
        ThrowChanclas();
        SpawnShootingTargets();

        
    }

    private void ThrowChanclas()
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

    private void SpawnShootingTargets()
    {
        if(shootingTargetSpawnDelay > 0)
        {
            shootingTargetSpawnDelay -= Time.deltaTime;
            return;
        }

        shootingTargetSpawnDelay = 1f;

        bool allSpawnersUsed = true;

        for(int i = 0; i < isShootingPointUsed.Length; i++)
        {
            if (!isShootingPointUsed[i])
            {
                allSpawnersUsed = false;
            }
        }

        if (allSpawnersUsed)
        {
            return;
        }

        if (finishedCoroutineFindings)
        {
            StartCoroutine(SpawnShootingPoint());
        }
    }

    private IEnumerator SpawnShootingPoint()
    {
        finishedCoroutineFindings = false;

        bool choseSpawnPoint = false;

        int randIndex = -1;

        while(!choseSpawnPoint)
        {
            randIndex = Random.Range(0, shootingPoints.Length);

            if (!isShootingPointUsed[randIndex])
            {
                isShootingPointUsed[randIndex] = true;
                choseSpawnPoint = true;
            }

            else
            {
                yield return new WaitForEndOfFrame();
            }
        }

        finishedCoroutineFindings = true;

        if(randIndex != -1)
        {
            GameObject newShootingTarget = Instantiate(shootingTargetPrefab, shootingTargetSpawnPoint.transform.position, shootingTargetPrefab.transform.rotation);
            Transform newShootingTargetTransform = newShootingTarget.transform;

            ShootingTargetBehaviour bhvr = newShootingTarget.GetComponent<ShootingTargetBehaviour>();

            bhvr.shootingTargetSpawnIndex = randIndex;
            bhvr.onShootingTargetShot += OnDestroyedShootingTarget;

            Vector3 startPos = shootingTargetSpawnPoint.transform.position;
            Vector3 finalPos = shootingPoints[randIndex].position;

            float lerp = 0;

            while(lerp < 1)
            {
                if(newShootingTarget == null)
                {
                    break;
                }

                newShootingTargetTransform.position = Vector3.Lerp(startPos, finalPos, lerp);

                lerp += Time.deltaTime * 3f;

                yield return new WaitForEndOfFrame();
            }

            if(newShootingTarget != null)
            {
                bhvr.finishedMovingToPosition = true;
                bhvr.initialPos = finalPos;
            }
        }

    }

    private void OnDestroyedShootingTarget(object target, int index)
    {
        isShootingPointUsed[index] = false;
    }
}
