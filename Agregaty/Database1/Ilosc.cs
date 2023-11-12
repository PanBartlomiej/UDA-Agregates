using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections;
using System.IO;
[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.UserDefined,
   IsInvariantToNulls = true,
   IsInvariantToDuplicates = false,
   IsInvariantToOrder = false,
   MaxByteSize = 8000)]
public struct Ilosc : IBinarySerialize
{
    private int n;
    public void Init()
    {
        n = 0;
    }

    public void Accumulate(SqlDouble Value)
    {
        n++;
    }

    public void Merge(Ilosc Group)
    {
        this.n += Group.n;
    }

    public SqlDouble Terminate()
    {
        return new SqlDouble(this.n);
    }
    public void Read(BinaryReader r)
    {
        this.n = r.ReadInt32();
    }
    public void Write(BinaryWriter w)
    {
        w.Write(this.n);
    }

}
