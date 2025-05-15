using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*<-+-*-~-=-=-~-*-+-baseClass-+-*-~-=-=-~-*-+->*/
public abstract class Item : MonoBehaviour
{
    public abstract double getItem();
}

/*<-+-*-~-=-=-~-*-+-Item-+-*-~-=-=-~-*-+->*/
/*=*==*==*==*=PP関連=*==*==*==*=*/
/*recoverPPs pp回復(小)*/
public class recoverPPs : Item
{
    private const double recoverPPS = 0.3;

    public override double getItem() { return recoverPPS; }
}
/*recoverPPm pp回復(中)*/
public class recoverPPm : Item
{
    private const double recoverPPM = 0.5;

    public override double getItem() { return recoverPPM; }
}
/*recoverPPl pp回復(大)*/
public class recoverPPl : Item
{
    private const double recoverPPL = 0.8;

    public override double getItem() { return recoverPPL; }
}

/*=*==*==*==*=銃関連=*==*==*==*=*/
/*recoverAmmos 弾数回復 (小)*/
public class recoverAmmos : Item
{
    private const double recoverAmmoS = 20;

    public override double getItem() { return recoverAmmoS; }
}
/*recoverAmmom 弾数回復 (中)*/
public class recoverAmmom : Item
{
    private const double recoverAmmoM = 50;

    public override double getItem() { return recoverAmmoM; }
}
/*recoverAmmol 弾数回復 (大)*/
public class recoverAmmol : Item
{
    private const double recoverAmmoL = 100;

    public override double getItem() { return recoverAmmoL; }
}