using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum walkType { Random, Waypoint};
    public walkType WalkType = walkType.Random;
    private Vector3 walkPoint;

    private bool isAware = false;
    private bool detecing = false;

    private int wayPointIndex = 0;
    public float fov = 120f;
    public float viewDistance = 10f;
    public float walkRadius = 10f;
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float losePlayer = 10f;//timer is seconds losing the player
    private float loseTimer = 0f;
    public int health = 100;

    public GameObject player;
    private NavMeshAgent agent;
    private Renderer render;
    public Transform[] wayPoints;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        render = GetComponent<Renderer>();
        animator = GetComponentInChildren<Animator>();
        walkPoint = RandomWalkPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            agent.speed = 0;
            animator.enabled = false;
            return;
        }
        if (isAware)
        {
            agent.SetDestination(player.transform.position);
            animator.SetBool("Aware", true);
            agent.speed = sprintSpeed;
            if (!detecing)
            {
                loseTimer += Time.deltaTime;
                if(loseTimer >= losePlayer)
                {
                    isAware = false;
                    loseTimer = 0;
                }
            }
            //render.material.color = Color.red;
        }
        else
        {
            Walk();
            animator.SetBool("Aware", false);
            agent.speed = walkSpeed;
            //render.material.color = Color.blue;
        }
        SearchForPlayer();
    }

    public void SearchForPlayer()
    {
        if(Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.transform.position)) < fov / 2f)
        {
            if(Vector3.Distance(player.transform.position, transform.position) < viewDistance)
            {
                RaycastHit hit;
                if(Physics.Linecast(transform.position, player.transform.position, out hit, -1))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        OnAware();
                    }
                    else
                    {
                        detecing = false;
                    }
                }
                else
                {
                    detecing = false;
                }
            }
            else
            {
                detecing = false;
            }
        }
        else
        {
            detecing = false;
        }
    }

    public void OnAware()
    {
        isAware = true;
        detecing = true;
        loseTimer = 0;
    }

    public void Walk()
    {
        if(WalkType == walkType.Random)
        {
            if (Vector3.Distance(transform.position, walkPoint) < 5f)
            {
                walkPoint = RandomWalkPoint();
            }
            else
            {
                agent.SetDestination(walkPoint);
            }
        }
        else
        {
            if (wayPoints.Length >= 2)
            {
                if (Vector3.Distance(wayPoints[wayPointIndex].position, transform.position) < 2f)
                {
                    if (wayPointIndex == wayPoints.Length - 1)
                    {
                        wayPointIndex = 0;
                    }
                    else
                    {
                        wayPointIndex++;
                    }
                }
                else
                {
                    agent.SetDestination(wayPoints[wayPointIndex].position);
                }
            }
            else
            {
                Debug.Log("Assign more waypoints to the AI/Enemy/Zomie" + gameObject.name);
            }
        }

    }

    public void GotHit(int damage)
    {
        health -= damage;
    }

    public Vector3 RandomWalkPoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * walkRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, walkRadius, -1);
        return new Vector3(navHit.position.x, transform.position.y, transform.position.z);
    }
}
