namespace Global
{
    public static class InputMapping
    {
        public enum Input
        {
            Horizontal,
            Vertical,
            RHorizontal,
            RVertical,
            LB,
            RB,
            LT,
            RT,
            X,
            Y,
            A,
            B
        }

        public enum PlayerTag
        {
            P1,
            P2,
            P3,
            P4
        }

        public static string GetInputName(PlayerTag playerTag, Input input)
        {
            return playerTag + "_" + input;
        }
    }
}
