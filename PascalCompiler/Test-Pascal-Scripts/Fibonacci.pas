program FibonacciSeries;
var
    n, i, term1, term2, nextTerm: integer;
begin
    writeln('Enter number of terms: ');
    readln(n);
    term1 := 0;
    term2 := 1;
    writeln('Fibonacci Series:');
    for i := 1 to n do
    begin
        write(term1, ' ');
        nextTerm := term1 + term2;
        term1 := term2;
        term2 := nextTerm;
    end;
end.
