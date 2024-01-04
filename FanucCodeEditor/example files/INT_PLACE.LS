/PROG  INT_PLACE
/ATTR
OWNER		= MNEDITOR;
COMMENT		= "";
PROG_SIZE	= 785;
CREATE		= DATE 21-09-22  TIME 07:49:54;
MODIFIED	= DATE 21-10-08  TIME 06:48:14;
FILE_NAME	= INT_PICK;
VERSION		= 0;
LINE_COUNT	= 32;
MEMORY_SIZE	= 1173;
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
   3:  R[10:Zoffset]=GI[2:HD1_Length]+GI[5:HD1_Jaw_Length]+R[3:ClearanceOffset]    ;
   4:   ;
   5:  !PLC <- Executing Command ;
   6:  GO[1:Status]=99 ;
   7:   ;
   8:  !Approach ;
   9:  PR[99:Temp]=PR[90:+250Z]    ;
  10:  PR[99,3:Temp]=PR[99,3:Temp]+R[10:Zoffset]    ;
  11:J PR[6:INT_OnPart_HD1] 100% CNT25 Tool_Offset,PR[99:Temp]    ;
  12:  R[2:RecoveryPos]=7    ;
  13:  PR[99:Temp]=PR[100:Zero]    ;
  14:  PR[99,3:Temp]=PR[99,3:Temp]+R[10:Zoffset]    ;
  15:L PR[6:INT_OnPart_HD1] 100mm/sec FINE Tool_Offset,PR[99:Temp]    ;
  16:  SOFTFLOAT[1] ;
  17:  PR[99:Temp]=PR[100:Zero]    ;
  18:  PR[99,3:Temp]=PR[99,3:Temp]+R[10:Zoffset]-10    ;
  19:L PR[6:INT_OnPart_HD1] 100mm/sec FINE Tool_Offset,PR[99:Temp]    ;
  20:   ;
  21:  !Place ;
  22:  CALL HD1_CLOSE    ;
  23:  CALL GRIP_OPEN    ;
  24:  SOFTFLOAT END ;
  25:   ;
  26:  !Depart ;
  27:  PR[99:Temp]=PR[90:+250Z]    ;
  28:  PR[99,3:Temp]=PR[99,3:Temp]+R[10:Zoffset]    ;
  29:L PR[6:INT_OnPart_HD1] 100mm/sec CNT25 Tool_Offset,PR[99:Temp]    ;
  30:   ;
  31:  !PLC <- Pick Finished ;
  32:  GO[1:Status]=7 ;
/POS
/END
