require forth-libs/string.fs

begin-structure passport
    field: passport:byr     \ Birth Year
    field: passport:iyr     \ Issue Year
    field: passport:eyr     \ Expiration Year
    field: passport:hgt     \ Height
    field: passport:hcl     \ Hair Color
    field: passport:ecl     \ Eye Color
    field: passport:pid     \ Passport ID
    field: passport:cid     \ Country ID
end-structure

10 constant NL
32 constant SP
58 constant COLON

\ Read a passport line into string and eof?
: read-passport-line ( fd -- string t )
  { fd }

  4096                  { bufsiz }
  bufsiz allocate throw { line-buf }
  0                     { k }
  false                 { prev-nl? }
  s" " string:make      { passport-line }
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
    passport-line true
    exit
  then

  \ ignore last newline
  line-buf k 1- string:make { passport-line-string }

  passport-line-string false
;

: byr-valid? ( byr -- t )
  { byr }

  byr 0= if false exit then

  byr string:to-number drop { byr1 }

  byr1 1920 >=
  byr1 2002 <= and
;

: iyr-valid? ( iyr -- t )
  { iyr }

  iyr 0= if false exit then

  iyr string:to-number drop { iyr1 }

  iyr1 2010 >=
  iyr1 2020 <= and
;

: eyr-valid? ( eyr -- t )
  { eyr }

  eyr 0= if false exit then

  eyr string:to-number drop { eyr1 }

  eyr1 2020 >=
  eyr1 2030 <= and
;

: hgt-valid? ( hgt -- t )
  { hgt }

  hgt 0= if false exit then

  hgt s" in" string:make string:ends-with { ends-with-in? }
  hgt s" cm" string:make string:ends-with { ends-with-cm? }

  ends-with-in?
  ends-with-cm? or invert
  if false exit then

  hgt s" in" string:make s" " string:make string:replace to hgt
  hgt s" cm" string:make s" " string:make string:replace to hgt

  hgt string:to-number drop { height }

  ends-with-in? if
    height 59 >=
    height 76 <= and
    exit
  then

  ends-with-cm? if
    height 150 >=
    height 193 <= and
    exit
  then

  ." Invalid state" abort
;

\ Returns true if a is between [b,c]
: between ( a b c -- t )
  { a b c }

  a b >=
  a c <= and
;

: hcl-valid? ( hcl -- t )
  { hcl }

  hcl 0= if false exit then

  hcl string:caddr c@ { first-char }

  first-char [char] # <> if false exit then
  hcl string:length @ 7 <> if false exit then

  true { valid? }

  hcl string:length @ 1 ?do
    hcl string:caddr i + c@ { c }

    c [char] 0 [char] 9 between { 0-9? }
    c [char] a [char] f between { a-f? }

    0-9?
    a-f? or invert if
      false to valid?
    then
  loop

  valid?
;

: ecl-eq? ( ecl caddr u -- t ) string:make string:compare true = ;

: ecl-valid? ( ecl -- t )
  { ecl }

  ecl 0= if false exit then

  ecl s" amb" ecl-eq?
  ecl s" blu" ecl-eq? or
  ecl s" brn" ecl-eq? or
  ecl s" gry" ecl-eq? or
  ecl s" grn" ecl-eq? or
  ecl s" hzl" ecl-eq? or
  ecl s" oth" ecl-eq? or
;

: pid-valid? ( pid -- t )
  { pid }

  pid 0= if false exit then

  pid [: [char] 0 [char] 9 between ;] string:every { all-digits? }

  pid string:length @ 9 = { 9-digits? }

  all-digits?
  9-digits? and
;

: cid-valid? ( cid -- t)
  { cid }

  true
;

: parse-passport ( passport-line -- passport )
  { passport-line }

  passport allocate throw { passport1 }
  passport1 passport erase

  SP passport-line string:tokenize { tokens }

  tokens list:tail @ { iter }

  begin
    iter list:node:nend?
  while
    iter list:node:data @ { token }

    COLON token string:tokenize { pair }

    pair 0 list:nth { first }
    pair 1 list:nth { second }

    first s" byr" string:make string:compare if second passport1 passport:byr ! then
    first s" iyr" string:make string:compare if second passport1 passport:iyr ! then
    first s" eyr" string:make string:compare if second passport1 passport:eyr ! then
    first s" hgt" string:make string:compare if second passport1 passport:hgt ! then
    first s" hcl" string:make string:compare if second passport1 passport:hcl ! then
    first s" ecl" string:make string:compare if second passport1 passport:ecl ! then
    first s" pid" string:make string:compare if second passport1 passport:pid ! then
    first s" cid" string:make string:compare if second passport1 passport:cid ! then

    iter list:node:next @ to iter
  repeat

  passport1
;

: passport-valid? ( passport -- t )
  { passport }

  passport passport:byr @ byr-valid?
  passport passport:iyr @ iyr-valid? and
  passport passport:eyr @ eyr-valid? and
  passport passport:hgt @ hgt-valid? and
  passport passport:hcl @ hcl-valid? and
  passport passport:ecl @ ecl-valid? and
  passport passport:pid @ pid-valid? and
  passport passport:cid @ cid-valid? and
;


: count-valid-passports ( filename -- passports )
  { filename }

  0 { count }

  filename string:raw r/o open-file throw { fd }
  begin
    fd read-passport-line { passport-line eof? }

    eof? invert
  while

    passport-line parse-passport { passport }

    passport passport-valid? { valid? }

    valid? if
      count 1+ to count
    then
  repeat

  count
;

s" day4-input.txt" string:make count-valid-passports .

bye
