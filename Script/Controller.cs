using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject flesh;
    public GameObject pl; // �����
    public Rigidbody2D rb;
    [SerializeField] Vector3[] point; // ����� �������� �����
    int startPoint; // ����� ������� �����, � ������� ���� ����� 
    public float health; // ��������
    public float speed; // �������� �����
    public float damage; //���� �����
    public float distance; //��������� �� ������
    public float dl; // �������� ����� ������
    void FixedUpdate()
    {
        Move(); // �������� � �������
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
        Vector3 poin = point[startPoint]; //�����, � ������� ���� ����� 
        if (pl == null)
        {  // �������� �� �����
            poin = point[startPoint];
            if (transform.position == point[startPoint])
            {
                startPoint++;
                if (startPoint >= point.Length) startPoint = 0;
            }
        }
        else
        {
            poin = pl.transform.position; // �������� �� �������
            
        }
        rb.MovePosition( Vector3.MoveTowards(transform.position, poin, speed * Time.deltaTime)); // ��������
        Vector3 difference = poin - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ); // �������
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            pl = other.gameObject; // ���������� ������
            return;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            pl = null; // ����� ��������� �� ����
            return;
        }
    }

    public void TakingDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Instantiate(flesh, transform.position,Quaternion.identity); // �������� �����
            Destroy(gameObject); // ����������� �������

        }
    }
    void Attack()
    {
        distance = Mathf.Sqrt(Mathf.Pow((pl.transform.position.x - transform.position.x), 2) + Mathf.Pow((pl.transform.position.y - transform.position.y), 2)); // ��������� �� ������� �� ������
        if (distance <= 1 && dl <= 0)
        {
            dl = 3;
           // pl.GetComponent<PlayerControll>().TakingDamage(damage); // ���� ������
        }
    }
}
