using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatiCliente
{
    public int Id { get; set; }
    public int Privacy { get; set; }
    public int Pubblicità { get; set; }
    public string PrivacyDate { get; set; } = "";
    public string Nome { get; set; }  = "";
    public string Cognome { get; set; } = "";
    public string Indirizzo { get; set; } = "";
    public string Citta { get; set; } = "";
    public string CAP { get; set; } = "";
    public string Tel { get; set; } = "";
    public string Email { get; set; } = "";
    public int RegMin { get; set; } //identifica il tipo di dati già raccolti per non ripetere domande
    public string DataNascita { get; set; } = "";
    public string Sesso { get; set; } = "";
    public string TokenFB { get; set; } = "";

    public override string ToString()
    {
        return string.Format("[Dati Cliente: Id = {0} Privacy={1}, Pubblicità={2},  PrivacyDate={3}, Nome={4}, Cognome={5}, Indirizzo={6}, Citta={7}, CAP={8}, Tel={9}, Email={10}, RegMin = {10}, DataNascita = {11}, Sesso = {12}]", Id, Privacy, Pubblicità, PrivacyDate, Nome, Cognome, Indirizzo, Citta, CAP, Tel, Email, RegMin, DataNascita, Sesso);
    }

}
