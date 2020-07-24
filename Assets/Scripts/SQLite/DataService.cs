using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;
using System;

public class DataService {

    private SQLiteConnection _connection;

    public DataService(string DatabaseName) {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        //Debug.Log("Final PATH: " + dbPath);     

    }
   
    /* **********************************************************
     * ***                                                    ***
     * ***    PRIVACY, PUBBLICAZIONE, TERMINI E CONDIZIONI    ***
     * ***                                                    ***
     * **********************************************************
    */
    public bool CheckDatiCliente()
    {
        var DBExists = _connection.GetTableInfo("DatiCliente");
        if (DBExists.Count == 0)
        {
            _connection.CreateTable<DatiCliente>();
            _connection.Insert(new DatiCliente
            {
                Id = 1,
                Privacy = 0

            });
            return false;
        }
        else
        {
            int termschecked = _connection.ExecuteScalar<int>("SELECT Privacy FROM DatiCliente WHERE Id = 1");
            if (termschecked == 1)
                return true;
            else
                return false;

        }
        
    }
    public bool CheckIfTableHasColumn(string table, string column)
    {
        int isExists = 0;
        try
        {
            isExists = _connection.ExecuteScalar<int>("SELECT COUNT (*) AS CNTREC FROM " + table + " WHERE " + column);
        }
        catch
        {

        }
        if (isExists == 0)
            return false;
        else
            return true;
    }
    public string GetFidelity(string Restaurant)
    {
        try
        {
            return _connection.ExecuteScalar<string>("SELECT Fidelity From CoordinateLocali WHERE Restaurant = " + (char)34 + Restaurant + (char)34);
        }
        catch
        {
            return "";
        }
    }
    public void SetFidelity(string Fidelity, string Restaurant)
    {
        _connection.Execute("Update Coordinatelocali set Fidelity = '" + Fidelity + "' where Restaurant = " + (char)34 + Restaurant + (char)34);
    }
    public void AddColumn (string Table, string Column)
    {
        try
        {
            _connection.Execute("Alter table " + Table + " add column " + Column);
        }
        catch
        {
        }

    }
    public bool GetRegMin()
    {
        int termschecked = _connection.ExecuteScalar<int>("SELECT RegMin FROM DatiCliente WHERE Id = 1");
        if (termschecked == 1)
            return true;
        else
            return false;
    }
    public void eraseAll()
    {
        _connection.DropTable<DatiCliente>();
        _connection.DropTable<CoordinateLocali>();
    }
    public bool GetTipoDati()
    {
        try
        {
            string indirizzo = _connection.ExecuteScalar<string>("SELECT Indirizzo FROM DatiCliente WHERE Id = 1");
            if (indirizzo.Length >= 6)
            {
                string citta = _connection.ExecuteScalar<string>("SELECT Citta FROM DatiCliente WHERE Id= 1");
                if (citta.Length >= 3)
                {
                    string cap = _connection.ExecuteScalar<string>("SELECT CAP FROM DatiCliente WHERE Id= 1");
                    if (cap.Length >= 5)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    public bool CheckAds()
    {
        int prv = _connection.ExecuteScalar<int>("SELECT Privacy FROM DatiCliente");
        if (prv == 1)
            return true;
        else
            return false;
    }
    public IEnumerable<DatiCliente> GetDatiCliente()
    {
        return _connection.Query<DatiCliente>("SELECT * FROM DatiCliente WHERE Id = 1");
    }
    public void UpdateDatiClienteMin( int privacy, int pubblicità, string privacyDate, string nome, string cognome, string tel, string email, int regmin)
    {
        var DBExists = _connection.GetTableInfo("DatiCliente");
        if (DBExists.Count == 0)
        {
            _connection.CreateTable<DatiCliente>();
        }
        _connection.CreateTable<DatiCliente>();
        _connection.Execute("Delete from DatiCliente where id = 1");
        _connection.Insert(new DatiCliente
        {
            Id = 1,
            Privacy = privacy,
            Pubblicità = pubblicità,
            PrivacyDate = privacyDate,
            Nome = nome,
            Cognome = cognome,
            Tel = tel,
            Email = email,
            RegMin = regmin
            
        });

    }
    public void UpdateDatiClienteTot( int privacy, int pubblicità, string privacyDate, string nome, string cognome, string indirizzo, string citta, string cap, string tel, string email)
    {
        var DBExists = _connection.GetTableInfo("DatiCliente");
        if (DBExists.Count == 0)
        {
            _connection.CreateTable<DatiCliente>();
        }
        _connection.CreateTable<DatiCliente>();
        _connection.Execute("Delete from DatiCliente where id = 1");
        _connection.Insert(new DatiCliente
        {
            Id = 1,
            Privacy = privacy,
            Pubblicità = pubblicità,
            PrivacyDate = privacyDate,
            Nome = nome,
            Cognome = cognome,
            Indirizzo = indirizzo,
            Citta = citta,
            CAP = cap,
            Tel = tel,
            Email = email
        });
    }
    public List<CoordinateLocali> GetVisitedLocali()
    {
        return _connection.Query<CoordinateLocali>("SELECT * FROM CoordinateLocali");
    }
    public void AddLocale(CoordinateLocali Cl)
    {
        bool toAdd = true;
        var DBExists = _connection.GetTableInfo("CoordinateLocali");
        if (DBExists.Count == 0)
        {
            _connection.CreateTable<CoordinateLocali>();
        }
        _connection.CreateTable<CoordinateLocali>();
        //IEnumerable<CoordinateLocali> Coord = _connection.ExecuteScalar<List<CoordinateLocali>>("SELECT * FROM DatiCliente");
        IEnumerable<CoordinateLocali> Coord = _connection.Table<CoordinateLocali>();
        foreach (CoordinateLocali C in Coord)
        {
            if (C.Restaurant == Cl.Restaurant)
            {
                _connection.Query<CoordinateLocali>("Update CoordinateLocali set Exipiry = " + Cl.Exipiry + " where id = " + C.id);
                _connection.Query<CoordinateLocali>("Update CoordinateLocali set Sala = '" + Cl.Sala + "' where id = " + C.id);
                _connection.Query<CoordinateLocali>("Update CoordinateLocali set Seat = '" + Cl.Seat + "' where id = " + C.id);
                toAdd = false;
            }
        }
        if (toAdd)
        {
            _connection.Insert(Cl);
        }
    }
    public static int DateTimeToUnixTimestamp(DateTime dateTime)

    {

        return (int)(TimeZoneInfo.ConvertTimeToUtc(dateTime) -

               new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;

    }
    public void DeleteExpired()
    {
        var DBExists = _connection.GetTableInfo("CoordinateLocali");
        if (DBExists.Count == 0)
        {
            _connection.CreateTable<CoordinateLocali>();
        }
        _connection.CreateTable<CoordinateLocali>();
        IEnumerable<CoordinateLocali> Coord = _connection.Table<CoordinateLocali>();
        foreach (CoordinateLocali C in Coord)
        {
            if (C.Exipiry <= DateTimeToUnixTimestamp(DateTime.Now))
            {

                _connection.Query<CoordinateLocali>("Update CoordinateLocali set Exipiry = null where id = " + C.id);
                _connection.Query<CoordinateLocali>("Update CoordinateLocali set Sala = null where id = " + C.id);
                _connection.Query<CoordinateLocali>("Update CoordinateLocali set Seat = null where id = " + C.id);

            }
        }
    }
    /*
    public void SaveOrdine(List<Comanda.Ordine> ordini, string OrderGuid)
    {
        foreach (Comanda.Ordine o in ordini)
        {
            var DBExists = _connection.GetTableInfo("OrdiniEff");
            if (DBExists.Count == 0)
            {
                _connection.CreateTable<OrdiniEff>();
            }
            _connection.CreateTable<OrdiniEff>();
            _connection.Execute("Delete from DatiCliente where id = 1");
            _connection.Insert(new OrdiniEff
            {
                GUIDordine = o.Guid,
                PrezzoBase = o.PrezzoBase,
                Nome =o.Nome,
                Prezzo = o.Prezzo,
                Quantità = o.Quantità,
                Indice = o.Indice,
                IDart = o.IDart,
                GUid = o.Guid,
                Val = o.Val
});
        }
    }
    */
}
