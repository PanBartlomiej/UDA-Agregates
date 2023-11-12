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
public struct Mediana : IBinarySerialize
{
    private List<double> tab;
    public void Init()
    {
        this.tab = new List<double>();
    }

    public void Accumulate(SqlDouble Value)
    {
        tab.Add((double)Value);
    }

    public void Merge(Mediana Group)
    {
        for (int i = 0; i < Group.tab.Count; i++)
            this.tab.Add((double)tab[i]);
    }

    public SqlDouble Terminate()
    {
        // Put your code here
        tab.Sort();
        if (tab.Count % 2 == 0)
        {
            return tab[tab.Count / 2];
        }
        else return (tab[tab.Count / 2 - 1] + tab[tab.Count / 2]) / 2.0;
       

    }
    public void Read(BinaryReader r)
    {
        int size = (int)r.ReadInt32();
        // a tu taka  niespodzianka
        tab = new List<double>(); //trzeba t¹ zmienn¹ tu inicjowaæ a nie tylko w init
        //wynika to z tego ¿e w momencie odczytywania ta zmienna jest nie zainicjowana 
        if(size >0)
        for (int i = 0; i < size; i++)
            this.tab.Add( r.ReadDouble());

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
