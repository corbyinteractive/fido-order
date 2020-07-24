using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Serialization;



public class Item
{
    [XmlAttribute("Nome")]
    public string nomePiatto;

    [XmlElement("Ingredienti")]
    public string Ingredienti;

    [XmlElement("Allergeni")]
    public int[] Allergeni;

    [XmlElement("conFoto")]
    public Boolean foto;

    [XmlElement("UrlFoto")]
    public string urlFoto;

    [XmlElement("Note")]
    public string notePiatto;

    [XmlElement("Prezzo")]
    public float prezzo;
}
