using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;
    private ParticleSystem juiceEffect;

    public int points = 1;
    float rotateSpeed;

    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juiceEffect = GetComponentInChildren<ParticleSystem>();

        rotateSpeed = Random.Range(0.0f, 4.0f);
    }

    void Update()
    {
        RotateFruit();
    }

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        VeganNinjaManager.Instance.IncreaseScore(points);

        VeganNinjaManager.Instance.isSliceFruit = true;
        rotateSpeed = 0.0f;

        // Disable the whole fruit
        fruitCollider.enabled = false;
        whole.SetActive(false);

        // Enable the sliced fruit
        sliced.SetActive(true);
        juiceEffect.Play();

        // Rotate based on the slice angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();

        // Add a force to each slice based on the blade direction
        foreach (Rigidbody slice in slices)
        {
            slice.velocity = fruitRigidbody.velocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }

    void RotateFruit()
    {
        gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, rotateSpeed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.Direction, blade.transform.position, blade.sliceForce);
        }
    }

}
