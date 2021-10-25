using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidClass : MonoBehaviour
{
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public GameObject thisBoid;

    List<BoidClass> boidNeighbours = new List<BoidClass>();

    private void Awake()
    {
        thisBoid = this.gameObject;
        position = transform.position;
        velocity = new Vector2(transform.right.x, transform.right.y);
    }

    private void Update()
    {
        //get Neighbours
        Collider2D[] boidCols = Physics2D.OverlapCircleAll(this.transform.position, BoidParent._awarenessRadius);       
        boidNeighbours.Clear();
        for (int i = 0; i < boidCols.Length; i++)
        {
            if (boidCols[i] != this.GetComponent<Collider2D>())
                boidNeighbours.Add(boidCols[i].gameObject.GetComponent<BoidClass>());
        }

        //check rules
        AvoidEdges();

        //Vector2 avoidEdge = Avoid();
        Vector2 alignment = Align();
        Vector2 cohesion = Cohesian();
        Vector2 seperate = Seperation();

        acceleration = new Vector2(0, 0);
        acceleration += alignment * BoidParent._alignment * BoidParent._maxMoveSpeed;
        acceleration += cohesion * BoidParent._cohesian * BoidParent._maxMoveSpeed;
        acceleration += seperate * BoidParent._seperation * BoidParent._maxMoveSpeed;
        //acceleration += avoidEdge * 1 * BoidParent._maxMoveSpeed;

        //apply to vars
        position += velocity * Time.deltaTime;
        velocity += acceleration * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, BoidParent._maxMoveSpeed);

        //apply to transform
        GetComponent<Transform>().position = position;
        transform.right = velocity;
    }

    Vector2 Avoid()
    {
        Vector2 avg = Vector2.zero;

        if (boidNeighbours.Count > 0)
        {
            foreach (BoidClass boid in boidNeighbours)
            {
                if (boid != null)
                {
                    Vector2 closestPoint = boid.GetComponent<Collider2D>().ClosestPoint(position);

                    //calculate distance
                    float d = Vector2.Distance(closestPoint, position);
                    if (d < BoidParent._awarenessRadius * BoidParent._avoidanceRadius)
                    {
                        Vector2 difference = position - closestPoint;
                        difference /= d;
                        avg += difference;
                    }
                }
            }
            avg /= boidNeighbours.Count;
            avg = avg.normalized * BoidParent._maxMoveSpeed;
            avg -= velocity;
            avg = Vector2.ClampMagnitude(avg, BoidParent._maxTurnEffect);
        }

        return avg;
    }

    void AvoidEdges() 
    {
        if (position.x < -BoidParent._boundSize.x)
            position.x = BoidParent._boundSize.x;
        else if (position.x > BoidParent._boundSize.x)
            position.x = -BoidParent._boundSize.x;

        if (position.y > BoidParent._boundSize.y)
            position.y = -BoidParent._boundSize.y;
        else if (position.y < -BoidParent._boundSize.y)
            position.y = BoidParent._boundSize.y;
    }

    Vector2 Align()
    {
        Vector2 avg = Vector2.zero;

        if (boidNeighbours.Count > 0)
        {
            foreach (BoidClass boid in boidNeighbours)
            {
                if (boid != null)
                    avg += boid.velocity;
            }
            avg /= boidNeighbours.Count;
            avg = avg.normalized * BoidParent._maxMoveSpeed;
            avg -= velocity;
            avg = Vector2.ClampMagnitude(avg, BoidParent._maxTurnEffect);
        }

        return avg;
    }

    Vector2 Cohesian()
    {
        Vector2 avg = Vector2.zero;

        if (boidNeighbours.Count > 0)
        {
            foreach (BoidClass boid in boidNeighbours)
            {
                if (boid != null)
                    avg += boid.position;
            }
            avg /= boidNeighbours.Count;
            avg -= position;
            avg = avg.normalized * BoidParent._maxMoveSpeed;
            avg -= velocity;
            avg = Vector2.ClampMagnitude(avg, BoidParent._maxTurnEffect);
        }

        return avg;
    }

    Vector2 Seperation()
    {
        Vector2 avg = Vector2.zero;

        if (boidNeighbours.Count > 0)
        {
            foreach (BoidClass boid in boidNeighbours)
            {
                if (boid != null)
                {
                    //calculate distance
                    float d = Vector2.Distance(boid.position, position);
                    if (d < BoidParent._awarenessRadius * BoidParent._avoidanceRadius)
                    {
                        Vector2 difference = position - boid.position;
                        difference /= d;
                        avg += difference;
                    }
                }
            }
            avg /= boidNeighbours.Count;
            avg = avg.normalized * BoidParent._maxMoveSpeed;
            avg -= velocity;
            avg = Vector2.ClampMagnitude(avg, BoidParent._maxTurnEffect);
        }

        return avg;
    }
}
