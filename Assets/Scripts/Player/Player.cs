using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement playerMovement;
    private SphereCollider sphereCollider;
    public AudioClip attacksound;
    private AudioSource audioSource;
    private Animator animator;
    public Transform sphereCast;

    public float soundInstensity = 5f;
    public float walkZombieRadius = 1f;
    public float sprintZombieRadius = 2f;
    public int damage = 30;

    public LayerMask zombieLayer;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        sphereCollider = GetComponent<SphereCollider>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        if (playerMovement.GetPlayerStealth() == 0)
        {
            sphereCollider.radius = walkZombieRadius;
        }
        else
        {
            sphereCollider.radius = sprintZombieRadius;
        }
    }

    public void Fire()
    {
        //audioSource.PlayOneShot(attacksound);
        animator.SetTrigger("Attack");
        Collider[] zombies = Physics.OverlapSphere(transform.position, soundInstensity, zombieLayer);
        for(int i = 0; i < zombies.Length; i++)
        {
            zombies[i].GetComponent<Enemy>().OnAware();
        }
        RaycastHit hit;
        if (Physics.SphereCast(sphereCast.position, 0.5f, sphereCast.TransformDirection(Vector3.forward), out hit, zombieLayer))
        {
            hit.transform.GetComponent<Enemy>().GotHit(damage);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            other.GetComponent<Enemy>().OnAware();
        }
    }
}
