public static class CraftMath
{
    public static int FloorDiv(int var0, int var1) {
        int var2 = var0 / var1;
        if ((var0 ^ var1) < 0 && var2 * var1 != var0) {
            --var2;
        }
        return var2;
    }
        
    public static int FloorMod(int var0, int var1)
    {
        // return ((var0 % var1) + var1) % var1;
        return var0 - FloorDiv(var0, var1) * var1;
    }
}
