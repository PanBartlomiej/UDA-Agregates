using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.UserDefined,
   IsInvariantToNulls = true,
   IsInvariantToDuplicates = false,
   IsInvariantToOrder = false,
   MaxByteSize = 8000)]
public struct Moda : IBinarySerialize
{
    private List<double> tab;
    class Para
    {
        public double wartosc;
        public int ilosc;
    }
    static Para utworzPare(double wartosc, int ilosc = 1)
    {
        Para p = new Para();
        p.wartosc = wartosc;
        p.ilosc = ilosc;
        return p;
    }
    static int wyszukaj(List<Para> lista, double szukana)
    {
        int L = 0, P = lista.Count - 1, sr = 0;
        while (L <= P)
        {
            sr = (L + P) / 2;
            if (lista[sr].wartosc == szukana)
                return sr;
            if (lista[sr].wartosc > szukana)
                P = sr - 1;
            else
                L = sr + 1;
        }
        return L;
    }


    public void Init()
    {
        this.tab = new List<double>();

    }

    public void Accumulate(SqlDouble Value)
    {
        tab.Add((double)Value);

    }

    public void Merge(Moda Group)
    {
        for (int i = 0; i < Group.tab.Count; i++)
            this.tab.Add((double)tab[i]);

    }

    public SqlString Terminate()
    {
        List<Para> lista = new List<Para>();
        lista.Add(utworzPare(tab[0]));
        for (int i = 1; i < tab.Count; i++)
        {
            int poz = wyszukaj(lista, tab[i]);
            if (poz < lista.Count && lista[poz].wartosc == tab[i])      lista[poz].ilosc++;
            else    lista.Insert(poz, utworzPare(tab[i]));
            
        }
        int max = 0;
        List<double> wynik = new List<double>();
        for (int i = 0; i < lista.Count; i++)
        {
            if (lista[i].ilosc > max)
            {
                wynik.Clear();
                max = lista[i].ilosc;
            }
            if (lista[i].ilosc == max)
                wynik.Add(lista[i].wartosc);
        }
        lista.Clear();

        SqlString str = "";
        for (int i = 0; i < wynik.Count; i++)
            str +=" "+ wynik[i]+ ";";
        return str;


    }
    public void Read(BinaryReader r)
    {
        int size = (int)r.ReadInt32();
        // a tu taka  niespodzianka
        tab = new List<double>(); //trzeba t¹ zmienn¹ tu inicjowaæ a nie tylko w init
                                  //wynika to z tego ¿e w momencie odczytywania ta zmienna jest nie zainicjowana

        for (int i = 0; i < size; i++)
            this.tab.Add(r.ReadDouble());

    }
    public void Write(BinaryWriter w)
    {
        w.Write((int)this.tab.Count);

        for (int i = 0; i < this.tab.Count; i++)
        {
            w.Write((double)this.tab[i]);
        }
    }

}
