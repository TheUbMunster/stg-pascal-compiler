program Structures(output);
{ arrays, a string-type, a record with variants, with, sets, pointers, a file of integer and a textfile }
const
   size = 5;
type
   index = 1..size;
   vector = array[index] of real;
   matrix = array[index, index] of real;
   name = packed array[1..8] of char;
   shape = (circle, square, triangle);
   figure = record
      area: real;
      case kind: shape of
         circle: (radius: real);
         square: (side: real);
         triangle: (base, height: real)
   end;
   charset = set of char;
   link = ^node;
   node = record
      value: integer;
      next: link
   end;
   intfile = file of integer;
var
   v: vector;
   m: matrix;
   nm: name;
   f: figure;
   letters: charset;
   digits: set of '0'..'9';
   p, q: link;
   data: intfile;
   t: text;
   i, j: index;
   total: real;
begin
   for i := 1 to size do
   begin
      v[i] := i * 1.5;
      for j := 1 to size do m[i, j] := v[i] + m[j][i]
   end;
   nm := 'abcdefgh';
   nm[1] := 'z';
   f.kind := circle;
   f.radius := 2.0;
   with f do
   begin
      area := radius * radius;
      kind := square;
      side := 3.0
   end;
   letters := ['a'..'z', 'A'..'Z'];
   digits := ['0'..'9'];
   letters := letters + ['_'] - ['x'] * letters;
   total := 0.0;
   if ('q' in letters) and not (letters <= digits) and (letters <> []) then total := 1.0;
   new(p);
   p^.value := 1;
   p^.next := nil;
   new(q);
   q^ := p^;
   if (p <> nil) and (p <> q) then dispose(q);
   dispose(p);
   rewrite(data);
   data^ := 42;
   put(data);
   reset(data);
   while not eof(data) do
   begin
      total := total + data^;
      get(data)
   end;
   rewrite(t);
   writeln(t, 'total: ', total:6:1);
   reset(t);
   if not eoln(t) then readln(t);
   writeln(nm, ' ', f.area:4:1)
end.
