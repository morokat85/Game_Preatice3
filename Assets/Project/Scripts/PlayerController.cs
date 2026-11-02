using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// STARTER — Player controller for the "Orb Collector" lab goal.
/// Attach to a cube named "Player" (add a Rigidbody, freeze rotation X/Z).
///
/// ALREADY WORKS: WASD movement with GetKey + GetAxis, frame-rate independent
/// via Time.deltaTime. A Debug.Log heartbeat proves Update runs every frame.
///
/// YOUR LAB TASKS live in the TODO comments. Each TODO uses one function
/// from this week's lecture. Complete them all to finish the game.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Lab wiring (drag your objects here)")]
    [SerializeField] private GameObject orbPrefab;   // TODO 5: used to spawn a bonus orb
    [SerializeField] private Transform cameraTransform; // used by TODO 3 (camera follow)

    [Header("Camera")]
    [SerializeField] private float cameraSmooth = 5f;

    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 cameraOffset;
    private bool quitting;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Debug.Log("[PlayerController] Ready. Collect all orbs to win.");
        
        if (cameraTransform != null)
            cameraOffset = cameraTransform.position - transform.position;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // A/D + arrows
        float v = Input.GetAxis("Vertical");   // W/S + arrows

        movement = new Vector3(h, 0f, v).normalized;
    }

    void FixedUpdate(){
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }

    // ---------------------------------------------------------------
    // TODO 1 (OnCollisionEnter): log the name of any wall you bump.
    // TODO 2 (OnTriggerEnter): if other.CompareTag("Orb"), Destroy the orb
    //       and Debug.Log the collection. (Orb prefab already tagged "Orb".)
    // TODO 3 (LateUpdate): move cameraTransform to
    //       player position + offset so it follows you smoothly.
    // TODO 4 (OnMouseDown is on the Orb script, not here — see Orb.cs.)
    // TODO 5 (OnDestroy): when the Player is destroyed, Instantiate one
    //       bonus orbPrefab at your last position.
    // ---------------------------------------------------------------

    //1
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("[PlayerController] Bumped into: " + collision.gameObject.name);
    }

    //2
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            Destroy(other.gameObject);
            Debug.Log("[PlayerController] Collected orb: " + other.name);
        }
    }

    //3
     void LateUpdate()
    {
        if (cameraTransform == null) return;

        Vector3 target = transform.position + cameraOffset;
        cameraTransform.position = Vector3.Lerp(
            cameraTransform.position, target, cameraSmooth * Time.deltaTime);
    }

    //5
     void OnApplicationQuit()
    {
        quitting = true;
    }

    void OnDestroy()
    {
        if (quitting || orbPrefab == null) return;
        if (!gameObject.scene.isLoaded) return;

        Instantiate(orbPrefab, transform.position, Quaternion.identity);
    }
}
