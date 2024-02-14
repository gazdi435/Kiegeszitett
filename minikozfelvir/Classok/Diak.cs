using MySql.Data.MySqlClient;
using System;
using System.Linq;

namespace minikozfelvir.Classok;

public class Diak : IFelvetelizo
{
    string   oM_Azonosito = "", 
             neve = "",
             email = "",
             ertesitesiCime = "";
    DateTime szuletesiDatum = DateTime.Now;
    int      matematika = 0,
             magyar = 0;

    public string OM_Azonosito {
        get => oM_Azonosito;
        set
        {
            if (value.Length != 11)
                throw new Exception($"Nem lehet hoszabb mint 11 karakter!");
            if (value.Any(G => !char.IsNumber(G)))
                throw new Exception("Az OM azonosítóban csak számok lehetnek!");
            oM_Azonosito = value;
        }
    }
    public string Neve {
        get => neve;
        set
        {
            value = value.Trim();
            if (!value.Any(G => G == ' '))
                throw new Exception("Legalább 2 szó legyen benne!");
            if(!value.Split(' ').All(G => char.IsUpper(G[0])))
                throw new Exception("Minden szó nagy betűvel kezdődjön!");
            if (Regexek.KoztesSzokoz.IsMatch(value))
                throw new Exception("Köztes többes szóköz nem lehet!");
            neve = value;
        }
    }
    public string Email {
        get => email;
        set
        {
            if (value.Count(G => G == '@') > 1)
                throw new Exception("Csak 1db @ jel lehet benne!");
            if (value.Any(G => G == ' '))
                throw new Exception("Nem lehet sehol szóköz benne!");
            email = value;
        }
    }
    public string ErtesitesiCime {
        get => ertesitesiCime;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("Nem lehet üres!");
            ertesitesiCime = value;
        }
    }
    public DateTime SzuletesiDatum {
        get => szuletesiDatum;
        set => szuletesiDatum = value;
    }
    public int Matematika {
        get => matematika;
        set
        {
            if (value > 50)
                throw new Exception("50-nél nagyobb nem lehet");
            if (value < -1)
                throw new Exception("0-nál nagyobb nem lehet");
            matematika = value;
        }
    }
    public int Magyar {
        get => magyar;
        set
        {
            if (value > 50)
                throw new Exception("50-nél nagyobb nem lehet");
            if (value < -1)
                throw new Exception("0-nál nagyobb nem lehet");
            magyar = value;
        }
    }

    public Diak() { }
    public Diak(params string[] adatok)
    {
        OM_Azonosito = adatok[0];
        Neve = adatok[1];
        Email = adatok[2];
        SzuletesiDatum = DateTime.Parse(adatok[3]);
        ErtesitesiCime = adatok[4];
        Matematika = int.Parse(adatok[5]);
        Magyar = int.Parse(adatok[6]);
    }
    public Diak(string oM_Azonosito, string neve, string email, string ertesitesiCime, DateTime szuletesiDatum, int? matematika, int? magyar)
    {
        OM_Azonosito = oM_Azonosito;
        Neve = neve;
        Email = email;
        ErtesitesiCime = ertesitesiCime;
        SzuletesiDatum = szuletesiDatum;
        Matematika = matematika ?? -1;
        Magyar = magyar ?? -1;
    }
    public Diak(MySqlDataReader reader)
    {
        OM_Azonosito = reader.GetString("OM_Azonosito");
        Neve = reader.GetString("Nev");
        Email = reader.GetString("Email");
        ErtesitesiCime = reader.GetString("Ertesitesi_cim");
        SzuletesiDatum = reader.GetDateTime("Szuletesi_datum");
        Matematika = reader.GetByte("Matek_eredmeny");
        Magyar = reader.GetByte("Magyar_eredmeny");
    }

    public void ModositCSVSorral(string csvString)
    {
        string[] adatok = csvString.Split(";");

        OM_Azonosito = adatok[0];
        Neve = adatok[1];
        Email = adatok[2];
        SzuletesiDatum = DateTime.Parse(adatok[3]);
        ErtesitesiCime = adatok[4];
        Matematika = int.Parse(adatok[5]);
        Magyar = int.Parse(adatok[6]);
    }
    public string CSVSortAdVissza() => ToString();
    public override string ToString() => $"{OM_Azonosito};{Neve};{Email};{SzuletesiDatum:d};{ErtesitesiCime};{Matematika};{Magyar}";
}
