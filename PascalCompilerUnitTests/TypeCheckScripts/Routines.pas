program Routines(output);
{ value, variable, procedural, functional and conformant-array parameters; forward, recursion, nested routines }
type
   row = array[1..3] of integer;
var
   count: integer;
   values: row;
   r: real;

procedure later(n: integer); forward;

function square(x: integer): integer;
begin
   square := x * x
end;

procedure swap(var a, b: integer);
var
   temp: integer;
begin
   temp := a;
   a := b;
   b := temp
end;

function factorial(n: integer): integer;
begin
   if n <= 1 then factorial := 1 else factorial := n * factorial(n - 1)
end;

procedure apply(function f(x: integer): integer; procedure report(n: integer); var target: integer);
begin
   target := f(target);
   report(target)
end;

procedure later;
begin
   count := count + n
end;

function sum(var a: array[low..high: integer] of integer): integer;
var
   i, s: integer;
begin
   s := 0;
   for i := low to high do s := s + a[i];
   sum := s
end;

function outer(limit: integer): integer;
   procedure inner;
   begin
      outer := limit * 2
   end;
begin
   inner
end;

begin
   count := 0;
   values[1] := 1;
   values[2] := 2;
   values[3] := 3;
   swap(values[1], values[3]);
   apply(square, later, count);
   count := factorial(5) + sum(values) + outer(4);
   r := count / 2;
   writeln(count, r:8:3)
end.
