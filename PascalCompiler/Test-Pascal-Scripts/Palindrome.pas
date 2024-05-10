program CheckPalindrome;
var
    str, reversed: string;
    i: integer;
    isPalindrome: boolean;
begin
    writeln('Enter a string: ');
    readln(str);
    reversed := '';
    for i := length(str) downto 1 do
        reversed := reversed + str[i];
    isPalindrome := str = reversed;
    if isPalindrome then
        writeln('The string is a palindrome.')
    else
        writeln('The string is not a palindrome.');
end.
