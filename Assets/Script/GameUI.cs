using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour {

    public GunScript gunscript;

    public void ShootAnimal()
    {
        gunscript.ShotAnimal();
    }

    public void ShootReset()
    {
        gunscript.Reset();
    }
}