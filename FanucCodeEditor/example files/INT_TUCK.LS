/PROG  INT_TUCK
/ATTR
OWNER		= MNEDITOR;
COMMENT		= "";
PROG_SIZE	= 345;
CREATE		= DATE 21-09-21  TIME 16:38:46;
MODIFIED	= DATE 21-09-28  TIME 08:09:58;
FILE_NAME	= ;
VERSION		= 0;
LINE_COUNT	= 11;
MEMORY_SIZE	= 673;
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
   1:  UFRAME_NUM=1 ;
   2:   ;
   3:  !PLC <- Executing Command ;
   4:  GO[1:Status]=99 ;
   5:   ;
   6:  !Move To Tuck ;
   7:J PR[5:jINT_Tuck] 100% CNT10    ;
   8:  R[2:RecoveryPos]=5    ;
   9:   ;
  10:  !PLC <- At Tuck ;
  11:  GO[1:Status]=5 ;
/POS
/END
