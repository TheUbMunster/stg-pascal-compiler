program Labels(output);
{ labels placed and jumped to within a block, out of a nested block, and inside the labeled statement itself }
label 10, 20, 99;
var
   i: integer;

procedure escape;
label 5;
begin
   i := 0;
5: i := i + 1;
   if i < 3 then goto 5;
   if i = 3 then goto 99
end;

begin
   i := 0;
10: i := i + 1;
   if i < 5 then goto 10;
20: begin
      if i > 100 then goto 20
   end;
   escape;
99: writeln(i)
end.
