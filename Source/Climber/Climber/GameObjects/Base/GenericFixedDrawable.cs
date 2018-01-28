namespace Climber
{
    public class GenericFixedDrawable : FixedDrawableEntity
    {
        string _Char;
        public override string Char => _Char;

        public GenericFixedDrawable(string character, int row) : base(row, null)
        {
            _Char = character;
        }
    }
}
