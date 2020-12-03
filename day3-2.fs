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

: slope-count ( map right down width -- encounters )
  { map right down width }

  right down width map count-encounters
;

: answer 
  s" day3-input.txt" string:make load-map { map width }
  map 1 1 width slope-count
  map 3 1 width slope-count *
  map 5 1 width slope-count *
  map 7 1 width slope-count *
  map 1 2 width slope-count *
;

answer .

bye
