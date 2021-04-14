variable fd
variable nread
variable min
variable max
variable letter

: input-file      s" day2-input.txt" ;
: open-file       input-file r/o open-file throw fd ! ;
: close-file      fd @ close-file throw ;
: read-line?      pad 80 fd @ read-line throw drop dup nread ! ;
: line            pad nread @ ;
: 1/search        search 0= throw 1 /string ;
: skip-dash       s" -" 1/search ;
: skip-space      s"  " 1/search ;
: skip-:          s" :" 1/search ;
: to-number       0 0 2swap >number 2drop drop ;
: parse-min       line skip-dash drop 1- pad - pad swap to-number ;
: parse-max       line skip-dash over >r skip-space drop 1- r@ - r> swap to-number ;
: parse-letter    line skip-space over >r skip-: drop 1- r@ - r> swap drop c@ ;
: in-range?       min @ >= swap max @ <= and ;
: letter-eq?      over c@ letter c@ = ;
: inc-count       postpone r> postpone 1+ postpone >r ; immediate
: skip-letter     1 /string ;

: check-valid?
  line skip-space skip-space
  0 >r
  begin ?dup 0<>
  while
    letter-eq? if inc-count then skip-letter
  repeat drop
  r@ r> in-range?
;

: solve
  open-file
  0
  begin
    read-line?
  while
    parse-min min !
    parse-max max !
    parse-letter letter !
    check-valid? if 1+ then
  repeat
  close-file
;

solve .
