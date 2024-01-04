/PROG  REGRIP_PLACEPICK
/ATTR
OWNER		= MNEDITOR;
COMMENT		= "";
PROG_SIZE	= 655;
CREATE		= DATE 21-10-01  TIME 03:58:36;
MODIFIED	= DATE 21-10-08  TIME 05:27:50;
FILE_NAME	= TABLEL_P;
VERSION		= 0;
LINE_COUNT	= 27;
MEMORY_SIZE	= 935;
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
   1:  UFRAME_NUM=3 ;
   2:  UTOOL_NUM=1 ;
   3:   ;
   4:  !PLC <- Executing Command ;
   5:  GO[1:Status]=99 ;
   6:   ;
   7:  !Get Table Position Offset ;
   8:  PR[99:Temp]=PR[21:Regrip_OnPart]    ;
   9:  PR[99,3:Temp]=PR[99,3:Temp]+GI[5:HD1_Jaw_Length]+R[3:ClearanceOffset]    ;
  10:   ;
  11:  !Approach ;
  12:J PR[99:Temp] 100% CNT100 Tool_Offset,PR[90:+250Z]    ;
  13:  R[2:RecoveryPos]=21    ;
  14:L PR[99:Temp] 100mm/sec FINE    ;
  15:   ;
  16:  !Place ;
  17:  CALL GRIP_OPEN    ;
  18:   ;
  19:  !Pick ;
  20:  CALL GRIP_CLOSE    ;
  21:  PAYLOAD[2] ;
  22:   ;
  23:  !Depart ;
  24:L PR[99:Temp] 100mm/sec CNT100 Tool_Offset,PR[90:+250Z]    ;
  25:   ;
  26:  !PLC <- Finished ;
  27:  GO[1:Status]=21 ;
/POS
/END
