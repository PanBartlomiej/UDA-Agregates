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
public struct Maksimum : IBinarySerialize
{
    private SqlDouble s1;
    private bool isStarted;
    public void Init()
    {
        isStarted = false;
    }

    public void Accumulate(SqlDouble Value)
    {
        if (isStarted) {
            if (Value > s1)
                s1 = Value; }
        else
        {
            s1 = Value;
            isStarted = true;
        }
    }

    public void Merge(Maksimum Group)
    {
        this.s1 = Group.s1;
    }

    public SqlDouble Terminate()
    {
        return s1;
    }
    public void Read(BinaryReader r)
    {
        this.s1 = r.ReadDouble();
        this.isStarted = r.ReadBoolean();
    }
    public void Write(BinaryWriter w)
    {

        w.Write((double)this.s1);
        w.Write(this.isStarted);

    }

}
