grammar DIO;

models : model+ ;
model : model_identifier fields? ;

model_identifier : IDENTIFIER model_inherit? COLON ;

fields : field field* ;
field : PIPE position? modifier type IDENTIFIER ;

position : PARENTHESIS_OPEN NUMERIC PARENTHESIS_CLOSE ;
modifier : MODIFIER ;
type : IDENTIFIER ;

model_inherit : BRACKET_OPEN inheriters BRACKET_CLOSE ;

inheriters : inherit (COMMA inherit)* ;
inherit : IDENTIFIER ;

compileUnit : EOF ;

fragment FT_REQUIRED : 'REQUIRED' | 'REQ' | 'R' ;
fragment FT_OPTIONAL : 'OPTIONAL' | 'OPT' | 'O' ;
fragment FT_ARRAY : 'ARRAY' | 'ARR' | 'A' ;
fragment FT_FLATTEN : 'FLATTEN' | 'FLT' | 'F' ;
fragment FT_FLATTEN_ARRAY : 'FLATTEN_ARRAY' | 'FLT_ARR' | 'FA' ;
MODIFIER : FT_REQUIRED | FT_OPTIONAL | FT_FLATTEN | FT_ARRAY | FT_FLATTEN_ARRAY ;

fragment LOWERCASE : [a-z] ;
fragment UPPERCASE : [A-Z] ;
fragment DIGIT : [0-9] ;
fragment UNDERSCORE : '_' ;
fragment WHITESPACE : [ \r\n\t] ;
fragment IDENTIFIER_FRAG : (LOWERCASE | UPPERCASE | UNDERSCORE)+;
IDENTIFIER : IDENTIFIER_FRAG ;
NUMERIC : DIGIT+ ;
WS : WHITESPACE -> channel(HIDDEN) ;
PIPE : '|' ;
COLON : ':' ;
PARENTHESIS_OPEN : '(' ;
PARENTHESIS_CLOSE : ')' ;
BRACKET_OPEN : '[' ;
BRACKET_CLOSE : ']' ;
COMMA : ',' ;

tokens_IDENTIFIER : IDENTIFIER ;
tokens_NUMERIC : NUMERIC ;
tokens_WS : WS;
tokens_MODIFIER : MODIFIER ;
tokens_TYPE : TYPE ;