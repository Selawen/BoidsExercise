using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private int boidAmount;
    private List<GameObject> boids;
    private Vector3 avgBoidPos;
    private Vector3 avgBoidDirection;

    [SerializeField] private int cohesion;
    [SerializeField] private int seperation;
    [SerializeField] private int alignment;

    // Start is called before the first frame update
    void Start()
    {
        boids = new List<GameObject>();
        avgBoidPos = new Vector3(0, 0, 0);
        avgBoidDirection = new Vector3(0, 0, 0);

        for (int i = 0;i < boidAmount; i++)
        {
            GameObject newBoid;
            newBoid = Instantiate(boidPrefab, new Vector3(i * Random.Range(0,10), i * Random.Range(0, 10), i * Random.Range(0, 10)), Quaternion.identity);
            boids.Add(newBoid);
            avgBoidPos += newBoid.transform.position;
        }

    }

    // Update is called once per frame
    void Update()
    {
        MoveBoids();
    }

    void MoveBoids()
    {
        Vector3 _v1;
        Vector3 _v2;
        Vector3 _v3;
        Vector3 _velocity;
        Vector3 _newAvgPos = new Vector3(0,0,0);
        Vector3 _newAvgDirection = new Vector3(0, 0, 0);

        foreach(GameObject boid in boids)
        {
            Vector3 _boidPos = boid.transform.position;
            _v1 = CohesionRule(_boidPos);
            _v2 = SeparationRule(boid);
            _v3 = AlignmentRule(boid);

            _velocity = _v1 + _v2;
            _velocity = Vector3.ClampMagnitude(_velocity, 0.5f);

            boid.transform.Rotate(_velocity);
            boid.transform.Translate(new Vector3(0,0.1f,0));

            _newAvgPos += boid.transform.position;
            _newAvgDirection += (_velocity/boidAmount);
        }

        avgBoidPos = _newAvgPos/boidAmount;
        avgBoidDirection = _newAvgDirection;
    }

    Vector3 CohesionRule(Vector3 _boidPos)
    {
        return (avgBoidPos - _boidPos) * cohesion;
    }

    Vector3 SeparationRule(GameObject _thisBoid)
    {
        Vector3 avoid = new Vector3(0,0,0);

        foreach (GameObject boid in boids)
        {
            if (boid != _thisBoid)
            {
                if ((_thisBoid.transform.position - boid.transform.position).magnitude < seperation)
                {
                    avoid = avoid + (_thisBoid.transform.position - boid.transform.position)/(_thisBoid.transform.position - boid.transform.position).magnitude;
                }
            }
        }
        return  avoid;
    }

    Vector3 AlignmentRule(GameObject _thisBoid)
    {
        _thisBoid.transform.Rotate(avgBoidDirection * (alignment/100));
        return avgBoidDirection;
    }
}
