program ReverseString;
var
    str, reversed: string;
    i: integer;
begin
    writeln('Enter a string: ');
    readln(str);
    reversed := '';
    for i := length(str) downto 1 do
        reversed := reversed + str[i];
    writeln('Reversed string is: ', reversed);
end.
