minidity
====

A tiny script compiler & vm.

Overview
----
__compiler__
```cs
var src = @"
    class foo {
        public globalVar;

        def _ctor() {
            globalVar = 1234;
        }
        def sum (a, b) {
            ret a + b;
        }
    }";

var program = MinidityCompiler.Compile(src);
```

__vm__
```cs
var vm = new VM<MemStateProvider>();
var gasLimit = 1000;
var gasUsed = 0;

var ret = vm.Execute(program.instructions, gasLimit, out gasUsed);

Console.WriteLine(ret);
```
```cs
Console.WriteLine(
    vm.stateProvider.GetState(
        ABISignature.Field("foo","global")));
```

Integrate with blockchain
----
`STSTATE` and `LDSTATE` instructions require __StateProvider__.

```cs
public interface IStateProvider
{
    int blockNo { get; }

    object GetState(string key);
    void SetState(string key, object value);
}
```
```cs
public class MinichainStateProvider : IStateProvider
{
    /* .... */
}
```

Syntax
----
```
class CONTRACT_NAME {
    /* FIELDS */
    public name;

    // Constructor
    def _ctor() {
        name = "MY_FIRST_MINIDITY_CONTRACT";
    }

    def sum(a, b) {
        ret a + b;
    }
}
```

__KeySpecs__
    * Dynamic variables
    * Class-based


Caller and Callee
----
```
def sum(a, b, c) {
    ret a + b + c;
}

sum (1, 2, 3);
```
```
:caller
push 1
push 2
push 3
call sum(a, b, c)

:sum(a, b, c)
stloc c
stloc b
stloc a
/* ... */
ret
```

Add internal calls
----

TODOTODOTODO
```cs
private void BuildInternalCalltable()
{
    callTable["Math.pow"] = -1;
}
```
```cs
private void PerformInternalCall(string signature)
{
    if (signature == "Math.pow")
    {
        var a = ctx.state.PopDouble();
        var b = ctx.state.PopDouble();

        ctx.state.Push(MMath.Pow(b, a));
    }
}
```
