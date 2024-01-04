/PROG  GRIP_CLOSE	  Macro
/ATTR
OWNER		= MNEDITOR;
COMMENT		= "";
PROG_SIZE	= 507;
CREATE		= DATE 21-10-08  TIME 04:53:32;
MODIFIED	= DATE 21-10-08  TIME 04:53:32;
FILE_NAME	= GRIP_CLO;
VERSION		= 0;
LINE_COUNT	= 18;
MEMORY_SIZE	= 939;
PROTECT		= READ_WRITE;
TCD:  STACK_SIZE	= 0,
      TASK_PRIORITY	= 50,
      TIME_SLICE	= 0,
      BUSY_LAMP_OFF	= 0,
      ABORT_REQUEST	= 0,
      PAUSE_REQUEST	= 0;
DEFAULT_GROUP	= *,*,*,*,*;
CONTROL_CODE	= 00000000 00000000;
/APPL

AUTO_SINGULARITY_HEADER;
  ENABLE_SINGULARITY_AVOIDANCE   : TRUE;
/MN
   1:  !Grip Close ;
   2:  LBL[1] ;
   3:  RO[1:Grip Open]=OFF ;
   4:  RO[2:Grip Close]=ON ;
   5:  WAIT   3.00(sec) ;
   6:   ;
   7:  !Check If Gripped On Part ;
   8:  IF RI[1:Grip Opened]=ON OR RI[2:Grip Closed]=ON,JMP LBL[2] ;
   9:  JMP LBL[3] ;
  10:   ;
  11:  !Failed To Grip On Part ;
  12:  LBL[2] ;
  13:  GO[2:Alarm]=2 ;
  14:  PAUSE ;
  15:  JMP LBL[1] ;
  16:   ;
  17:  !Successfully Gripped Part ;
  18:  LBL[3] ;
/POS
/END
