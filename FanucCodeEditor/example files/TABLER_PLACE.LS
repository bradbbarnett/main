/PROG  TABLER_PLACE
/ATTR
OWNER		= MNEDITOR;
COMMENT		= "";
PROG_SIZE	= 667;
CREATE		= DATE 21-09-27  TIME 15:59:34;
MODIFIED	= DATE 21-10-07  TIME 04:58:28;
FILE_NAME	= INT_PLAC;
VERSION		= 0;
LINE_COUNT	= 25;
MEMORY_SIZE	= 947;
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
   1:  UFRAME_NUM=2 ;
   2:  UTOOL_NUM=1 ;
   3:   ;
   4:  !PLC <- Executing Command ;
   5:  GO[1:Status]=99 ;
   6:   ;
   7:  !Get Table Position Offset ;
   8:  PR[99:Temp]=PR[16:TableR_OnPart]    ;
   9:  PR[99,2:Temp]=(PR[99,2:Temp]+(GI[3:Table_Row]*R[5:Table_Row_Dist])) ;
  10:  PR[99,1:Temp]=(PR[99,1:Temp]+(GI[4:Table_Col]*R[6:Table_Col_Dist])) ;
  11:  PR[99,3:Temp]=PR[99,3:Temp]+GI[5:HD1_Jaw_Length]    ;
  12:   ;
  13:  !Approach ;
  14:J PR[99:Temp] 100% CNT25 Tool_Offset,PR[90:+250Z]    ;
  15:  R[2:RecoveryPos]=17    ;
  16:L PR[99:Temp] 100mm/sec FINE    ;
  17:   ;
  18:  !Place ;
  19:  CALL GRIP_OPEN    ;
  20:   ;
  21:  !Depart ;
  22:L PR[99:Temp] 100mm/sec CNT25 Tool_Offset,PR[90:+250Z]    ;
  23:   ;
  24:  !PLC <- Pick Finished ;
  25:  GO[1:Status]=17 ;
/POS
/END
