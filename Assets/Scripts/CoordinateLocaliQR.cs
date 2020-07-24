using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class CoordinateLocaliQR
{
    [JsonIgnore]
    public int Id { get; set; }
    [JsonProperty("R")]
    public string Ristorante { get; set; }
    [JsonProperty("U")]
    public string Utenza { get; set; }
    [JsonProperty("S")]
    public string Sala { get; set; }
    [JsonProperty("T")]
    public string Tavolo { get; set; }
    [JsonProperty("E")]
    public int Scadenza{ get; set; }
    [JsonProperty("D")]
    public int Db { get; set; }
}
