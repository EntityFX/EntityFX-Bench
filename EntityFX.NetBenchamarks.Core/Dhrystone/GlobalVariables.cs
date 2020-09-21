namespace EntityFX.NetBenchamarks.Core.Dhrystone
{
    public class GlobalVariables : DhrystoneConstants
    {

        protected static Record_Type Record_Glob,
                                    Next_Record_Glob;
        protected static int Int_Glob;
        protected static bool Bool_Glob;
        protected static char Char_Glob_1,
                                    Char_Glob_2;
        protected static int[] Array_Glob_1 = new int[128];
        protected static int[,] Array_Glob_2 = new int[128, 128];
        protected static Record_Type First_Record = new Record_Type(),
                                    Second_Record = new Record_Type();
    }


}
