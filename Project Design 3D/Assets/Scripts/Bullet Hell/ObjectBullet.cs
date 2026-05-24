using UnityEngine;

public class ObjectBullet : MonoBehaviour
{
    public float bulletLife = 1f;
    public float rotation = 0f;
    public float speed = 1f;

    private Vector2 spawnPoint;
    private float timer = 0f;

    void OnEnable()
    {
        timer = 0f;
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
    }

    void Update()
    {
        if (timer > bulletLife)
        {
            BulletPool.Instance.ReturnBullet(gameObject);
            return;
        }

        timer += Time.deltaTime;
        transform.position = Movement(timer);
    }

    private Vector2 Movement(float timer)
    {
        float x = timer * speed * (-transform.up.x);
        float y = timer * speed * (-transform.up.y);
        return new Vector2(x + spawnPoint.x, y + spawnPoint.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 || other.CompareTag("Player"))
        {
            if (PlayerHealthSystem.Instance != null)
            {
                PlayerHealthSystem.Instance.TakeDamage();
            }

            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }
}