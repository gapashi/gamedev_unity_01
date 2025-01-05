using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{   
    // se o player encostar no coração, será coletado e conta +1 vida
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerController>().life++;
            Destroy(this.gameObject);
        }
    }
}
