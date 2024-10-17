using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTargetBehaviour : MonoBehaviour
{
    public event EventHandler<int> onShootingTargetShot;
    public int shootingTargetSpawnIndex;

    public bool finishedMovingToPosition = false;

    public Vector3 initialPos;

    private float sinOffset;

    [SerializeField] private AudioClip explosionSound;

    private void Start()
    {
        sinOffset = UnityEngine.Random.Range(1f, 40f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Bullet"))
        {
            onShootingTargetShot?.Invoke(this, shootingTargetSpawnIndex);

            GameObject explosionGO = new GameObject("Explosion");
            explosionGO.transform.position = this.transform.position;

            AudioSource explosionAudioSource = explosionGO.AddComponent<AudioSource>();
            explosionAudioSource.clip = explosionSound;
            explosionAudioSource.spatialBlend = 1;

            explosionAudioSource.Play();

            Destroy(explosionGO, explosionSound.length + .1f);

            Destroy(gameObject, .05f);
        }
    }

    private void Update()
    {
        if (!finishedMovingToPosition) return;

        transform.position = initialPos + new Vector3(0, .1f * Mathf.Sin(Time.time + sinOffset), 0);
    }
}
