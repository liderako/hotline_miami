using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public AudioSource _aDeadSound;
    [SerializeField] private GameObject _leg;
    [SerializeField] private GameObject _head;

    private Weapon _weapon;
    [SerializeField] private GameObject _gameObjectWeapon;
    [SerializeField] public float timeoutFire;
    [SerializeField] private GameObject _player;
    [SerializeField] public bool isPlayer;

    public GameObject nextCheckpoint;
    public string room;
    [SerializeField]private bool isPatrol;
    private bool _isDead;
    public bool _moveAnotherRoom;
    public float _speed = 3;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gm.amountEnemy += 1;
        timeoutFire = Time.time;
        _gameObjectWeapon.transform.parent = transform;
        _gameObjectWeapon.transform.localPosition = new Vector2(
            _gameObjectWeapon.GetComponent<Weapon>().positionInHand.x,
            _gameObjectWeapon.GetComponent<Weapon>().positionInHand.y
        );
        _gameObjectWeapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); 
        _gameObjectWeapon.GetComponent<Animator>().SetBool("take", true);
        _weapon = _gameObjectWeapon.GetComponent<Weapon>();
        _weapon.owner = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDead)
            return;
        if (isPlayer && !GameManager.gm.isEnd && room != GameManager.gm.playerController.room && !_moveAnotherRoom)
        {
            nextCheckpoint = GameObject.Find(room).GetComponent<Room>().findPath(GameManager.gm.playerController.room);
            if (nextCheckpoint == null)
            {
                isPlayer = false;
            }
            else
            {
                _moveAnotherRoom = true;
            }
        }
        else if (isPlayer && !GameManager.gm.isEnd && room != GameManager.gm.playerController.room)
        {
            moveOnCheckPoint();
        }
        else if (isPlayer && !GameManager.gm.isEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
            Vector3 difference = _player.transform.position - transform.position; 
            difference.Normalize();
            float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + 90);
            _moveAnotherRoom = false;
            fire();
        }
        else if (!GameManager.gm.isEnd && isPatrol)
        {
            moveOnCheckPoint();
        }

        if ( _gameObjectWeapon != null)
        {
            _gameObjectWeapon.transform.localPosition = new Vector2(_gameObjectWeapon.GetComponent<Weapon>().positionInHand.x,_gameObjectWeapon.GetComponent<Weapon>().positionInHand.y);   
        }
    }

    private void moveOnCheckPoint()
    {
        float step = _speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, nextCheckpoint.transform.position, step);
            
        Vector3 difference = nextCheckpoint.transform.position - transform.position; 
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + 90); 
            
        if (transform.rotation.y != 0 || transform.rotation.x != 0)
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y, 0);
        }
        if (Vector3.Distance(transform.position, nextCheckpoint.transform.position) < 0.05f)
        {
            nextCheckpoint = nextCheckpoint.GetComponent<checkpoint>().nextCheckpoint;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (gameObject.GetComponent<PolygonCollider2D>().OverlapPoint(new Vector2(other.gameObject.transform.position.x, other.gameObject.transform.position.y)))
        {
            if (other.gameObject.tag == "Player")
            {
                isPlayer = true;
            }   
        }
    }
    
    private void fire()
    {
        if (_gameObjectWeapon != null)
        {
            if (_weapon.ammo > 0 || _weapon.ammo == -1)
            {
                if (_weapon.category == "auto" && Time.time - timeoutFire > _weapon.timeout)
                {
                    ;
                    _weapon.fireEnemy(_player.transform.position);
                    timeoutFire = Time.time;
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                }
                else if (_weapon.category == "notAuto" && Time.time - timeoutFire > _weapon.timeout)
                {
                    _weapon.fireEnemy(new Vector3(_player.transform.position.x, _player.transform.position.y, -10));
                    timeoutFire = Time.time;
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                }
            }
        }
    }

    public void dead()
    {
        _aDeadSound.Play();
        _isDead = true;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        StartCoroutine(deleteObject());
    }

    public IEnumerator deleteObject()
    {
        _head.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        GameManager.gm.countEnemy += 1;
        if (GameManager.gm.countEnemy == GameManager.gm.amountEnemy)
        {
            GameManager.gm.isEnd = true;
            GameManager.gm.isVictory = true;
        }
    }
}
