using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public enum Opcode
    {
        Nop,

        Abort,

        // CALL
        Ret,
        EmitEvent,
        Call, Calli, Callthis,

        // STACK-MANAGEMENT
        Push, Pop,

        // MATH OPERATION
        Inc, Dec,
        Add, Sub, Mul, Div,

        // IF
        Cmp, G, Ge, L, Le,

        // JUMP
        Jmp,
        JmpTrue,
        JmpG, JmpL, JmpEq, JmpGE, JmpLE,

        // STORE/LOAD
        Ststate, Ststate2, Ldstate, Ldstate2,
        Stloc, Stloc2, Ldloc,

        // DEBUG
        Print
    }
}
