namespace Utils.Extensions
{
    public static class FloatExtension
    {
        public static float GetRatio(this float first, float second, bool isReverse = false)
        {
            /*var isFirstBigger = first > second;
            isFirstBigger = isReverse ? !isFirstBigger : isFirstBigger;
            var ratio =  isFirstBigger
                ? first / second 
                : second / first;*/
            return first/second;
        }
    }
}