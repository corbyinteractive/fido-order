using SQLite4Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateLocali
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string Utenza { get; set; }
    public string Restaurant { get; set; }
    public string LongName { get; set; }
    public string Sala { get; set; }
    public string Seat { get; set; }
    public int Exipiry { get; set; }
    public int Db { get; set; }
    public string Fidelity { get; set; }
}
