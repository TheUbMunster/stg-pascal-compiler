grammar TestGrammar;

options {
    language=CSharp;
}

prog:	expr EOF ;
expr:	expr ('*'|'/') expr
    |	expr ('+'|'-') expr
    |	INT
    |	'(' expr ')'
    ;
NEWLINE : [\r\n]+ -> skip;
WS: [ \n\t\r]+ -> skip;
INT     : [0-9]+ ;