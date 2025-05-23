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
        if (other.gameObject.CompareTag("EnemyCollider"))
        {
            baseEnemy enemyScript;
            if (other.transform.parent.parent.GetComponent<baseEnemy>() == null)
            {
                Debug.Log("敵のスクリプトが見つかりません");
                return;
            }
            enemyScript = other.transform.parent.parent.GetComponent<baseEnemy>();

            // ダメージをゲームマネージャーから取得する
            int damage = GameManager.GMInstance.getDamage();

            // 敵にダメージを与える
            enemyScript.addDamage(damage);

            // シェルを破壊する
            Destroy(this.gameObject);
        }
        else if (other.gameObject.name == "Shell")
        {
            return; // なにもしない
        }
        if (!other.gameObject.CompareTag("Player"))
        {
            // TODO shell破壊 アニメーション 音
            Destroy(this.gameObject);
        }
    }
}
