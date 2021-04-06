variable fd

: input-file  s" day1-input.txt" ;

: fd>pad      pad 80 fd @ read-line throw drop ;
: read-line?  fd>pad ?dup if pad swap evaluate , false else true then ;
: read        here begin read-line? until 0 , ;
: open        input-file r/o open-file throw fd ! ;
: close       fd @ close-file throw ;
: load-file   open read close ;

: 2020?       + 2020 = ;

: find-match?
  begin
    2dup @ 2020? if @ nip exit then
    dup @ 0<>
  while
    cell+
  repeat
  2drop false
;

: find-pair
  begin
    dup @ over find-match? ?dup if swap @ exit else cell+ then
  again
;

: answer      load-file find-pair * u. ;

answer
