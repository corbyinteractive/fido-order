using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Serialization;

public class Locale
{
    [XmlAttribute("Nome")]
    public string s_nomeLocale;

    [XmlElement("RichiediDatiAccesso")]
    public Boolean s_RichiediDatiAccesso;

    [XmlElement("Utenza")]
    public string s_utenza;

    [XmlElement("ColorTheme")]
    public string s_ColorTheme;

    [XmlElement("ColorFrame")]
    public string s_ColorFrame;

    [XmlElement("LimitPZ")]
    public float s_LimitPZ;

    [XmlElement("LimitVAL")]
    public float s_LimitVAL;

    [XmlElement("LimitMinVAL")]
    public float s_LimitMinVAL;

    [XmlElement("Provincia")]
    public string s_Provincia;

    [XmlElement("Landing")]
    public string s_Landing;

    [XmlElement("LandingGen")]
    public string s_LandingGen;

    [XmlElement("ShortName")]
    public string s_ShortName;

    [XmlElement("Base")]
    public Boolean s_Base;

    [XmlElement("ConsegnaInterna")]
    public Boolean s_consegnaInterna;

    [XmlElement("EnableMD")]
    public Boolean s_EnableMD;

    [XmlElement("TakeAway")]
    public Boolean s_TakeAway;

    [XmlElement("Delivery")]
    public Boolean s_Delivery;

    [XmlElement("HideTotal")]
    public Boolean s_HideTotal;

    [XmlElement("PinPag")]
    public string s_pinPag;

    [XmlElement("EnablePagaCons")]
    public Boolean s_EnablePagaCons;

    [XmlElement("EnablePP")]
    public string s_enablePP;

    [XmlElement("CID")]
    public string s_CID;

    [XmlElement("SKE")]
    public string s_SKE;

    [XmlElement("EnableNEXI")]
    public string s_enableNEXI;

    [XmlElement("NEXIalias")]
    public string s_NEXIalias;

    [XmlElement("NEXImac")]
    public string s_NEXImac;
}
