## TODO
* Add file config for various meta-compiler options (e.g., window size for source code printer, color profiles for source code printer)
* Unit tests:
	- More rigourously test LegalCharacterSet
	- Rigourously test TT methods in Lexer & make more efficient
	- Rigourously test Source.GetLineColFromFileLocation, GetLexTokenSourceLine etc.
* Make sure all linebreak codes handles (\r\n?|\n)
* Get comment clairification from ISO
* Switch appropriate code over to stringbuilder
* Cache token content
* Token.ToString make the linebreak replacer show what kind of linebreak it was
* Clean up formatting of the regexes dictionary
* Clean up comments in TokenType (LexerUtil)
* Determine what situations warrant throwing actual exceptions vs (and/or) AddMessage(error)
* ?Remove custom exception types? (e.g., LexerException)
* Break up CommandProcessor
* Add verbosity level flag to CommandProcessor
* Make PrintMessage use parse + lex/parse + lex + typecheck information when avaliable (for progressively more accurate code coloring)
* Write Parser influenced by 6.1 & 6.2, possibly more.
* Gather the flags and pass them to every function (lex, parse etc) so that these functions can make use of this information
* Add the ability for AddMessage() to take multiple file locations in case a single error relates to multiple portions of the file.
* To reduce confusion:
	- Rename Token class to LexToken
	- Rename TokenType enum values to have the "Lex" prefix
	- Rename ASTNode deriving classes to have the "AST" prefix
	- Rename ASTNodeType enum values to have the "AST" prefix
	- Prefix ASTNode data members with _0 _1 _2 _3 ... etc in the order that those data fields represent the parsed data?

It seems that this grammar has some ambiguous situations. E.g., a simple character-string can be parsed either as a constant or an unsigned-constant. What to do?

* Add the ability for the user to "highlight" the parse tree, i.e., they can select a node to "highlight" and the associated source code gets highlighed:
should look something like this:

Node being examined: Node.ToString(0, true)
4  {these are} //regular coloring (white on black)
     -----v
5  {lines of} //this line has its color inverted (black on white)
6  {source code} //this line has its color inverted (black on white)
7-21 ... (omitted)
22 {these are} //this line has its color inverted (black on white)
23 {lines of} //this line has its color inverted (black on white)
\-----^
24 {source code} //regular coloring (white on black)

Similar to the above, do this for the lex list?

Maybe like the "split" layout in GDB?

change the flag system so that no dashes are non flags;
one dash are single-character flags (i.e., -a -b is the same as -ab)
two dash are word flags (i.e., --help doesn't get split up)