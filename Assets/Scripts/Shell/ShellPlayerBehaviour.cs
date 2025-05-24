using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellPlayerBehaviour : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

            // ダメージ音を鳴らす
            audioSource.PlayOneShot(hitSound, 0.25f);

            // シェルを破壊したように見せる
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;

            StartCoroutine(DestroyAfterSound());
            return;
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

    IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(hitSound.length);
        Destroy(gameObject);
    }
}
