require reader.fs
require forth-libs/list.fs
require forth-libs/string.fs

0 \ password structure
dup constant password->min     1 cells +
dup constant password->max     1 cells +
dup constant password->letter  1 cells +
dup constant password->phrase  1 cells +
constant password:struct

: password:min      password->min + ;
: password:max      password->max + ;
: password:letter   password->letter + ;
: password:phrase   password->phrase + ;
: password:min@     password:min @ ;
: password:max@     password:max @ ;
: password:letter@  password:letter @ ;
: password:phrase@  password:phrase @ ;
: password:allocate password:struct allocate throw ;

: password:expand { password -- min max letter phrase }
  password password:min@
  password password:max@
  password password:letter@
  password password:phrase@
;

: password:valid? { password -- t }
  true { valid } \ assume valid
  password password:expand { min max letter phrase }
  letter 0 string:nth to letter

  phrase min 1- string:nth { c-min }
  phrase max 1- string:nth { c-max }

  c-min letter =
  c-max letter =
  xor
;

: password:make { min max letter phrase -- password }
  password:allocate { password }

  min    password password:min !
  max    password password:max !
  letter password password:letter !
  phrase password password:phrase !

  password
;

\ parse string with format "a-b" into min max
: parse-min-max { range -- min max }
  [char] - range string:tokenize { range-tokens }
  range-tokens 0 list:nth { min }
  min string:to-number drop to min

  range-tokens 1 list:nth { max }
  max string:to-number drop to max

  min max
;

\ parse string with format "a:" into letter
: parse-letter { string -- letter }
  [char] : string string:tokenize { letter-tokens }

  letter-tokens 0 list:nth
;

\ parse string with format "a-b c: def" into password
: parse-password { line -- password }
  bl line string:tokenize { tokens }

  tokens 0 list:nth parse-min-max { min max }
  tokens 1 list:nth parse-letter { letter }
  tokens 2 list:nth { phrase }

  min max letter phrase password:make
;

: read-passwords { filename -- passwords }
  list:make { passwords }
  256 { bufsiz }
  bufsiz allocate throw { line-buf }

  filename string:raw r/o open-file throw { fd }
  begin
    line-buf bufsiz fd read-line throw
  while
    { readn }

    line-buf readn string:make { line }
    line parse-password { password }
    passwords password list:append to passwords
  repeat drop \ readn

  line-buf free
  passwords
;

: answer
  s" day2-input.txt" string:make read-passwords { passwords }
  passwords 0 [: password:valid? if 1+ then ;] list:reduce .
;

answer

bye
