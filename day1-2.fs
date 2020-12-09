: input-file  s" day1-input.txt" ;

\ takes fd as input and reads into 80 char pad area, returns number of
\ chars read ( addr -- u )
: fd>pad      pad 80 3 pick read-line throw drop ;

\ takes fd as input, writes to current DP, and preserves fd and returns boolean
\ indicating eof ( addr -- addr t )
: read-line?  fd>pad ?dup if pad swap evaluate , false else true then ;

\ takes fd as input, and returns dictionary data containing zero
\ terminated cells ( addr -- addr addr)
: read        here begin read-line? until 0 , ;

( -- fd )
: open        input-file r/o open-file throw ;

( addr -- )
: close       close-file throw ;

( -- addr )
: load-file   open read swap close ;

( num1 num2 num3 -- t )
: 2020?       + + 2020 - 0= ;

( num1 num2 addr --- num3 t ) 
: find-match? begin dup @ 3 pick 3 pick 2020? if @ true exit then cell+ dup @ 0<> while repeat 2drop drop false ;

( num1 addr --- num2 num3 t ) 
: find-pair?  begin dup @ 2 pick 2 pick find-match? if true exit then cell+ dup @ 0<> while repeat drop false ;

( addr --- num1 num2 num3 ) 
: find-triple begin dup @ over find-pair? if 3 roll drop 3 roll drop exit then drop cell+ again ;

load-file find-triple * * u. drop

bye
