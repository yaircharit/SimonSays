using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AppSettings", menuName = "AppSettings")]
public class AppSettings : ScriptableObject
{
    public string configFileName = "SimonSaysConfig.firebase";
    

}
