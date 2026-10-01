namespace PhysicianBooking.CPanel.Helper
{
    using Dapper;
    using System.Data;

    public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        // This runs when SENDING a date to the database
        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.Value = value.ToDateTime(new TimeOnly(0, 0));
            parameter.DbType = DbType.Date;
        }

        // This runs when READING a date from the database
        public override DateOnly Parse(object value)
        {
            if (value is DateTime dateTime)
            {
                return DateOnly.FromDateTime(dateTime);
            }
            return DateOnly.FromDateTime(DateTime.Parse(value.ToString()));
        }
    }
}
