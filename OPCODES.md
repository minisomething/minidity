MVM Instructions 
====

Table
----

| Opcode | Name | Description | Operand | Gas |
| --- | --- | --- | --- | --- |
| `0x00` | NOP | Does nothing | - | 1 |
| `0x00` | ABORT | Aborts execution | - | 1 |
| `0x00` | RET | Returns to the previous caller | - | 1 |
| `0x00` | CALL | Jumps to the specific method | MethodSig | 1 |
| `0x00` | CALLI | Jumps to the specific method | MethodSig | 1 |
| `0x00` | CALLTHIS | Jumps to the specific method | MethodSig | 1 |
| `0x00` | PUSH | Pushes the given object to stack | AnyObject | 1 |
| `0x00` | POP | Removes item from stack | - | 1 |
| `0x00` | INC | Increments the value of last object from stack | - | 1 |
| `0x00` | DEC | Decrements the value of last object from stack | - | 1 |
| `0x00` | ADD | Adds two items from stack and push the result | - | 1 |
| `0x00` | SUB | Substracts two items from stack and push the result | - | 1 |
| `0x00` | MUL | Multiplies two items from stack and push the result | - | 1 |
| `0x00` | DIV | Divides two items from stack and push the result | - | 1 |
| `0x00` | CMP | Compares two items from stack and push the result | - | 1 |
| `0x00` | G | Compares two items from stack and push the result | - | 1 |
| `0x00` | GE | Compares two items from stack and push the result | - | 1 |
| `0x00` | L | Compares two items from stack and push the result | - | 1 |
| `0x00` | LE | Compares two items from stack and push the result | - | 1 |
| `0x00` | JMP | Compares two items from stack and push the result | - | 1 |
| `0x00` | JMPG | Compares two items from stack and push the result | - | 1 |
| `0x00` | JMPGE | Compares two items from stack and push the result | - | 1 |
| `0x00` | JMPL | Compares two items from stack and push the result | - | 1 |
| `0x00` | JMPLE | Compares two items from stack and push the result | - | 1 |
| `0x00` | JMPEQ | Compares two items from stack and push the result | - | 1 |
| `0x00` | STSTATE | Stores value into blockchain | FieldSig | 1 |
| `0x00` | STSTATE2 | Stores value into blockchain | FieldSig | 1 |
| `0x00` | LDSTATE | Loads value from blockchain | FieldSig | 1 |
| `0x00` | LDSTATE2 | Loads value from blockchain | FieldSig | 1 |
| `0x00` | STLOC | Stores value into local variable | VarSig | 1 |
| `0x00` | STLOC2 | Stores value into local variable | VarSig | 1 |
| `0x00` | LDLOC | Loads value from local variable | VarSig | 1 |
| `0x00` | PRINT | for debugging purpose | - | 1 |

Details
----
### NOP
This instruction does nothing. Used internally to make paddings.

### ABORT
Aborts current execution. The result of transaction will be marked as `FAILED` and every `STSTATE` operation will be reverted.

### RET
Returns from the current method. 
It does not returning a value actually. The last object of stack will be treated as `return value`.

### STLOC
Pops last object from stack and stores into local variable.

### LDLOC
Loads object from local variable and pushes into stack.

### PRINT
(for Debugging) Pops last object from stack and print it.
