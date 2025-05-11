using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBehaviour : MonoBehaviour
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
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player was shot");
            PlayerParams playerParams = other.transform.parent.GetComponent<PlayerParams>();
            playerParams.cntHit += 1;
        }
        if (!other.gameObject.CompareTag("Enemy"))
        {
            // 敵に当たったなら貫通
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject);
    }
}
