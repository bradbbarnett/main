/PROG  HD2_OPEN	  Macro
/ATTR
OWNER		= MNEDITOR;
COMMENT		= "";
PROG_SIZE	= 425;
CREATE		= DATE 21-09-21  TIME 16:55:40;
MODIFIED	= DATE 21-09-28  TIME 07:45:50;
FILE_NAME	= ;
VERSION		= 0;
LINE_COUNT	= 16;
MEMORY_SIZE	= 865;
PROTECT		= READ_WRITE;
TCD:  STACK_SIZE	= 0,
      TASK_PRIORITY	= 50,
      TIME_SLICE	= 0,
      BUSY_LAMP_OFF	= 0,
      ABORT_REQUEST	= 0,
      PAUSE_REQUEST	= 0;
DEFAULT_GROUP	= 1,*,*,*,*;
CONTROL_CODE	= 00000000 00000000;
/APPL

AUTO_SINGULARITY_HEADER;
  ENABLE_SINGULARITY_AVOIDANCE   : TRUE;
/MN
   1:  !Chuck Open ;
   2:  LBL[1] ;
   3:  DO[18:HD1 Unclamp]=PULSE,1.0sec ;
   4:   ;
   5:  !Check If Chuck Opened ;
   6:  WAIT DI[18:HD1 Unclamped]=ON TIMEOUT,LBL[2] ;
   7:  JMP LBL[3] ;
   8:   ;
   9:  !Failed To Open Chuck ;
  10:  LBL[2] ;
  11:  GO[2:Alarm]=6 ;
  12:  PAUSE ;
  13:  JMP LBL[1] ;
  14:   ;
  15:  !Successfully Opened Chuck ;
  16:  LBL[3] ;
/POS
/END
