using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnemy : baseEnemy
{
    private bool isRun = false;
    protected override void Spawn()
    {
        setBaseParams(
            speed: 20f
        );
    }

    protected override void Act()
    {
        if (isGetDamageOnFrame && !isRun)
        {
            isRun = true;
            AudioManager.AMInstance.PlayEnemyRunAwaySound();
        }
        if (isRun)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}