# CPL - Compiled Programming Language

## About
A compiler for a low level programming language, made for fun and learning.

## Steps
- Lexer (Not implemented yet)
- Parsing (Not implemented yet)
- AST (Working on it)
- IR (Working on it)
- Backend (Working on it): x64, ARM, etc
- Analysis [Skipped for now]
- Optimization [Skipped for now]
- Assembly Generation

I'm focused only on the backend of the compiler right now. That's more fun than making a tokenizer, which I have done too many times to count.

# Code examples

## `var` and `const`
In CPL, and unlike C, there are specific keywords for declaring variables and functions.
This is done mainly to ease parsing but also serves to aid code legibility at a glance.
The type of something is right handed to the declaration, before a `:`.
Additionally, there are no semi colons in CPL.

```
var hello : int = 10
const world : int = 20
world = 30 # Error
```

## Hello World
C functions work out of the box. CPL follows the System-V ABI.
```
extern function puts(const str : byte*) = 0

function main() : void {
  puts("Hello World")
}
```

## `finally`, `assert` and `if not zero`
To aid resource cleanup logic, CPL provides the `finally` block which will always be called before the function exit.
```
extern function malloc(const size : long) : void*

function main() : void {
	const mem : void* = malloc(100)
	mem! # Assert not zero: If we failed to allocate, jump to finally
} finally {
	# If not zero: The twin of 'assert not zero'
	mem? free(mem)
}
```

# Loops
For now, only `while` loops are supported in CPL.
```
var x : int = 10
while (x > 0) {
	x = x - 1
}
```

# Only One assignment operator
In CPL, there is no `+=`, `-=`, etc. Only `=`.
You'll need to do `x = x + 1` instead of `x += 1`
