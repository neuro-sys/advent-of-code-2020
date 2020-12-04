require forth-libs/string.fs

begin-structure passport
    field: byr     \ Birth Year
    field: iyr     \ Issue Year
    field: eyr     \ Expiration Year
    field: hgt     \ Height
    field: hcl     \ Hair Color
    field: ecl     \ Eye Color
    field: pid     \ Passport ID
    field: cid     \ Country ID
end-structure

10 constant NL
32 constant SP

\ Read a password line into string and eof?
: read-password-line ( fd -- string t )
  { fd }

  4096                  { bufsiz }
  bufsiz allocate throw { line-buf }
  0                     { k }
  false                 { prev-nl? }
  s" " string:make      { password-line }
  false                 { eof? }

  begin
    1 allocate throw              { char-buf }
    char-buf 1 fd read-file throw { readn }
    char-buf c@ 10 =              { nl? }
    nl? prev-nl? and              { block-end? }

    readn 0 = to eof?

    nl? to prev-nl?

    eof? invert block-end? invert and
  while
    char-buf c@ { c }

    c NL = if SP to c then \ if newline then replace with space

    c line-buf k + c!

    k 1+ to k
  repeat

  eof? if
    false true
    exit
  then

  \ ignore last newline
  line-buf k 1- string:make { password-line-string }

  password-line-string false
;

: count-valid-passports ( filename -- passports )
  { filename }

  0 { count }

  filename string:raw r/o open-file throw { fd }
  begin
    fd read-password-line { password-line eof? }

    eof? invert
  while
    32 password-line string:tokenize { tokens }

    tokens [:
      { token }

      token s" cid" string:make string:index-of

      -1 <>
    ;] list:some { contains-cid? }

    contains-cid? if 8 else 7 then { num-field-required }

    tokens list:length num-field-required = { valid? }

    valid? if
      count 1+ to count
    then
  repeat

  count
;

s" day4-input.txt" string:make count-valid-passports .

bye
