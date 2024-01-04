/PROG  GRIP_OPEN	  Macro
/ATTR
OWNER		= MNEDITOR;
COMMENT		= "";
PROG_SIZE	= 435;
CREATE		= DATE 21-10-08  TIME 04:54:24;
MODIFIED	= DATE 21-10-08  TIME 04:54:24;
FILE_NAME	= ;
VERSION		= 0;
LINE_COUNT	= 17;
MEMORY_SIZE	= 871;
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
   1:  !Grip Open ;
   2:  LBL[1] ;
   3:  RO[1:Grip Open]=ON ;
   4:  RO[2:Grip Close]=OFF ;
   5:   ;
   6:  !Check If Opened ;
   7:  WAIT RI[1:Grip Opened]=ON TIMEOUT,LBL[2] ;
   8:  JMP LBL[3] ;
   9:   ;
  10:  !Failed To Open Gripper ;
  11:  LBL[2] ;
  12:  GO[2:Alarm]=1 ;
  13:  PAUSE ;
  14:  JMP LBL[1] ;
  15:   ;
  16:  !Successfully Opened Gripper ;
  17:  LBL[3] ;
/POS
/END
