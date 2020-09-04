using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema.Protobuf.CSharp.Enums
{
    public enum UserActionType
    {
        Login = 0,
        Logout,
    }

    public enum DateRangeType
    {
        Months = 1,
        Week = 2,
        Today = 3
    }

    public enum LevelType
    {
        None = 0,
        EndGame = 1
    }

    public enum EServer
    {
        None = 0,
        StandAlone = 1,
        Lobby = 2,
        Game = 3,
        Match = 4,
        Synchronize = 5,

        Gateway = 998,
        Bridge = 999,
    }

    public enum EGateway
    {

    }

    public enum EBridge
    {

    }

    public enum ECloudPlatform
    {
        None = 0,
        Azure = 1,
        Alibaba = 2,
        Ibm = 3,
        Google = 4,
        Amazon = 5,
        Max = 6,
    }

    public enum EConfigKind
    {
        None = 0,

        Avaliable = 1,

        Notice = 2,

        All = 9999,
    }
}
