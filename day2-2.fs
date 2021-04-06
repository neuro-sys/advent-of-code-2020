variable fd
variable nread
variable min
variable max
variable letter

: input-file      s" day2-input.txt" ;
: open-file       input-file r/o open-file throw fd ! ;
: close-file      fd @ close-file throw ;
: read-line?      pad 80 fd @ read-line throw drop dup nread ! ;
: get-line        pad nread @ ;
: 1/search        search 0= throw 1 /string ;
: skip-dash       s" -" 1/search ;
: skip-space      s"  " 1/search ;
: skip-:          s" :" 1/search ;
: parse-min       get-line skip-dash drop 1- pad - pad swap 0 0 2swap >number 2drop drop  ;
: parse-max       get-line skip-dash over >r skip-space drop 1- r@ - r> swap
                  0 0 2swap >number 2drop drop ;
: parse-letter    get-line skip-space over >r skip-: drop 1- r@ - r> swap drop c@ ;

: check-valid?
  get-line skip-space skip-space drop >r
  r@ min @ 1- + c@ letter c@ =
  r> max @ 1- + c@ letter c@ = xor
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
