using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Player player;

    private Vector3 direction;
    private float speed;
    private float damage;
    private float lifeTime = 3f; // 3초 뒤 자동 파괴

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Vector3 dir, float spd, float dmg, Sprite sprite)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;

        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.stats.currentHealth -= (int)damage;
            Debug.Log($"Player hit! Damage: {damage}");
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
