# Pascal Compiler

# ***WIP***

**The below information may not be applicable (yet) as this is a WIP**

This is a Pascal compiler I wrote as a personal project in C#, specifically, adhering to ISO/IEC 7185:1990(E).
Any reference to "the spec", refers to this spec.
It probably has some flaws, so I wouldn't use it for anything important, just a fun project.
Shooting for ?linux x86_64 ABI?.

Compiler structure:
Lexer->Parser->Check->Emit->Assemble (MASM)->Link (ld)

This compiler supports optimization flags (O0, O1, O2, O3). Not that any optimizations are particularly impressive, but simply
proof that the code structure supports conditional optimization goals.

One of the fun parts about writing compilers is how oftentimes the spec leaves to the developer certain implementation details.
Besides adhering to ISO/IEC 7185:1990(E), here are the details about various parts of my compiler that are relevant to those who use it.

##string-character

Referenced at the beginning of section 6.1.7 is lexem "string-character". This lexem is a set of characters determined to be legal within a string.
Although it's contents are up to the compiler developer (me), it does have a restriction, namely, the apostrophe ''' is not a legal string-character.
For now (subject to change), the following C# style regex matches all characters I've determined to be a member of "string-character".

[0-9A-Za-z_-\(\)]