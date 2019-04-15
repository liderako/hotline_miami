using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int _speed;
    [SerializeField] public GameObject _weaponObject;
    [SerializeField] private AudioSource _takeWeaponSound;
    [SerializeField] private AudioSource _aDeadSound;
    private Weapon _weapon;
    [SerializeField] private GameObject _leg;
    [SerializeField] private GameObject _head;

    public string room;
    private float timeoutFire;
    private bool inTriggerWeapon;
    private bool _isDead;
    private string nameObjectWeapon;
    public bool isWeapon;


    void Start()
    {
        timeoutFire = Time.time;
    }

    void Update()
    {
        if (!_isDead)
        {
            TakeWeapon();
            if (Input.GetMouseButtonDown(1))
                ThrowWeapon();
        }
    }

    public void FixedUpdate()
    {
        if (!_isDead)
        {
            Rotate();
            Move();
            Fire();
        }

        if (isWeapon)
        {
            _weaponObject.transform.localPosition = new Vector2(_weaponObject.GetComponent<Weapon>().positionInHand.x,_weaponObject.GetComponent<Weapon>().positionInHand.y);
        }
    }

    private void Rotate()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + 90); 
    }

    private void Fire()
    {
        if (isWeapon && (_weapon.ammo > 0 || _weapon.ammo == -1))
        {
            if (_weapon.category == "auto" && Input.GetMouseButton(0) && Time.time - timeoutFire > _weapon.timeout)
            {
                _weapon.firePlayer();
                timeoutFire = Time.time;
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);  
            }
            else if (_weapon.category == "notAuto" && Input.GetMouseButtonDown(0) && Time.time - timeoutFire > _weapon.timeout)
            {
                _weapon.firePlayer();
                timeoutFire = Time.time;
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }
    }

    private void TakeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inTriggerWeapon && nameObjectWeapon != null)
            {
                if (isWeapon && nameObjectWeapon == _weaponObject.name)
                {
                    putWeapon();
                    return;
                }
                putWeapon();
                _takeWeaponSound.Play(1);
                _weaponObject = Instantiate(GameObject.Find(nameObjectWeapon).gameObject, transform.position,
                    Quaternion.identity);
                string name = (GameObject.Find(nameObjectWeapon).gameObject.name);
                Destroy(GameObject.Find(nameObjectWeapon).gameObject);
                _weaponObject.name = name;
                if (_weaponObject == null)
                    return;
                _weaponObject.transform.parent = transform;
                _weaponObject.transform.localPosition = new Vector2(_weaponObject.GetComponent<Weapon>().positionInHand.x,_weaponObject.GetComponent<Weapon>().positionInHand.y);
                _weaponObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); 
                _weaponObject.GetComponent<Animator>().SetBool("take", true);
                _weapon = _weaponObject.GetComponent<Weapon>();
                _weapon.owner = gameObject.name;
                isWeapon = true;
            }   
        }
    }

    private void ThrowWeapon()
    {
        if (isWeapon)
        {
            _weaponObject.transform.parent = transform.parent;
            _weaponObject.GetComponent<Animator>().SetBool("take", false);
            _weapon._throw = true;
            _weapon._pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            _weapon.owner = "";
            _weaponObject = null;
            _weapon = null;
            isWeapon = false;
        }
    }
    
    private void putWeapon()
    {
        if (isWeapon)
        {
            _weaponObject.transform.parent = transform.parent;
            _weaponObject.GetComponent<Animator>().SetBool("take", false);
            _weapon.owner = "";
            _weaponObject = null;
            _weapon = null;
            isWeapon = false;
        }
    }

    public int getAmmo()
    {
        if (!isWeapon)
            return 0;
        return _weapon.ammo;
    }

    public void setAmmo(int ammo)
    {
        if (!isWeapon || _weapon.ammo == -1)
            return;
        _weapon.ammo += ammo;
    }
    
    public void dead()
    {
        _leg.gameObject.GetComponent<Animator>().SetBool("Move", false);
        _aDeadSound.Play();
        _isDead = true;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GameManager.gm.isEnd = true;
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Weapon")
        {
            inTriggerWeapon = false;
            nameObjectWeapon = null;
        }
    }
    
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Weapon")
        {
            inTriggerWeapon = true;
            nameObjectWeapon = other.transform.name;
        }
        if (other.gameObject.tag == "Room")
        {
            room = other.gameObject.name;
        }
    }

    private void Move()
    {
        bool _stay = true;
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition += -Vector3.right * Time.deltaTime * _speed;
            _leg.gameObject.GetComponent<Animator>().SetBool("Move", true);
            _stay = false;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.localPosition += -Vector3.up * Time.deltaTime * _speed;
            _leg.gameObject.GetComponent<Animator>().SetBool("Move", true);
            _stay = false;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            transform.localPosition += Vector3.right * Time.deltaTime * _speed;
            _leg.gameObject.GetComponent<Animator>().SetBool("Move", true);
            _stay = false;
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition += Vector3.up * Time.deltaTime * _speed;
            _leg.gameObject.GetComponent<Animator>().SetBool("Move", true);
            _stay = false;
        }

        if (_stay)
        {
            _leg.gameObject.GetComponent<Animator>().SetBool("Move", false);
        }
    }
}
