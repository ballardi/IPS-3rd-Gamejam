using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObstacleSpawnerScript : MonoBehaviour {

    public GameObject ObstacleAPrefab;

    public GameObject ObstacleASpawnPoint;
    public float spawnCountdown = 1;

    private float spawnCountdownTimer;

    void Awake() {
        Assert.IsNotNull(ObstacleAPrefab);
        Assert.IsNotNull(ObstacleASpawnPoint);
        spawnCountdownTimer = spawnCountdown;
    }

    void Update() {

        spawnCountdownTimer += Time.deltaTime;
        if (spawnCountdownTimer > spawnCountdown) {
            SpawnObstacle();
            spawnCountdownTimer = 0;
        }
    }

    private void SpawnObstacle() {
        GameObject.Instantiate(ObstacleAPrefab, ObstacleASpawnPoint.transform.position, Quaternion.identity, transform);
    }
}
