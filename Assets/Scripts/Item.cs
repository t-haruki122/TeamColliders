using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*<-+-*-~-=-=-~-*-+-base(0)Class-+-*-~-=-=-~-*-+->*/
/*item アイテム*/
public abstract class Item : MonoBehaviour
{
    public abstract double getItem();
}

/*<-+-*-~-=-=-~-*-+-base(1)Class-+-*-~-=-=-~-*-+->*/
/*weapon 武器*/
public abstract class Weapon : Item {}
public abstract class RecoverPP : Item {}
public abstract class RecoverAmmo : Item {}

/*<-+-*-~-=-=-~-*-+-Item-+-*-~-=-=-~-*-+->*/
/*=*==*==*==*=PP関連=*==*==*==*=*/
/*recoverPPs pp回復(小)*/
public class recoverPPs : RecoverPP
{
    private const double recoverPPS = 0.3;

    public override double getItem() { return recoverPPS; }
}
/*recoverPPm pp回復(中)*/
public class recoverPPm : RecoverPP
{
    private const double recoverPPM = 0.5;

    public override double getItem() { return recoverPPM; }
}
/*recoverPPl pp回復(大)*/
public class recoverPPl : RecoverPP
{
    private const double recoverPPL = 0.8;

    public override double getItem() { return recoverPPL; }
}

/*=*==*==*==*=弾関連=*==*==*==*=*/
/*recoverAmmos 弾数回復 (小)*/
public class recoverAmmos : RecoverAmmo
{
    private const double recoverAmmoS = 20;

    public override double getItem() { return recoverAmmoS; }
}
/*recoverAmmom 弾数回復 (中)*/
public class recoverAmmom : RecoverAmmo
{
    private const double recoverAmmoM = 50;

    public override double getItem() { return recoverAmmoM; }
}
/*recoverAmmol 弾数回復 (大)*/
public class recoverAmmol : RecoverAmmo
{
    private const double recoverAmmoL = 100;

    public override double getItem() { return recoverAmmoL; }
}

/*=*==*==*==*=武器関連=*==*==*==*=*/
/*素手(weaponID = 0)*/
public class unarmed : Weapon
{
    private const double weaponID = 0;

    public override double getItem() { return weaponID; }
}
/*scorpion(weaponID = 1)*/
public class scorpion : Weapon
{
    private const double weaponID = 1;

    public override double getItem() { return weaponID; }
}

/*=*==*==*==*=ワールドギミック関連=*==*==*==*=*/
public class Key : Item
{
    private int keyID = 0;
    public Key(int keyID) {
        this.keyID = keyID;
    }

    public override double getItem() { return keyID; }
}