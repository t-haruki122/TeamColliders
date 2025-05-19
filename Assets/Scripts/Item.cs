using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*<-+-*-~-=-=-~-*-+-base(0)Class-+-*-~-=-=-~-*-+->*/
/*item アイテム*/
public abstract class Item
{
    public abstract double getItem();
    public virtual string itemName() { return "アイテム"; }
    public virtual int itemAmount() { return 0; }
}

/*<-+-*-~-=-=-~-*-+-base(1)Class-+-*-~-=-=-~-*-+->*/
public abstract class Weapon : Item
{
    protected abstract int getWeaponID();
    public override double getItem() { return (double)getWeaponID(); }
    public override string itemName() { return "何かしらの武器"; }
}

public abstract class RecoverPP : Item
{
    protected abstract double getRecoverDegree();
    public override double getItem() { return getRecoverDegree(); }
    public override string itemName() { return "PP回復"; }
}

public abstract class RecoverAmmo : Item
{
    protected abstract int getAmmoAmount();
    public override double getItem() { return (double)getAmmoAmount(); }
    public override string itemName() { return "弾薬"; }
    public override int itemAmount() { return getAmmoAmount(); }
}

/*<-+-*-~-=-=-~-*-+-Item-+-*-~-=-=-~-*-+->*/
/*=*==*==*==*=PP関連=*==*==*==*=*/
/*recoverPPs pp回復(小)*/
public class recoverPPs : RecoverPP
{
    protected override double getRecoverDegree() { return 0.3; }
    public override string itemName() { return "PP回復(小)"; }
}
/*recoverPPm pp回復(中)*/
public class recoverPPm : RecoverPP
{
    protected override double getRecoverDegree() { return 0.5; }
    public override string itemName() { return "PP回復(中)"; }
}
/*recoverPPl pp回復(大)*/
public class recoverPPl : RecoverPP
{
    protected override double getRecoverDegree() { return 0.8; }
    public override string itemName() { return "PP回復(大)"; }
}

/*=*==*==*==*=弾関連=*==*==*==*=*/
/*recoverAmmos 弾数回復 (小)*/
public class recoverAmmos : RecoverAmmo
{
    protected override int getAmmoAmount() { return 20; }
}
/*recoverAmmom 弾数回復 (中)*/
public class recoverAmmom : RecoverAmmo
{
    protected override int getAmmoAmount() { return 50; }
}
/*recoverAmmol 弾数回復 (大)*/
public class recoverAmmol : RecoverAmmo
{
    protected override int getAmmoAmount() { return 100; }
}

/*=*==*==*==*=武器関連=*==*==*==*=*/
/*素手(weaponID = 0)*/
public class unarmed : Weapon
{
    protected override int getWeaponID() { return 0; }
    public override string itemName() { return "素手"; }
}
/*scorpion(weaponID = 1)*/
public class scorpion : Weapon
{
    protected override int getWeaponID() { return 1; }
    public override string itemName() { return "スコーピオン"; }
}

/*=*==*==*==*=ワールドギミック関連=*==*==*==*=*/
public class Key : Item
{
    private int keyID = 0;
    public Key(int keyID) {
        this.keyID = keyID;
    }
    public override double getItem() { return keyID; }
    public override string itemName() { return "鍵"; }
}