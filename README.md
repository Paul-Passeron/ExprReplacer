# Expression Replacer

## Small proof-of-concept expression replacer written in C#.

This expression replacer assumes that no side-effects are possible, meaning we can replace any expression with their value.

## Running
### On Linux:
- You need the ```dotnet-sdk``` to be able to run C# code.
- To run the code, simply run the following command in your shell while being in the project directory
```
dotnet run
```

### On Windows
- Using Visual Studio, open ```ExprReplacer.csproj``` and run the program inside the IDE.

## Expected Output:
```
Expression: x + 2 * y + cos(x + 2 * y)

==========================
Unoptimized:
==========================
expr_2: x
expr_4: 2
expr_5: y
expr_3: expr_4 * expr_5
expr_1: expr_2 + expr_3
expr_8: x
expr_10: 2
expr_11: y
expr_9: expr_10 * expr_11
expr_7: expr_8 + expr_9
expr_6: cos(expr_7)
expr_0: expr_1 + expr_6
result: expr_0

==========================
Optimized:
==========================
expr_2: x
expr_4: 2
expr_5: y
expr_3: expr_4 * expr_5
expr_1: expr_2 + expr_3
expr_6: cos(expr_1)
expr_0: expr_1 + expr_6
result: expr_0
```
