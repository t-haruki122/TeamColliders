using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonS_third : MonoBehaviour
{
    // タグごとの共通カウントを保持する辞書
    private static Dictionary<string, int> tagPressCounts = new Dictionary<string, int>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string tag = gameObject.tag;

            // タグがまだ辞書にないなら初期化
            if (!tagPressCounts.ContainsKey(tag))
            {
                tagPressCounts[tag] = 0;
            }

            // カウントをインクリメント
            tagPressCounts[tag]++;

            // 奇数回なら+5、偶数回なら-5
            int direction = (tagPressCounts[tag] % 2 == 1) ? 5 : -5;

            // 移動処理
            if (tag == "Redbutton")
            {
                MoveWalls("C_Red", new Vector3(0, direction, 0));
            }
            else if (tag == "Bluebutton")
            {
                MoveWalls("C_Blue", new Vector3(direction * 3, 0, 0));
            }
            else if (tag == "Yellowbutton")
            {
                MoveWalls("C_Yellow", new Vector3(0, 0, direction * -3));
            }
        }
    }

    private void MoveWalls(string wallPrefix, Vector3 moveDirection)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith(wallPrefix))
            {
                obj.transform.position += moveDirection;
            }
        }
    }
}
