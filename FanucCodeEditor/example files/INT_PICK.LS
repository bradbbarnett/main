/PROG  INT_PICK
/ATTR
OWNER		= MNEDITOR;
COMMENT		= "";
PROG_SIZE	= 667;
CREATE		= DATE 21-09-21  TIME 16:47:56;
MODIFIED	= DATE 21-10-08  TIME 05:16:42;
FILE_NAME	= ;
VERSION		= 0;
LINE_COUNT	= 28;
MEMORY_SIZE	= 943;
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
   2:  UTOOL_NUM=1 ;
   3:  R[10:Zoffset]=GI[6:HD2_Length]+GI[7:HD2_Jaw_Length]+R[3:ClearanceOffset]    ;
   4:   ;
   5:  !PLC <- Executing Command ;
   6:  GO[1:Status]=99 ;
   7:   ;
   8:  !Approach ;
   9:  PR[99:Temp]=PR[90:+250Z]    ;
  10:  PR[99,3:Temp]=PR[99,3:Temp]+R[10:Zoffset]    ;
  11:J PR[7:INT_OnPart_HD2] 100% CNT25 Tool_Offset,PR[99:Temp]    ;
  12:  R[2:RecoveryPos]=6    ;
  13:  PR[99:Temp]=PR[100:Zero]    ;
  14:  PR[99,3:Temp]=PR[99,3:Temp]+R[10:Zoffset]    ;
  15:L PR[7:INT_OnPart_HD2] 100mm/sec FINE Tool_Offset,PR[99:Temp]    ;
  16:   ;
  17:  !Pick ;
  18:  CALL GRIP_PART    ;
  19:  CALL HD2_OPEN    ;
  20:  PAYLOAD[2] ;
  21:   ;
  22:  !Depart ;
  23:  PR[99:Temp]=PR[89:+500Z]    ;
  24:  PR[99,3:Temp]=PR[99,3:Temp]+R[10:Zoffset]    ;
  25:L PR[7:INT_OnPart_HD2] 100mm/sec CNT25 Tool_Offset,PR[99:Temp]    ;
  26:   ;
  27:  !PLC <- Pick Finished ;
  28:  GO[1:Status]=6 ;
/POS
/END
