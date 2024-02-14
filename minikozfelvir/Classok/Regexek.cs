using System.Text.RegularExpressions;

namespace minikozfelvir.Classok;

public static class Regexek
{
    #region __PreviewTextInput__
    public static Regex OM_PTI => new(@"[0-9]");
    public static Regex Nev_PTI => new(@"[A-ZÁÉÍÓÖŐÚÜŰa-záéíóöőúüű ]");
    public static Regex Email_PTI => new(@"[^\s]");
    public static Regex Datum_PTI => new(@"[0-9\.]");
    public static Regex Cim_PTI => new(@"[A-Za-zÁÉÍÓÖŐÚÜŰáéíóöőúüű0-9\s.,-]");
    public static Regex Szam_PTI => new(@"[\-0-9]");
    #endregion

    #region __Validate__
    public static Regex OM_V => new(@"^[0-9]{11}$");
    public static Regex Nev_V => new(@"^[A-ZÁÉÍÓÖŐÚÜŰ][a-záéíóöőúüű]+(?> [A-ZÁÉÍÓÖŐÚÜŰ][a-záéíóöőúüű]+){1,}$");
    public static Regex Email_V => new(@"^[^ ]+@[^ ]+\.[a-z]{2,}$");
    public static Regex Datum_V => new(@"[0-9]{4}\.[0-9]{1,2}\.[0-9]{1,2}\.");
    public static Regex Cim_V => new(@"^[A-Za-zÁÉÍÓÖŐÚÜŰáéíóöőúüű0-9\s.,-]{1,100}$");
    public static Regex Szam_V => new(@"^-?[1-5]?[0-9]$");
    #endregion

    #region __Egyéb__
    public static Regex KoztesSzokoz => new(@".+ {2,}.+");
    #endregion
}
