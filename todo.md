## TODO
* Add file config for various meta-compiler options (e.g., window size for source code printer, color profiles for source code printer)
* Unit tests
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