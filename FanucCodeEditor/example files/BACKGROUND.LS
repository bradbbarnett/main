/PROG  BACKGROUND
/ATTR
OWNER		= MNEDITOR;
COMMENT		= "";
PROG_SIZE	= 443;
CREATE		= DATE 21-09-28  TIME 07:31:56;
MODIFIED	= DATE 21-10-05  TIME 05:33:28;
FILE_NAME	= ;
VERSION		= 0;
LINE_COUNT	= 14;
MEMORY_SIZE	= 923;
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
  ENABLE_SINGULARITY_AVOIDANCE   : FALSE;
/MN
   1:  !Heart Beat ;
   2:  IF (DI[13:BG Prog Hndshk]=ON),DO[13:BG Prog Running]=(ON) ;
   3:  IF (DI[13:BG Prog Hndshk]=OFF),DO[13:BG Prog Running]=(OFF) ;
   4:   ;
   5:  !Gripper Status ;
   6:  DO[21:Grip Opened]=(RI[1:Grip Opened]) ;
   7:  DO[22:Grip Closed]=(RI[2:Grip Closed]) ;
   8:   ;
   9:  !Payload ;
  10:  IF (RI[1:Grip Opened]=ON),$PLST_PARNUM[1]=(1) ;
  11:   ;
  12:  !Fault Reset ;
  13:  LBL[2] ;
  14:  IF (UI[5:Fault reset]=ON),GO[2:Alarm]=(0) ;
/POS
/END
