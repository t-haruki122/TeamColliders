using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellPlayerBehaviour : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip hitSound;

    private GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        explosionPrefab = ExplosionPrefab.EPInstance.getShellExplosionPrefab();
    }

    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Shell") return;
        if (other.gameObject.CompareTag("Player")) return;
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
        }

        // ダメージ音を鳴らす
        audioSource.PlayOneShot(hitSound, 0.25f);

        // エフェクトを表示する
        GameObject effect = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 3f); // 3秒後に自動削除（エフェクトが終わるタイミング）

        // シェルを破壊したように見せる
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;

        StartCoroutine(DestroyAfterSound());
        return;
    }

    IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(hitSound.length);
        Destroy(gameObject);
    }
}
