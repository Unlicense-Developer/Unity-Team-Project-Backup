using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnManager: MonoBehaviour
{
    [SerializeField] private GameObject[] fruitPrefabs;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private AudioClip spawnSoundClip;

    AudioSource spawnSound;

    Collider spawnArea;

    [Range(0f, 1f)] public float bombChance = 0.05f;

    public float minSpawnDelay = 0.25f;
    public float maxSpawnDelay = 1f;

    public float minAngle = -15f;
    public float maxAngle = 15f;

    public float minForce = 18f;
    public float maxForce = 22f;

    public float maxLifetime = 5f;

    void Awake()
    {
        spawnArea = GetComponent<Collider>();
        spawnSound = GetComponent<AudioSource>();
    }

    public void StartSpawn()
    {
        VeganNinjaManager.Instance.SetPlaying(true);
        spawnSound.Play();
        WorldSoundManager.Instance.StopBGM();
        StartCoroutine(Spawn());
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator Spawn()
    {
        yield return new WaitForSeconds(3.0f);

        spawnSound.clip = spawnSoundClip;

        while (enabled)
        {
            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            spawnSound.Play();

            if (Random.value < bombChance) {
                prefab = bombPrefab;
            }

            Vector3 position = new Vector3
            {
                x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
            };

            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
            GameObject fruit = Instantiate(prefab, position, rotation);
            Destroy(fruit, maxLifetime);

            float force = Random.Range(minForce, maxForce);
            fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);

            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }

}
