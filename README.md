# Expression Replacer

## Small proof-of-concept expression replacer written in C#.

This expression replacer assumes that no side-effects are possible, meaning we can replace any expression with their value.

## Running
### On Linux:
- You need the ```dotnet-sdk``` to be able to run C# code.
- To run the code, simply run the following command in your shell while being in the project directory
```
dotnet run --project ExprReplacer
```

### On Windows
- Using Visual Studio, open ```ExprReplacer.sln``` and run the program inside the IDE.

## Expected Output:
```
Expression: x + 2 * 7 + cos(x + 4 + 10)

==========================
Unoptimized:
==========================
expr_2: x
expr_4: 2
expr_5: 7
expr_3: expr_4 * expr_5
expr_1: expr_2 + expr_3
expr_8: x
expr_10: 4
expr_11: 10
expr_9: expr_10 + expr_11
expr_7: expr_8 + expr_9
expr_6: cos(expr_7)
expr_0: expr_1 + expr_6
result: expr_0

==========================
Optimized (Not Folded):
==========================
expr_2: x
expr_4: 2
expr_5: 7
expr_3: expr_4 * expr_5
expr_1: expr_2 + expr_3
expr_9: 4
expr_10: 10
expr_8: expr_9 + expr_10
expr_7: expr_2 + expr_8
expr_6: cos(expr_7)
expr_0: expr_1 + expr_6
result: expr_0

==========================
Optimized (Folded):
==========================
expr_2: x
expr_3: 14
expr_1: expr_2 + expr_3
expr_4: cos(expr_1)
expr_0: expr_1 + expr_4
result: expr_0
```

## TODO
- [x] Add support for constant folding expressions
    ```
    x + 2 * 7 + cos(x + 10 + 4) should output:
    expr_0: x
    expr_1: 14
    expr_2: expr_0 + expr_1
    expr_3: cos(expr_2)
    expr_4: expr_2 + expr_3
    result: expr_4
    ```

    For the moment, here is what it outputs:
    ```
    expr_2: x
    expr_4: 2
    expr_5: 7
    expr_3: expr_4 * expr_5
    expr_1: expr_2 + expr_3
    expr_9: 10
    expr_10: 4
    expr_8: expr_9 + expr_10
    expr_7: expr_2 + expr_8
    expr_6: cos(expr_7)
    expr_0: expr_1 + expr_6
    result: expr_0
    ```
