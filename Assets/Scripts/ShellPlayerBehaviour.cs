using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellPlayerBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log(other.gameObject.name + " was shot by player");
            EnemyBehaviour enemyBehaviour;
            if (other.gameObject.name == "Body") {
                enemyBehaviour = other.transform.parent.GetComponent<EnemyBehaviour>();
            }
            else {
                enemyBehaviour = other.transform.parent.parent.GetComponent<EnemyBehaviour>();
            }
            enemyBehaviour.damage += 7;
            Destroy(this.gameObject);
        }
        else if (other.gameObject.name == "Shell")
        {
            // 敵のShellを破壊し進む
        }
        if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
