using System;

namespace minikozfelvir.Classok;

public interface IFelvetelizo
{

    string OM_Azonosito { get; set; }
    string Neve { get; set; }
    string ErtesitesiCime { get; set; }
    string Email { get; set; }
    DateTime SzuletesiDatum { get; set; }
    int Matematika { get; set; }
    int Magyar { get; set; }

    string CSVSortAdVissza();

    void ModositCSVSorral(string csvString);
}

