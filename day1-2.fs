variable fd

: input-file  s" day1-input.txt" ;

: fd>pad      pad 80 fd @ read-line throw drop ;
: read-line?  fd>pad ?dup if pad swap evaluate , false else true then ;
: read        here begin read-line? until 0 , ;
: open        input-file r/o open-file throw fd ! ;
: close       fd @ close-file throw ;
: load-file   open read close ;

: 2020?  ( num1 num2 num3 -- t )  + + 2020 - 0= ;

: find-match? ( num1 num2 addr --- num3 t )
  begin
    dup @ 3 pick 3 pick 2020?
    if @ true exit then
    cell+ dup @ 0=
  until
  2drop drop false
;

: find-pair? ( num1 addr --- num2 num3 t )
  begin
    dup @ 2 pick 2 pick find-match? if true exit then
    cell+ dup @ 0=
  until
  drop false
;

: find-triple ( addr --- num1 num2 num3 )
  begin
    dup @ over find-pair? if 3 roll drop 3 roll drop exit then
    drop cell+
  again
;

load-file find-triple * * u. drop
