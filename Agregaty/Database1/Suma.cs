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
public struct Suma : IBinarySerialize
{
    private SqlDouble s1;
    public void Init()
    {
        this.s1 = 0;
        // Put your code here
    }

    public void Accumulate(SqlDouble Value)
    {
        
        s1 += Value;

        // Put your code here
    }

    public void Merge(Suma Group)
    {
        this.s1 += Group.s1;
    }

    public SqlDouble Terminate()
    {
        // Put your code here
        return s1;

    }
    public void Read(BinaryReader r)
    {
        this.s1 = r.ReadDouble();

    }
    public void Write(BinaryWriter w)
    {

        w.Write((double)this.s1);

    }

    // This is a place-holder member field



}
