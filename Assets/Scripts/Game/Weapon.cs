using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Vector2 positionInHand;
    private Vector3 shootDirection;

    [SerializeField]private GameObject _bullet;
    [SerializeField]public int ammo;
    [SerializeField] public float timeout;
    [SerializeField] private BoxCollider2D _box;
    public string category;
    public float speed = 2;
    public string owner;
    public bool _throw;
    public bool _isBullet;
    public Vector3 _pos;
    
    // Update is called once per frame
    void Update()
    {
        if (_throw)
        {
            StartCoroutine(rot());
        }
    }

    public IEnumerator rot()
    {
        gameObject.tag = "Untagged";
        int _lock = 5;
        if (ammo == -1)
        {
            _isBullet = true;
            _lock = 8;
        }
        if (!GameManager.gm.isEnd)
        {
            float step = 3 * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _pos, step);
            for (int i = 0; i < _lock; i++)
            {
                if (_throw)
                {
                    _box.enabled = true;
                    transform.Rotate(0, 0, 10);
                    yield return new WaitForSeconds(0.05f);
                }
            }

            _box.enabled = false;
            _throw = false;
        }
        gameObject.tag = "Weapon";
    }

    public void fireEnemy(Vector3 direction)
    {
        gameObject.GetComponent<AudioSource>().Play(1);
        if (ammo != -1)
        {
            ammo--;
        }

        if (ammo > 0 || ammo == -1)
        {
            shootDirection = new Vector3();
            shootDirection = direction;
            shootDirection.z = 0.0f;
            shootDirection = shootDirection-transform.position;
            GameObject go = Instantiate(_bullet, transform.position, Quaternion.Euler(new Vector3(0,0,0)));
            go.SetActive(true);
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(shootDirection.x * speed, shootDirection.y * speed);
            go.GetComponent<Bullet>().owner = owner;
            go.GetComponent<Bullet>().tagOwner = "Enemy";
            
            Vector3 difference = direction - go.transform.position;
            difference.Normalize();
            float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            go.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
        }  
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_isBullet && _throw)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyController>().dead();
            }
        }
        if (_throw && other.gameObject.tag != "Player")
        {
            _throw = false;
            _box.enabled = false;
        }
    }

    public void firePlayer()
    {
        gameObject.GetComponent<AudioSource>().Play(1);
        if (ammo != -1)
        {
            ammo--;
        }

        if (ammo > 0 || ammo == -1)
        {
            shootDirection = new Vector3();
            shootDirection = Input.mousePosition;
            shootDirection.z = 0.0f;
            shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
            shootDirection = shootDirection-transform.position;
            GameObject go = Instantiate(_bullet, transform.position, Quaternion.Euler(new Vector3(0,0,0)));
            go.SetActive(true);
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(shootDirection.x * speed, shootDirection.y * speed);
            go.GetComponent<Bullet>().owner = owner;
            
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - go.transform.position;
            difference.Normalize();
            float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            go.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
        }
    }
}