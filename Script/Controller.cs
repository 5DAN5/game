using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject flesh;
    public GameObject pl; // игрок
    public Rigidbody2D rb;
    [SerializeField] Vector3[] point; // точки движения зомби
    int startPoint; // номер текущей точки, в которую идет зомби 
    public float health; // здоровье
    public float speed; // скорость зомби
    public float damage; //сила урона
    public float distance; //дистанция до игрока
    public float dl; // задержка перед атакой
    void FixedUpdate()
    {
        Move(); // движение и поворот
        if(pl != null)
        {
            Attack();
        }
        if(dl > 0)
        {
            dl -= Time.deltaTime;
        }
    }
    void Move()
    {
        Vector3 poin = point[startPoint]; //точка, в которую идет зомби 
        if (pl == null)
        {  // движение по точке
            poin = point[startPoint];
            if (transform.position == point[startPoint])
            {
                startPoint++;
                if (startPoint >= point.Length) startPoint = 0;
            }
        }
        else
        {
            poin = pl.transform.position; // движения за игроком
            
        }
        rb.MovePosition( Vector3.MoveTowards(transform.position, poin, speed * Time.deltaTime)); // движение
        Vector3 difference = poin - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ); // поворот
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            pl = other.gameObject; // назначения игрока
            return;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            pl = null; // игрок пропадает из виду
            return;
        }
    }

    public void TakingDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Instantiate(flesh, transform.position,Quaternion.identity); // создание плоти
            Destroy(gameObject); // уничтожения обьекта

        }
    }
    void Attack()
    {
        distance = Mathf.Sqrt(Mathf.Pow((pl.transform.position.x - transform.position.x), 2) + Mathf.Pow((pl.transform.position.y - transform.position.y), 2)); // дистанция от обьекта до игрока
        if (distance <= 1 && dl <= 0)
        {
            dl = 3;
           // pl.GetComponent<PlayerControll>().TakingDamage(damage); // урон игроку
        }
    }
}
