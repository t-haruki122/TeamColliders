using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBehaviour : MonoBehaviour
{
    public string shellShooter = "敵";

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
            // Debug.Log("Player was shot");
            GameManager.GMInstance.addHit();
            MessageStream.MSInstance.addMessage(new HitMessage(shellShooter));
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        Destroy(this.gameObject);
    }
}
