namespace Core.Details
{
    public class Detail
    {
        private readonly DetailType _detailType;

        public DetailType Type => _detailType;

        public Detail(DetailType detailType)
        {
            _detailType = detailType;
        }

        public override string ToString()
        {
            return _detailType.ToString();
        }
    }
}