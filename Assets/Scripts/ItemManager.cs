using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : GameManager
{
    /*singleton*/
    public static ItemManager IMInstance {get; private set;}

    /*<-+-*-~-=-=-~-*-+-member-+-*-~-=-=-~-*-+->*/
    /*pp回復*/
    private const double recoverPPs = 0.3;
    private const double recoverPPm = 0.5;
    private const double recoverPPl = 0.8;
    /*弾数*/
    private const int recoverAmmos = 20;
    private const int recoverAmmom = 50;
    private const int recoverAmmol = 100;

    /*<-+-*-~-=-=-~-*-+-eventMethod-+-*-~-=-=-~-*-+->*/
    void Awake() {
        if (IMInstance == null) {
            IMInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    /*<-+-*-~-=-=-~-*-+-method-+-*-~-=-=-~-*-+->*/
    public void recoverPPS() { addPP(recoverPPs); }
    public void recoverPPM() { addPP(recoverPPm); }
    public void recoverPPL() { addPP(recoverPPl); }

    public void recoverAmmoS() { addAmmo(recoverAmmos); }
    public void recoverAmmoM() { addAmmo(recoverAmmom); }
    public void recoverAmmoL() { addAmmo(recoverAmmol); }
}
