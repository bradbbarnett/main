/PROG  PNS0002
/ATTR
OWNER		= MNEDITOR;
COMMENT		= "HOME / RECOVERY";
PROG_SIZE	= 1233;
CREATE		= DATE 21-10-05  TIME 06:02:44;
MODIFIED	= DATE 21-10-06  TIME 04:13:22;
FILE_NAME	= ;
VERSION		= 0;
LINE_COUNT	= 67;
MEMORY_SIZE	= 1725;
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
   1:  !Recovery Program ;
   2:  !To Get Robot Home ;
   3:  LBL[99] ;
   4:  SELECT R[2:RecoveryPos]=1,JMP LBL[1] ;
   5:         =2,JMP LBL[2] ;
   6:         =5,JMP LBL[5] ;
   7:         =6,JMP LBL[6] ;
   8:         =7,JMP LBL[7] ;
   9:         =10,JMP LBL[10] ;
  10:         =11,JMP LBL[11] ;
  11:         =15,JMP LBL[15] ;
  12:         =16,JMP LBL[16] ;
  13:   ;
  14:  !Zero ;
  15:  LBL[1] ;
  16:J PR[10:jTableL_Tuck] 50% FINE    ;
  17:  R[2:RecoveryPos]=10    ;
  18:  JMP LBL[99] ;
  19:   ;
  20:  !Home ;
  21:  LBL[2] ;
  22:J PR[2:jHome] 50% FINE    ;
  23:  R[2:RecoveryPos]=0    ;
  24:  GO[1:Status]=0 ;
  25:  ABORT ;
  26:   ;
  27:  !INT Tuck ;
  28:  LBL[5] ;
  29:J PR[5:jINT_Tuck] 50% FINE    ;
  30:  R[2:RecoveryPos]=2    ;
  31:  JMP LBL[99] ;
  32:   ;
  33:  !INT Pick ;
  34:  LBL[6] ;
  35:L PR[7:INT_OnPart_HD2] 100mm/sec CNT100 Tool_Offset,PR[90:+250Z]    ;
  36:  R[2:RecoveryPos]=5    ;
  37:  JMP LBL[99] ;
  38:   ;
  39:  !INT Place ;
  40:  LBL[7] ;
  41:L PR[6:INT_OnPart_HD1] 100mm/sec CNT100 Tool_Offset,PR[90:+250Z]    ;
  42:  R[2:RecoveryPos]=5    ;
  43:  JMP LBL[99] ;
  44:   ;
  45:  !Table Left Tuck ;
  46:  LBL[10] ;
  47:J PR[10:jTableL_Tuck] 50% FINE    ;
  48:  R[2:RecoveryPos]=2    ;
  49:  JMP LBL[99] ;
  50:   ;
  51:  !Table Left Pick Or Place ;
  52:  LBL[11] ;
  53:L PR[11:TableL_OnPart] 100mm/sec CNT100 Tool_Offset,PR[90:+250Z]    ;
  54:  R[2:RecoveryPos]=10    ;
  55:  JMP LBL[99] ;
  56:   ;
  57:  !Table Right Tuck ;
  58:  LBL[15] ;
  59:J PR[11:TableL_OnPart] 50% FINE    ;
  60:  R[2:RecoveryPos]=2    ;
  61:  JMP LBL[99] ;
  62:   ;
  63:  !Table Right Pick Or Place ;
  64:  LBL[16] ;
  65:L PR[16:TableR_OnPart] 100mm/sec CNT100 Tool_Offset,PR[90:+250Z]    ;
  66:  R[2:RecoveryPos]=15    ;
  67:  JMP LBL[99] ;
/POS
/END
