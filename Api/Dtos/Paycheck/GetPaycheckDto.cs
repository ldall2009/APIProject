namespace Api.Dtos.Paycheck
{
    public class GetPaycheckDto
    {
        public string? EmployeeName { get; set; }
        public decimal TotalEarnings { get; set; }
        public ICollection<GetPaycheckDeductionDto> Deductions { get; set; } = new List<GetPaycheckDeductionDto>();
        public decimal NetPay
        {
            get
            {
                return TotalEarnings - Deductions.Sum(s => s.Amount);
            }
        }
    }
}
