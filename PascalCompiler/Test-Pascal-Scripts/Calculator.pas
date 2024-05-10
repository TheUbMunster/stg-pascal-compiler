program SimpleCalculator;
var
    num1, num2, result: real;
    operator: char;
begin
    writeln('Enter first number: ');
    readln(num1);
    writeln('Enter operator (+, -, *, /): ');
    readln(operator);
    writeln('Enter second number: ');
    readln(num2);
    case operator of
        '+': result := num1 + num2;
        '-': result := num1 - num2;
        '*': result := num1 * num2;
        '/': result := num1 / num2;
    end;
    writeln('Result: ', result);
end.
