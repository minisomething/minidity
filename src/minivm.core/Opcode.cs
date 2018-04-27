﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public enum Opcode
    {
        Nop,

        Ret,

        // STACK-MANAGEMENT
        Push, Pop,

        // MATH OPERATION
        Inc, Dec,
        Add, Sub, Mul, Div,

        // IF
        Cmp,

        // CALL
        Call, Calli, Callthis,

        // JUMP
        Jmp, JmpG, JmpL, JmpEq, JmpGE, JmpLE,

        // STORE/LOAD
        Ststate, Ldstate,
        Stloc, Ldloc,

        // DEBUG
        Print
    }
}