include forth-libs/string.fs

: count-encounters ( right down width map -- count )
  { right down width map }

  0 { count }
  right down { x y }
  begin
    y width * x +
    map string:length@ <
  while
    y width * x + { i }
    map i string:nth { c }

    [char] # c = if
      count 1+ to count
    then

    x right + width mod to x
    y down + to y
  repeat

  count
;

\ Loads the terrain map into map as a string
: load-map ( filename -- map width )
  { filename }
  256 { bufsiz }
  bufsiz allocate throw { line-buf }

  s" " string:make { map }

  0 { width }

  filename string:raw r/o open-file throw { fd }
  begin
    line-buf bufsiz fd read-line throw
  while
    { readn }

    width 0= if readn to width then

    line-buf readn string:make { line }

    map line string:append to map
  repeat drop \ readn

 map width
;

: answer ( right down -- encounters )
  { right down }
  s" day3-input.txt" string:make load-map { map width }

  right down width map count-encounters
;

3 1 answer .
bye
