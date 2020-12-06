variable fd

: input-file  s" day1-input.txt" ;

: fd>pad      pad 80 fd @ read-line throw drop ;
: read-line?  fd>pad dup 0= if drop true else pad swap evaluate , false then ;
: read        here begin read-line? until 0 , ;
: open        input-file r/o open-file throw fd ! ;
: close       fd @ close-file throw ;
: load-file   open read close ;

: 2020?       + 2020 = ;
: next-cell   1 cells + ;
: find-match  begin 2dup @ 2020? if @ exit then dup @ 0<> while next-cell repeat drop false ;
: find-pair   begin dup @ over find-match dup 0<> if rot drop exit then 2drop next-cell again ;
: answer      load-file find-pair * u. ;

answer
bye






