using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticTestingSystem.Framework.Common
{
    [Flags]
    public enum ColumnFlags{}


    public enum SqlLike
    {
        StartWith = 1,
        EndWith = 2,
        AnyWhere = 3
    }

    public enum Connection
    {
        Default,
        And,
        OR


    }

    public enum LevelType
    {
        Group,
        Item,
        Element
    }
    public enum MenuKey
    {
        Separator,
        New,
        Add,
        Delete,
        Modify,
        MoveUp,
        MoveDown,
        Expand,
        ExpandAll,
        Collapse,
        CollapseAll,
        InsertAbove,
        InsertBelow,
        SaveChangedItem,
        SaveAllChangedItems




    }
    public enum Position
    { 
        Above,
        Below
    
    }

    public enum Sequence
    { 
        Desc,
        Asc
    }

    public enum ExecType
    { 
        Read,
        Write    
    }

    public enum OptionType
    { 
        Default,
        Module,
        Database,
        File,
        Instrument,
        Variant,
    }

    public enum OperatorEnum
    { 
        Equal,
        LessThan,
        LessThanAndEqual,
        MoreThan,
        MoreThanAndEqual,
        None  
    }

    public enum LimitConnector
    { 
        None,
        And,
        Or   
    }

    public enum Authority
    { 
        Operator,
        Guest,
        User,
        Administrator,   
    }

    public enum CommunicationType
    { 
        SerialPort,
        GPIB,
        TCP,
        USB,
        UDP,
        None,
    }
    public enum TerminationCharacter
    { 
        LineFeed,
        CarriageReturn,
        LF_CR,
        None
    }

    public enum PortType
    {
        RS232, USB, GPIB, LAN, None
    }
}
