using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeeperRange : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetComponentInParent<Animator>().Play("attack", -1);
        }
    }
}
