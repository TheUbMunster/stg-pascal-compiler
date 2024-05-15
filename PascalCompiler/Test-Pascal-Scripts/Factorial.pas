program Factorial;
var
    num, fact, i: integer; {declare vars}
begin
    writeln('Enter a number: '); {get input}
    readln(num);
    fact := 1;
    for i := 1 to num do
        fact := fact * i; (*factorialize
        yes yes*) 
    writeln('Factorial of ', num, ' is: ', fact);
end.
