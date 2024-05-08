# Pascal Compiler

# ***WIP***

**The below information may not be applicable (yet) as this is a WIP**

This is a Pascal compiler I wrote as a personal project in C#, specifically, adhering to ISO/IEC 7185:1990
It probably has some flaws, so I wouldn't use it for anything important, just a fun project.
Shooting for ?linux x86_64 ABI?.

Compiler structure:
Lexer->Parser->Check->Emit->Assemble (MASM)->Link (ld)

This compiler supports optimization flags (O0, O1, O2, O3). Not that any optimizations are particularly impressive, but simply
proof that the code structure supports conditional optimization goals.
