program Scalars(input, output);
{ every simple type, every operator table, every structured statement, text input and output }
const
   limit = 10;
   negative = -limit;
   pi = 3.14159;
   blank = ' ';
   greeting = 'hello';
type
   colour = (red, green, blue);
   small = 1..limit;
   shade = red..green;
var
   i, j: integer;
   n: small;
   x, y: real;
   c: char;
   b: Boolean;
   hue: colour;
   s: shade;
begin
   i := negative;
   j := limit * 2 + i div 3 - i mod 2;
   x := pi * 2.0e0;
   y := x / 2 + i;
   n := 5;
   i := n + 1;
   c := blank;
   b := (c = ' ') and not (i > j) or (x <= y);
   hue := green;
   s := red;
   if hue = green then i := ord(hue) else i := ord(red);
   while i < limit do i := succ(i);
   repeat i := pred(i) until i <= 0;
   for n := 1 to limit do j := j + n;
   for hue := blue downto red do b := hue in [red, blue];
   case hue of
      red: x := 1.0;
      green, blue: x := 2.5
   end;
   x := abs(x) + sqr(y) + sin(x) + cos(x) + exp(x) + ln(y + 10) + sqrt(abs(x)) + arctan(x);
   i := trunc(x) + round(y) + abs(i) + sqr(i);
   c := chr(ord(c) + 1);
   b := odd(i) and (c <> 'z') and (greeting < 'world') and (greeting = 'hello');
   read(i, x, c);
   readln;
   write(i, ' ', x:10:2, ' ', c, b, greeting:8);
   writeln(i:5);
   writeln
end.
