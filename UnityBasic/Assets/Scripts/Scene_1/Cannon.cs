using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    GameObject turret;
    [SerializeField]
    GameObject shotPoint;
    [SerializeField]
    GameObject Bullet;
    [SerializeField]
    LineRenderer predLine;
    [SerializeField]
    GameObject predSphere;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // à⁄ìÆ
        float moveSpeed = 10.0f;
        Vector3 vec = Vector3.zero;
        if (Input.GetKey(KeyCode.W))    
        {// ëO
            vec += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {// å„
            vec -= transform.forward;
        }
        if(vec != Vector3.zero)
        {
            transform.position += vec * moveSpeed * Time.deltaTime;
        }

        // âÒì]
        float rotateSpeed = 100.0f;
        Vector3 rot = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {// ç∂
            rot += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {// âE
            rot += new Vector3(0, 1, 0);
        }
        if(rot != Vector3.zero)
        {
            transform.Rotate(rot * rotateSpeed * Time.deltaTime);
        }

        // ñCìÉÇÃâÒì]
        if (Input.GetKey(KeyCode.Q))
        {
            turret.transform.localRotation *= Quaternion.Euler(0.0f, -1.0f, 0.0f);
        }
        if(Input.GetKey(KeyCode.E))
        {
            turret.transform.localRotation *= Quaternion.Euler(0.0f, 1.0f, 0.0f);
        }

        // éÀåÇ
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj;
            obj = Instantiate(Bullet, shotPoint.transform.position, Quaternion.identity);

            obj.GetComponent<Bullet>().SetDirection(turret.transform.forward);
        }

        // ó\ë™ê¸
        RaycastHit hit;

        if(Physics.SphereCast(shotPoint.transform.position, Bullet.transform.localScale.x * 0.5f, turret.transform.forward, out hit, 30))
        {
            predLine.SetPosition(0, shotPoint.transform.position);
            predLine.SetPosition(1, shotPoint.transform.position + (turret.transform.forward * hit.distance));
            predLine.gameObject.SetActive(true);
            
            predSphere.transform.position = shotPoint.transform.position + (turret.transform.forward * hit.distance);
            predSphere.gameObject.SetActive(true);
        }
        else
        {
            predLine.gameObject.SetActive(false);
            predSphere.gameObject.SetActive(false);
        }
    }
}
