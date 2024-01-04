/PROG  TABLEL_PLACE
/ATTR
OWNER		= MNEDITOR;
COMMENT		= "";
PROG_SIZE	= 671;
CREATE		= DATE 21-09-27  TIME 15:59:28;
MODIFIED	= DATE 21-10-08  TIME 05:15:48;
FILE_NAME	= TABLE_PL;
VERSION		= 0;
LINE_COUNT	= 25;
MEMORY_SIZE	= 951;
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
   8:  PR[99:Temp]=PR[11:TableL_OnPart]    ;
   9:  PR[99,1:Temp]=(PR[99,1:Temp]+(GI[3:Table_Row]*R[5:Table_Row_Dist])) ;
  10:  PR[99,2:Temp]=(PR[99,2:Temp]-(GI[4:Table_Col]*R[6:Table_Col_Dist])) ;
  11:  PR[99,3:Temp]=PR[99,3:Temp]+GI[5:HD1_Jaw_Length]+R[3:ClearanceOffset]    ;
  12:   ;
  13:  !Approach ;
  14:J PR[99:Temp] 100% CNT25 Tool_Offset,PR[90:+250Z]    ;
  15:  R[2:RecoveryPos]=11    ;
  16:L PR[99:Temp] 100mm/sec FINE    ;
  17:   ;
  18:  !Place ;
  19:  CALL GRIP_OPEN    ;
  20:   ;
  21:  !Depart ;
  22:L PR[99:Temp] 100mm/sec CNT25 Tool_Offset,PR[90:+250Z]    ;
  23:   ;
  24:  !PLC <- Pick Finished ;
  25:  GO[1:Status]=12 ;
/POS
/END
