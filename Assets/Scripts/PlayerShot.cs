using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    public GameObject shellPrefab;
    private Animator animator;

    public bool isActivePlayerShot = true;
    public int firingRate = 15;
    public int shellSpeed = 2000;

    public bool ballisticGapCompensation = true;

    private int frameCount = 0;

    // メインカメラ
    public GameObject mainCamera;

    // 音
    private AudioSource audioSource;
    public AudioClip gunSound;


    void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool isFiring = GameManager.GMInstance.getIsFiring();
        animator.SetBool("isFiring", isFiring);

        if (!isFiring) return;

        frameCount += 1;

        if (frameCount == firingRate)
        {
            frameCount = 0;

            Vector3 mainCameraDir = mainCamera.transform.rotation * Vector3.forward;
            Quaternion rotation = mainCamera.transform.rotation;
            // クロスヘアと弾道のギャップを補正
            if (ballisticGapCompensation) {
                if (Physics.Raycast(mainCamera.transform.position, mainCameraDir, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.CompareTag("EnemyCollider")) {
                        // Debug.Log("Player target: "+ hit.collider.gameObject.name);
                        rotation = Quaternion.LookRotation(hit.collider.transform.GetComponent<Renderer>().bounds.center - transform.position);
                    }
                }
            }
            transform.rotation = rotation;

            // 弾を発射
            if (GameManager.GMInstance.getRemainingAmmo() > 0) {
                audioSource.PlayOneShot(gunSound, 2f);
                GameObject shell = Instantiate(shellPrefab, transform.position, rotation);
                Rigidbody shellRb = shell.GetComponent<Rigidbody>();
                GameManager.GMInstance.reduceAmmo();
    
                // 弾速は自由に設定
                shellRb.AddForce(transform.forward * shellSpeed);
    
                // 3秒後に砲弾を破壊する
                Destroy(shell, 3.0f);
            }
        }
    }
}
