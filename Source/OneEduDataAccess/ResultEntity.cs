using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ResultEntity
/// </summary>
public class ResultEntity
{
    public ResultEntity()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public ResultEntity(bool res, string msg)
    {
        Res = res;
        Msg = msg;
    }

    public bool Res { get; set; }
    public string Msg { get; set; }
    public object ResObject { get; set; }
}