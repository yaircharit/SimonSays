using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigSettings", menuName = "Settings/Config")]
public class ConfigSettings : ScriptableObject
{
    public string configFileName = "SimonSaysConfig.firebase";
    public AppConfig easy;
}
