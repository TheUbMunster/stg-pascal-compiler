program Factorial;
var
    num, fact, i: integer;
begin
    writeln('Enter a number: ');
    readln(num);
    fact := 1;
    for i := 1 to num do
        fact := fact * i;
    writeln('Factorial of ', num, ' is: ', fact);
end.
