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
public struct Średnia : IBinarySerialize
{
    private SqlInt32 s1;
    private SqlInt32 ile;
    public void Init()
    {
        s1 = 0;
        ile = 0;
        // Put your code here
    }

    public void Accumulate(SqlInt32 Value)
    {
        this.s1 += Value;
        this.ile+=1;
        // Put your code here
    }

    public void Merge (Średnia Group)
    {
        this.s1 += Group.s1;
        this.ile += Group.ile;
        // Put your code here
    }

    public SqlDouble Terminate ()
    {
        // Put your code here
        return s1.ToSqlDouble()/ile.ToSqlDouble();
    }
    public void Read(BinaryReader r)
    {
        this.s1 = r.ReadInt32();
        this.ile = r.ReadInt32();

    }
    public void Write(BinaryWriter w)
    {

        w.Write((int)this.s1);
        w.Write((int)this.ile);

    }

}
