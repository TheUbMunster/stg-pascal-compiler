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

As a quick side note, after getting around to writing the parser portion, I've determined that the spec's grammar structure
freaking sucks. There's no clear categories to the grammar, and although some non-terminals are comprised of a deterministic
number of children, others are defined to be a messy expression of optional elements, and *or* expressions causing silly branching.

One of the fun parts about writing compilers is how oftentimes the spec leaves to the developer certain implementation details.
Besides adhering to ISO/IEC 7185:1990(E), here are the details about various parts of my compiler that are relevant to those who use it.

## string-character

Referenced at the beginning of section 6.1.7 is lexem "string-character". This lexem is a set of characters determined to be legal within a string.
Although it's contents are up to the compiler developer (me), it does have a restriction, namely, the apostrophe ''' is not a legal string-character.
For now (subject to change), every character in the "legalChars" array at the top of LegalCharacterSet.cs except the apostrophe is a legal string-character.

## up arrow

More a clairification, but the special-symbol "up arrow" doesn't exist in the UTF8 character space (our preferred character encoding).
As per note 1 in section 6.1.9, the "up arrow"'s reference token is the carat ^, and as per the spec, the alternative token is @.

## line endings

The pascal compiler is compatible with windows-style line endings (CRLF), unix-style line endings (LF), and old-mac line endings (CR),
however, mixing line endings will cause the compiler to emit a "mixed line endings" suggestion, and using old-mac line endings (CR), will
cause the compiler to emit a "you shouldn't use old-mac style line endings for file compatibility with other computers, especially unix" warning.

## source file encoding

For now, I only guarantee functionality with UTF8 encoded source files. Any other encodings may work, but I haven't tested for them.
This may change in the future.