using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Date" ,menuName = "Date/Date")]
public class Date : ScriptableObject
{
    public List<LevelDate> levelDates = new List<LevelDate>();
}
