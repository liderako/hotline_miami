using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float timeLife;
    private bool life;
    public string owner;
    public string tagOwner;
    
    void Start()
    {
        StartCoroutine(fly());
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (owner != other.gameObject.name && tagOwner != other.gameObject.tag)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyController>().dead();
            }
            else if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerController>().dead();
            }
            Destroy(gameObject);
        }
        if (life)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator fly()
    {
        yield return new WaitForSeconds(0.01f);
        life = true;
        yield return new WaitForSeconds(timeLife);
        Destroy(gameObject);
    }
}