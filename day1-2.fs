variable fd

0 value num1
0 value num2
0 value num3

: input-file  s" day1-input.txt" ;

: fd>pad      pad 80 fd @ read-line throw drop ;
: read-line?  fd>pad ?dup if pad swap evaluate , false else true then ;
: read        here begin read-line? until 0 , ;
: open        input-file r/o open-file throw fd ! ;
: close       fd @ close-file throw ;
: load-file   open read close ;

: 2020?       num1 + num2 + 2020 = ;
: find-match? begin dup @ 2020? if @ exit then dup @ 0<> while cell+ repeat drop false ;
: find-pair?  begin dup @ to num2 dup find-match? ?dup if to num3 true exit then dup @ 0<> while cell+ repeat drop false ;
: find-triple begin dup @ to num1 dup find-pair? if @ to num2 @ to num1 exit then cell+ again ; 

load-file find-triple num1 num2 num3 * * u.

bye
