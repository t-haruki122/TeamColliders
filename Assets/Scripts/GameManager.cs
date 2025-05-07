using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*singleton*/
    public static GameManager Instance {get; private set;}

    /*member*/
    private int score = 0;
    private int hit = 0;
    private const double hitCoefficient = 0.95;
    private int combo = 0;
    private const int baseScore = 100;
    

    public int Hit {
        get {return hit;}
        set {hit = value;}
    }
    public int Combo{
        get {return combo;}
        set {combo = value;}
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
