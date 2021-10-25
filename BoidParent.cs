using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidParent : MonoBehaviour
{
    public Vector2 boundSize;

    [Range(0,1)]
    public float alignment = 1f;
    [Range(0, 1)]
    public float seperation = 1f;
    [Range(0, 1)]
    public float cohesian = 1f;

    public float awarenessRadius = 5f;
    [Range(0, 1)]
    public float avoidanceRadius = 0.5f;
    public float maxMoveSpeed = 2f;
    public float maxTurnEffect = 1f;

    public static Vector2 _boundSize;
    public static float _alignment;
    public static float _seperation;
    public static float _cohesian;
    public static float _maxTurnEffect;
    public static float _awarenessRadius;
    public static float _avoidanceRadius;
    public static float _maxMoveSpeed;
    public int numBoids = 100;
    public GameObject boidPrefab;

    private List<BoidClass> boidAgents = new List<BoidClass>();


    private void Start()
    {
        _boundSize = boundSize;

        _alignment = alignment;
        _seperation = seperation;
        _cohesian = cohesian;

        _avoidanceRadius = avoidanceRadius;
        _awarenessRadius = awarenessRadius;
        _maxMoveSpeed = maxMoveSpeed;
        _maxTurnEffect = maxTurnEffect;

        SpawnBois();
    }

    private void Update()
    {
        _boundSize = boundSize;

        _alignment = alignment;
        _seperation = seperation;
        _cohesian = cohesian;

        _avoidanceRadius = avoidanceRadius;
        _awarenessRadius = awarenessRadius;
        _maxMoveSpeed = maxMoveSpeed;
        _maxTurnEffect = maxTurnEffect;
    }

    void SpawnBois() 
    {
        //spawn all the boids
        for (int i = 0; i < numBoids; i++)
        {
            Vector2 boidPos = new Vector2(
                Random.Range(-_boundSize.x, _boundSize.x), 
                Random.Range(-_boundSize.y, _boundSize.y));
            Instantiate(boidPrefab, boidPos, Quaternion.Euler(transform.forward * Random.Range(0, 360)), this.transform);
        }
    }
}
